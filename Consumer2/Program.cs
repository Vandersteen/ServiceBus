using Consumer2;
using MassTransit;
using Producer;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddLogging(
            b =>
            {
                b.AddSimpleConsole(
                    options =>
                    {
                        options.IncludeScopes = true;
                        options.SingleLine = true;
                        options.TimestampFormat = "hh:mm:ss ";
                    }
                );
            }
        );

        services.AddMassTransit(
            x =>
            {
                x.SetKebabCaseEndpointNameFormatter();
                x.AddConsumer<MessageConsumer>();

                x.UsingAzureServiceBus(
                    (ctx, cfg) =>
                    {
                        cfg.Host(
                            "***"
                        );

                        cfg.SubscriptionEndpoint<Message>(
                            "consumer2",
                            e =>
                            {
                                // e.UseDelayedRedelivery(r => r.Intervals(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10)));
                                // e.UseRetry(r => r.Immediate(3));
                                    
                                e.ConfigureConsumer<MessageConsumer>(ctx);
                            }
                        );

                        cfg.ConfigureEndpoints(ctx);
                    }
                );
            }
        );
    })
    .Build();

await host.RunAsync();