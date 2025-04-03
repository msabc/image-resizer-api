using ImageResizer.Api.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace ImageResizer.Api.OpenAPI
{
    internal sealed class BearerSecuritySchemeTransformer(IAuthenticationSchemeProvider authenticationSchemeProvider) : IOpenApiDocumentTransformer
    {
        public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
        {
            var authenticationSchemes = await authenticationSchemeProvider.GetAllSchemesAsync();
            if (authenticationSchemes.Any(authScheme => authScheme.Name == Authentication.BearerAuthenticationSchemeName))
            {
                var requirements = new Dictionary<string, OpenApiSecurityScheme>
                {
                    [Authentication.BearerAuthenticationSchemeName] = new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = Authentication.BearerAuthenticationSchemeName,
                        In = ParameterLocation.Header,
                        BearerFormat = "JWT (JSON Web Token)"
                    }
                };

                document.Components ??= new OpenApiComponents();
                document.Components.SecuritySchemes = requirements;
            }
        }
    }
}
