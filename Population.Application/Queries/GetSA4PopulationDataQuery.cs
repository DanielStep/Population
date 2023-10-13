using MediatR;
using Population.Domain;

namespace Population.Application.Queries
{
    public class SA4PopulationDataPayload
    {
        public string ASGS_2016 { get; set; }
        public string Sex { get; set; }
    }

    public class GetSA4PopulationDataQuery : IRequest<SA4PopulationQueryResult>
    {
        public SA4PopulationDataPayload Payload { get; set; }
    }

    public class GetSA4PopulationDataQueryHandler : IRequestHandler<GetSA4PopulationDataQuery, SA4PopulationQueryResult>
    {
        private readonly IRepository _repo;
        public GetSA4PopulationDataQueryHandler(IRepository repo)
        {
            _repo = repo;
        }
        public async Task<SA4PopulationQueryResult> Handle(GetSA4PopulationDataQuery request, CancellationToken cancellationToken)
        {
            var data = await _repo.GetSA4SexPopulation(request.Payload.ASGS_2016, request.Payload.Sex);
            var regionName = data.FirstOrDefault()?.Region;

            var result = new SA4PopulationQueryResult(request.Payload.ASGS_2016, regionName, data);
            return result;
        }
    }
}
