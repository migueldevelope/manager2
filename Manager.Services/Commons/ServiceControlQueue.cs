using Manager.Core.Interfaces;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manager.Services.Commons
{
  public class ServiceControlQueue : IServiceControlQueue
  {
    private readonly IQueueClient queueClient;

    public ServiceControlQueue(string serviceBusConnectionString, string queueName)
    {
      queueClient = new QueueClient(serviceBusConnectionString, queueName);
    }

    public async Task SendMessageAsync(dynamic view)
    {
      try
      {
        var message = new Message(Encoding.UTF8.GetBytes(view));
        await queueClient.SendAsync(message);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public void RegisterOnMessageHandlerAndReceiveMesssages()
    {
      try
      {
        var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
        {
          MaxConcurrentCalls = 1,
          AutoComplete = false
        };

        queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private async Task ProcessMessagesAsync(Message message, CancellationToken token)
    {
      var body = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(message.Body));

      await queueClient.CompleteAsync(message.SystemProperties.LockToken);
    }

    private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
    {
      var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
      return Task.CompletedTask;
    }

  }
}
