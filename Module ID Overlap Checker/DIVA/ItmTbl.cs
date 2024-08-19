using Microsoft.Extensions.Configuration;
using Module_ID_Overlap_Checker.Util;

namespace Module_ID_Overlap_Checker.DIVA
{
    public class ItmTbl
    {
        public ItmTbl(Mod mod)
        {
            this.Mod_Name = mod.Name;
            this.Mod_Folder_Path = mod.Folder_Path;
            this.itmData = new(mod);
        }
        public string Mod_Name { get; set; }
        public string Mod_Folder_Path { get; set; }

        public ItmData itmData { get; set; }
    }

    public class ItmData
    {
        public string Mod_Name { get; set; }
        public string Mod_Folder_Path { get; set; }

        public Dictionary<string, IConfiguration> itm_tbl {  get; set; }

        public ItmData(Mod mod)
        {
            this.Mod_Name = mod.Name;
            this.Mod_Folder_Path = mod.Folder_Path;
            this.itm_tbl = new();
        }

        public static IConfigurationRoot GetCharaTbl(string dirName, string fileTextItemTable)
        {
            IConfigurationRoot ret = null;

            var builder = new ConfigurationBuilder();
            var dir = "./" + dirName + "/" + fileTextItemTable;
            if (File.Exists(dir))
            {
                builder.AddIniFile(dir);
                try
                {
                    ret = builder.Build();
                }
                catch (Exception e)
                {
                    ToolUtil.ErrorLog(e.Message + " : " + e.StackTrace);
                    ret = null;

                    //throw e;
                }
            }

            return ret;
        }


        public string GetCharaData(string chara, string key)
        {
            IConfiguration chara_itm_tbl = null;
            try
            {

                chara_itm_tbl = itm_tbl[chara];
            }
            catch (KeyNotFoundException knfe)
            {
                return null;
            }

            if (chara_itm_tbl == null) { return null; }
            return chara_itm_tbl[key];
        }
    }
}
