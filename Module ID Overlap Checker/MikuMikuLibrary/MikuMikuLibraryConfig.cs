using System.Diagnostics;

namespace Module_ID_Overlap_Checker.MikuMikuLibrary
{
    public class MikuMikuLibraryConfig
    {
        public static string FILE_FARC_PACK = "FarcPack.exe";
        public string Path { get; set; }
        public string FarcPack_Path { get { return this.Path + "\\" + FILE_FARC_PACK; } }

        public MikuMikuLibraryConfig()
        {
        }

        public void Load(AppConfig appConfig, string fileName)
        {
            // 外部EXEを起動する
            var info = new ProcessStartInfo(FarcPack_Path)
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
