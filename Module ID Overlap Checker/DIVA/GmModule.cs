using Module_ID_Overlap_Checker.Util;

namespace Module_ID_Overlap_Checker.DIVA
{
    public class GmModule
    {
        public static readonly string FILE_FRAC_MODULE_MOD = "mod_gm_module_tbl.farc";
        public static readonly string FILE_BIN_MODULE_MOD = "gm_module_id.bin";
        public static readonly string FILE_FRAC_CUSTOMIZE_MODULE_MOD = "mod_gm_customize_item_tbl.farc";
        public static readonly string FILE_BIN_CUSTOMIZE_MODULE_MOD = "gm_customize_item_id.bin";

        public Mod Mod { get; set; }

        // DB全体
        public ItemTbl Module_ItemTbl { get; set; }

        // モジュールのみを抜粋したもの
        public ItemTbl Module_Item { get; set; }

        // DB全体
        public ItemTbl CustomizeModule_ItemTbl { get; set; }

        // モジュールのみを抜粋したもの
        public ItemTbl Customize_Item { get; set; }

        private string ExtractFilePath { get; set; }
        private string ExtractDirectoryPath { get; set; }
        private string ExtractCustomizeFilePath { get; set; }
        private string ExtractCustomizeDirectoryPath { get; set; }

        public GmModule(Mod mod)
        {
            this.Mod = mod;
            this.Module_ItemTbl = new(mod);
            this.Module_Item = new(mod);
            this.CustomizeModule_ItemTbl = new(mod);
            this.Customize_Item = new(mod);
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
            var path = string.Join("/", this.Mod.Path, "rom", FILE_FRAC_MODULE_MOD);

            if (File.Exists(path))
            {
                File.Copy(path, FILE_FRAC_MODULE_MOD, true);
            }
            else
            {
                this.Module_ItemTbl = null;
            }

            var path_cust = string.Join("/", this.Mod.Path, "rom", FILE_FRAC_CUSTOMIZE_MODULE_MOD);
            if (File.Exists(path_cust))
            {
                File.Copy(path_cust, FILE_FRAC_CUSTOMIZE_MODULE_MOD, true);
            }
            else
            {
                this.CustomizeModule_ItemTbl = null;
            }
        }

        private void Extract()
        {
            var path = FILE_FRAC_MODULE_MOD;

            if (File.Exists(path))
            {
                ToolUtil.ExecFarcPack(Program.appConfig, path);
            }
            else
            {
                this.Module_ItemTbl = null;
            }

            var path_cust = FILE_FRAC_CUSTOMIZE_MODULE_MOD;

            if (File.Exists(path_cust))
            {
                ToolUtil.ExecFarcPack(Program.appConfig, path_cust);
            }
            else
            {
                this.CustomizeModule_ItemTbl = null;
            }
        }

        private void BinLoad()
        {
            if (this.Module_ItemTbl != null)
            {
                this.ExtractDirectoryPath = Path.GetFileNameWithoutExtension(FILE_FRAC_MODULE_MOD);
                this.ExtractFilePath = string.Join("/", this.ExtractDirectoryPath, FILE_BIN_MODULE_MOD);

                this.Module_ItemTbl.SetItems(this.ExtractFilePath);
            }


            if (this.CustomizeModule_ItemTbl != null)
            {
                this.ExtractCustomizeDirectoryPath = Path.GetFileNameWithoutExtension(FILE_FRAC_CUSTOMIZE_MODULE_MOD);
                this.ExtractCustomizeFilePath = string.Join("/", this.ExtractCustomizeDirectoryPath, FILE_BIN_CUSTOMIZE_MODULE_MOD);

                this.CustomizeModule_ItemTbl.SetItems(this.ExtractCustomizeFilePath);
            }
        }

        private void LoadModuleLine()
        {
            if (this.Module_ItemTbl != null)
            {
                foreach (var line in this.Module_ItemTbl.Items)
                {
                    if (line.Parameter[2] == "id")
                    {
                        this.Module_Item.Items.Add(line);
                    }
                }
            }
        }

        private void Delete()
        {
            if (File.Exists(this.ExtractFilePath))
            {
                File.Delete(this.ExtractFilePath);
            }
            if (Directory.Exists(this.ExtractDirectoryPath) && Directory.EnumerateFileSystemEntries(this.ExtractDirectoryPath).Any() == false)
            {
                Directory.Delete(this.ExtractDirectoryPath);
            }

            if (File.Exists(this.ExtractCustomizeFilePath))
            {
                File.Delete(this.ExtractCustomizeFilePath);
            }
            if (Directory.Exists(this.ExtractCustomizeDirectoryPath) && Directory.EnumerateFileSystemEntries(this.ExtractCustomizeDirectoryPath).Any() == false)
            {
                Directory.Delete(this.ExtractCustomizeDirectoryPath);
            }
        }

        public string GetModuleData(string key, string str_jp)
        {
            if (this.Module_ItemTbl == null)
            {
                return "";
            }

            return this.Module_ItemTbl.GetItemValue(key);
        }

        public string GetCustomizeModuleData(string key, string str_jp)
        {
            if (this.CustomizeModule_ItemTbl == null)
            {
                return "";
            }

            return this.CustomizeModule_ItemTbl.GetItemValue(key);
        }

        public Item GetModuleByValue(string key_regex, string value)
        {
            if (this.Module_ItemTbl == null)
            {
                return new();
            }

            return this.Module_ItemTbl.GetItemValueByRegex(key_regex, value);
        }

        public Item GetCustomizeModuleByValue(string key_regex, string value)
        {
            if (this.CustomizeModule_ItemTbl == null || this.CustomizeModule_ItemTbl.Items == null)
            {
                return new();
            }

            return this.CustomizeModule_ItemTbl.GetItemValueByRegex(key_regex, value);
        }
    }
}
