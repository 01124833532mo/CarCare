
using AutoMapper;
using CarCare.Core.Domain.Entities.Identity;
using CarCare.Shared.ErrorModoule.Exeptions;
using CareCare.Core.Application.Abstraction.Models.Auth;
using CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Roles;
using CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Users;
using CareCare.Core.Application.Abstraction.Models.Auth.RegisterDtos;
using CareCare.Core.Application.Abstraction.Models.Auth.UserDtos;
using CareCare.Core.Application.Abstraction.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CarCare.Core.Application.Services
{

    public class AuthService(IOptions<JwtSettings> jwtSettings, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IMapper mapper) : IAuthService
    {

        private readonly JwtSettings _jwtSettings = jwtSettings.Value;

        public async Task CreateRoleAsync(RoleDto roleDto)
        {
            var roleExsits = await roleManager.RoleExistsAsync(roleDto.Name);

            if (!roleExsits)
            {
                var result = await roleManager.CreateAsync(new IdentityRole(roleDto.Name.Trim()));

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


        public async Task UpdateRole(string id, RoleDto roleDto)
        {
            var roleExsists = await roleManager.RoleExistsAsync(roleDto.Name);
            if (!roleExsists)
            {
                var role = await roleManager.FindByIdAsync(id);
                role.Name = roleDto.Name;
                await roleManager.UpdateAsync(role);
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
                return response;
            }
            else
            {
                var response = new TechDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email!,
                    NationalId = user.NationalId!,
                    PhoneNumber = user.PhoneNumber!,
                    Type = user.Type.ToString(),
                    Token = await GenerateTokenAsync(user)
                };
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
        Name = u.UserName!,
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
                Name = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Type = user.Type.ToString(),
                Roles = allRoles.Select(
                    r => new RoleDto()
                    {
                        Id = r.Id,
                        Name = r.Name,
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

        public async Task<UserRoleViewModel> EditeUser(string id, UserEditeDto viewModel)
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
                Name = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Type = user.Type.ToString(),
                RolesToReturn = ExistedRoles

            };

            return usertoreturn;

        }
    }
}
