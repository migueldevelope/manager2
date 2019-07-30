using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceControlQueue
  {
    void SendMessageAsync(dynamic view);
    void RegisterOnMessageHandlerAndReceiveMesssages();
    void StartMathMaturity();
    string ServiceBusConnectionString();
  }
}
