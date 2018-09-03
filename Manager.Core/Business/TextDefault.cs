﻿using Manager.Core.Base;
using Manager.Core.Enumns;

namespace Manager.Core.Business
{
  public class TextDefault : BaseEntity
  {
    public string Name { get; set; }
    public string Content { get; set; }
    public EnumTypeText TypeText { get; set; }
    public Company Company { get; set; }
    public TextDefault Template { get; set; }
  }
}