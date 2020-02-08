using System;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Chambers.API.DocumentManagement.Filters
{
    public class ValidateMimeMultipartContentFilter : ActionFilterAttribute
    {
        private readonly ILogger<ValidateMimeMultipartContentFilter> _logger;

        public ValidateMimeMultipartContentFilter(ILogger<ValidateMimeMultipartContentFilter> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private static bool IsMultipartContentType(string contentType) =>
            !string.IsNullOrEmpty(contentType) &&
            contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!IsMultipartContentType(context.HttpContext.Request.ContentType))
            {
                _logger.LogDebug("Mimetype is not accepted: {ContentType}",context.HttpContext.Request.ContentType);
                context.Result = new StatusCodeResult(415);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}