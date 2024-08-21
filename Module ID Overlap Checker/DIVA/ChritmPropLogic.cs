using Microsoft.Extensions.Configuration;
using Module_ID_Overlap_Checker.Manager;
using Module_ID_Overlap_Checker.Util;
using System.Diagnostics;
using System.Text;

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
                if (File.Exists(mod.Folder_Path +"/rom/"+ ChritmPropLogic.FILE_FARC_CHRITM_PROP_MOD))
                {
                    File.Copy(mod.Folder_Path + "/rom/" + ChritmPropLogic.FILE_FARC_CHRITM_PROP_MOD, ChritmPropLogic.FILE_FARC_CHRITM_PROP_MOD, true);
                    ToolUtil.ExecFarcPack(appConfig, ChritmPropLogic.FILE_FARC_CHRITM_PROP_MOD);

                    string dirName = Path.GetFileNameWithoutExtension(FILE_FARC_CHRITM_PROP_MOD);

                    mod.Itm_Tbl.Add("MIK", Mod.GetCharaTbl(mod, ChritmPropLogic.FILE_TXT_ITEM_TABLE_MIK));
                    mod.Itm_Tbl.Add("RIN", Mod.GetCharaTbl(mod, ChritmPropLogic.FILE_TXT_ITEM_TABLE_RIN));
                    mod.Itm_Tbl.Add("LEN", Mod.GetCharaTbl(mod, ChritmPropLogic.FILE_TXT_ITEM_TABLE_LEN));
                    mod.Itm_Tbl.Add("LUK", Mod.GetCharaTbl(mod, ChritmPropLogic.FILE_TXT_ITEM_TABLE_LUK));
                    mod.Itm_Tbl.Add("MEI", Mod.GetCharaTbl(mod, ChritmPropLogic.FILE_TXT_ITEM_TABLE_MEI));
                    mod.Itm_Tbl.Add("KAI", Mod.GetCharaTbl(mod, ChritmPropLogic.FILE_TXT_ITEM_TABLE_KAI));
                    mod.Itm_Tbl.Add("HAK", Mod.GetCharaTbl(mod, ChritmPropLogic.FILE_TXT_ITEM_TABLE_HAK));
                    mod.Itm_Tbl.Add("TET", Mod.GetCharaTbl(mod, ChritmPropLogic.FILE_TXT_ITEM_TABLE_TET));
                    mod.Itm_Tbl.Add("NER", Mod.GetCharaTbl(mod, ChritmPropLogic.FILE_TXT_ITEM_TABLE_NER));
                    mod.Itm_Tbl.Add("SAK", Mod.GetCharaTbl(mod, ChritmPropLogic.FILE_TXT_ITEM_TABLE_SAK));

                    ChritmPropLogic.Init();
                }
            }
        }

        public static void ViewTest(DivaModManager dmm)
        {
            StringBuilder sb = new();
            sb.Append("mod\tchara\tkey_name\titem\tkey_no\tvalue\n");      // header

            foreach (var mod in dmm.Mods)
            {
                sb.AppendLine(ViewTestChara("MIK", mod, FILE_TXT_ITEM_TABLE_MIK));
                sb.AppendLine(ViewTestChara("RIN", mod, FILE_TXT_ITEM_TABLE_RIN));
                sb.AppendLine(ViewTestChara("LEN", mod, FILE_TXT_ITEM_TABLE_LEN));
                sb.AppendLine(ViewTestChara("LUK", mod, FILE_TXT_ITEM_TABLE_LUK));
                sb.AppendLine(ViewTestChara("MEI", mod, FILE_TXT_ITEM_TABLE_MEI));
                sb.AppendLine(ViewTestChara("KAI", mod, FILE_TXT_ITEM_TABLE_KAI));
                sb.AppendLine(ViewTestChara("HAK", mod, FILE_TXT_ITEM_TABLE_HAK));
                sb.AppendLine(ViewTestChara("TET", mod, FILE_TXT_ITEM_TABLE_TET));
                sb.AppendLine(ViewTestChara("NER", mod, FILE_TXT_ITEM_TABLE_NER));
                sb.AppendLine(ViewTestChara("SAK", mod, FILE_TXT_ITEM_TABLE_SAK));
            }

            StringBuilder sb_out = new();
            foreach(var line in sb.ToString().Split("\n"))
            {
                if (string.IsNullOrEmpty(line.Trim())) continue;
                sb_out.AppendLine(line);
            }

            FileUtil.WriteFile_UTF_8_NO_BOM(sb_out.ToString(), "result.txt", false);
        }

        private static string ViewTestChara(string chara_name, Mod mod, string key)
        {
            if (mod.Itm_Tbl == null)
            {
                return null;
            }

            var data = mod.GetCharaData(chara_name, "item.length");
            if (data == null) { return null; }
            int item_length = int.Parse(data);

            string s = "";


            for (int i = 0; i < item_length; i++)
            {
                var key_name = "item." + i.ToString() + ".name";
                var key_no = "item." + i.ToString() + ".no";
                s += string.Join("\t",
                    "[" + mod.Name + "]",
                    chara_name,
                    key_name,
                    mod.GetCharaData(chara_name, key_name),
                    key_no,
                    mod.GetCharaData(chara_name, key_no)
                ) + "\n";
            }

            return s;

        }
    }
}
