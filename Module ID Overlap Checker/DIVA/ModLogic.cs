using Module_ID_Overlap_Checker.Manager;
using Module_ID_Overlap_Checker.Util;
using System.Text;

namespace Module_ID_Overlap_Checker.DIVA
{
    internal class ModLogic
    {
        public static readonly string DIR_CHRITM_PROP = "chritm_prop";
        public static readonly string FILE_FARC_CHRITM_PROP = "chritm_prop.farc";
        public static readonly string FILE_FARC_CHRITM_PROP_MOD = "mod_chritm_prop.farc";
        public static readonly string FILE_FARC_CHRITM_PROP_MDATA = "mdata_chritm_prop.farc";


        public static bool Init()
        {
            bool ret = false;

            string dirName = Path.GetFileNameWithoutExtension(FILE_FARC_CHRITM_PROP_MOD);

            FileUtil.Delete(ModLogic.FILE_FARC_CHRITM_PROP);
            FileUtil.Delete(ModLogic.FILE_FARC_CHRITM_PROP_MOD);
            FileUtil.Delete(ModLogic.FILE_FARC_CHRITM_PROP_MDATA);
            FileUtil.Delete(ToolUtil.FILE_RESULT);
            FileUtil.Delete(ToolUtil.FILE_LOG);

            foreach (var key in DivaUtil.CHARA_ITM_TBL.Keys)
            {
                FileUtil.Delete(dirName + "/" + DivaUtil.CHARA_ITM_TBL[key]);
            }

            // フォルダが空なら削除
            if (Directory.Exists(dirName) && Directory.EnumerateFileSystemEntries(dirName).Any() == false)
            {
                Directory.Delete(dirName);
            }

            ret = true;

            return ret;
        }

        public static void Load(AppConfig appConfig, DivaModManager dmm)
        {
            foreach (var mod in dmm.Mods)
            {
                Console.WriteLine($"{ToolUtil.CONSOLE_PREFIX}[Load] {mod.Name} Loading.");
                mod.GmModuleTblLoad();
                if (File.Exists(mod.Path + "/rom/" + ModLogic.FILE_FARC_CHRITM_PROP_MOD))
                {
                    File.Copy(mod.Path + "/rom/" + ModLogic.FILE_FARC_CHRITM_PROP_MOD, ModLogic.FILE_FARC_CHRITM_PROP_MOD, true);
                    ToolUtil.ExecFarcPack(appConfig, ModLogic.FILE_FARC_CHRITM_PROP_MOD);

                    string dirName = Path.GetFileNameWithoutExtension(FILE_FARC_CHRITM_PROP_MOD);

                    foreach (var chara_key in DivaUtil.CHARA_ITM_TBL.Keys)
                    {
                        mod.Item_Tbl.Add(chara_key, Mod.GetCharaTbl(mod, chara_key, DivaUtil.CHARA_ITM_TBL[chara_key]));
                    }

                    ModLogic.Init();
                }


            }
        }

        public static void ViewChara(AppConfig config, DivaModManager dmm)
        {
            StringBuilder sb = new();

            // header
            sb.Append(string.Join("\t",
                "Mod Name",
                "Chara",
                "Cos ID",
                "Module Key",
                "Module ID",
                "Item No",
                "Sub ID",
                "Item Name",
                "Item Name(" + config.Config.Lang + ")")
            );
            sb.Append("\n");

            foreach (var mod in dmm.Mods)
            {
                Console.WriteLine($"{ToolUtil.CONSOLE_PREFIX}[Execute] {mod.Name} Execute.");
                foreach (var chara_key in DivaUtil.CHARA_ITM_TBL.Keys)
                {
                    sb.Append(ViewCharaItems(config, chara_key, mod, DivaUtil.CHARA_ITM_TBL[chara_key]));
                }
            }

            StringBuilder sb_out = new();
            foreach (var line in sb.ToString().Split("\r\n"))
            {
                if (string.IsNullOrEmpty(line.Trim())) continue;
                sb_out.Append(line + "\n");
            }

            FileUtil.WriteFile_UTF_8_NO_BOM(sb_out.ToString(), ToolUtil.FILE_RESULT, false);
        }

        private static string ViewCharaItems(AppConfig config, string chara_name, Mod mod, string file_name)
        {
            if (mod.Item_Tbl == null || mod.Item_Tbl.Count == 0)
            {
                return null;
            }

            StringBuilder sb = new();

            if (mod.Item_Tbl[chara_name].Count == 0)
            {
                return null;
            }


            List<Item> Gm_Module_ItemTbl = new();
            if (mod.Module.Module_ItemTbl == null)
            {
                return null;
            }
            foreach (var item in mod.Module.Module_ItemTbl.Items)
            {
                Item tmp = new Item();
                tmp.Parameter = item.Parameter;
                tmp.Value = item.Value;
                Gm_Module_ItemTbl.Add(tmp);
            }

            List<Item> GmCustomizeModule = new();
            foreach (var item in mod.CustomizeModule.Module_ItemTbl.Items)
            {
                Item tmp = new Item();
                tmp.Parameter = item.Parameter;
                tmp.Value = item.Value;
                GmCustomizeModule.Add(tmp);
            }

            List<Item> Item_Tbl = new();
            foreach (var item_tbl in mod.Item_Tbl[chara_name])
            {
                foreach (var item in item_tbl.Items)
                {
                    Item tmp = new();
                    tmp.Parameter = item.Parameter;
                    tmp.Value = item.Value;
                    Item_Tbl.Add(tmp);
                }
            }

            mod.ModDB = new ModDataBase(Item_Tbl, Gm_Module_ItemTbl, GmCustomizeModule);
            Result result = new(mod, chara_name);

            sb.Append(result.ToString(config));

            return sb.ToString();
        }
    }
}
