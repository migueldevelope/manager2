using Manager.Core.Base;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  /// <summary>
  /// Objeto persiste no banco de dados para auto gestão
  /// </summary>
  public class AutoManager : BaseEntity
  {
    public ViewListPersonBase Person { get; set; }
    public ViewListPersonBase Requestor { get; set; }
    public List<string> Workflow { get; set; }
    public EnumStatusAutoManager StatusAutoManager { get; set; }
    public DateTime? OpenDate { get; set; }
  }
}
