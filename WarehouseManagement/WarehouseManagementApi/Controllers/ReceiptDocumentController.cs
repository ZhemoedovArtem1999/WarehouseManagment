using DataAccessLayer.Infrastructure.FilterModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
using WarehouseManagementApi.Exceptions;
using WarehouseManagementApi.Infrastructure.Abstraction;
using WarehouseManagementApi.Infrastructure.Services;
using WarehouseManagementApi.Models;
using WarehouseManagementApi.Models.ReceiptDocument;

namespace WarehouseManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptDocumentController : ControllerBase
    {
        private readonly IReceiptDocumentService receiptDocumentService;
        public ReceiptDocumentController(IReceiptDocumentService receiptDocumentService)
        {
            this.receiptDocumentService = receiptDocumentService;
        }

        [HttpGet("getFilter")]
        [ProducesResponseType(typeof(ReceiptFilterData), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<Results<Ok<ReceiptFilterData>, BadRequest<ValidationProblemDetails>>> GetDataFilter(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await receiptDocumentService.GetFilterData(cancellationToken);
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
        [ProducesResponseType(typeof(IEnumerable<ReceiptDocumentViewDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<Results<Ok<IEnumerable<ReceiptDocumentViewDto>>, BadRequest<ValidationProblemDetails>>> GetDocuments([FromQuery] ReceiptDocumentFilter filter, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await receiptDocumentService.GetDocuments(filter, cancellationToken);
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
        [ProducesResponseType(typeof(ReceiptResourceReferences), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<Results<Ok<ReceiptResourceReferences>, BadRequest<ValidationProblemDetails>>> GetReferences(int? documentId, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await receiptDocumentService.GetReferences(documentId, cancellationToken);
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

        [HttpGet("document/{id}")]
        [ProducesResponseType(typeof(ReceiptDocumentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<Results<Ok<ReceiptDocumentDto>, BadRequest<ValidationProblemDetails>>> GetDocument(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await receiptDocumentService.GetDocument(id, cancellationToken);
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
        public async Task<Results<Ok<string>, BadRequest<ValidationProblemDetails>>> CreateDocument([FromBody] ReceiptDocumentDto documentDto, CancellationToken cancellationToken = default)
        {
            try
            {
                await receiptDocumentService.CreateDocumentAsync(documentDto, cancellationToken);
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
        public async Task<Results<Ok<string>, BadRequest<ValidationProblemDetails>>> UpdateDocument([FromBody] ReceiptDocumentDto documentDto, CancellationToken cancellationToken = default)
        {
            try
            {
                await receiptDocumentService.UpdateDocumentAsync(documentDto, cancellationToken);

                return TypedResults.Ok("Документ обновлен");
            }
            catch (InvalidUpdateOperationException ex)
            {
                var problems = new ValidationProblemDetails()
                {
                    Title = ex.Message,
                    Detail = ex.IdObject.ToString()
                };
                return TypedResults.BadRequest(problems);
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

        [HttpPost("delete")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<Results<Ok<string>, BadRequest<ValidationProblemDetails>>> DeleteDocument([FromBody] int documentId, CancellationToken cancellationToken = default)
        {
            try
            {
                await receiptDocumentService.DeleteDocumentAsync(documentId, cancellationToken);

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
