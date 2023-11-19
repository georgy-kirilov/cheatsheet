using Microsoft.AspNetCore.Http;

namespace Shared.Authentication;

internal sealed class JwtInsideCookieMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        if (!context.Request.Headers.ContainsKey(JwtAuthConstants.Header))
        {
            var cookie = context.Request.Cookies[JwtAuthConstants.Cookie];

            if (cookie is not null)
            {
                context.Request.Headers.Append(JwtAuthConstants.Header, $"Bearer {cookie}");
            }
        }

        await next(context);
    }
}
