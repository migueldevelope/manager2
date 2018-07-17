﻿using Manager.Core.Base;
using Manager.Core.Enumns;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  public class MailLog: BaseEntity
  {
    public string Subject { get; set; }
    public MailLogAddress From { get; set; }
    public List<MailLogAddress> To { get; set; }
    public List<MailLogAddress> CopyTo { get; set; }
    public List<MailLogAddress> CopyBcc { get; set; }
    public DateTime Included { get; set; }
    public string _idPerson { get; set; }
    public string NamePerson { get; set; }
    public EnumPriorityMail Priority { get; set; }
    public string Body { get; set; }
    public EnumStatusMail StatusMail { get; set; }
    public List<string> KeySendGrid { get; set; }
  }
}
