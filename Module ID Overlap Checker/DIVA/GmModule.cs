using Microsoft.Extensions.Configuration;
using Module_ID_Overlap_Checker.Util;
using Nett;

namespace Module_ID_Overlap_Checker.DIVA
{
    public class GmModule
    {
        public static readonly string FILE_FRAC_GM_MODULE_MOD = "mod_gm_module_tbl.farc";
        public static readonly string FILE_BIN_GM_MODULE_MOD = "gm_module_id.bin";

        public Mod Mod { get; set; }

        // DB全体
        public ItemTbl Gm_Module_ItemTbl { get; set; }

        // モジュールのみを抜粋したもの
        public ItemTbl Gm_Module_Item { get; set; }

        private string ExtractFilePath { get; set; }
        private string ExtractDirectoryPath { get; set; }

        public GmModule(Mod mod)
        {
            this.Mod = mod;
            this.Gm_Module_ItemTbl = new ItemTbl(mod);
            this.Gm_Module_Item = new ItemTbl(mod);
        }

        public void Load()
        {
            this.Copy();
            this.Extract();
            this.BinLoad();
            this.LoadModuleLine();
            this.Delete();
        }

        private void Copy()
        {
            var path = string.Join("/", this.Mod.Path, "rom", FILE_FRAC_GM_MODULE_MOD);

            if (File.Exists(path))
            {
                File.Copy(path, FILE_FRAC_GM_MODULE_MOD, true);
            }
            else
            {
                this.Gm_Module_ItemTbl = null;
            }
        }

        private void Extract()
        {
            var path = FILE_FRAC_GM_MODULE_MOD;

            if (File.Exists(path))
            {
                ToolUtil.ExecFarcPack(Program.appConfig, path);
            }
            else
            {
                this.Gm_Module_ItemTbl = null;
            }
        }

        private void BinLoad()
        {
            if (this.Gm_Module_ItemTbl == null)
            {
                return;
            }

            this.ExtractDirectoryPath = Path.GetFileNameWithoutExtension(FILE_FRAC_GM_MODULE_MOD);
            this.ExtractFilePath = string.Join("/", this.ExtractDirectoryPath, FILE_BIN_GM_MODULE_MOD);

            this.Gm_Module_ItemTbl.SetItems(this.ExtractFilePath);
        }

        private void LoadModuleLine()
        {
            if (this.Gm_Module_ItemTbl == null)
            {
                return;
            }

            foreach (var line in this.Gm_Module_ItemTbl.Items)
            {
                if (line.Parameter[2] == "id")
                {
                    this.Gm_Module_Item.Items.Add(line);
                }
            }
        }

        private void Delete()
        {
            if(File.Exists(this.ExtractFilePath))
            {
                File.Delete(this.ExtractFilePath);
            }

            // フォルダが空なら削除
            if (Directory.Exists(this.ExtractDirectoryPath) && Directory.EnumerateFileSystemEntries(this.ExtractDirectoryPath).Any() == false)
            {
                Directory.Delete(this.ExtractDirectoryPath);
            }
        }

        public string GetGmModuleData(AppConfig config, string key, string str_jp)
        {
            if (this.Gm_Module_ItemTbl == null)
            {
                return "";
            }

            return this.Gm_Module_ItemTbl.GetItemValue(key);
        }
    }
}
