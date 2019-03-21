﻿using Manager.Core.Base;
using System;
namespace Manager.Core.Business
{
  /// <summary>
  /// Objeto persiste no banco de dados
  /// </summary>
  public class Log : BaseEntity
  {
    public Person Person { get; set; }
    public string Description { get; set; }
    public DateTime? DataLog { get; set; }
    public string Local { get; set; }
  }
}
