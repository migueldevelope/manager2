using Manager.Core.Base;
using Manager.Views.Enumns;
using System;

namespace Manager.Core.BusinessModel
{
    /// <summary>
    /// Coleção para comentarios onboarding/monitoring
    /// </summary>
    public class ListComments : BaseEntityId
    {
        public string Comments { get; set; }
        public string CommentsSpeech { get; set; }
        public DateTime? Date { get; set; }
        public EnumStatusView StatusView { get; set; }
        public EnumUserComment UserComment { get; set; }
        public string SpeechLink { get; set; }
        public decimal TotalTime { get; set; }
    }
}
