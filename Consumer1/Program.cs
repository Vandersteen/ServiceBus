using Consumer1;
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
                    x.AddConsumersFromNamespaceContaining<MessageConsumer>();

                    x.UsingAzureServiceBus(
                        (ctx, cfg) =>
                        {
                            cfg.Host(
                                "***"
                            );

                            cfg.ConfigureEndpoints(ctx);
                        }
                    );
                }
            );
    })
    .Build();

await host.RunAsync();