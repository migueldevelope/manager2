using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Views.BusinessCrud;
using Manager.Views.Enumns;
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
    private readonly string serviceBusConnectionString;

    public ServiceControlQueue(string _serviceBusConnectionString, IServiceMaturity _serviceMaturity)
    {
      serviceBusConnectionString = _serviceBusConnectionString;
      queueClient = new QueueClient(serviceBusConnectionString, "journey");
      serviceMaturity = _serviceMaturity;
    }

    private void Maturity_Tick(object Sender, EventArgs e)
    {
      try
      {
        //Math Maturity
        if(DateTime.Now.Day == 1)
          Task.Run(() => serviceMaturity.MathMonth());
        
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public void StartMathMaturity()
    {
      try
      {
        
       var timer = new System.Timers.Timer
       {
          //24 hours em milliseconds
          Interval = 86400000
        };
        timer.Elapsed += new System.Timers.ElapsedEventHandler(Maturity_Tick);
        timer.Enabled = true;
        timer.Start();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public void SendMessageAsync(dynamic view)
    {
      try
      {
        var message = new Message(Encoding.UTF8.GetBytes(view));
         queueClient.SendAsync(message);
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

    public string ServiceBusConnectionString()
    {
      return serviceBusConnectionString;
    }
  }
}
