using Kavenegar;
using Microsoft.Extensions.Options;
using static System.String;

namespace IranKhodro
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ICircularManager _circularManager;
        private readonly IOptions<Settings> _options;
        private readonly KavenegarApi _api;
        public Worker(ILogger<Worker> logger, ICircularManager circularManager, IOptions<Settings> options)
        {
            _logger = logger;
            _circularManager = circularManager;
            _options = options;
            _api = new KavenegarApi(_options.Value.ApiKey);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                var list = await _circularManager.GetCircularList();
                if (DateTime.Now.Hour == 15 && list.Count > 0)
                {

                    _logger.LogInformation("Send Message at: {time}", DateTimeOffset.Now);
                    var receptors = _options.Value.Receptors.Split(',').ToList();
                    var message = Join(" , ", list);
                    message = $"لیست بخشنامه های جدید : {message}";

                    var result = await _api.Send(_options.Value.SenderNumber, receptors, message);


                }
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}