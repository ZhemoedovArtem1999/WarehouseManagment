using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagementApi.Infrastructure.Abstraction;
using WarehouseManagementApi.Models;

namespace WarehouseManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourceController : ControllerBase
    {
        private readonly IResourceService resourceService;

        public ResourceController(IResourceService resourceService)
        {
            this.resourceService = resourceService;
        }

        [HttpGet("{isArchive}")]
        [ProducesResponseType(typeof(ResourceDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<Results<Ok<ResourceDto>, BadRequest<ValidationProblemDetails>>> Get(bool isArchive = false, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await resourceService.GetAllResourceAsync(isArchive, cancellationToken);
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

        [HttpGet("get/{id}")]
        [ProducesResponseType(typeof(ResourceItem), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<Results<Ok<ResourceItem>, BadRequest<ValidationProblemDetails>>> GetById(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await resourceService.GetResourceAsync(id, cancellationToken);

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
        public async Task<Results<Ok<string>, BadRequest<ValidationProblemDetails>>> Create([FromBody] string name, CancellationToken cancellationToken = default)
        {
            try
            {
                await resourceService.CreateResourceAsync(name, cancellationToken);

                return TypedResults.Ok("Ресурс создан");
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
        public async Task<Results<Ok<string>, BadRequest<ValidationProblemDetails>>> Update([FromBody] ResourceUpdateRequestDto requestDto, CancellationToken cancellationToken = default)
        {
            try
            {
                await resourceService.UpdateResourceAsync(requestDto, cancellationToken);

                return TypedResults.Ok("Ресурс обновлен");
            }
            catch (InvalidOperationException ex)
            {
                var problems = new ValidationProblemDetails()
                {
                    Title = ex.Message
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

        [HttpPost("updateState")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<Results<Ok<string>, BadRequest<ValidationProblemDetails>>> UpdateState([FromBody] ResourceUpdateStateRequestDto requestDto, CancellationToken cancellationToken = default)
        {
            try
            {
                await resourceService.EditStateAsync(requestDto, cancellationToken);

                string message = requestDto.IsArchive ? "Ресурс отправлен в архив" : "Ресурс разархивирован";

                return TypedResults.Ok(message);
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
        public async Task<Results<Ok<string>, BadRequest<ValidationProblemDetails>>> Delete([FromBody] int id, CancellationToken cancellationToken = default)
        {
            try
            {
                await resourceService.DeleteResource(id, cancellationToken);

                return TypedResults.Ok("Ресурс удален");
            }
            catch (KeyNotFoundException ex)
            {
                var problems = new ValidationProblemDetails()
                {
                    Title = ex.Message
                };
                return TypedResults.BadRequest(problems);

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
    }
}
