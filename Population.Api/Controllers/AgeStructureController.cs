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
            var result = await _mediator.Send(new GetSA4PopulationDataQuery() { Payload = payload }, cancellationToken);
            return Ok(result);
        }

        [HttpGet("age-structure-diff/{ASGS_2016}/{sex}/{year1}/{year2}")]
        public async Task<IActionResult> GetAgeStructureDiff(string ASGS_2016, string sex, int year1, int year2, CancellationToken cancellationToken)
        {
            var payload = new SA4PopulationAgeDiffPayload() { ASGS_2016 = ASGS_2016, Sex = sex, YearLower = year1, YearHigher = year2 };
            var result = await _mediator.Send(new GetSA4PopulationAgeDiffQuery() { Payload = payload }, cancellationToken);
            return Ok(result);
        }

    }
}
