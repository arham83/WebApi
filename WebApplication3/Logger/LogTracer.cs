using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using System;
using System.Diagnostics;
using System.Threading.Tasks;


namespace WebApplication3.Logger
{
    public class LogTracer
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogTracer> _logger;

        public LogTracer(RequestDelegate next, ILogger<LogTracer> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, IAuthenticationSchemeProvider schemeProvider)
        {
            var stopwatch = Stopwatch.StartNew();
            var requestAccept = context.Request.Headers["Accept"].ToString();
            var requestHost = context.Request.Headers["Host"].ToString();
            var requestAuthorization = context.Request.Headers["Authorization"].ToString();

            _logger.LogInformation($"Protocol: {context.Request.Scheme}/{context.Request.Protocol}");
            _logger.LogInformation($"Method: {context.Request.Method}");
            _logger.LogInformation($"Scheme: {context.Request.Scheme}");
            _logger.LogInformation($"PathBase: {context.Request.PathBase}");
            _logger.LogInformation($"Path: {context.Request.Path}");
            _logger.LogInformation($"Accept: {requestAccept}");
            _logger.LogInformation($"Host: {requestHost}");
            _logger.LogInformation($"Authorization: {requestAuthorization}");
            _logger.LogInformation($"Username: {context.User?.Identity?.Name}");
            _logger.LogInformation($"Time: {DateTime.UtcNow}");
            // Capture the response
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            _logger.LogInformation($"Status Code: {context.Response.StatusCode}");

            // Log the response and payload
            responseBody.Seek(0, SeekOrigin.Begin);
            var response = await new StreamReader(responseBody).ReadToEndAsync();
            _logger.LogInformation($"Response: {response}");

            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            _logger.LogInformation($"Request completion Time: {elapsedMilliseconds} ms");

            // Restore the original response body stream
            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}
