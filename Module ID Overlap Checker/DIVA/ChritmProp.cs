using Microsoft.Extensions.Configuration;
using Module_ID_Overlap_Checker.Manager;
using Module_ID_Overlap_Checker.Util;
using System.Diagnostics;
using System.Text;

namespace Module_ID_Overlap_Checker.DIVA
{
    internal class ChritmProp
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

        public string Mod_Folder { get; set; }


        public static bool Init()
        {
            bool ret = false;
            try
            {
                string dirName = Path.GetFileNameWithoutExtension(FILE_FARC_CHRITM_PROP_MOD);

                FileUtil.Delete(ChritmProp.FILE_FARC_CHRITM_PROP);
                FileUtil.Delete(ChritmProp.FILE_FARC_CHRITM_PROP_MOD);
                FileUtil.Delete(ChritmProp.FILE_FARC_CHRITM_PROP_MDATA);

                FileUtil.Delete(dirName + "/" + ChritmProp.FILE_TXT_ITEM_TABLE_MIK);
                FileUtil.Delete(dirName + "/" + ChritmProp.FILE_TXT_ITEM_TABLE_RIN);
                FileUtil.Delete(dirName + "/" + ChritmProp.FILE_TXT_ITEM_TABLE_LEN);
                FileUtil.Delete(dirName + "/" + ChritmProp.FILE_TXT_ITEM_TABLE_LUK);
                FileUtil.Delete(dirName + "/" + ChritmProp.FILE_TXT_ITEM_TABLE_MEI);
                FileUtil.Delete(dirName + "/" + ChritmProp.FILE_TXT_ITEM_TABLE_KAI);
                FileUtil.Delete(dirName + "/" + ChritmProp.FILE_TXT_ITEM_TABLE_HAK);
                FileUtil.Delete(dirName + "/" + ChritmProp.FILE_TXT_ITEM_TABLE_TET);
                FileUtil.Delete(dirName + "/" + ChritmProp.FILE_TXT_ITEM_TABLE_NER);
                FileUtil.Delete(dirName + "/" + ChritmProp.FILE_TXT_ITEM_TABLE_SAK);

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
                if (File.Exists(mod.Folder_Path +"/rom/"+ ChritmProp.FILE_FARC_CHRITM_PROP_MOD))
                {
                    File.Copy(mod.Folder_Path + "/rom/" + ChritmProp.FILE_FARC_CHRITM_PROP_MOD, ChritmProp.FILE_FARC_CHRITM_PROP_MOD, true);
                    ToolUtil.ExecFarcPack(appConfig, ChritmProp.FILE_FARC_CHRITM_PROP_MOD);

                    string dirName = Path.GetFileNameWithoutExtension(FILE_FARC_CHRITM_PROP_MOD);

                    ItmTbl itm_tbl = new ItmTbl(mod);

                    itm_tbl.itmData.itm_tbl.Add("MIK", ItmData.GetCharaTbl(dirName, ChritmProp.FILE_TXT_ITEM_TABLE_MIK));
                    itm_tbl.itmData.itm_tbl.Add("RIN", ItmData.GetCharaTbl(dirName, ChritmProp.FILE_TXT_ITEM_TABLE_RIN));
                    itm_tbl.itmData.itm_tbl.Add("LEN", ItmData.GetCharaTbl(dirName, ChritmProp.FILE_TXT_ITEM_TABLE_LEN));
                    itm_tbl.itmData.itm_tbl.Add("LUK", ItmData.GetCharaTbl(dirName, ChritmProp.FILE_TXT_ITEM_TABLE_LUK));
                    itm_tbl.itmData.itm_tbl.Add("MEI", ItmData.GetCharaTbl(dirName, ChritmProp.FILE_TXT_ITEM_TABLE_MEI));
                    itm_tbl.itmData.itm_tbl.Add("KAI", ItmData.GetCharaTbl(dirName, ChritmProp.FILE_TXT_ITEM_TABLE_KAI));
                    itm_tbl.itmData.itm_tbl.Add("HAK", ItmData.GetCharaTbl(dirName, ChritmProp.FILE_TXT_ITEM_TABLE_HAK));
                    itm_tbl.itmData.itm_tbl.Add("TET", ItmData.GetCharaTbl(dirName, ChritmProp.FILE_TXT_ITEM_TABLE_TET));
                    itm_tbl.itmData.itm_tbl.Add("NER", ItmData.GetCharaTbl(dirName, ChritmProp.FILE_TXT_ITEM_TABLE_NER));
                    itm_tbl.itmData.itm_tbl.Add("SAK", ItmData.GetCharaTbl(dirName, ChritmProp.FILE_TXT_ITEM_TABLE_SAK));

                    mod.itmTbl = itm_tbl;

                    ChritmProp.Init();
                }
            }
        }

        public static void ViewTest(DivaModManager dmm)
        {
            StringBuilder sb = new();

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

            FileUtil.WriteFile_UTF_8_NO_BOM(sb.ToString(), "result.txt", false);
        }

        private static string ViewTestChara(string chara_name, Mod mod, string key)
        {
            if (mod.itmTbl == null)
            {
                return null;
            }

            var data = mod.itmTbl.itmData.GetCharaData(chara_name, "item.length");
            if (data == null) { return null; }
            int item_length = int.Parse(data);

            string s = "";
            for (int i = 0; i < item_length; i++)
            {
                var key_name = "item." + i.ToString() + ".name";
                var key_no = "item." + i.ToString() + ".no";
                s += string.Join("\t", 
                    "[" + mod.Name + "]", 
                    mod.itmTbl.itmData.GetCharaData(chara_name, key_name), 
                    chara_name, mod.itmTbl.itmData.GetCharaData(chara_name, key_no)
                ) + "\n";
            }

            return s;

        }
    }
}
