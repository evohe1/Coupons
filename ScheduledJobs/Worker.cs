using Grpc.Shared.Commands;
using MassTransit;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduledJobs
{
    public class Worker : BackgroundService
    {
        readonly IBus _bus;

        public Worker(IBus bus)
        {
            _bus = bus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //var sendPoint = await _bus.GetSendEndpoint(new Uri("queue:Product")).ConfigureAwait(false);
            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    //await _bus.Publish(new Message { Text = $"The time is {DateTimeOffset.Now}" }, stoppingToken);
            //    await sendPoint.Send(new QueryProductPrice { Url = "https://www.sokmarket.com.tr/altinbas-demlik-poset-sade-200gr-40li-p-34726/" }, stoppingToken);
            //    await Task.Delay(100000, stoppingToken);
            //}
        }
    }
}
