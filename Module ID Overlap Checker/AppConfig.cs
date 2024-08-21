using Microsoft.Extensions.Configuration;
using Module_ID_Overlap_Checker.Manager;
using Module_ID_Overlap_Checker.MikuMikuLibrary;

namespace Module_ID_Overlap_Checker
{
    public class AppConfig
    {
        public static string FILE_INI = "Module ID Overlap Checker.ini";

        static AppConfig Instance;

        public Config Config { get; set; }

        public DivaModManagerConfig DivaModManager { get; set; }
        public MikuMikuLibraryConfig MikuMikuLibrary { get; set; }

        public AppConfig() { }
        public static AppConfig Get()
        {
            if (Instance != null) return Instance;

            if (File.Exists(AppConfig.FILE_INI) == false)
            {
                return null;
            }
            else
            {
                Instance = new ConfigurationBuilder()
                .AddIniFile(AppConfig.FILE_INI)
                .Build()
                .Get<AppConfig>();
            }
            return Instance;
        }
    }
}
