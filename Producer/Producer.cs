using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Producer;

public class Producer : BackgroundService
{
    readonly IBus _bus;
    private readonly ILogger<Producer> _logger;

    public Producer(IBus bus, ILogger<Producer> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Producing message");
        await _bus.Publish(new Message() { Id = Guid.NewGuid() }, stoppingToken);
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }
}