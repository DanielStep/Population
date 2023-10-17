using MediatR;

namespace Population.Application
{
    public class IngestPopulationDataCmd : IRequest<Unit>
    {
        public string CsvPath { get; set; }
        public class IngestPopulationDataCmdHandler : IRequestHandler<IngestPopulationDataCmd, Unit>
        {
            private readonly IParser _csvParser;
            private readonly IRepository _repo;
            public IngestPopulationDataCmdHandler(IParser csvParser, IRepository repo)
            {
                _csvParser = csvParser;
                _repo = repo;
            }

            public async Task<Unit> Handle(IngestPopulationDataCmd cmd, CancellationToken cancellationToken)
            {
                var dataTable = _csvParser.Parse(cmd.CsvPath);
                //validate data table against existing year
                await _repo.LoadPopulationData(dataTable);
                await _repo.NormalisePopulationTable();

                return Unit.Value;
            }
        }
    }
}
