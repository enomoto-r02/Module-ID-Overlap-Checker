using Module_ID_Overlap_Checker.DIVA;
using Module_ID_Overlap_Checker.Manager;
using Module_ID_Overlap_Checker.MikuMikuLibrary;
using Module_ID_Overlap_Checker.Util;

namespace Module_ID_Overlap_Checker
{
    class Program
    {
        public static readonly AppConfig appConfig = AppConfig.Get();
        public static readonly string start_dt = DateTime.Now.ToString("yyyyMMdd_HHmmss");

        static void Main(string[] args)
        {
            Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "Tool Start.");

            try
            {
                if (appConfig == null)
                {
                    Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "File \"" + AppConfig.FILE_INI + "\" is Not Found.");
                    Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "Tool Failed.");
                    ToolUtil.ErrorLog("File \"" + AppConfig.FILE_INI + "\" is Not Found.");

                    return;
                }
                if (File.Exists(appConfig.DivaModManager.ConfigPath) == false)
                {
                    Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "File \"" + appConfig.DivaModManager.ConfigPath + "\" in " + AppConfig.FILE_INI + " is Not Found.");
                    Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "Tool Failed.");
                    ToolUtil.ErrorLog("File \"" + appConfig.DivaModManager.ConfigPath + "\" in \"" + AppConfig.FILE_INI + "\" is Not Found.");

                    return;
                }

                if (File.Exists(appConfig.MikuMikuLibrary.FarcPack_Path) == false)
                {
                    Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "File \"" + appConfig.MikuMikuLibrary.FarcPack_Path + "\" in " + MikuMikuLibraryConfig.FILE_FARC_PACK + " is Not Found.");
                    Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "Tool Failed.");
                    ToolUtil.ErrorLog("File \"" + appConfig.DivaModManager.ConfigPath + "\" in \"" + MikuMikuLibraryConfig.FILE_FARC_PACK + "\" is Not Found.");

                    return;
                }

                if (ModLogic.Init() == false)
                {
                    Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "ChritmProp.Init() is Failed.");
                    Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "Tool Failed.");
                    return;
                }

                DivaModManager dmm = new(appConfig);

                if (dmm.Mods.Count == 0)
                {
                    Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "There are no mods to merge.");

                    return;
                }

                Console.WriteLine(dmm.ToStringMods());

                ModLogic.Load(appConfig, dmm);

                ModLogic.ViewChara(appConfig, dmm);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "Unexpected error. Check the log file.");
                ToolUtil.ErrorLog("Unexpected error. Check the log file.\n" + ex.InnerException + "\n" + ex.Message + "\n" + ex.StackTrace);
            }
            finally
            {
                Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "Tool End.");
                Console.WriteLine();
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
            }
        }
    }
}