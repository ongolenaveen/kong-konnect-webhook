using System.Net;
using Api.Template.Services.Interfaces;
using Api.Template.Shared.Utilities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Api.Template.App.HttpTriggers
{
    public class ReceiveEventsFunction(
        IEventsService eventsService,
        ILogger<ReceiveEventsFunction> logger)
    {

        [Function("ReceiveEventsFunction")]
        [OpenApiOperation(operationId: "Receive Events", tags: ["Events"], Description = ".")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "ApiKey", In = OpenApiSecurityLocationType.Header)]
        [OpenApiResponseWithoutBody(HttpStatusCode.OK, Description = "Received")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(ApiProblemDetails), Description = "Bad Request.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(ApiProblemDetails), Description = "Internal Server Error.")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "events")] HttpRequestData req)
        {
            var reader = new StreamReader(req.Body);
            var requestContent = await reader.ReadToEndAsync();
            logger.LogInformation(requestContent);

            foreach (var header in req.Headers)
                foreach (var value in header.Value)
                    logger.LogInformation($"{header.Key} : {value}");
            
            await eventsService.ProcessEvent(requestContent);
            var response = req.CreateResponse(HttpStatusCode.Created);
            response.Headers.Add("Content-Type", "application/json");
            return response;
        }
    }
}
