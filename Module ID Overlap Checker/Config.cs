namespace Module_ID_Overlap_Checker
{
    public class Config
    {
        public bool MergePvField { get; set; }
        public bool MergeStageData { get; set; }

        public bool BackupPvDb { get; set; }
        public bool OverRidePvDb { get; set; }
        public bool AnotherSongMarkPrefix { get; set; }
        public string AnotherSongMarkPrefixStr { get; set; }
        public bool AnotherSongMarkSuffix { get; set; }
        public string AnotherSongMarkSuffixStr { get; set; }

        public Config()
        {
            this.BackupPvDb = false;
        }
    }
}
