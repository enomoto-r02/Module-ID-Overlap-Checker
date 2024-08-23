using Module_ID_Overlap_Checker.Manager;
using Module_ID_Overlap_Checker.Util;
using System.Security.Cryptography.X509Certificates;
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
            sb.Append(string.Join("\t", 
                "Mod Name", 
                "Chara",
                "Module ID",
                "Sub ID", 
                "Module Name",
                "Module Name(" + config.Config.Lang + ")")
            +"\n");      // header

            foreach (var mod in dmm.Mods)
            {
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


            List<Item> names = new();
            foreach (var item in mod.gmModule.Gm_Module_ItemTbl.Items)
            {
                if (item.Parameter.Length == 3 && item.Parameter[0] == "module" && item.Parameter[2] == "name")
                {
                    Item name = new Item();
                    name.Parameter = item.Parameter;
                    name.Value = item.Value;
                    names.Add(name);
                }
            }

            List<Item> customize_names = new();
            foreach (var item in mod.gmCustomizeModule.Gm_Module_ItemTbl.Items)
            {
                if (item.Parameter.Length == 3 && item.Parameter[0] == "cstm_item" && item.Parameter[2] == "name")
                {
                    Item name = new Item();
                    name.Parameter = item.Parameter;
                    name.Value = item.Value;
                    customize_names.Add(name);
                }
            }

            List<Item> nos = new();
            foreach (var item_tbl in mod.Item_Tbl[chara_name])
            {
                foreach (var item in item_tbl.Items)
                {
                    if (item.Parameter.Length == 3 && item.Parameter[0] == "item" && item.Parameter[2] == "no")
                    {
                        Item no = new();
                        no.Parameter = item.Parameter;
                        no.Value = item.Value;
                        nos.Add(no);
                    }
                }
            }

            List<Item> cos_ids = new();
            foreach (var item_tbl in mod.Item_Tbl[chara_name])
            {
                foreach (var item in item_tbl.Items)
                {
                    if (item.Parameter.Length == 4 && item.Parameter[0] == "cos" && item.Parameter[2] == "item")
                    {
                        Item id = new();
                        id.Parameter = item.Parameter;
                        id.Value = item.Value;
                        cos_ids.Add(id);
                    }
                }
            }

            List<Item> sub_ids = new();
            foreach (var item_tbl in mod.Item_Tbl[chara_name])
            {
                foreach (var item in item_tbl.Items)
                {
                    if (item.Parameter.Length == 3 && item.Parameter[0] == "item" && item.Parameter[2] == "sub_id")
                    {
                        Item id = new();
                        id.Parameter = item.Parameter;
                        id.Value = item.Value;
                        sub_ids.Add(id);
                    }
                }
            }

            mod.modDB = new ModDataBase(nos, names, sub_ids, cos_ids, customize_names);
            Result result = new(mod, chara_name);

            sb.Append(result.ToString(config));

            return sb.ToString();
        }
    }
}
