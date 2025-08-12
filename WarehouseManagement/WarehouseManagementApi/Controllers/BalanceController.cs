using DataAccessLayer.Infrastructure.FilterModel;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagementApi.Infrastructure.Abstraction;
using WarehouseManagementApi.Models.Balance;
using WarehouseManagementApi.Models.ReceiptDocument;

namespace WarehouseManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BalanceController : ControllerBase
    {
        private readonly IBalanceService balanceService;
        public BalanceController(IBalanceService balanceService)
        {
            this.balanceService = balanceService;
        }

        [HttpGet("getFilter")]
        [ProducesResponseType(typeof(BalanceFilterData), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<Results<Ok<BalanceFilterData>, BadRequest<ValidationProblemDetails>>> GetDataFilter(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await balanceService.GetFilterData(cancellationToken);
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
        [ProducesResponseType(typeof(IEnumerable<BalanceDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<Results<Ok<IEnumerable<BalanceDto>>, BadRequest<ValidationProblemDetails>>> GetBalance([FromQuery] BalanceFilter filter, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await balanceService.GetBalance(filter, cancellationToken);
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
    }
}
