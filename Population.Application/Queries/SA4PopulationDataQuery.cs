using MediatR;
using Population.Domain;

namespace Population.Application.Queries
{
    public class SA4PopulationDataPayload
    {
        public string ASGS_2016 { get; set; }
        public string Sex { get; set; }
    }

    public class SA4PopulationDataQuery : IRequest<SA4PopulationDataQueryResult>
    {
        public SA4PopulationDataPayload Payload { get; set; }
    }

    public class SA4PopulationDataQueryHandler : IRequestHandler<SA4PopulationDataQuery, SA4PopulationDataQueryResult>
    {
        private readonly IRepository _repo;
        public SA4PopulationDataQueryHandler(IRepository repo)
        {
            _repo = repo;
        }
        public async Task<SA4PopulationDataQueryResult> Handle(SA4PopulationDataQuery request, CancellationToken cancellationToken)
        {
            var data = await _repo.GetSA4PopData(request.Payload.ASGS_2016, request.Payload.Sex);
            var regionName = data.FirstOrDefault()?.Region;

            var result = new SA4PopulationDataQueryResult(request.Payload.ASGS_2016, regionName, data);
            return result;
        }
    }
}
