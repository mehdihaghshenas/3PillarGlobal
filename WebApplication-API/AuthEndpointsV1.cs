using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication_API.DTO;

namespace WebApplication_API
{
    public static class AuthEndpointsV1
    {
        public static RouteGroupBuilder MapAuthApiV1(this RouteGroupBuilder groupBuilder)
        {
            groupBuilder.MapPost("/createUser", CreateUser);
            groupBuilder.MapPost("/getToken", GetToken).Produces<LoginOutputViewModel>();

            groupBuilder.MapPost("/addSampleData", CreateSampleData).AllowAnonymous();
            return groupBuilder;
        }
        [AllowAnonymous]
        private static async Task<IResult> CreateSampleData(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                await userManager.CreateAsync(new IdentityUser() { UserName = "admin", Email = "admin@test.com" }, "Admin@123");
                await userManager.CreateAsync(new IdentityUser() { UserName = "user", Email = "user@test.com" }, "User@123");
                var adminRole = await roleManager.FindByNameAsync("admin");
                if (adminRole == null)
                {
                    var res = await roleManager.CreateAsync(new IdentityRole { Name = "admin" });
                    if (res.Succeeded)
                    {
                        adminRole = await roleManager.FindByNameAsync("admin");
                    }
                    else
                    {
                        return Results.BadRequest();
                    }
                }
                var claims = await roleManager.GetClaimsAsync(adminRole!);
                if (claims == null || claims.Count == 0)
                {
                    await roleManager.AddClaimAsync(adminRole!, new Claim("permission", "GetInvoiceReport"));
                }
                var admin = await userManager.FindByNameAsync("admin");
                if (admin != null)
                    await userManager.AddToRoleAsync(admin, "admin");
                return Results.Ok();
            }
            else
                return Results.BadRequest("Initial data already created!!!");
        }

        private static async Task<IResult> GetToken(LoginDto user, UserManager<IdentityUser> userMgr, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            var identityUsr = await userMgr.FindByNameAsync(user.UserName);
            if (identityUsr == null)
            {
                return Results.BadRequest();
            }
            else if (await userMgr.CheckPasswordAsync(identityUsr, user.Password))
            {
                var issuer = configuration["Jwt:Issuer"];
                var audience = configuration["Jwt:Audience"];
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(issuer: issuer,
                    audience: audience,
                    signingCredentials: credentials, claims: await ClaimHelper.GetClaimsAsync(identityUsr, userMgr, roleManager), expires: DateTime.UtcNow.AddHours(2));
                var tokenHandler = new JwtSecurityTokenHandler();
                var stringToken = tokenHandler.WriteToken(token);
                return TypedResults.Ok(new LoginOutputViewModel() { UserName = identityUsr.UserName!, Token = stringToken });
            }
            else
            {
                return Results.Unauthorized();
            }
        }

        [AllowAnonymous]
        private static async Task<IResult> CreateUser(UserInputModel user, UserManager<IdentityUser> userMgr)
        {

            var identityUser = new IdentityUser()
            {
                UserName = user.UserName,
                Email = user.Email
            };

            var result = await userMgr.CreateAsync(identityUser, user.Password);

            if (result.Succeeded)
            {
                return Results.Ok("User created successfully");
            }
            else
            {
                return Results.BadRequest(result);
            }
        }
    }
}
