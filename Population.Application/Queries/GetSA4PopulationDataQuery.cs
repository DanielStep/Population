using MediatR;
using Population.Domain;

namespace Population.Application.Queries
{

    public class GetSA4PopulationDataQuery : IRequest<SA4PopulationQueryResultDTO>
    {
        public SA4PopulationDataPayload Payload { get; set; }
    }

    public class GetSA4PopulationDataQueryHandler : IRequestHandler<GetSA4PopulationDataQuery, SA4PopulationQueryResultDTO>
    {
        private readonly IRepository _repo;
        public GetSA4PopulationDataQueryHandler(IRepository repo)
        {
            _repo = repo;
        }
        public async Task<SA4PopulationQueryResultDTO> Handle(GetSA4PopulationDataQuery request, CancellationToken cancellationToken)
        {
            var data = await _repo.GetSA4SexPopulation(request.Payload.RegionCodeType, request.Payload.GenericRegionCode, request.Payload.Sex);

            var result = new SA4PopulationQueryResultDTO(request.Payload.RegionCodeType, request.Payload.GenericRegionCode, data);
            return result;
        }
    }
}
