using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace CommonLibrary.CmnSwagger.Extension;

public static class CmnSwaggerBuilderExtensions
{
    /// <summary>使用通用Swagger</summary>
    public static IApplicationBuilder UseCmnSwagger(this IApplicationBuilder app, string projectName)
    {
        //app.UseStaticFiles(new StaticFileOptions()
        //{
        //    RequestPath = "/swagger/v1",
        //    FileProvider = new PhysicalFileProvider(Path.Combine(AppContext.BaseDirectory, @"swagger/v1")),
        //});

        app.UseSwagger(c =>
        {
            c.PreSerializeFilters.Add((doc, httpReq) =>
            {
                var xForwardedProto = httpReq.Headers["X-Forwarded-Proto"].ToString();
                var cForwardedProto = httpReq.Headers["CloudFront-Forwarded-Proto"].ToString();

                var sheme =
                    string.IsNullOrWhiteSpace(xForwardedProto) == false
                        ? xForwardedProto
                        : string.IsNullOrWhiteSpace(cForwardedProto) == false
                            ? cForwardedProto
                            : httpReq.Scheme;

                doc.Servers = new List<OpenApiServer>()
                {
                    new OpenApiServer()
                    {
                        Url = $"{sheme}://{httpReq.Host.Value}{httpReq.Headers["X-Forwarded-Prefix"]}",
                    }
                };
            });
        });

        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("v1/swagger.json", $"{projectName} v1");

            //Show more of the model by default
            c.DefaultModelExpandDepth(0);

            //Close all of the major nodes
            c.DocExpansion(DocExpansion.None);

            //Show the example by default
            c.DefaultModelRendering(ModelRendering.Example);

            ////Turn on Try it by default
            //c.EnableTryItOutByDefault();

            //Performance Requirement - sorry. Highlighting kills javascript rendering on big json
            //Turns off syntax highlight which causing performance issues...
            c.ConfigObject.AdditionalItems.Add("syntaxHighlight", false);

            //Reverts Swagger UI 2.x  theme which is simpler not much performance benefit...
            c.ConfigObject.AdditionalItems.Add("theme", "agate");
        });

        return app;
    }


}
