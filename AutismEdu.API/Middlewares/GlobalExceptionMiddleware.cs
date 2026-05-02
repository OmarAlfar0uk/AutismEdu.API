
using System.Net;
using System.Text.Json;
using AutismEdu.API.Exceptions;
using FluentValidation;
namespace AutismEdu.API.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                var statusCode = HttpStatusCode.InternalServerError;
                object errorResponse;
                var logAsError = true;

                switch (ex)
                {
                    case FluentValidation.ValidationException validationEx:
                        statusCode = HttpStatusCode.BadRequest;
                        logAsError = false;
                        errorResponse = new
                        {
                            statusCode = (int)statusCode,
                            message = "Validation failed.",
                            errors = validationEx.Errors
                                .Select(e => new
                                {
                                    Field = e.PropertyName.Replace("RegisterDto.", ""),
                                    e.ErrorMessage
                                })
                        };
                        break;

                    case BadHttpRequestException badReqEx:
                        statusCode = HttpStatusCode.BadRequest;
                        logAsError = false;
                        errorResponse = new
                        {
                            statusCode = (int)statusCode,
                            message = badReqEx.Message
                        };
                        break;

                    case KeyNotFoundException keyNotFoundEx:
                        statusCode = HttpStatusCode.NotFound;
                        logAsError = false;
                        errorResponse = new
                        {
                            statusCode = (int)statusCode,
                            message = keyNotFoundEx.Message
                        };
                        break;

                    case ConflictException conflictEx:
                        statusCode = HttpStatusCode.Conflict;
                        logAsError = false;
                        errorResponse = new
                        {
                            statusCode = (int)statusCode,
                            message = conflictEx.Message,
                            timestamp = DateTime.UtcNow.ToString("o")
                        };
                        break;

                    case ForbiddenException forbiddenEx:
                        statusCode = HttpStatusCode.Forbidden;
                        logAsError = false;
                        errorResponse = new
                        {
                            statusCode = (int)statusCode,
                            message = forbiddenEx.Message,
                            timestamp = DateTime.UtcNow.ToString("o")
                        };
                        break;

                    case UnauthorizedAccessException unauthorizedEx:
                        logAsError = false;
                        // 401 = لم يُسجل دخول المستخدم
                        // 403 = لا يملك صلاحية (Forbidden)
                        if (context.User.Identity?.IsAuthenticated ?? false)
                        {
                            statusCode = HttpStatusCode.Forbidden; // 403
                            errorResponse = new
                            {
                                statusCode = (int)statusCode,
                                message = "You do not have permission to perform this action."
                            };
                        }
                        else
                        {
                            statusCode = HttpStatusCode.Unauthorized; // 401
                            errorResponse = new
                            {
                                statusCode = (int)statusCode,
                                message = string.IsNullOrWhiteSpace(unauthorizedEx.Message)
                                    ? "Unauthorized."
                                    : unauthorizedEx.Message
                            };
                        }
                        break;

                    default:
                        statusCode = HttpStatusCode.InternalServerError;
                        errorResponse = new
                        {
                            statusCode = (int)statusCode,
                            message = "An unexpected error occurred.",
                            details = _env.IsDevelopment() ? ex.Message : null
                        };
                        break;
                }

                if (logAsError)
                    _logger.LogError(ex, "Unhandled exception occurred");
                else
                    _logger.LogWarning(ex, "Handled exception occurred");

                context.Response.StatusCode = (int)statusCode;
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(errorResponse, options);

                await context.Response.WriteAsync(json);
            }
        }
    }
}
