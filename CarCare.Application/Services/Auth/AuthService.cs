
using AutoMapper;
using CarCare.Core.Domain.Contracts.Persistence;
using CarCare.Core.Domain.Entities.Identity;
using CarCare.Core.Domain.Entities.ServiceTypes;
using CarCare.Shared.ErrorModoule.Exeptions;
using CarCare.Shared.Models.Roles;
using CareCare.Core.Application.Abstraction.Models.Auth;
using CareCare.Core.Application.Abstraction.Models.Auth._Common;
using CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Common;
using CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Roles;
using CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Technicals;
using CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Users;
using CareCare.Core.Application.Abstraction.Models.Auth.ForgetPassword;
using CareCare.Core.Application.Abstraction.Models.Auth.ForgetPasswordByEmailDtos;
using CareCare.Core.Application.Abstraction.Models.Auth.RegisterDtos;
using CareCare.Core.Application.Abstraction.Models.Auth.UpdatingUsersDtos;
using CareCare.Core.Application.Abstraction.Models.Auth.UserDtos;
using CareCare.Core.Application.Abstraction.Services;
using CareCare.Core.Application.Abstraction.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CarCare.Core.Application.Services.Auth
{

	public class AuthService(
		IOptions<JwtSettings> jwtSettings, UserManager<ApplicationUser> userManager,
		SignInManager<ApplicationUser> signInManager
		, RoleManager<IdentityRole> roleManager
		, ISMSServices smsServices
		, IMapper mapper
		, IEmailServices emailServices,
		IUnitOfWork unitOfWork) : IAuthService
	{

		private readonly JwtSettings _jwtSettings = jwtSettings.Value;

		#region Role
		public async Task<RolesToReturn> CreateRoleAsync(RoleDtoBase roleDto)
		{
			var roleExsits = await roleManager.RoleExistsAsync(roleDto.Name);

			if (!roleExsits)
			{
				var result = await roleManager.CreateAsync(new IdentityRole(roleDto.Name.Trim()));
				var role = await roleManager.FindByNameAsync(roleDto.Name);
				var mappedroleresult = new RolesToReturn() { Id = role!.Id, Name = role.Name! };
				return mappedroleresult;
			}
			else
			{
				throw new BadRequestExeption("This Role already Exists");
			}


		}


		public async Task<IEnumerable<RolesToReturn>> GetRolesAsync()
		{

			var roles = await roleManager.Roles.ToListAsync();
			var result = mapper.Map<IEnumerable<RolesToReturn>>(roles);
			return result;

		}


		public async Task<RolesToReturn> UpdateRole(string id, RoleDtoBase roleDto)
		{
			var roleExsists = await roleManager.RoleExistsAsync(roleDto.Name);
			if (!roleExsists)
			{
				var role = await roleManager.FindByIdAsync(id);
				role!.Name = roleDto.Name;
				await roleManager.UpdateAsync(role);
				var result = new RolesToReturn() { Id = role!.Id, Name = role.Name! };
				return result;
			}
			else
			{
				throw new BadRequestExeption("this Role Already is Exsists");
			}
		}

		public async Task DeleteRole(string id)
		{
			var role = await roleManager.FindByIdAsync(id);
			if (role is null)
			{
				throw new NotFoundExeption(nameof(role), id);
			}
			await roleManager.DeleteAsync(role!);
		}

		#endregion


		#region Sign (in - up)

		public async Task<BaseUserDto> LoginAsync(LoginDto loginDto)
		{
			var user = await userManager.Users.Where(u => u.PhoneNumber == loginDto.PhoneNumber).FirstOrDefaultAsync();
			if (user == null)
				throw new UnAuthorizedExeption("Invalid Login");

			var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: true);

			if (!user.EmailConfirmed)
				throw new UnAuthorizedExeption("Email is not Confirmed");

			if (result.IsLockedOut)
				throw new UnAuthorizedExeption("Email is Locked Out");

			if (!result.Succeeded)
				throw new UnAuthorizedExeption("Invalid Login");

			var serviceid = user.ServiceId ?? 0; // Provide a default value (e.g., 0)
			var servicetype = await unitOfWork.GetRepository<ServiceType, int>().GetAsync(serviceid);
			if (user.Type == Types.User)
			{
				var response = new UserDto
				{
					Id = user.Id,
					FullName = user.FullName,
					PhoneNumber = user.PhoneNumber!,
					Email = user.Email!,
					Type = user.Type.ToString(),
					Token = await GenerateTokenAsync(user)
				};
				await CheckRefreshToken(userManager, user, response);

				return response;
			}
			else
			{


				var response = new TechDto
				{
					Id = user.Id,
					FullName = user.FullName!,
					PhoneNumber = user.PhoneNumber!,
					Email = user.Email!,
					NationalId = user.NationalId!,
					Type = user.Type.ToString(),
					ServiceName = servicetype?.Name,
					Token = await GenerateTokenAsync(user)
				};
				await CheckRefreshToken(userManager, user, response);

				return response;
			}
		}


		public async Task<UserDto> RegisterUserAsync(UserRegisterDto userRegisterDto)
		{
			var user = new ApplicationUser
			{
				PhoneNumber = userRegisterDto.PhoneNumber,
				FullName = userRegisterDto.FullName,
				Type = userRegisterDto.Type,
				Email = userRegisterDto.Email,
				UserName = userRegisterDto.Email
			};

			var getphone = await userManager.Users.Where(u => u.PhoneNumber == user.PhoneNumber).FirstOrDefaultAsync();

			if (getphone is not null && getphone.PhoneNumber == userRegisterDto.PhoneNumber)
				throw new BadRequestExeption("Phone is Already Registered");

			var getmail = await userManager.Users.Where(U => U.Email == user.Email).FirstOrDefaultAsync();

			if (getmail is not null && getmail.Email == userRegisterDto.Email)
				throw new BadRequestExeption("Email is Already Registered");


			var result = await userManager.CreateAsync(user, userRegisterDto.Password);


			if (!result.Succeeded)
				throw new ValidationExeption() { Errors = result.Errors.Select(E => E.Description) };

			var email = new ForgetPasswordByEmailDto() { Email = user.Email };
			await ConfirmationCodeSendByEmailAsync(email);


			var roleResult = await userManager.AddToRoleAsync(user, Types.User.ToString());
			if (!roleResult.Succeeded)
				throw new ValidationExeption() { Errors = roleResult.Errors.Select(E => E.Description) };


			var respone = new UserDto
			{
				Id = user.Id,
				FullName = user.FullName,
				PhoneNumber = user.PhoneNumber,
				Email = user.Email,
				Type = user.Type.ToString(),
				Token = await GenerateTokenAsync(user),


			};
			return respone;
		}


		public async Task<TechDto> RegisterTechAsync(TechRegisterDto techRegisterDto)
		{

			var tech = new ApplicationUser
			{
				FullName = techRegisterDto.FullName,
				PhoneNumber = techRegisterDto.PhoneNumber,
				Email = techRegisterDto.Email,
				UserName = techRegisterDto.Email,
				NationalId = techRegisterDto.NationalId,
				Type = techRegisterDto.Type,
				ServiceId = techRegisterDto.ServiceId,
			};

			var nationalid = await userManager.Users.Where(u => u.NationalId == techRegisterDto.NationalId).FirstOrDefaultAsync();
			if (nationalid is not null)
				throw new BadRequestExeption("NationalId is Already Registered");


			var servicetype = await unitOfWork.GetRepository<ServiceType, int>().GetAsync(techRegisterDto.ServiceId);
			if (servicetype is null)
				throw new NotFoundExeption("No Found Service With This Id", nameof(techRegisterDto.ServiceId));

			var getphone = await userManager.Users.Where(u => u.PhoneNumber == tech.PhoneNumber).FirstOrDefaultAsync();

			if (getphone is not null && getphone.PhoneNumber == techRegisterDto.PhoneNumber)
				throw new BadRequestExeption("Phone is Already Registered");

			var getmail = await userManager.Users.Where(U => U.Email == tech.Email).FirstOrDefaultAsync();

			if (getmail is not null && getmail.Email == techRegisterDto.Email)
				throw new BadRequestExeption("Email is Already Registered");

			var result = await userManager.CreateAsync(tech, techRegisterDto.Password);

			if (!result.Succeeded)
				throw new ValidationExeption() { Errors = result.Errors.Select(E => E.Description) };

			var email = new ForgetPasswordByEmailDto() { Email = tech.Email };
			await ConfirmationCodeSendByEmailAsync(email);



			var roleResult = await userManager.AddToRoleAsync(tech, Types.Technical.ToString());
			if (!roleResult.Succeeded)
				throw new ValidationExeption() { Errors = roleResult.Errors.Select(E => E.Description) };






			var response = new TechDto
			{
				Id = tech.Id,
				Email = techRegisterDto.Email,
				FullName = techRegisterDto.FullName,
				PhoneNumber = techRegisterDto.PhoneNumber,
				NationalId = techRegisterDto.NationalId,
				Type = techRegisterDto.Type.ToString(),
				ServiceName = servicetype.Name,
				Token = await GenerateTokenAsync(tech),

			};
			return response;
		}

		public async Task<TechDto> GetTechLocationAsync(ClaimsPrincipal claimsPrincipal, double techLatitude, double techLongitude)
		{
			var techId = claimsPrincipal.FindFirst(ClaimTypes.PrimarySid)!.Value;

			if (techId is null)
				throw new NotFoundExeption(nameof(techId), techId!);

			var tech = await userManager.FindByIdAsync(techId);

			if (tech is null)
				throw new NotFoundExeption(nameof(techId), techId);

			tech.TechLatitude = techLatitude;
			tech.TechLongitude = techLongitude;

			var result = await userManager.UpdateAsync(tech);


			if (!result.Succeeded)
				throw new ValidationExeption() { Errors = result.Errors.Select(E => E.Description) };


			var respone = mapper.Map<TechDto>(tech);

			return respone;


		}



		#endregion


		#region DashBoard

		public async Task<IEnumerable<UserViewModel>> GetAllUsers()
		{

			var users = await userManager.Users.Where(u => u.Type == Types.User)
	.Select(u => new UserViewModel
	{
		Id = u.Id,
		FullName = u.FullName!,
		PhoneNumber = u.PhoneNumber!,
		Email = u.Email!,
		Type = u.Type.ToString()
	})
	.ToListAsync();

			foreach (var user in users)
			{
				// Await the GetRolesAsync call properly here
				user.Roles = await userManager.GetRolesAsync(await userManager.FindByIdAsync(user.Id));
			}

			return users;

		}

		public async Task<UserDto> CreateUser(CreateUserDro createUserDro)
		{
			var user = new ApplicationUser
			{
				FullName = createUserDro.Name,
				PhoneNumber = createUserDro.PhoneNumber,
				Type = createUserDro.Type,
				Email = createUserDro.Email
			};

			var getphone = await userManager.Users
				.Where(u => u.PhoneNumber == user.PhoneNumber)
				.FirstOrDefaultAsync();

			if (getphone is not null && getphone.PhoneNumber == createUserDro.PhoneNumber)
				throw new BadRequestExeption("Phone is Already Registered");

			var email = await userManager.FindByEmailAsync(user.Email);
			if (email is not null) throw new BadRequestExeption($" Email is Already Exsist ,Please Enter Anthor Email!");


			var result = await userManager.CreateAsync(user, createUserDro.Password);

			if (!result.Succeeded)
				throw new ValidationExeption() { Errors = result.Errors.Select(E => E.Description) };



			// Assign the "User" role to the newly created user
			var roleResult = await userManager.AddToRoleAsync(user, Types.User.ToString());
			if (!roleResult.Succeeded)
				throw new ValidationExeption() { Errors = roleResult.Errors.Select(E => E.Description) };

			var refresktoken = GenerateRefreshToken();

			user.RefreshTokens.Add(new RefreshToken()
			{
				Token = refresktoken.Token,
				ExpireOn = refresktoken.ExpireOn
			});

			await userManager.UpdateAsync(user);

			var response = new UserDto
			{
				Id = user.Id,
				PhoneNumber = user.PhoneNumber,
				Email = user.Email!,
				Type = user.Type.ToString(),
				FullName = user.FullName,
				Token = await GenerateTokenAsync(user),
				RefreshToken = refresktoken.Token,
				RefreshTokenExpirationDate = refresktoken.ExpireOn

			};

			return response;
		}

		public async Task<UserRoleViewModel> GetUser(string id)
		{
			var user = await userManager.FindByIdAsync(id);
			if (user is null) throw new NotFoundExeption("User Not Found", nameof(id));
			var allRoles = await roleManager.Roles.ToListAsync();
			var viewModel = new UserRoleViewModel()
			{
				Id = user.Id,
				Name = user.FullName!,
				PhoneNumber = user.PhoneNumber!,
				Email = user.Email!,
				Type = user.Type.ToString(),
				Roles = allRoles.Select(
					r => new RoleDto()
					{
						Id = r.Id,
						Name = r.Name!,
						IsSelected = userManager.IsInRoleAsync(user, r.Name).Result
					}).Where(u => u.IsSelected == true).ToList()
			};

			return viewModel;
		}

		public async Task<string> DeleteUser(string id)
		{
			var user = await userManager.FindByIdAsync(id);

			if (user is null) throw new NotFoundExeption("User Not Found", nameof(id));

			var result = await userManager.DeleteAsync(user);

			if (result.Succeeded)
				return "Delete Successed";
			else
			{
				return "Operation Faild";
			}


		}

		public async Task<UserRoleViewModel> EditeUser(string id, EditDashDto viewModel)
		{


			var user = await userManager.FindByIdAsync(id);
			if (user is null) throw new NotFoundExeption("User Not Found", nameof(id));

			var userRoles = await userManager.GetRolesAsync(user);
			if (userRoles is null) throw new NotFoundExeption("Do Not Roles For This User", nameof(id));



			foreach (var role in viewModel.Roles)
			{
				if (userRoles.Any(r => r == role.Name) && !role.IsSelected)
				{
					await userManager.RemoveFromRoleAsync(user, role.Name);
				}

				if (!userRoles.Any(r => r == role.Name) && role.IsSelected)
				{
					await userManager.AddToRoleAsync(user, role.Name);
				}
			}

			var ExistedRoles = await userManager.GetRolesAsync(user);

			var usertoreturn = new UserRoleViewModel()
			{

				Id = user.Id,
				Name = user.FullName!,
				PhoneNumber = user.PhoneNumber!,
				Email = user.Email!,
				Type = user.Type.ToString(),
				Roles = ExistedRoles

			};

			return usertoreturn;

		}

		public async Task<IEnumerable<TechViewModel>> GetAllTechnicals()
		{
			var techs = await userManager.Users.Where(u => u.Type == Types.Technical).Include(u => u.ServiceType)
   .Select(u => new TechViewModel
   {
	   Id = u.Id,
	   FullName = u.FullName!,
	   PhoneNumber = u.PhoneNumber!,
	   Email = u.Email!,
	   NationalId = u.NationalId!,
	   Type = u.Type.ToString(),
	   ServiceName = u.ServiceType!.Name
   })
   .ToListAsync();

			foreach (var tech in techs)
			{
				// Await the GetRolesAsync call properly here
				tech.Roles = await userManager.GetRolesAsync(await userManager.FindByIdAsync(tech.Id));
			}

			return techs;
		}

		public async Task<TechDto> CreateTech(CreateTechnicalDto createTechnicalDto)
		{
			var user = new ApplicationUser
			{
				FullName = createTechnicalDto.Name,
				PhoneNumber = createTechnicalDto.PhoneNumber,
				Type = createTechnicalDto.Type,
				Email = createTechnicalDto.Email,
				NationalId = createTechnicalDto.NationalId,
			};

			var getphone = await userManager.Users
				.Where(u => u.PhoneNumber == user.PhoneNumber)
				.FirstOrDefaultAsync();

			if (getphone is not null && getphone.PhoneNumber == createTechnicalDto.PhoneNumber)
				throw new UnAuthorizedExeption("Phone is Already Registered");

			var email = await userManager.FindByEmailAsync(user.Email);
			if (email is not null) throw new BadRequestExeption($" Email is Already Exsist ,Please Enter Anthor Email!");

			var result = await userManager.CreateAsync(user, createTechnicalDto.Password);

			if (!result.Succeeded)
				throw new ValidationExeption() { Errors = result.Errors.Select(E => E.Description) };

			// Assign the "User" role to the newly created user
			var roleResult = await userManager.AddToRoleAsync(user, Types.Technical.ToString());
			if (!roleResult.Succeeded)
				throw new ValidationExeption() { Errors = roleResult.Errors.Select(E => E.Description) };

			var refresktoken = GenerateRefreshToken();

			user.RefreshTokens.Add(new RefreshToken()
			{
				Token = refresktoken.Token,
				ExpireOn = refresktoken.ExpireOn
			});

			await userManager.UpdateAsync(user);

			var response = new TechDto
			{
				Id = user.Id,
				PhoneNumber = user.PhoneNumber,
				Type = user.Type.ToString(),
				FullName = user.FullName,
				Email = user.Email,
				NationalId = user.NationalId,
				Token = await GenerateTokenAsync(user),
				RefreshToken = refresktoken.Token,
				RefreshTokenExpirationDate = refresktoken.ExpireOn
			};

			return response;
		}

		public async Task<TechRoleViewModel> GetTechnical(string id)
		{
			var tech = await userManager.FindByIdAsync(id);
			if (tech is null) throw new NotFoundExeption("This Technical Not Found", nameof(id));
			var allRoles = await roleManager.Roles.ToListAsync();
			var viewModel = new TechRoleViewModel()
			{
				Id = tech.Id,
				Name = tech.FullName!,
				PhoneNumber = tech.PhoneNumber!,
				Email = tech.Email!,
				NationalId = tech.NationalId!,
				Type = tech.Type.ToString(),
				ServiceName = tech.ServiceType!.Name,
				Profit = tech.TechProfit,
				Roles = allRoles.Select(
					r => new RoleDto()
					{
						Id = r.Id,
						Name = r.Name!,
						IsSelected = userManager.IsInRoleAsync(tech, r.Name!).Result
					}).Where(u => u.IsSelected == true).ToList()
			};

			return viewModel;
		}

		public async Task<string> DeleteTechnical(string id)
		{

			var tech = await userManager.FindByIdAsync(id);

			if (tech is null) throw new NotFoundExeption("Technical Not Found", nameof(id));

			var result = await userManager.DeleteAsync(tech);

			if (result.Succeeded)
				return "Delete Successed";
			else
				return "Operation Faild";

		}

		public async Task<TechRoleViewModel> EditeTechnical(string id, EditDashDto viewModel)
		{
			var tech = await userManager.FindByIdAsync(id);
			if (tech is null) throw new NotFoundExeption("Technical Not Found", nameof(id));

			var userRoles = await userManager.GetRolesAsync(tech);
			if (userRoles is null) throw new NotFoundExeption("Do Not Roles For This Technical", nameof(id));



			foreach (var role in viewModel.Roles)
			{
				if (userRoles.Any(r => r == role.Name) && !role.IsSelected)
				{
					await userManager.RemoveFromRoleAsync(tech, role.Name);
				}

				if (!userRoles.Any(r => r == role.Name) && role.IsSelected)
				{
					await userManager.AddToRoleAsync(tech, role.Name);
				}
			}

			var ExistedRoles = await userManager.GetRolesAsync(tech);

			var usertoreturn = new TechRoleViewModel()
			{

				Id = tech.Id,
				Name = tech.FullName!,
				PhoneNumber = tech.PhoneNumber!,
				Email = tech.Email!,
				NationalId = tech.NationalId!,
				Type = tech.Type.ToString(),
				Roles = ExistedRoles,

			};

			return usertoreturn;
		}

		public async Task<UserDto> GetCurrentUser(ClaimsPrincipal claims)
		{
			var phone = claims.FindFirstValue(ClaimTypes.MobilePhone);
			if (phone is null) throw new NotFoundExeption("Do Not Exsists This Phone", nameof(phone));

			var user = await userManager.Users.Where(u => u.Type == Types.User && u.PhoneNumber == phone).FirstOrDefaultAsync();
			if (user is null) throw new NotFoundExeption("No User For This Phone Or This Technical Not User ", nameof(phone));

			var mappeduser = mapper.Map<UserDto>(user);
			mappeduser.Token = await GenerateTokenAsync(user);
			await CheckRefreshToken(userManager, user, mappeduser);


			return mappeduser;

		}

		public async Task<TechDto> GetCurrentTechnical(ClaimsPrincipal claims)
		{
			var phone = claims.FindFirstValue(ClaimTypes.MobilePhone);
			if (phone is null) throw new NotFoundExeption("Do Not Exsists This Phone", nameof(phone));

			var Tech = await userManager.Users.Where(u => u.Type == Types.Technical && u.PhoneNumber == phone).Include(u => u.ServiceType).FirstOrDefaultAsync();
			if (Tech is null) throw new NotFoundExeption("No Technical For This Phone Or This User Not Technical ", nameof(phone));

			var mappedTech = mapper.Map<TechDto>(Tech);
			mappedTech.Token = await GenerateTokenAsync(Tech);
			await CheckRefreshToken(userManager, Tech, mappedTech);


			return mappedTech;
		}

		public async Task<UserDto> GetCurrentAdmin(ClaimsPrincipal claims)
		{
			var phone = claims.FindFirstValue(ClaimTypes.MobilePhone);
			if (phone is null) throw new NotFoundExeption("Do Not Exsists This Phone", nameof(phone));

			var Admin = await userManager.Users.Where(u => u.PhoneNumber == phone).FirstOrDefaultAsync();
			if (Admin is null) throw new NotFoundExeption("No Technical For This Phone Or This User Not Technical ", nameof(phone));

			var mappedTech = mapper.Map<UserDto>(Admin);
			mappedTech.Type = Roles.Admin;

			mappedTech.Token = await GenerateTokenAsync(Admin);
			await CheckRefreshToken(userManager, Admin, mappedTech);


			return mappedTech;
		}

		public async Task<UserDto> UpdateUserByUser(ClaimsPrincipal claims, UpdateUserDto userDto)
		{
			var userId = claims.FindFirst(ClaimTypes.PrimarySid)?.Value;

			if (userId is null)
				throw new UnAuthorizedExeption("UnAuthorized , You Are Not Allowed");

			var user = await userManager.FindByIdAsync(userId);

			if (user is null)
				throw new NotFoundExeption("No User For This Id", nameof(userId));

			var getphone = await userManager.Users.Where(u => u.PhoneNumber == userDto.PhoneNumber).FirstOrDefaultAsync();

			if (getphone is not null && getphone.PhoneNumber == userDto.PhoneNumber && userDto.PhoneNumber != user.PhoneNumber)
				throw new BadRequestExeption("Phone is Already Registered");

			var getEmail = await userManager.Users.Where(u => u.Email == userDto.Email).FirstOrDefaultAsync();


			if (getEmail is not null && getEmail.Email == userDto.Email && userDto.Email != user.Email)
				throw new BadRequestExeption("Email is Already Registered");

			user.PhoneNumber = userDto.PhoneNumber;
			user.FullName = userDto.FullName!;
			user.Email = userDto.Email;


			var result = await userManager.UpdateAsync(user);


			if (!result.Succeeded)
				throw new ValidationExeption() { Errors = result.Errors.Select(E => E.Description) };

			var respone = new UserDto
			{
				Id = user.Id,
				FullName = user.FullName!,
				PhoneNumber = user.PhoneNumber!,
				Email = user.Email!,
				Type = user.Type.ToString(),
				Token = await GenerateTokenAsync(user),
			};
			return respone;


		}

		public async Task<TechDto> UpdateTechByTech(ClaimsPrincipal claims, UpdateTechDto techDto)
		{
			var techId = claims.FindFirst(ClaimTypes.PrimarySid)?.Value;

			if (techId is null)
				throw new UnAuthorizedExeption("UnAuthorized , You Are Not Allowed");

			var tech = await userManager.FindByIdAsync(techId);

			if (tech is null)
				throw new NotFoundExeption("No User For This Id", nameof(techId));

			var getphone = await userManager.Users.Where(u => u.PhoneNumber == techDto.PhoneNumber).FirstOrDefaultAsync();

			if (getphone is not null && getphone.PhoneNumber == techDto.PhoneNumber && techDto.PhoneNumber != tech.PhoneNumber)
				throw new BadRequestExeption("Phone is Already Registered");

			var getEmail = await userManager.Users.Where(u => u.Email == techDto.Email).FirstOrDefaultAsync();


			if (getEmail is not null && getEmail.Email == techDto.Email && techDto.Email != tech.Email)
				throw new BadRequestExeption("Email is Already Registered");

			tech.PhoneNumber = techDto.PhoneNumber;
			tech.FullName = techDto.FullName!;
			tech.Email = techDto.Email;
			tech.NationalId = techDto.NationalId;


			var result = await userManager.UpdateAsync(tech);


			if (!result.Succeeded)
				throw new ValidationExeption() { Errors = result.Errors.Select(E => E.Description) };

			var respone = new TechDto
			{
				Id = tech.Id,
				FullName = tech.FullName!,
				PhoneNumber = tech.PhoneNumber!,
				Email = tech.Email!,
				Type = tech.Type.ToString(),
				NationalId = tech.NationalId!,
				Profit = tech.TechProfit,
				Token = await GenerateTokenAsync(tech),
			};
			return respone;


		}


		public async Task<ChangePasswordToReturn> ChangePasswordAsync(ClaimsPrincipal claims, ChangePasswordDto changePasswordDto)
		{




			// Get the logged-in user's ID from the token
			var userId = claims.FindFirst(ClaimTypes.PrimarySid)?.Value;

			if (userId is null) throw new UnAuthorizedExeption("UnAuthorized , You Are Not Allowed");


			// Retrieve the user from the database
			var user = await userManager.FindByIdAsync(userId);

			if (user is null) throw new NotFoundExeption("No User For This Id", nameof(userId));


			// Verify the current password
			var isCurrentPasswordValid = await userManager.CheckPasswordAsync(user, changePasswordDto.CurrentPassword);

			if (!isCurrentPasswordValid)
			{
				throw new BadRequestExeption("This Current Password InCorrect");
			}

			// Change the password
			var result = await userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);

			if (!result.Succeeded)
			{
				throw new ValidationExeption() { Errors = result.Errors.Select(p => p.Description) };
			}

			// Optionally, generate a new token for the user
			var newToken = await GenerateTokenAsync(user);

			return new ChangePasswordToReturn()
			{
				Message = "Password changed successfully.",
				Token = newToken
			};

		}



		private async Task<BaseUserDto> UserReturn(ApplicationUser? user, RefreshToken newrefreshtoken)
		{
			if (user.Type == Types.User)
			{
				var response = new UserDto
				{
					Id = user.Id,
					FullName = user.FullName!,
					PhoneNumber = user.PhoneNumber!,
					Email = user.Email!,
					Type = user.Type.ToString(),
					Token = await GenerateTokenAsync(user),
					RefreshToken = newrefreshtoken.Token,
					RefreshTokenExpirationDate = newrefreshtoken.ExpireOn
				};

				return response;
			}
			else
			{
				var response = new TechDto
				{
					Id = user.Id,
					FullName = user.FullName!,
					Email = user.Email!,
					NationalId = user.NationalId!,
					PhoneNumber = user.PhoneNumber!,
					Type = user.Type.ToString(),
					Token = await GenerateTokenAsync(user),
					RefreshToken = newrefreshtoken.Token,
					RefreshTokenExpirationDate = newrefreshtoken.ExpireOn
				};

				return response;
			}
		}

		#endregion


		#region Token

		private async Task<string> GenerateTokenAsync(ApplicationUser user)
		{
			var userClaims = await userManager.GetClaimsAsync(user);
			var rolesAsClaims = new List<Claim>();

			var roles = await userManager.GetRolesAsync(user);


			foreach (var role in roles)
				rolesAsClaims.Add(new Claim(ClaimTypes.Role, role.ToString()));

			IEnumerable<Claim> claims;

			if (user.Type == Types.Technical)
			{
				claims = new List<Claim>()
				{
				new Claim(ClaimTypes.PrimarySid,user.Id),
				new Claim(ClaimTypes.Email,user.Email!),
				new Claim(ClaimTypes.MobilePhone,user.PhoneNumber!),
				new Claim("Type",user.Type.ToString()),
				new Claim("NationalId",user.NationalId!)
				}
			   .Union(userClaims)
			   .Union(rolesAsClaims);
			}
			else
			{
				claims = new List<Claim>()
				{
				new Claim(ClaimTypes.PrimarySid,user.Id),
				new Claim(ClaimTypes.MobilePhone,user.PhoneNumber!),
				new Claim(ClaimTypes.Email,user.Email!),
				new Claim("Type",user.Type.ToString()),
				}
			   .Union(userClaims)
			   .Union(rolesAsClaims);
			}
			var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
			var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

			var tokenObj = new JwtSecurityToken(

				issuer: _jwtSettings.Issuer,
				audience: _jwtSettings.Audience,
				expires: DateTime.UtcNow.AddDays(_jwtSettings.DurationInDays),
				claims: claims,
				signingCredentials: signingCredentials
				);


			return new JwtSecurityTokenHandler().WriteToken(tokenObj);
		}

		private RefreshToken GenerateRefreshToken()
		{
			var randomNumber = new byte[32];

			var genrator = new RNGCryptoServiceProvider();

			genrator.GetBytes(randomNumber);

			return new RefreshToken()
			{
				Token = Convert.ToBase64String(randomNumber),
				CreatedOn = DateTime.UtcNow,
				ExpireOn = DateTime.UtcNow.AddDays(_jwtSettings.JWTRefreshTokenExpire)


			};


		}
		private async Task CheckRefreshToken(UserManager<ApplicationUser> userManager, ApplicationUser? user, BaseUserDto response)
		{
			if (user!.RefreshTokens.Any(t => t.IsActice))
			{
				var acticetoken = user.RefreshTokens.FirstOrDefault(x => x.IsActice);
				response.RefreshToken = acticetoken!.Token;
				response.RefreshTokenExpirationDate = acticetoken.ExpireOn;
			}
			else
			{

				var refreshtoken = GenerateRefreshToken();
				response.RefreshToken = refreshtoken.Token;
				response.RefreshTokenExpirationDate = refreshtoken.ExpireOn;

				user.RefreshTokens.Add(new RefreshToken()
				{
					Token = refreshtoken.Token,
					ExpireOn = refreshtoken.ExpireOn,
				});
				await userManager.UpdateAsync(user);
			}
		}

		private string? ValidateToken(string token)
		{
			var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

			var TokenHandler = new JwtSecurityTokenHandler();

			try
			{
				TokenHandler.ValidateToken(token, new TokenValidationParameters()
				{
					IssuerSigningKey = authKey,
					ValidateIssuerSigningKey = true,
					ValidateIssuer = false,
					ValidateAudience = false,
					ValidateLifetime = false,
					ClockSkew = TimeSpan.Zero,

				}, out SecurityToken validatedToken);

				var jwtToken = (JwtSecurityToken)validatedToken;

				return jwtToken.Claims.First(x => x.Type == ClaimTypes.PrimarySid).Value;

			}
			catch
			{
				return null;
			}
		}


		public async Task<BaseUserDto> GetRefreshTokenAsync(RefreshDto refreshDto, CancellationToken cancellationToken = default)
		{
			var userId = ValidateToken(refreshDto.Token);

			if (userId is null) throw new NotFoundExeption("User id Not Found", nameof(userId));

			var user = await userManager.FindByIdAsync(userId);
			if (user is null) throw new NotFoundExeption("User Do Not Exists", nameof(user.Id));

			var UserRefreshToken = user!.RefreshTokens.SingleOrDefault(x => x.Token == refreshDto.RefreshToken && x.IsActice);

			if (UserRefreshToken is null) throw new NotFoundExeption("Invalid Token", nameof(userId));

			UserRefreshToken.RevokedOn = DateTime.UtcNow;

			var newtoken = await GenerateTokenAsync(user);

			var newrefreshtoken = GenerateRefreshToken();

			user.RefreshTokens.Add(new RefreshToken()
			{
				Token = newrefreshtoken.Token,
				ExpireOn = newrefreshtoken.ExpireOn
			});

			await userManager.UpdateAsync(user);

			return await UserReturn(user, newrefreshtoken);

		}


		public async Task<bool> RevokeRefreshTokenAsync(RefreshDto refreshDto, CancellationToken cancellationToken = default)
		{
			var userId = ValidateToken(refreshDto.Token);

			if (userId is null) return false;

			var user = await userManager.FindByIdAsync(userId);
			if (user is null) return false;

			var UserRefreshToken = user!.RefreshTokens.SingleOrDefault(x => x.Token == refreshDto.RefreshToken && x.IsActice);

			if (UserRefreshToken is null) return false;

			UserRefreshToken.RevokedOn = DateTime.UtcNow;

			await userManager.UpdateAsync(user);
			return true;
		}


		#endregion


		#region Confirmation

		public async Task<SuccessDto> ForgetPasswordByEmailasync(ForgetPasswordByEmailDto emailDto)
		{
			var user = await userManager.Users.Where(u => u.Email == emailDto.Email).FirstOrDefaultAsync();

			if (user is null)
				throw new BadRequestExeption("Invalid Email");

			var ResetCode = RandomNumberGenerator.GetInt32(100_0, 999_9);

			var ResetCodeExpire = DateTime.UtcNow.AddMinutes(15);

			user.EmailConfirmResetCode = ResetCode;
			user.EmailConfirmResetCodeExpiry = ResetCodeExpire;

			var result = await userManager.UpdateAsync(user);

			if (!result.Succeeded)
				throw new BadRequestExeption("Something Went Wrong While Sending Reset Code");

			var Email = new EmailDto()
			{
				To = emailDto.Email,
				Subject = "Reset Code For CarCare Account",
				Body = $"We Have Recived Your Request For Reset Your Account Password, \nYour Reset Code Is ==> [ {ResetCode} ] <== \nNote: This Code Will Be Expired After 15 Minutes!",
			};

			await emailServices.SendEmail(Email);

			var SuccessObj = new SuccessDto()
			{
				Status = "Success",
				Message = "We Have Sent You The Reset Code"
			};

			return SuccessObj;
		}

		public async Task<SuccessDto> ForgetPasswordByPhoneAsync(ForgetPasswordByPhoneDto forgetPasswordDto)
		{
			var user = await userManager.Users.Where(U => U.PhoneNumber == forgetPasswordDto.PhoneNumber).FirstOrDefaultAsync();
			if (user is null)
				throw new BadRequestExeption("Invalid Phone Number");

			var ResetPhoneCode = RandomNumberGenerator.GetInt32(100_0, 999_9);

			var ResetCodeExpire = DateTime.UtcNow.AddMinutes(15);

			user.PhoneConfirmResetCode = ResetPhoneCode;
			user.PhoneConfirmResetCodeExpiry = ResetCodeExpire;

			var result = await userManager.UpdateAsync(user);

			if (!result.Succeeded)
				throw new BadRequestExeption("Something Went Wrong While Sending Reset Code");

			var SMSMsg = new SMSDto()
			{
				Body = $"Your code is {ResetPhoneCode}. It expires in 15 minutes.",
				PhoneNumber = forgetPasswordDto.PhoneNumber
			};
			await smsServices.SendSMS(SMSMsg);

			var SuccessObj = new SuccessDto()
			{
				Status = "Success",
				Message = "We Have Sent You The Reset Code"
			};
			return SuccessObj;
		}

		public async Task<SuccessDto> VerifyCodeByEmailAsync(ResetCodeConfirmationByEmailDto resetCodeDto)
		{
			var user = await userManager.Users.Where(u => u.Email == resetCodeDto.Email).FirstOrDefaultAsync();

			if (user is null)
				throw new BadRequestExeption("Invalid Email");

			if (user.EmailConfirmResetCode != resetCodeDto.ResetCode)
				throw new BadRequestExeption("The Provided Code Is Invalid");

			if (user.EmailConfirmResetCodeExpiry < DateTime.UtcNow)
				throw new BadRequestExeption("The Provided Code Has Been Expired");

			var SuccessObj = new SuccessDto()
			{
				Status = "Success",
				Message = "Reset Code Is Verified, Please Proceed To Change Your Password"
			};

			return SuccessObj;
		}

		public async Task<SuccessDto> VerifyCodeByPhoneAsync(ResetCodeConfirmationByPhoneDto resetCodeDto)
		{
			var user = await userManager.Users.Where(U => U.PhoneNumber == resetCodeDto.PhoneNumber).FirstOrDefaultAsync();

			if (user is null)
				throw new BadRequestExeption("Invalid Phone Number");

			if (user.PhoneConfirmResetCode != resetCodeDto.ResetCode)
				throw new BadRequestExeption("The Provided Code Is Invalid");

			if (user.PhoneConfirmResetCodeExpiry < DateTime.UtcNow)
				throw new BadRequestExeption("The Provided Code Has Been Expired");

			var SuccessObj = new SuccessDto()
			{
				Status = "Success",
				Message = "Reset Code Is Verified, Please Proceed To Change Your Password"
			};

			return SuccessObj;
		}

		public async Task<UserDto> ResetPasswordByEmailAsync(ResetPasswordByEmailDto resetCodeDto)
		{
			var user = await userManager.Users.Where(u => u.Email == resetCodeDto.Email).FirstOrDefaultAsync();

			if (user is null)
				throw new BadRequestExeption("Invalid Email");

			var RemovePass = await userManager.RemovePasswordAsync(user);

			if (!RemovePass.Succeeded)
				throw new BadRequestExeption("Something Went Wrong While Reseting Your Password");

			var newPass = await userManager.AddPasswordAsync(user, resetCodeDto.NewPassword);

			if (!newPass.Succeeded)
				throw new BadRequestExeption("Something Went Wrong While Reseting Your Password");

			var mappedUser = new UserDto
			{
				FullName = user.FullName!,
				Id = user.Id,
				PhoneNumber = user.PhoneNumber!,
				Email = user.Email!,
				Type = user.Type.ToString(),
				Token = await GenerateTokenAsync(user),
			};

			return mappedUser;
		}


		public async Task<UserDto> ResetPasswordByPhoneAsync(ResetPasswordByPhoneDto resetPasswordDto)
		{
			var user = await userManager.Users.Where(U => U.PhoneNumber == resetPasswordDto.PhoneNumber).FirstOrDefaultAsync();

			if (user is null)
				throw new BadRequestExeption("Invalid Phone Number");

			var removedPassword = await userManager.RemovePasswordAsync(user);

			if (!removedPassword.Succeeded)
				throw new BadRequestExeption("Something Went Wrong While Reseting Your Password");

			var newPassword = await userManager.AddPasswordAsync(user, resetPasswordDto.NewPassword);

			if (!newPassword.Succeeded)
				throw new BadRequestExeption("Something Went Wrong While Reseting Your Password");

			var mappedUser = new UserDto
			{
				FullName = user.FullName!,
				Id = user.Id,
				PhoneNumber = user.PhoneNumber!,
				Email = user.Email!,
				Type = user.Type.ToString(),
				Token = await GenerateTokenAsync(user),
			};

			return mappedUser;
		}


		public async Task<SuccessDto> ConfirmationCodeSendByEmailAsync(ForgetPasswordByEmailDto emailDto)
		{
			var result = await ForgetPasswordByEmailasync(emailDto);

			return result;
		}

		public async Task<SuccessDto> ConfirmationCodeSendByPhoneAsync(ForgetPasswordByPhoneDto phoneDto)
		{
			var result = await ForgetPasswordByPhoneAsync(phoneDto);

			return result;
		}


		public async Task<SuccessDto> ConfirmEmailAsync(ConfirmationEmailCodeDto codeDto)
		{
			var user = await userManager.Users.Where(U => U.Email == codeDto.Email).FirstOrDefaultAsync();

			if (user is null)
				throw new BadRequestExeption("Invalid Email");

			if (user.EmailConfirmResetCode != codeDto.ConfirmationCode)
				throw new BadRequestExeption("The Provided Code Is Invalid");

			if (user.EmailConfirmResetCodeExpiry < DateTime.UtcNow)
				throw new BadRequestExeption("The Provided Code Has Been Expired");

			user.EmailConfirmed = true;

			var result = await userManager.UpdateAsync(user);

			if (!result.Succeeded)
				throw new BadRequestExeption("Something Went Wrong While Confirming Email");

			var SuccessObj = new SuccessDto()
			{
				Status = "Success",
				Message = "Email Has Been Confirmed"
			};

			return SuccessObj;
		}


		public async Task<SuccessDto> ConfirmPhoneAsync(ConfirmationPhoneCodeDto codeDto)
		{
			var user = await userManager.Users.Where(U => U.PhoneNumber == codeDto.PhoneNumber).FirstOrDefaultAsync();
			if (user is null)
				throw new BadRequestExeption("Invalid Phone Number");

			if (user.PhoneConfirmResetCode != codeDto.ConfirmationCode)
				throw new BadRequestExeption("The Provided Code Is Invalid");

			if (user.PhoneConfirmResetCodeExpiry < DateTime.UtcNow)
				throw new BadRequestExeption("The Provided Code Has Been Expired");

			user.PhoneNumberConfirmed = true;

			var result = await userManager.UpdateAsync(user);

			if (!result.Succeeded)
				throw new BadRequestExeption("Something Went Wrong While Confirming Phone Number");

			var SuccessObj = new SuccessDto()
			{
				Status = "Success",
				Message = "Phone Number Has Been Confirmed"
			};

			return SuccessObj;
		}






		#endregion

		public async Task<TechDto> UpdateOrSetTechnicalLocation(UpdateTechnicalLocationDto request)
		{
			if (string.IsNullOrEmpty(request.Id))
				throw new BadRequestExeption("Technical id Is Reuequerd");

			var Tech = await userManager.Users.FirstOrDefaultAsync(t => t.Id == request.Id);
			if (Tech is null)
				throw new NotFoundExeption("No Found Technical With This Id", nameof(request.Id));

			Tech.TechLongitude = request.TechLongitude;
			Tech.TechLatitude = request.TechLatitude;

			var result = await userManager.UpdateAsync(Tech);

			if (!result.Succeeded)
				throw new ValidationExeption() { Errors = result.Errors.Select(E => E.Description) };

			var returnedTech = mapper.Map<TechDto>(Tech);
			return returnedTech;

		}

		public async Task<string> RateTechnical(ClaimsPrincipal claims, decimal rate, string technicalid)
		{
			if (rate < 1 || rate > 5)
				throw new BadRequestExeption("Rate must be between 1 and 5.");


			var userid = claims.FindFirstValue(ClaimTypes.PrimarySid);

			if (userid is null)
				throw new NotFoundExeption("User not found. With This id", nameof(userid));




			var technicalUser = await userManager.FindByIdAsync(technicalid);
			if (technicalUser is null)
			{
				throw new NotFoundExeption("Technical not found. With This id", nameof(technicalid));
			}

			// Update the technical user's rating
			technicalUser.TechRate = (technicalUser.TechRate * technicalUser.Count + rate) / (technicalUser.Count + 1);
			technicalUser.Count += 1;

			// Save changes to the database
			var result = await userManager.UpdateAsync(technicalUser);
			if (!result.Succeeded)
				throw new BadRequestExeption("Failed to update the technical user's rating");


			return "Rating submitted successfully.";
		}

	}

}
