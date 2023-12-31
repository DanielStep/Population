using MediatR;

namespace Population.Application.Queries
{
    public class GetSA4PopulationAgeDiffQuery : IRequest<SA4PopulationDiffResultDTO>
    {
        public SA4PopulationAgeDiffPayload Payload { get; set; }
    }

    public class GetSA4PopulationAgeDiffQueryHandler : IRequestHandler<GetSA4PopulationAgeDiffQuery, SA4PopulationDiffResultDTO>
    {
        private readonly IRepository _repo;
        public GetSA4PopulationAgeDiffQueryHandler(IRepository repo)
        {
            _repo = repo;
        }
        public async Task<SA4PopulationDiffResultDTO> Handle(GetSA4PopulationAgeDiffQuery request, CancellationToken cancellationToken)
        {
            var data = await _repo.GetSA4PopulationAgeDiff(request.Payload.RegionCodeType, request.Payload.GenericRegionCode, request.Payload.Sex, request.Payload.YearLower, request.Payload.YearHigher);
            var result = new SA4PopulationDiffResultDTO(request.Payload.RegionCodeType, request.Payload.GenericRegionCode, request.Payload.YearLower, request.Payload.YearHigher, data);
            return result;
        }
    }
}
