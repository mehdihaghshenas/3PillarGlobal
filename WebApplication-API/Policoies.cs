using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;

namespace WebApplication_API
{
    public static class Policoies
    {
        public const string InvoicePolicy = "InvoicePolicies";
        public static AuthorizationBuilder RegisterPolicies(this AuthorizationBuilder authorizationBuilder)
        {
            return authorizationBuilder.AddPolicy(InvoicePolicy, policy => policy.RequireClaim("permission", "GetInvoiceReport"));
        }
    }
}
