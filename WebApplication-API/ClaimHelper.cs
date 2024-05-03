using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebApplication_API
{
    public class ClaimHelper
    {


        private static async Task<IList<Claim>> _GetRoleClaimsAsync(IList<string> roles, RoleManager<IdentityRole> roleManager)
        {
            List<Claim> roleClaims = new();
            foreach (var r in roles)
            {
                var role = await roleManager.FindByNameAsync(r);
                if (role != null)
                    roleClaims.AddRange(await roleManager.GetClaimsAsync(role));
            }
            return roleClaims;
        }
        public static async Task<List<Claim>> GetClaimsAsync(IdentityUser user, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var claims = new List<Claim> { };

            var roles = (await userManager.GetRolesAsync(user)).Select(x => x.ToLower()).ToList();
            var userRoles = roles.Select(r => new Claim(ClaimTypes.Role, r)).ToArray();
            var userClaims = await userManager.GetClaimsAsync(user).ConfigureAwait(false);
            var roleClaims = await _GetRoleClaimsAsync(roles, roleManager).ConfigureAwait(false);
            claims.AddRange(userClaims);
            claims.AddRange(roleClaims);
            claims.AddRange(userRoles);
            var securityStampClaimType = new ClaimsIdentityOptions().SecurityStampClaimType;
            claims.AddRange(
            [
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim("fullName", user.UserName!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(securityStampClaimType, user.SecurityStamp!)]);
            return claims;
        }
    }
}
