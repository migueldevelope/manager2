using Manager.Core.Base;
using Manager.Views.Enumns;
using System;

namespace Manager.Core.Business
{

  /// <summary>
  /// Objeto persiste no banco de dados - Logs para mensageria
  /// </summary>
  public class LogMessages : BaseEntity
  {
    public Person Person { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
    public DateTime Register { get; set; }
    public EnumStatusMessage StatusMessage { get; set; }
  }
}
