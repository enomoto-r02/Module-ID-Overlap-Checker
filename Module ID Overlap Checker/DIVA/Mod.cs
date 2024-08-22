using Microsoft.Extensions.Configuration;
using Nett;

namespace Module_ID_Overlap_Checker.DIVA
{
    public class Mod
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public int Mod_Priority { get; set; }
        public bool Enabled { get; set; }

        public Dictionary<string, List<ItemTbl>> Item_Tbl { get; set; }

        public StrArray StrArray { get; set; }

        public Mod(int priority, string name, string enabled, string folder_path)
        {
            this.Mod_Priority = -1;
            this.Mod_Priority = priority;
            this.Name = name;
            this.Path = folder_path;
            this.Enabled = bool.Parse(enabled);
            this.Item_Tbl = new();
            this.StrArray = new(this);
        }

        public static List<ItemTbl> GetCharaTbl(Mod mod, string chara_name, string fileTextItemTable)
        {
            List<ItemTbl> ret = new();

            var builder = new ConfigurationBuilder();
            var path = "./" + System.IO.Path.GetFileNameWithoutExtension(ChritmPropLogic.FILE_FARC_CHRITM_PROP_MOD) + "/" + fileTextItemTable;
            if (File.Exists(path))
            {
                foreach (var line in File.ReadAllLines(path))
                {
                    if (string.IsNullOrEmpty(line.Trim()) || line.StartsWith("#"))
                    {
                        continue;
                    }
                    var foo = line.Split("=")[0].Split(".");
                    var bar = string.Concat(line.Split("=")[1]);
                    Item item = new Item(mod, foo, bar);
                    ItemTbl itemTbl = new(mod);
                    itemTbl.Items.Add(item);

                    ret.Add(itemTbl);
                }
            }

            return ret;
        }

        public string GetArrayStr(AppConfig config, string sub_id, string value, string str_jp)
        {
            if (this.StrArray.Str_Array_Toml == null)
            {
                return "";
            }

            var type = sub_id == "10" ? "module" : "customize";

            try
            {
                var lang_low = config.Config.Lang.ToLower();
                if (lang_low == "jp")
                {
                    return str_jp;
                }
                else if (lang_low == "en")
                {
                    return this.StrArray.Str_Array_Toml.Get<TomlTable>(type).Get(value).ToString();
                }
                else
                {
                    return this.StrArray.Str_Array_Toml.Get<TomlTable>(config.Config.Lang).Get<TomlTable>(type).Get(value).ToString();
                }
            }
            catch (Exception e)
            {
                return "";
            }
        }
    }
}
