﻿using Manager.Core.Base;
using Manager.Views.Enumns;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Core.Business
{
  public class User : BaseEntity
  {
    public string Name { get; set; }
    public string Document { get; set; }
    public string Mail { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }
    public DateTime? DateBirth { get; set; }
    public DateTime? DateAdm { get; set; }
    public Schooling Schooling { get; set; }
    public string PhotoUrl { get; set; }
    public long Coins { get; set; }
    public EnumChangePassword ChangePassword { get; set; }
    public string ForeignForgotPassword { get; set; }
    public string PhoneFixed { get; set; }
    public string DocumentID { get; set; }
    public string DocumentCTPF { get; set; }
    public EnumSex Sex { get; set; }
  }
}
