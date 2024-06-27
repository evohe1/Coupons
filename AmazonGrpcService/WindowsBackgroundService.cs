//public sealed class WindowsBackgroundService : BackgroundService
//{
//    private readonly ILogger<WindowsBackgroundService> _logger;

//    public WindowsBackgroundService(
//        ILogger<WindowsBackgroundService> logger) =>
//        (_logger) = (logger);


//    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//    {
//        while (!stoppingToken.IsCancellationRequested)
//        {
//            Console.WriteLine("amazon çalışıyor");

//            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
//        }
//    }
//}