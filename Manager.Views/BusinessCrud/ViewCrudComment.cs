﻿using Manager.Views.Enumns;
using System;

namespace Manager.Views.BusinessCrud
{
    public class ViewCrudComment : _ViewCrud
    {
        public string Comments { get; set; }
        public string CommentsSpeech { get; set; }
        public DateTime? Date { get; set; }
        public EnumStatusView StatusView { get; set; }
        public EnumUserComment UserComment { get; set; }
        public string SpeechLink { get; set; }
        public string TotalTime { get; set; }
    }
}
