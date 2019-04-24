using Manager.Core.Base;
using Manager.Views.Enumns;
using System;

namespace Manager.Core.Business
{
  /// <summary>
  /// Este iria utilizar para gerenciar o serviço de email, porém acabei não utilizando
  /// </summary>
  public class ConfigurationNotification : BaseEntity
  {
    public string Name { get; set; }
    public EnumStatusNotification StatusNotification { get; set; }
    public EnumTypeNotification TypeNotification { get; set; }
    public EnumTimeNotification TimeNotification { get; set; }
    public EnumWeek TimeWeek { get; set; }
    public DateTime TimeDay { get; set; }
    public DateTime LastSend { get; set; }
    public DateTime NextSend { get; set; }
    public string Parameter { get; set; }
  }
}
