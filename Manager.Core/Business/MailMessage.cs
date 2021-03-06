﻿using Manager.Core.Base;
using Manager.Views.Enumns;

namespace Manager.Core.Business
{
  /// <summary>
  ///  Objeto persiste no banco de dados
  /// </summary>
  public class MailMessage : BaseEntity
  {
    public string Name { get; set; }
    public string Url { get; set; }
    public string Body { get; set; }
    public EnumTypeMailMessage Type { get; set; }
    public string Token { get; set; }
  }
}
