
using AutoMapper;
using CarCare.Core.Application.Services.SMS;
using CarCare.Core.Domain.Entities.Identity;
using CarCare.Shared.ErrorModoule.Exeptions;
using CarCare.Shared.Models.Roles;
using CareCare.Core.Application.Abstraction.Models.Auth;
using CareCare.Core.Application.Abstraction.Models.Auth._Common;
using CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Common;
using CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Roles;
using CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Technicals;
using CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Users;
using CareCare.Core.Application.Abstraction.Models.Auth.ForgetPassword;
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

namespace CarCare.Core.Application.Services
{

	public class AuthService(
		IOptions<JwtSettings> jwtSettings, UserManager<ApplicationUser> userManager,
		SignInManager<ApplicationUser> signInManager
		, RoleManager<IdentityRole> roleManager
		, ISMSServices smsServices
		, IMapper mapper) : IAuthService
	{

		private readonly JwtSettings _jwtSettings = jwtSettings.Value;

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
				role.Name = roleDto.Name;
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



		public async Task<BaseUserDto> LoginAsync(LoginDto loginDto)
		{
			var user = await userManager.Users.Where(u => u.PhoneNumber == loginDto.PhoneNumber).FirstOrDefaultAsync();
			if (user == null)
				throw new UnAuthorizedExeption("Invalid Login");

			var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: true);

			//if (result.IsNotAllowed)
			//	throw new UnAuthorizedExeption("Email is not Confirmed");

			if (result.IsLockedOut)
				throw new UnAuthorizedExeption("Email is Locked Out");

			if (!result.Succeeded)
				throw new UnAuthorizedExeption("Invalid Login");


			if (user.Type == Types.User)
			{
				var response = new UserDto
				{
					Id = user.Id,
					UserName = user.UserName,
					PhoneNumber = user.PhoneNumber!,
					Type = user.Type.ToString(),
					Token = await GenerateTokenAsync(user)
				};
				await CheckRefreskToken(userManager, user, response);

				return response;
			}
			else
			{
				var response = new TechDto
				{
					Id = user.Id,
					UserName = user.UserName!,
					Email = user.Email!,
					NationalId = user.NationalId!,
					PhoneNumber = user.PhoneNumber!,
					Type = user.Type.ToString(),
					Token = await GenerateTokenAsync(user)
				};
				await CheckRefreskToken(userManager, user, response);

				return response;
			}
		}



		public async Task<UserDto> RegisterUserAsync(UserRegisterDto userRegisterDto)
		{
			var user = new ApplicationUser
			{
				PhoneNumber = userRegisterDto.PhoneNumber,
				UserName = userRegisterDto.UserName,
				Type = userRegisterDto.Type,
			};

			var getphone = await userManager.Users.Where(u => u.PhoneNumber == user.PhoneNumber).FirstOrDefaultAsync();

			if (getphone is not null && getphone.PhoneNumber == (userRegisterDto.PhoneNumber))
				throw new UnAuthorizedExeption("Phone is Already Registered");


			var result = await userManager.CreateAsync(user, userRegisterDto.Password);


			if (!result.Succeeded)
				throw new ValidationExeption() { Errors = result.Errors.Select(E => E.Description) };

			var respone = new UserDto
			{
				Id = user.Id,
				UserName = user.UserName,
				PhoneNumber = user.PhoneNumber,
				Type = user.Type.ToString(),
				Token = await GenerateTokenAsync(user)
			};
			return respone;
		}

		public async Task<TechDto> RegisterTechAsync(TechRegisterDto techRegisterDto)
		{
			var tech = new ApplicationUser
			{
				UserName = techRegisterDto.UserName,
				PhoneNumber = techRegisterDto.PhoneNumber,
				Email = techRegisterDto.Email,
				NationalId = techRegisterDto.NationalId,
				Type = techRegisterDto.Type,
			};

			var getphone = await userManager.Users.Where(u => u.PhoneNumber == tech.PhoneNumber).FirstOrDefaultAsync();

			if (getphone is not null && getphone.PhoneNumber == (techRegisterDto.PhoneNumber))
				throw new UnAuthorizedExeption("Phone is Already Registered");

			var result = await userManager.CreateAsync(tech, techRegisterDto.Password);

			if (!result.Succeeded)
				throw new ValidationExeption() { Errors = result.Errors.Select(E => E.Description) };

			var response = new TechDto
			{
				Id = tech.Id,
				Email = techRegisterDto.Email,
				UserName = techRegisterDto.UserName,
				PhoneNumber = techRegisterDto.PhoneNumber,
				NationalId = techRegisterDto.NationalId,
				Type = techRegisterDto.Type.ToString(),
				Token = await GenerateTokenAsync(tech)
			};
			return response;
		}

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
				expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
				claims: claims,
				signingCredentials: signingCredentials
				);


			return new JwtSecurityTokenHandler().WriteToken(tokenObj);
		}

		public async Task<IEnumerable<UserViewModel>> GetAllUsers()
		{

			var users = await userManager.Users.Where(u => u.Type == Types.User)
	.Select(u => new UserViewModel
	{
		Id = u.Id,
		UserName = u.UserName!,
		PhoneNumber = u.PhoneNumber!,
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
				UserName = createUserDro.Name,
				PhoneNumber = createUserDro.PhoneNumber,
				Type = createUserDro.Type,
			};

			var getphone = await userManager.Users
				.Where(u => u.PhoneNumber == user.PhoneNumber)
				.FirstOrDefaultAsync();

			if (getphone is not null && getphone.PhoneNumber == createUserDro.PhoneNumber)
				throw new UnAuthorizedExeption("Phone is Already Registered");

			var result = await userManager.CreateAsync(user, createUserDro.Password);

			if (!result.Succeeded)
				throw new ValidationExeption() { Errors = result.Errors.Select(E => E.Description) };

			// Assign the "User" role to the newly created user
			var roleResult = await userManager.AddToRoleAsync(user, Types.User.ToString());
			if (!roleResult.Succeeded)
				throw new ValidationExeption() { Errors = roleResult.Errors.Select(E => E.Description) };

			var response = new UserDto
			{
				Id = user.Id,
				PhoneNumber = user.PhoneNumber,
				Type = user.Type.ToString(),
				UserName = user.UserName,
				Token = await GenerateTokenAsync(user)
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
				Name = user.UserName!,
				PhoneNumber = user.PhoneNumber!,
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
				Name = user.UserName!,
				PhoneNumber = user.PhoneNumber!,
				Type = user.Type.ToString(),
				Roles = ExistedRoles

			};

			return usertoreturn;

		}

		public async Task<IEnumerable<TechViewModel>> GetAllTechnicals()
		{
			var techs = await userManager.Users.Where(u => u.Type == Types.Technical)
   .Select(u => new TechViewModel
   {
	   Id = u.Id,
	   UserName = u.UserName!,
	   PhoneNumber = u.PhoneNumber!,
	   Email = u.Email!,
	   NationalId = u.NationalId!,
	   Type = u.Type.ToString()
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
				UserName = createTechnicalDto.Name,
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

			var result = await userManager.CreateAsync(user, createTechnicalDto.Password);

			if (!result.Succeeded)
				throw new ValidationExeption() { Errors = result.Errors.Select(E => E.Description) };

			// Assign the "User" role to the newly created user
			var roleResult = await userManager.AddToRoleAsync(user, Types.Technical.ToString());
			if (!roleResult.Succeeded)
				throw new ValidationExeption() { Errors = roleResult.Errors.Select(E => E.Description) };

			var response = new TechDto
			{
				Id = user.Id,
				PhoneNumber = user.PhoneNumber,
				Type = user.Type.ToString(),
				UserName = user.UserName,
				Email = user.Email,
				NationalId = user.NationalId,
				Token = await GenerateTokenAsync(user)
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
				Name = tech.UserName!,
				PhoneNumber = tech.PhoneNumber!,
				Email = tech.Email!,
				NationalId = tech.NationalId!,
				Type = tech.Type.ToString(),
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
				Name = tech.UserName!,
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

			return mappeduser;

		}

		public async Task<TechDto> GetCurrentTechnical(ClaimsPrincipal claims)
		{
			var phone = claims.FindFirstValue(ClaimTypes.MobilePhone);
			if (phone is null) throw new NotFoundExeption("Do Not Exsists This Phone", nameof(phone));

			var Tech = await userManager.Users.Where(u => u.Type == Types.Technical && u.PhoneNumber == phone).FirstOrDefaultAsync();
			if (Tech is null) throw new NotFoundExeption("No Technical For This Phone Or This User Not Technical ", nameof(phone));

			var mappedTech = mapper.Map<TechDto>(Tech);
			mappedTech.Token = await GenerateTokenAsync(Tech);

			return mappedTech;
		}

		public async Task<UserDto> GetCurrentAdmin(ClaimsPrincipal claims)
		{
			var phone = claims.FindFirstValue(ClaimTypes.MobilePhone);
			if (phone is null) throw new NotFoundExeption("Do Not Exsists This Phone", nameof(phone));

			var Admin = await userManager.Users.Where(u => u.PhoneNumber == phone).FirstOrDefaultAsync();
			if (Admin is null) throw new NotFoundExeption("No Technical For This Phone Or This User Not Technical ", nameof(phone));

			var mappedTech = mapper.Map<UserDto>(Admin);
			mappedTech.Token = await GenerateTokenAsync(Admin);
			mappedTech.Type = Roles.Admin;

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

			if (getphone is not null && getphone.PhoneNumber == (userDto.PhoneNumber) && userDto.PhoneNumber != user.PhoneNumber)
				throw new UnAuthorizedExeption("Phone is Already Registered");


			user.PhoneNumber = userDto.PhoneNumber;
			user.UserName = userDto.UserName!;
			user.Address = userDto.Address;


			var result = await userManager.UpdateAsync(user);


			if (!result.Succeeded)
				throw new ValidationExeption() { Errors = result.Errors.Select(E => E.Description) };

			var respone = new UserDto
			{
				Id = user.Id,
				UserName = user.UserName!,
				PhoneNumber = user.PhoneNumber!,
				Type = user.Type.ToString(),
				Token = await GenerateTokenAsync(user),
			};
			return respone;


		}

		public async Task<UserDto> UpdateTechByTech(ClaimsPrincipal claims, UpdateTechDto techDto)
		{
			var techId = claims.FindFirst(ClaimTypes.PrimarySid)?.Value;

			if (techId is null)
				throw new UnAuthorizedExeption("UnAuthorized , You Are Not Allowed");

			var user = await userManager.FindByIdAsync(techId);

			if (user is null)
				throw new NotFoundExeption("No User For This Id", nameof(techId));

			var getphone = await userManager.Users.Where(u => u.PhoneNumber == techDto.PhoneNumber).FirstOrDefaultAsync();

			if (getphone is not null && getphone.PhoneNumber == (techDto.PhoneNumber) && techDto.PhoneNumber != user.PhoneNumber)
				throw new UnAuthorizedExeption("Phone is Already Registered");


			user.PhoneNumber = techDto.PhoneNumber;
			user.UserName = techDto.UserName!;
			user.Address = techDto.Address;
			user.NationalId = techDto.NationalId;


			var result = await userManager.UpdateAsync(user);


			if (!result.Succeeded)
				throw new ValidationExeption() { Errors = result.Errors.Select(E => E.Description) };

			var respone = new UserDto
			{
				Id = user.Id,
				UserName = user.UserName!,
				PhoneNumber = user.PhoneNumber!,
				Type = user.Type.ToString(),
				Token = await GenerateTokenAsync(user),
			};
			return respone;


		}


		public async Task<ChangePasswordToReturn> ChangePasswordAsynce(ClaimsPrincipal claims, ChangePasswordDto changePasswordDto)
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
		private async Task CheckRefreskToken(UserManager<ApplicationUser> userManager, ApplicationUser? user, BaseUserDto response)
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

		private async Task<BaseUserDto> UserReturn(ApplicationUser? user, RefreshToken newrefreshtoken)
		{
			if (user.Type == Types.User)
			{
				var response = new UserDto
				{
					Id = user.Id,
					UserName = user.UserName!,
					PhoneNumber = user.PhoneNumber!,
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
					UserName = user.UserName!,
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


		public async Task<SuccessDto> ForgetPasswordAsync(ForgetPasswordDto forgetPasswordDto)
		{
			var user = await userManager.Users.Where(U => U.PhoneNumber == forgetPasswordDto.PhoneNumber).FirstOrDefaultAsync();
			if (user is null)
				throw new UnAuthorizedExeption("Invalid Phone Number");

			var ResetPhoneCode = RandomNumberGenerator.GetInt32(100_000, 999_999);

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

		public async Task<SuccessDto> VerifyCodeAsync(ResetCodeConfirmationDto resetCode)
		{
			var user = await userManager.Users.Where(U => U.PhoneNumber == resetCode.PhoneNumber).FirstOrDefaultAsync();

			if (user is null)
				throw new UnAuthorizedExeption("Invalid Phone Number");

			if (user.PhoneConfirmResetCode != resetCode.ResetCode)
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

		public async Task<UserDto> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
		{
			var user = await userManager.Users.Where(U => U.PhoneNumber == resetPasswordDto.PhoneNumber).FirstOrDefaultAsync();

			if (user is null)
				throw new UnAuthorizedExeption("Invalid Phone Number");

			var removedPassword = await userManager.RemovePasswordAsync(user);

			if (!removedPassword.Succeeded)
				throw new BadRequestExeption("Something Went Wrong While Reseting Your Password");

			var newPassword = await userManager.AddPasswordAsync(user, resetPasswordDto.NewPassword);

			if (!newPassword.Succeeded)
				throw new BadRequestExeption("Something Went Wrong While Reseting Your Password");

			var mappedUser = new UserDto
			{
				UserName = user.UserName!,
				Id = user.Id,
				PhoneNumber = user.PhoneNumber!,
				Type = user.Type.ToString(),
				Token = await GenerateTokenAsync(user),
			};

			return mappedUser;
		}


		public async Task<SuccessDto> ConfirmationCodeSendAsync(ForgetPasswordDto confirmationCodeDto)
		{
			var result = await ForgetPasswordAsync(confirmationCodeDto);

			return result;
		}


		public async Task<SuccessDto> ConfirmPhoneAsync(ConfirmationPhoneCodeDto codeDto)
		{
			var user = await userManager.Users.Where(U => U.PhoneNumber == codeDto.PhoneNumber).FirstOrDefaultAsync();
			if (user is null)
				throw new UnAuthorizedExeption("Invalid Phone Number");

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

	}
}
