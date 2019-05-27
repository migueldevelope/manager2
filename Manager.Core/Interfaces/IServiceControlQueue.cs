using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceControlQueue
  {
    Task SendMessageAsync(dynamic view);
    void RegisterOnMessageHandlerAndReceiveMesssages();
  }
}
