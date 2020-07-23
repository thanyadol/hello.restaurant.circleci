
//using LoggerService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;

using System.Net;

using System.Threading.Tasks;


//log
using Serilog;
using System.IO;
using cvx.lct.vot.api.Extensions;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Http.Internal;
using cvx.lct.vot.api.Environment;
using Newtonsoft.Json;
using cvx.lct.vot.api.Exceptions;
using System.Linq;
using cvx.lct.vot.api.Models;
using System.Security.Principal;
using Chevron.Identity.AspNet.Client;
using cvx.lct.vot.api.APIs.Exceptions;

namespace cvx.lct.vot.api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerFactory _loggerFactory;
        //  private readonly IPathFinderService _pathFinder;
        private readonly Serilog.ILogger Logg;
        private static TelemetryClient _telemetry;

        public ExceptionMiddleware(RequestDelegate next, ILoggerFactory logger)//,
        {
            _loggerFactory = logger;
            _next = next;

            Logg = new LoggerConfiguration()
                .MinimumLevel.Error()
                //.WriteTo.RollingFile($@"{path}/exception.txt", retainedFileCountLimit: 7)
                .WriteTo.Console()
                // .WriteTo.Sink(new EntityLogEventSink())
                .CreateLogger();

            if (_telemetry == null)
            {
                _telemetry = new TelemetryClient();
            }

        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }

            catch (PlanNotValidException ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
            catch (UserNotAuthenException ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
            catch (UserNotAuthorizeException ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
            catch (TravelNotValidException ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
            catch (ClientNotSuccessException ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }

        }


        private Task HandleExceptionAsync(HttpContext context, PlanNotValidException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            // this.TraceAsync(context, exception);

            return context.Response.WriteAsync(new ErrorDetails()
            {
                RequestId = Guid.Parse(context.Items["id"].ToString()),
                StatusCode = context.Response.StatusCode,
                Message = exception.Message,
                Type = exception.GetType().ToString(),
                Verb = "HTTP" + context.Request.Method,
                Endpoint = context.Request.Path,
                Source = exception.Source,

                ValidateErrors = exception.ValidateErrors,

                HelpLink = exception.HelpLink,
                StackTrace = exception.StackTrace,

            }.ToString());

        }


        private Task HandleExceptionAsync(HttpContext context, TravelNotValidException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            //this.TraceAsync(context, exception);

            return context.Response.WriteAsync(new ErrorDetails()
            {
                RequestId = Guid.Parse(context.Items["id"].ToString()),
                StatusCode = context.Response.StatusCode,
                Message = exception.Message,
                Type = exception.GetType().ToString(),
                Verb = "HTTP" + context.Request.Method,
                Endpoint = context.Request.Path,
                Source = exception.Source,

                NonPendingItems = exception.NonPendingItems,
                ValidateErrors = exception.ValidateErrors,

                HelpLink = exception.HelpLink,
                StackTrace = exception.StackTrace,

            }.ToString());

        }


        private Task HandleExceptionAsync(HttpContext context, UserNotAuthenException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

            this.TraceAsync(context, exception);
            this.TraceAsync(context, exception, true);

            return context.Response.WriteAsync(new ErrorDetails()
            {
                RequestId = Guid.Parse(context.Items["id"].ToString()),
                StatusCode = context.Response.StatusCode,
                Message = exception.Message,
                Type = exception.GetType().ToString(),
                Verb = "HTTP" + context.Request.Method,
                Endpoint = context.Request.Path,
                Source = exception.Source,

                HelpLink = exception.HelpLink,
                StackTrace = exception.StackTrace,

            }.ToString());

        }

        private Task HandleExceptionAsync(HttpContext context, UserNotAuthorizeException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

            this.TraceAsync(context, exception);
            this.TraceAsync(context, exception, true);

            return context.Response.WriteAsync(new ErrorDetails()
            {
                RequestId = Guid.Parse(context.Items["id"].ToString()),
                StatusCode = context.Response.StatusCode,
                Message = exception.Message,
                Type = exception.GetType().ToString(),
                Verb = "HTTP" + context.Request.Method,
                Endpoint = context.Request.Path,
                Source = exception.Source,

                HelpLink = exception.HelpLink,
                StackTrace = exception.StackTrace,

            }.ToString());

        }


        private Task HandleExceptionAsync(HttpContext context, ClientNotSuccessException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception.StatusCode;

            this.TraceAsync(context, exception);

            return context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = exception.StatusCode,
                Verb = exception.Verb,
                Endpoint = exception.Endpoint,
                Message = exception.Message,
                Source = exception.Source,
                HelpLink = exception.HelpLink,
                Type = exception.GetType().ToString(),
                StackTrace = exception.StackTrace,

            }.ToString());
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            this.TraceAsync(context, exception);
            this.TraceAsync(context, exception, true);

            return context.Response.WriteAsync(new ErrorDetails()
            {
                RequestId = Guid.Parse(context.Items["id"].ToString()),
                StatusCode = context.Response.StatusCode,
                Message = exception.Message,
                Type = exception.GetType().ToString(),
                Verb = "HTTP" + context.Request.Method,
                Endpoint = context.Request.Path,
                Source = exception.Source,

                HelpLink = exception.HelpLink,
                StackTrace = exception.StackTrace,

            }.ToString());

        }

        private void TraceAsync(HttpContext context, Exception ex, bool isUnexpected)
        {
            var body = context.Items["body"].ToString();

            //TODO: Save log to chosen datastore
            var trace = JsonConvert.SerializeObject(ex); ;
            var cai = context.Items["unique"].ToString();
            var elapsedMs = 34;
            var requestId = context.Items["id"].ToString();
            var requestPath = context.Request.Path.ToString();
            var requestScheme = $"{context.Request.Scheme} {context.Request.Host}{context.Request.Path} {context.Request.QueryString} {body}";

            Logg.Error("Executed {@requestId} {@requestPath} {@requestScheme} {@trace} in {Elapsed:000} ms. by {unique} {type}",
                         requestId, requestPath, requestScheme, trace, elapsedMs, cai, EFLogType.EXCEPTION.GetDescription());

        }

        private void TraceAsync(HttpContext context, Exception ex)
        {
            if (context.Response.HasStarted)
                return;

            _telemetry.TrackException(ex);
            var trace = ex.Message;

            string body = "";
            string requestId = requestId = context.TraceIdentifier;

            IPrincipal principal = context.User;
            var User = new CvxClaimsPrincipal(principal);
            var cai = User.IsUser ? User.Email : User.ObjectId;
            var requestPath = context.Request.Path.ToString();
            var requestScheme = $"{context.Request.Scheme} {context.Request.Host}{context.Request.Path} {context.Request.QueryString} {body}";

            _telemetry.TrackTrace($"Executed RequestId:{requestId}, RequestPath:{requestPath}, RequestScheme:{requestScheme},CAI:{cai} , Trace:{trace}",
                                    Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Error);
        }

    }
}