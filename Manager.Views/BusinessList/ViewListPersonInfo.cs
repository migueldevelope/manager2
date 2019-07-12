﻿using Manager.Views.Enumns;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Views.BusinessList
{
  public class ViewListPersonInfo : _ViewListBase
  {
    public EnumTypeJourney TypeJourney { get; set; }
    public string Occupation { get; set; }
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idManager { get; set; }
    public string Manager { get; set; }
  }
}
