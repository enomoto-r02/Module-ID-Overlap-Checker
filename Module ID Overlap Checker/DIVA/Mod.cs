using Microsoft.Extensions.Configuration;
using Module_ID_Overlap_Checker.Util;

namespace Module_ID_Overlap_Checker.DIVA
{
    public class Mod
    {
        public string Name { get; set; }
        public string Folder_Path { get; set; }
        public int Mod_Priority { get; set; }
        public bool Enabled { get; set; }

        public Dictionary<string, IConfiguration> Itm_Tbl { get; set; }

        public StrArray StrArray { get; set; }

        public Mod(int priority, string name, string enabled, string folder_path)
        {
            this.Mod_Priority = -1;
            this.Itm_Tbl = new();
            this.Mod_Priority = priority;
            this.Name = name;
            this.Enabled = bool.Parse(enabled);
            this.Folder_Path = folder_path;
            this.StrArray = new(this);
        }

        public static IConfigurationRoot GetCharaTbl(Mod mod, string fileTextItemTable)
        {
            IConfigurationRoot ret = null;

            var builder = new ConfigurationBuilder();
            var dir = "./" + Path.GetFileNameWithoutExtension(ChritmPropLogic.FILE_FARC_CHRITM_PROP_MOD) + "/" + fileTextItemTable;
            if (File.Exists(dir))
            {
                builder.AddIniFile(dir);
                try
                {
                    ret = builder.Build();
                }
                catch (Exception e)
                {
                    ToolUtil.ErrorLog("\"" + mod.Name + "\" mod is Failed. \n" + e.Message + "\n" + e.InnerException);
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

                chara_itm_tbl = Itm_Tbl[chara];
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
