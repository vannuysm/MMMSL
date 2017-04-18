using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using mmmsl.Models;

namespace mmmsl
{
    public static class Auth0Extensions
    {
        private static readonly RandomNumberGenerator CryptoRandom = RandomNumberGenerator.Create();
        private const string CorrelationPrefix = ".AspNetCore.Correlation.";
        private const string CorrelationProperty = ".xsrf";
        private const string CorrelationMarker = "N";
        private const string NonceProperty = "N";

        private static string BuildRedirectUri(HttpRequest request, PathString redirectPath)
        {
            return request.Scheme + "://" + request.Host + request.PathBase + redirectPath;
        }

        private static void GenerateCorrelationId(HttpContext httpContext, OpenIdConnectOptions options, AuthenticationProperties properties)
        {
            if (properties == null) {
                throw new ArgumentNullException(nameof(properties));
            }

            var bytes = new byte[32];
            CryptoRandom.GetBytes(bytes);
            var correlationId = Base64UrlTextEncoder.Encode(bytes);

            var cookieOptions = new CookieOptions {
                HttpOnly = true,
                Secure = httpContext.Request.IsHttps,
                Expires = properties.ExpiresUtc
            };

            properties.Items[CorrelationProperty] = correlationId;

            var cookieName = CorrelationPrefix + options.AuthenticationScheme + "." + correlationId;

            httpContext.Response.Cookies.Append(cookieName, CorrelationMarker, cookieOptions);
        }

        public static LockContext GenerateLockContext(this HttpContext httpContext, OpenIdConnectOptions options, string returnUrl = null)
        {
            var lockContext = new LockContext();
            lockContext.ClientId = options.ClientId;

            Uri authorityUri;
            if (Uri.TryCreate(options.Authority, UriKind.Absolute, out authorityUri)) {
                lockContext.Domain = authorityUri.Host;
            }

            var callbackUrl = BuildRedirectUri(httpContext.Request, options.CallbackPath);
            lockContext.CallbackUrl = callbackUrl;

            var nonce = options.ProtocolValidator.GenerateNonce();
            httpContext.Response.Cookies.Append(
                OpenIdConnectDefaults.CookieNoncePrefix + options.StringDataFormat.Protect(nonce),
                NonceProperty,
                new CookieOptions {
                    HttpOnly = true,
                    Secure = httpContext.Request.IsHttps,
                    Expires = DateTime.UtcNow + options.ProtocolValidator.NonceLifetime
                });
            lockContext.Nonce = nonce;

            var properties = new AuthenticationProperties() {
                ExpiresUtc = options.SystemClock.UtcNow.Add(options.RemoteAuthenticationTimeout),
                RedirectUri = returnUrl ?? "/"
            };
            properties.Items[OpenIdConnectDefaults.RedirectUriForCodePropertiesKey] = callbackUrl;
            GenerateCorrelationId(httpContext, options, properties);

            lockContext.State = Uri.EscapeDataString(options.StateDataFormat.Protect(properties));

            return lockContext;
        }
    }
}