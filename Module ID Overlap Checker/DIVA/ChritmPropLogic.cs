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

        public static readonly string FILE_TXT_ITEM_TABLE_MIK = "mikitm_tbl.txt";
        public static readonly string FILE_TXT_ITEM_TABLE_RIN = "rinitm_tbl.txt";
        public static readonly string FILE_TXT_ITEM_TABLE_LEN = "lenitm_tbl.txt";
        public static readonly string FILE_TXT_ITEM_TABLE_LUK = "lukitm_tbl.txt";
        public static readonly string FILE_TXT_ITEM_TABLE_MEI = "meiitm_tbl.txt";
        public static readonly string FILE_TXT_ITEM_TABLE_KAI = "kaiitm_tbl.txt";
        public static readonly string FILE_TXT_ITEM_TABLE_HAK = "hakitm_tbl.txt";
        public static readonly string FILE_TXT_ITEM_TABLE_TET = "tetitm_tbl.txt";
        public static readonly string FILE_TXT_ITEM_TABLE_NER = "neritm_tbl.txt";
        public static readonly string FILE_TXT_ITEM_TABLE_SAK = "sakitm_tbl.txt";


        public static bool Init()
        {
            bool ret = false;
            try
            {
                string dirName = Path.GetFileNameWithoutExtension(FILE_FARC_CHRITM_PROP_MOD);

                FileUtil.Delete(ChritmPropLogic.FILE_FARC_CHRITM_PROP);
                FileUtil.Delete(ChritmPropLogic.FILE_FARC_CHRITM_PROP_MOD);
                FileUtil.Delete(ChritmPropLogic.FILE_FARC_CHRITM_PROP_MDATA);

                FileUtil.Delete(dirName + "/" + ChritmPropLogic.FILE_TXT_ITEM_TABLE_MIK);
                FileUtil.Delete(dirName + "/" + ChritmPropLogic.FILE_TXT_ITEM_TABLE_RIN);
                FileUtil.Delete(dirName + "/" + ChritmPropLogic.FILE_TXT_ITEM_TABLE_LEN);
                FileUtil.Delete(dirName + "/" + ChritmPropLogic.FILE_TXT_ITEM_TABLE_LUK);
                FileUtil.Delete(dirName + "/" + ChritmPropLogic.FILE_TXT_ITEM_TABLE_MEI);
                FileUtil.Delete(dirName + "/" + ChritmPropLogic.FILE_TXT_ITEM_TABLE_KAI);
                FileUtil.Delete(dirName + "/" + ChritmPropLogic.FILE_TXT_ITEM_TABLE_HAK);
                FileUtil.Delete(dirName + "/" + ChritmPropLogic.FILE_TXT_ITEM_TABLE_TET);
                FileUtil.Delete(dirName + "/" + ChritmPropLogic.FILE_TXT_ITEM_TABLE_NER);
                FileUtil.Delete(dirName + "/" + ChritmPropLogic.FILE_TXT_ITEM_TABLE_SAK);

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

                    mod.Item_Tbl.Add("MIK", Mod.GetCharaTbl2(mod, "MIK", ChritmPropLogic.FILE_TXT_ITEM_TABLE_MIK));
                    mod.Item_Tbl.Add("RIN", Mod.GetCharaTbl2(mod, "RIN", ChritmPropLogic.FILE_TXT_ITEM_TABLE_RIN));
                    mod.Item_Tbl.Add("LEN", Mod.GetCharaTbl2(mod, "LEN", ChritmPropLogic.FILE_TXT_ITEM_TABLE_LEN));
                    mod.Item_Tbl.Add("LUK", Mod.GetCharaTbl2(mod, "LUK", ChritmPropLogic.FILE_TXT_ITEM_TABLE_LUK));
                    mod.Item_Tbl.Add("MEI", Mod.GetCharaTbl2(mod, "MEI", ChritmPropLogic.FILE_TXT_ITEM_TABLE_MEI));
                    mod.Item_Tbl.Add("KAI", Mod.GetCharaTbl2(mod, "KAI", ChritmPropLogic.FILE_TXT_ITEM_TABLE_KAI));
                    mod.Item_Tbl.Add("HAK", Mod.GetCharaTbl2(mod, "HAK", ChritmPropLogic.FILE_TXT_ITEM_TABLE_HAK));
                    mod.Item_Tbl.Add("TET", Mod.GetCharaTbl2(mod, "TET", ChritmPropLogic.FILE_TXT_ITEM_TABLE_TET));
                    mod.Item_Tbl.Add("NER", Mod.GetCharaTbl2(mod, "NER", ChritmPropLogic.FILE_TXT_ITEM_TABLE_NER));
                    mod.Item_Tbl.Add("SAK", Mod.GetCharaTbl2(mod, "SAK", ChritmPropLogic.FILE_TXT_ITEM_TABLE_SAK));

                    ChritmPropLogic.Init();
                }
            }
        }

        public static void ViewTest(AppConfig config, DivaModManager dmm)
        {
            StringBuilder sb = new();
            sb.Append("mod\tchara\tkey_name\titem\tkey_no\tvalue\tLang("+config.Config.Lang+")\n");      // header

            foreach (var mod in dmm.Mods)
            {
                foreach (var chara_key in DivaUtil.CHARA_ITM_TBL.Keys)
                {
                    sb.Append(ViewTestChara(config, chara_key, mod, DivaUtil.CHARA_ITM_TBL[chara_key]));
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

        private static string ViewTestChara(AppConfig config, string chara_name, Mod mod, string file_name)
        {
            if (mod.Item_Tbl == null || mod.Item_Tbl.Count == 0)
            {
                return null;
            }

            StringBuilder sb = new();

            Item name = new();
            Item no= new();

            var chara = "RIN";

            if (mod.Item_Tbl[chara].Count == 0)
            {
                return null;
            }

            foreach (var item_tbl in mod.Item_Tbl[chara])
            {
                foreach (var item in item_tbl.Items)
                {
                    if (item.Parameter.Length == 3 && item.Parameter[2] == "name")
                    {
                        name.Parameter = item.Parameter;
                        name.Value = item.Value;
                    }
                }
            }
            foreach (var item_tbl in mod.Item_Tbl[chara])
            {
                foreach (var item in item_tbl.Items)
                {
                    if (item.Parameter.Length == 3 && item.Parameter[2] == "no")
                    {
                        no.Parameter = item.Parameter;
                        no.Value = item.Value;
                    }
                }
            }

            Result result = new(mod, chara_name, no, name);
            sb.Append(result.ToString(config));

            return sb.ToString();
        }
    }
}
