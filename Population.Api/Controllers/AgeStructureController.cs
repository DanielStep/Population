using MediatR;
using Microsoft.AspNetCore.Mvc;
using Population.Application.Queries;

namespace Population.Api.Controllers
{
    [ApiController]
    [Route("api")]
    public class AgeStructureController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AgeStructureController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("age-structure/{ASGS_2016}/{sex}")]
        public async Task<IActionResult> GetAgeStructure(string ASGS_2016, string sex, CancellationToken cancellationToken)
        {
            var payload = new SA4PopulationDataPayload() { ASGS_2016 = ASGS_2016, Sex = sex };
            var result = await _mediator.Send(new SA4PopulationDataQuery() { Payload = payload }, cancellationToken);
            return Ok(result);
        }

    }
}
