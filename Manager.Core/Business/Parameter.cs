﻿using Manager.Core.Base;

namespace Manager.Core.Business
{
  /// <summary>
  ///  Objeto persiste no banco de dados
  /// </summary>
  public class Parameter : BaseEntity
  {
    public string Name { get; set; }
    public string Content { get; set; }
  }
}
