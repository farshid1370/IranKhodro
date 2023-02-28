
using Microsoft.Extensions.Options;
using static System.String;

namespace IranKhodro
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ICircularManager _circularManager;
        private readonly IOptions<Settings> _options;
        private readonly IEmailManager _emailManager;

        public Worker(ILogger<Worker> logger, ICircularManager circularManager, IOptions<Settings> options, IEmailManager emailManager)
        {
            _logger = logger;
            _circularManager = circularManager;
            _options = options;
            _emailManager = emailManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                
                if (DateTime.Now.Hour == 16 )
                {
                    var list = await _circularManager.GetCircularList();
                    if (list.Count==0) continue;
                    _logger.LogInformation("Send Message at: {time}", DateTimeOffset.Now);
                    var receptors = _options.Value.Recepters.Split(',').ToList();
                    var message = Join(" , ", list);

                    receptors.ForEach(receptor =>
                    {
                        _emailManager.Send(receptor, message);
                    });

                }
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}