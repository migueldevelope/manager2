﻿using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;

namespace Manager.Core.Business
{
  /// <summary>
  ///  Objeto persiste no banco de dados - modelos de email
  /// </summary>
  public class MailModel : BaseEntity
  {
    public string Name { get; set; }
    public string Message { get; set; }
    public string Subject { get; set; }
    public string Link { get; set; }
    public EnumStatus StatusMail { get; set; }
    public EnumTypeFrequence TypeFrequence { get; set; }
    public byte Day { get; set; }
    public DayOfWeek Weekly { get; set; }
    public EnumTypeMailModel TypeMailModel { get; set; }
    public ViewListMailModel GetViewList()
    {
      return new ViewListMailModel()
      {
        _id = _id,
        Name = Name,
        StatusMail = StatusMail,
        Subject = Subject,
        TypeFrequence = TypeFrequence,
        Day = Day,
        Link = Link,
        Weekly = Weekly,
        TypeMailModel = TypeMailModel
      };
    }

    public ViewCrudMailModel GetViewCrud()
    {
      return new ViewCrudMailModel()
      {
        _id = _id,
        Name = Name,
        StatusMail = StatusMail,
        Subject = Subject,
        TypeFrequence = TypeFrequence,
        Day = Day,
        Link = Link,
        Message = Message,
        Weekly = Weekly,
        TypeMailModel = TypeMailModel
      };
    }
  }
}
