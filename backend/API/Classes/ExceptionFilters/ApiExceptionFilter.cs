using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using API.Utils.Interfaces;
using API.Utils.Models.ResponseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Serialization;
using Utils.Exceptions;

namespace API.Classes.ExceptionFilters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        private IApiResponseFormatter _apiResponseFormatter;

        public ApiExceptionFilter(IApiResponseFormatter apiResponseFormatter)
        {
            _apiResponseFormatter = apiResponseFormatter;
        }

        public void OnException(ExceptionContext context)
        {
            HttpStatusCode status;
            var exceptionDetails = new ApiErrorDetails();

            var exceptionType = context.Exception.GetType();
            if (exceptionType == typeof(UnauthorizedAccessException))
            {
                exceptionDetails.Message = "Unauthorized Access";
                status = HttpStatusCode.Unauthorized;
            }
            else if (exceptionType == typeof(AccessDeniedException))
            {
                exceptionDetails.Message = context.Exception.Message;
                status = HttpStatusCode.Forbidden;
            }
            else if (exceptionType == typeof(NotImplementedException))
            {
                exceptionDetails.Message = "A server error occurred.";
                status = HttpStatusCode.NotImplemented;
            }
            else if (exceptionType == typeof(BadRequestException))
            {
                exceptionDetails.Message = (context.Exception as BadRequestException)?.OverallMessage;
                exceptionDetails.FieldSpecificMessages = (context.Exception as BadRequestException)?.FieldSpecificMessages;
                status = HttpStatusCode.BadRequest;
            }
            else if (exceptionType == typeof(DemoAppException))
            {
                exceptionDetails.Message = (context.Exception as DemoAppException)?.OverallMessage;
                exceptionDetails.FieldSpecificMessages = (context.Exception as DemoAppException)?.FieldSpecificMessages;
                status = HttpStatusCode.InternalServerError;
            }
            else
            {
                exceptionDetails.Message = context.Exception.Message;
                status = HttpStatusCode.InternalServerError;
            }

            context.ExceptionHandled = true;

            var apiResponseData = _apiResponseFormatter.GetErrorResponseBody(exceptionDetails);

            var response = context.HttpContext.Response;
            response.StatusCode = (int)status;
            response.WriteAsync(JsonConvert.SerializeObject(apiResponseData,  new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }));
        }
    }
}
