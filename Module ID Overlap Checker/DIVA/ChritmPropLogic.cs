using Microsoft.Extensions.Configuration;
using Module_ID_Overlap_Checker.Manager;
using Module_ID_Overlap_Checker.Util;
using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace Module_ID_Overlap_Checker.DIVA
{
    internal class ChritmPropLogic
    {
        public static readonly string DIR_CHRITM_PROP = "chritm_prop";
        public static readonly string FILE_FARC_CHRITM_PROP = "chritm_prop.farc";
        public static readonly string FILE_FARC_CHRITM_PROP_MOD = "mod_chritm_prop.farc";
        public static readonly string FILE_FARC_CHRITM_PROP_MDATA = "mdata_chritm_prop.farc";


        public static bool Init()
        {
            bool ret = false;
            try
            {
                string dirName = Path.GetFileNameWithoutExtension(FILE_FARC_CHRITM_PROP_MOD);

                FileUtil.Delete(ChritmPropLogic.FILE_FARC_CHRITM_PROP);
                FileUtil.Delete(ChritmPropLogic.FILE_FARC_CHRITM_PROP_MOD);
                FileUtil.Delete(ChritmPropLogic.FILE_FARC_CHRITM_PROP_MDATA);

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
            }
            catch (Exception e)
            {
                Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "ChritmProp#Init is Failed");
                ToolUtil.ErrorLog(ToolUtil.CONSOLE_PREFIX + "ChritmProp#Init is Failed");
                throw e;
            }

            return ret;
        }

        public static void Load(AppConfig appConfig, DivaModManager dmm)
        {
            foreach(var mod in dmm.Mods)
            {
                if (File.Exists(mod.Path +"/rom/"+ ChritmPropLogic.FILE_FARC_CHRITM_PROP_MOD))
                {
                    File.Copy(mod.Path + "/rom/" + ChritmPropLogic.FILE_FARC_CHRITM_PROP_MOD, ChritmPropLogic.FILE_FARC_CHRITM_PROP_MOD, true);
                    ToolUtil.ExecFarcPack(appConfig, ChritmPropLogic.FILE_FARC_CHRITM_PROP_MOD);

                    string dirName = Path.GetFileNameWithoutExtension(FILE_FARC_CHRITM_PROP_MOD);

                    foreach (var chara_key in DivaUtil.CHARA_ITM_TBL.Keys)
                    {
                        mod.Item_Tbl.Add(chara_key, Mod.GetCharaTbl(mod, chara_key, DivaUtil.CHARA_ITM_TBL[chara_key]));
                    }

                    ChritmPropLogic.Init();
                }
            }
        }

        public static void ViewChara(AppConfig config, DivaModManager dmm)
        {
            StringBuilder sb = new();
            sb.Append("mod\tchara\tkey_name\titem\tkey_no\tvalue\tLang("+config.Config.Lang+")\n");      // header

            foreach (var mod in dmm.Mods)
            {
                foreach (var chara_key in DivaUtil.CHARA_ITM_TBL.Keys)
                {
                    sb.Append(ViewCharaItems(config, chara_key, mod, DivaUtil.CHARA_ITM_TBL[chara_key]));
                }
            }

            StringBuilder sb_out = new();
            foreach(var line in sb.ToString().Split("\r\n"))
            {
                if (string.IsNullOrEmpty(line.Trim())) continue;
                sb_out.Append(line+"\n");
            }

            FileUtil.WriteFile_UTF_8_NO_BOM(sb_out.ToString(), "result.txt", false);
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
            foreach (var item_tbl in mod.Item_Tbl[chara_name])
            {
                foreach (var item in item_tbl.Items)
                {
                    if (item.Parameter.Length == 3 && item.Parameter[2] == "name")
                    {
                        Item name = new Item();
                        name.Parameter = item.Parameter;
                        name.Value = item.Value;
                        names.Add(name);
                    }
                }
            }

            List<Item> nos = new();
            foreach (var item_tbl in mod.Item_Tbl[chara_name])
            {
                foreach (var item in item_tbl.Items)
                {
                    if (item.Parameter.Length == 3 && item.Parameter[2] == "no")
                    {
                        Item no = new();
                        no.Parameter = item.Parameter;
                        no.Value = item.Value;
                        nos.Add(no);
                    }
                }
            }

            List<Item> sub_ids = new();
            foreach (var item_tbl in mod.Item_Tbl[chara_name])
            {
                foreach (var item in item_tbl.Items)
                {
                    if (item.Parameter.Length == 3 && item.Parameter[2] == "sub_id")
                    {
                        Item sub_id = new();
                        sub_id.Parameter = item.Parameter;
                        sub_id.Value = item.Value;
                        sub_ids.Add(sub_id);
                    }
                }
            }

            Result result = new(mod, chara_name, nos, names, sub_ids);
            sb.Append(result.ToString(config));

            return sb.ToString();
        }
    }
}
