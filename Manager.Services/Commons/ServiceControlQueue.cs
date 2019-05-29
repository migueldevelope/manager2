using Manager.Core.Base;
using Manager.Core.Interfaces;
using Manager.Views.BusinessCrud;
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
    private readonly IServiceMaturity serviceMaturity;

    public ServiceControlQueue(string serviceBusConnectionString, string queueName, IServiceMaturity _serviceMaturity)
    {
      queueClient = new QueueClient(serviceBusConnectionString, queueName);
      serviceMaturity = _serviceMaturity;
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
      var view = JsonConvert.DeserializeObject<ViewCrudMaturityRegister>(Encoding.UTF8.GetString(message.Body));
      serviceMaturity.SetUser(new BaseUser() {
        _idAccount = view._idAccount
      });

      serviceMaturity.NewMaturityRegister(view);

      await queueClient.CompleteAsync(message.SystemProperties.LockToken);
    }

    private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
    {
      var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
      return Task.CompletedTask;
    }

  }
}
