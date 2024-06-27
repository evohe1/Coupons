using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MassTransit;
using ScheduledJobs;

var host = Host.CreateDefaultBuilder(args)
             .ConfigureServices((hostContext, services) =>
             {
                 services.AddMassTransit(x =>
                 {
                     x.AddConsumer<MessageConsumer>();
                     x.AddConsumer<EmailConsumer>();

                     x.SetKebabCaseEndpointNameFormatter();

                     x.UsingRabbitMq((context, cfg) =>
                     {
                         cfg.ReceiveEndpoint("scheduled-jobs", e =>
                         {
                             e.Consumer<MessageConsumer>(context);
                             e.Consumer<EmailConsumer>(context);


                         });
                         cfg.Host("127.0.0.1", "/", h =>
                         {
                             h.Username("admin");
                             h.Password("sanane");
                         });
                         cfg.ConfigureEndpoints(context);
                     });
                 });

                 services.AddHostedService<Worker>();
             });

await host.Build().RunAsync().ConfigureAwait(false);


Console.WriteLine("hellO");