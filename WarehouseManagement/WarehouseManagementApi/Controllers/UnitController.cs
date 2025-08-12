using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagementApi.Infrastructure.Abstraction;
using WarehouseManagementApi.Models.Unit;

namespace WarehouseManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        private readonly IUnitService unitService;

        public UnitController(IUnitService unitService)
        {
            this.unitService = unitService;
        }

        [HttpGet("{isArchive}")]
        [ProducesResponseType(typeof(UnitDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<Results<Ok<UnitDto>, BadRequest<ValidationProblemDetails>>> Get(bool isArchive = false, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await unitService.GetAllUnitAsync(isArchive, cancellationToken);
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
        [ProducesResponseType(typeof(UnitItem), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<Results<Ok<UnitItem>, BadRequest<ValidationProblemDetails>>> GetById(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await unitService.GetUnitAsync(id, cancellationToken);

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
                await unitService.CreateUnitAsync(name, cancellationToken);

                return TypedResults.Ok("Единица измерения создана");
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
        public async Task<Results<Ok<string>, BadRequest<ValidationProblemDetails>>> Update([FromBody] UnitUpdateRequestDto requestDto, CancellationToken cancellationToken = default)
        {
            try
            {
                await unitService.UpdateUnitAsync(requestDto, cancellationToken);

                return TypedResults.Ok("Единица измерения обновлена");
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
        public async Task<Results<Ok<string>, BadRequest<ValidationProblemDetails>>> UpdateState([FromBody] UnitUpdateStateRequestDto requestDto, CancellationToken cancellationToken = default)
        {
            try
            {
                await unitService.EditStateAsync(requestDto, cancellationToken);

                string message = requestDto.IsArchive ? "Единица измерения отправлена в архив" : "Единица измерения разархивирована";

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
                await unitService.DeleteUnitAsync(id, cancellationToken);

                return TypedResults.Ok("Единица измерения удалена");
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
