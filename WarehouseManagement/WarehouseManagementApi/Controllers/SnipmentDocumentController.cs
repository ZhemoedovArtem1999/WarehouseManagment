using DataAccessLayer.Infrastructure.FilterModel;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagementApi.Infrastructure.Abstraction;
using WarehouseManagementApi.Models.SnipmentDocument;

namespace WarehouseManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SnipmentDocumentController : ControllerBase
    {
        private readonly ISnipmentDocumentService snipmentDocumentService;
        public SnipmentDocumentController(ISnipmentDocumentService snipmentDocumentService)
        {
            this.snipmentDocumentService = snipmentDocumentService;
        }

        [HttpGet("getFilter")]
        [ProducesResponseType(typeof(SnipmentFilterData), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<Results<Ok<SnipmentFilterData>, BadRequest<ValidationProblemDetails>>> GetDataFilter(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await snipmentDocumentService.GetFilterData(cancellationToken);
                return TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                var problems = new ValidationProblemDetails()
                {
                    Title = "Произошла непредвиденная ошибка!"
                };

                return TypedResults.BadRequest(problems);
            }
        }

        [HttpGet("filter")]
        [ProducesResponseType(typeof(IEnumerable<SnipmentDocumentViewDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<Results<Ok<IEnumerable<SnipmentDocumentViewDto>>, BadRequest<ValidationProblemDetails>>> GetDocuments([FromQuery] SnipmentDocumentFilter filter, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await snipmentDocumentService.GetDocuments(filter, cancellationToken);
                return TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                var problems = new ValidationProblemDetails()
                {
                    Title = "Произошла непредвиденная ошибка!"
                };

                return TypedResults.BadRequest(problems);
            }
        }

        [HttpGet("getReferences")]
        [ProducesResponseType(typeof(SnipmentResourceReferences), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<Results<Ok<SnipmentResourceReferences>, BadRequest<ValidationProblemDetails>>> GetReferences(int? documentId, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await snipmentDocumentService.GetReferences(documentId, cancellationToken);
                return TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                var problems = new ValidationProblemDetails()
                {
                    Title = "Произошла непредвиденная ошибка!"
                };

                return TypedResults.BadRequest(problems);
            }
        }

        [HttpGet("getResourcesBalance")]
        [ProducesResponseType(typeof(IEnumerable<SnipmentResourceDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<Results<Ok<IEnumerable<SnipmentResourceDto>>, BadRequest<ValidationProblemDetails>>> GetResourcesBalance(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await snipmentDocumentService.GetResourceBalance(cancellationToken);
                return TypedResults.Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                var problems = new ValidationProblemDetails()
                {
                    Title = ex.Message
                };
                
                return TypedResults.BadRequest(problems);

            }
            catch (Exception ex)
            {
                var problems = new ValidationProblemDetails()
                {
                    Title = "Произошла непредвиденная ошибка!"
                };

                return TypedResults.BadRequest(problems);
            }
        }

        [HttpGet("document/{id}")]
        [ProducesResponseType(typeof(SnipmentDocumentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<Results<Ok<SnipmentDocumentDto>, BadRequest<ValidationProblemDetails>>> GetDocument(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await snipmentDocumentService.GetDocument(id, cancellationToken);
                return TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                var problems = new ValidationProblemDetails()
                {
                    Title = "Произошла непредвиденная ошибка!"
                };

                return TypedResults.BadRequest(problems);
            }
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<Results<Ok<string>, BadRequest<ValidationProblemDetails>>> CreateDocument([FromBody] SnipmentDocumentDto documentDto, CancellationToken cancellationToken = default)
        {
            try
            {
                await snipmentDocumentService.CreateDocumentAsync(documentDto, cancellationToken);
                return TypedResults.Ok("Документ создан");
            }
            catch (InvalidOperationException ex)
            {
                var problems = new ValidationProblemDetails()
                {
                    Title = ex.Message
                };
                return TypedResults.BadRequest(problems);

            }
            catch (Exception ex)
            {
                var problems = new ValidationProblemDetails()
                {
                    Title = "Произошла непредвиденная ошибка!"
                };

                return TypedResults.BadRequest(problems);
            }
        }

        [HttpPost("update")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<Results<Ok<string>, BadRequest<ValidationProblemDetails>>> UpdateDocument([FromBody] SnipmentDocumentDto documentDto, CancellationToken cancellationToken = default)
        {
            try
            {
                await snipmentDocumentService.UpdateDocumentAsync(documentDto, cancellationToken);

                return TypedResults.Ok("Документ обновлен");
            }
            catch (InvalidOperationException ex)
            {
                var problems = new ValidationProblemDetails()
                {
                    Title = ex.Message,
                };
                return TypedResults.BadRequest(problems);

            }
            catch (KeyNotFoundException ex)
            {
                var problems = new ValidationProblemDetails()
                {
                    Title = ex.Message
                };
                return TypedResults.BadRequest(problems);

            }
            catch (Exception ex)
            {
                var problems = new ValidationProblemDetails()
                {
                    Title = "Произошла непредвиденная ошибка!"
                };

                return TypedResults.BadRequest(problems);
            }
        }

        [HttpPost("revoke")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<Results<Ok<string>, BadRequest<ValidationProblemDetails>>> RevokeDocument([FromBody] int documentId, CancellationToken cancellationToken = default)
        {
            try
            {
                await snipmentDocumentService.RevokeDocumentAsync(documentId, cancellationToken);

                return TypedResults.Ok("Документ отозван");
            }
            catch (KeyNotFoundException ex)
            {
                var problems = new ValidationProblemDetails()
                {
                    Title = ex.Message
                };
                return TypedResults.BadRequest(problems);

            }
            catch (Exception ex)
            {
                var problems = new ValidationProblemDetails()
                {
                    Title = "Произошла непредвиденная ошибка!"
                };

                return TypedResults.BadRequest(problems);
            }
        }

        [HttpPost("delete")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<Results<Ok<string>, BadRequest<ValidationProblemDetails>>> DeleteDocument([FromBody] int documentId, CancellationToken cancellationToken = default)
        {
            try
            {
                await snipmentDocumentService.DeleteDocumentAsync(documentId, cancellationToken);

                return TypedResults.Ok("Документ удален");
            }
            catch (InvalidOperationException ex)
            {
                var problems = new ValidationProblemDetails()
                {
                    Title = ex.Message,
                };
                return TypedResults.BadRequest(problems);

            }
            catch (KeyNotFoundException ex)
            {
                var problems = new ValidationProblemDetails()
                {
                    Title = ex.Message
                };
                return TypedResults.BadRequest(problems);

            }
            catch (Exception ex)
            {
                var problems = new ValidationProblemDetails()
                {
                    Title = "Произошла непредвиденная ошибка!"
                };

                return TypedResults.BadRequest(problems);
            }
        }
    }
}
