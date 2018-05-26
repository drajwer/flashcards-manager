namespace FlashcardsManager.Core.Options
{
    public class LearningServiceOptions
    {
        public int MinProgress { get; set; }
        public int MinKnownProgress { get; set; }
        public int MaxHardProgress { get; set; }
        public int MaxProgress { get; set; }
        public int OnSuccess { get; set; }
        public int OnPartial { get; set; }
        public int OnFailure { get; set; }
    }
}
