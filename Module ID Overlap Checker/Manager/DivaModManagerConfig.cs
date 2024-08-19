namespace Module_ID_Overlap_Checker.Manager
{
    public class DivaModManagerConfig
    {

        public static string FILE_CONFIG = "Config.json";
        public string Path { get; set; }
        public string ConfigPath { get { return this.Path + "\\" + FILE_CONFIG; } }

        public DivaModManagerConfig()
        {
        }
    }
}
