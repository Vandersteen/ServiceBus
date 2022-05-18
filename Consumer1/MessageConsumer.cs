using MassTransit;
using Producer;

namespace Consumer1;

public class MessageConsumerDefinition : ConsumerDefinition<MessageConsumer>
{
    public MessageConsumerDefinition()
    {
        EndpointName = "Consumer1";
    }
}

public class MessageConsumer : IConsumer<Message>
{
    private readonly ILogger<MessageConsumer> _logger;

    public MessageConsumer(ILogger<MessageConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<Message> context)
    {
        _logger.LogInformation("Received Text: {Text}", context.Message.Id);
        throw new Exception("Ooops");
        //return Task.CompletedTask;
    }
}