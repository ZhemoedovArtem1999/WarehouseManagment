using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagementApi.Infrastructure.Abstraction;
using WarehouseManagementApi.Models;

namespace WarehouseManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService clientService;

        public ClientController(IClientService clientService)
        {
            this.clientService = clientService;
        }

        [HttpGet("{isArchive}")]
        [ProducesResponseType(typeof(ClientDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<Results<Ok<ClientDto>, BadRequest<ValidationProblemDetails>>> Get(bool isArchive = false, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await clientService.GetAllClientAsync(isArchive, cancellationToken);
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
        [ProducesResponseType(typeof(ClientItem), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<Results<Ok<ClientItem>, BadRequest<ValidationProblemDetails>>> GetById(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await clientService.GetClientAsync(id, cancellationToken);

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
        public async Task<Results<Ok<string>, BadRequest<ValidationProblemDetails>>> Create([FromBody] ClientInsertRequestDto requestDto, CancellationToken cancellationToken = default)
        {
            try
            {
                await clientService.CreateClientAsync(requestDto, cancellationToken);

                return TypedResults.Ok("Клиент создан");
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
        public async Task<Results<Ok<string>, BadRequest<ValidationProblemDetails>>> Update([FromBody] ClientUpdateRequestDto requestDto, CancellationToken cancellationToken = default)
        {
            try
            {
                await clientService.UpdateClientAsync(requestDto, cancellationToken);

                return TypedResults.Ok("Клиент обновлен");
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
        public async Task<Results<Ok<string>, BadRequest<ValidationProblemDetails>>> UpdateState([FromBody] ClientUpdateStateRequestDto requestDto, CancellationToken cancellationToken = default)
        {
            try
            {
                await clientService.EditStateAsync(requestDto, cancellationToken);

                string message = requestDto.IsArchive ? "Клиент отправлен в архив" : "Клиент разархивирован";

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
                await clientService.DeleteClientAsync(id, cancellationToken);

                return TypedResults.Ok("Клиент удален");
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
