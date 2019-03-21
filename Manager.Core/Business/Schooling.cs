﻿using Manager.Core.Base;
using Manager.Views.Enumns;

namespace Manager.Core.Business
{
  /// <summary>
  ///  Objeto persiste no banco de dados
  /// </summary>
  public class Schooling : BaseEntity
  {
    public string Name { get; set; }
    public string Complement { get; set; }
    public EnumTypeSchooling Type { get; set; }
    public Schooling Template { get; set; }
    public long Order { get; set; }
  }
}
