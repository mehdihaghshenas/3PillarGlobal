using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication_API.Domains;
using WebApplication_API.DTO;
using WebApplication_API.Services;

namespace WebApplication_API
{
    public static class ApiEndpoints
    {
        public static RouteGroupBuilder MapApiEndpoints(this RouteGroupBuilder routeBuilder)
        {
            routeBuilder.MapPost("/Create", Create).WithName("CreateInvoice").Produces<Invoice>().RequireAuthorization();
            routeBuilder.MapGet("/ListAllInvoice", (IInvoiceServices service) =>
            {
                return service.GetAllWithDetailsAsync();
            }).WithName("GetAllInvoices").WithOpenApi();

            routeBuilder.MapGet("/Dapper/ListAllInvoice", (IInvoiceReportService service) =>
            {
                return service.GetAllFactorsAsync();
            }).WithName("GetAllInvoicesDapper").WithOpenApi().RequireAuthorization(Policoies.InvoicePolicy);

            routeBuilder.MapPost("/Dapper/Create", (IInvoiceReportService service, [FromBody] Invoice invoice) =>
            {
                return service.InsertInvoiceOnlyAsync(invoice);
            }).WithName("CreateInvoicesDapper").WithOpenApi().RequireAuthorization(Policoies.InvoicePolicy);

            return routeBuilder;
        }
        public static async Task<IResult> Create([FromBody] InvoiceModel invoice, IInvoiceServices service)
        {
            try
            {
                var i = await service.AddInvoiceAsync(invoice);
                return Results.Ok(i);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }

    }
}
