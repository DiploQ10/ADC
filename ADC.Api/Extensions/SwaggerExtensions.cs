namespace ADC.Api.Extensions;

public static class SwaggerExtensions
{
    public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "ADC API V1");
            c.RoutePrefix = "swagger";
        });
        return app;
    }
}
