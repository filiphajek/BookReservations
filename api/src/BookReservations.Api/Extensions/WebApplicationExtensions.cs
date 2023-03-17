namespace BookReservations.Api.Extensions
{
    public static class WebApplicationExtensions
    {
#pragma warning disable ASP0014
        public static WebApplication UseSpaDevelopmentServer(this WebApplication app, IConfiguration configuration)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
                app.UseSpa(i => i.UseProxyToSpaDevelopmentServer(configuration.GetSection("SpaDevServer").Value!));
            }
            return app;
#pragma warning restore ASP0014
        }
    }
}
