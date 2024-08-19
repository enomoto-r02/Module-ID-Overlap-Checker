using System.Diagnostics;

namespace Module_ID_Overlap_Checker.Util
{
    public static class ToolUtil
    {
        public static string FILE_LOG = "Module ID Overlap Checker.log";

        public static string CONSOLE_PREFIX = "[Module ID Overlap Checker] ";
        public static string LOG_PREFIX = "[" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "] ";

        public static void DebugLog(string str)
        {
            FileUtil.WriteFile_UTF_8_NO_BOM(LOG_PREFIX + "[Debug] " + str + "\n", FILE_LOG, true);
        }

        public static void InfoLog(string str)
        {
            FileUtil.WriteFile_UTF_8_NO_BOM(LOG_PREFIX + "[Infomation] " + str + "\n", FILE_LOG, true);
        }

        public static void WarnLog(string str)
        {
            FileUtil.WriteFile_UTF_8_NO_BOM(LOG_PREFIX + "[Warning] " + str + "\n", FILE_LOG, true);
        }

        public static void ErrorLog(string str)
        {
            FileUtil.WriteFile_UTF_8_NO_BOM(LOG_PREFIX + "[Error] " + str + "\n", FILE_LOG, true);
        }

        public static void ExecFarcPack(AppConfig appConfig, string fileName)
        {
            // 外部EXEを起動する
            var info = new ProcessStartInfo(appConfig.MikuMikuLibrary.FarcPack_Path)
            {
                // コマンドラインパラメータを指定する
                ArgumentList = { fileName }
            };
            Process? p = Process.Start(info);

            if (p != null)
            {
                // 別EXEが終了するまで待つ場合
                p.WaitForExit();
                //Console.WriteLine("DataBaseConverter 実行完了");

                // 別EXEを強制終了する場合
                // System.Threading.Thread.Sleep(5000);
                // p.Kill();

                // 別EXEからプログラムの終了コードを受け取る場合
                //int code = p.ExitCode;
            }
        }
    }
}
