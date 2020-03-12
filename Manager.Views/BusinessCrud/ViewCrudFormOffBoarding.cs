using Manager.Views.Enumns;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Views.BusinessCrud
{
    public class ViewCrudFormOffBoarding
    {
        public List<ViewCrudOffBoardingQuestions> Questions { get; set; }
        public List<ViewCrudOffBoardingQuestions> QuestionsManager { get; set; }
        public DateTime? DateOff { get; set; }
        public EnumTypeOff TypeOff { get; set; }
        public EnumReasonOff Reason { get; set; }
        public string ObsReason { get; set; }
        public string Observation { get; set; }
        public DateTime? Date { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string _idInterviewer { get; set; }
        public string NameInterviewer { get; set; }
        public EnumStatusFormOffBoarding StatusFormOffBoarding { get; set; }
    }
}
