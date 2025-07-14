using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using BaseCleanArchitecture.Domain.Resources;

namespace BaseCleanArchitecture.API.Middlewares
{
    public class SecurityMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SecurityMiddleware> _logger;

        public SecurityMiddleware(RequestDelegate next, ILogger<SecurityMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Permitir acceso completo al endpoint de login
                if (context.Request.Path.StartsWithSegments("/api/Auth/login"))
                {
                    await _next(context);
                    return;
                }

                // Validar headers de seguridad
                if (!ValidateSecurityHeaders(context))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await context.Response.WriteAsJsonAsync(new { error = Messages.DatosInvalidos });
                    return;
                }

                // Validar origen de la petición
                if (!ValidateOrigin(context))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    await context.Response.WriteAsJsonAsync(new { error = Messages.NoAutorizado });
                    return;
                }

                // Validar método HTTP
                if (!ValidateHttpMethod(context))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                    await context.Response.WriteAsJsonAsync(new { error = Messages.OperacionNoPermitida });
                    return;
                }

                // Validar tamaño de la petición
                if (!ValidateRequestSize(context))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.RequestEntityTooLarge;
                    await context.Response.WriteAsJsonAsync(new { error = Messages.DatosInvalidos });
                    return;
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el middleware de seguridad");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsJsonAsync(new { error = Messages.ErrorInternoServidor });
            }
        }

        private bool ValidateSecurityHeaders(HttpContext context)
        {
            // Validar Content-Type para POST/PUT
            if (context.Request.Method == "POST" || context.Request.Method == "PUT")
            {
                if (!context.Request.Headers.ContainsKey("Content-Type") ||
                    !context.Request.Headers["Content-Type"].ToString().Contains("application/json"))
                {
                    _logger.LogWarning("Content-Type inválido en la petición");
                    return false;
                }
            }

            return true;
        }

        private bool ValidateOrigin(HttpContext context)
        {
            // Validar origen de la petición
            var origin = context.Request.Headers["Origin"].ToString();
            var allowedOrigins = new[] { 
                "http://localhost:3000", 
                "http://localhost:8000",
                "http://localhost:8080",
                "http://localhost:4200",
                "http://localhost:5173",
                "http://127.0.0.1:3000",
                "http://127.0.0.1:8000",
                "http://127.0.0.1:8080",
                "http://127.0.0.1:4200",
                "http://127.0.0.1:5173",
                "https://iespro.com",
                "https://www.iespro.com",
                "https://api.iespro.com",
                "https://app.iespro.com"
            };

            // Si no hay origen (como en Swagger UI), permitir la petición
            if (string.IsNullOrEmpty(origin))
                return true;

            if (!allowedOrigins.Contains(origin))
            {
                _logger.LogWarning("Origen no permitido: {Origin}", origin);
                return false;
            }

            return true;
        }

        private bool ValidateHttpMethod(HttpContext context)
        {
            // Validar métodos HTTP permitidos
            var allowedMethods = new[] { "GET", "POST", "PUT", "DELETE" };
            return allowedMethods.Contains(context.Request.Method);
        }

        private bool ValidateRequestSize(HttpContext context)
        {
            // Limitar tamaño de petición a 50MB
            const int maxRequestSize = 50 * 1024 * 1024; // 50MB
            
            _logger.LogInformation($"Tamaño de la petición: {context.Request.ContentLength ?? 0} bytes");
            
            if (context.Request.ContentLength > maxRequestSize)
            {
                _logger.LogWarning($"Petición demasiado grande: {context.Request.ContentLength} bytes");
                return false;
            }

            return true;
        }
    }
} 