using Module_ID_Overlap_Checker.DIVA;
using Module_ID_Overlap_Checker.Util;
using System.Reflection;
using System.Text;

namespace Module_ID_Overlap_Checker
{
    public class Result
    {
        public Mod Mod { get; set; }
        public string Chara_Name { get; set; }

        public Result(Mod mod, string chara_name)
        {
            this.Mod = mod;
            this.Chara_Name = chara_name;
        }

        public string ToString(AppConfig config)
        {
            StringBuilder sb = new StringBuilder();

            var item_nos = this.Mod.ModDB.GetItemTblByRegex(@"item\.\d+\.no");

            foreach (var item_no in item_nos)
            {
                var item_name = this.Mod.ModDB.GetItemTblByKey("item." + item_no.Parameter[1] + ".name");
                var item_subid = this.Mod.ModDB.GetItemTblByKey("item." + item_no.Parameter[1] + ".sub_id");

                var cos_item = this.Mod.ModDB.GetItemTblByValueRegex(@"cos\.\d+\.item\.\d+", item_no.Value);

                // 紐づくモジュールあり
                if (cos_item != null && cos_item.Count > 0)
                {
                    var cos_id = this.Mod.ModDB.GetItemTblByKey("cos." + cos_item[0].Parameter[1] + ".id");
                    var cos_key = "COS_" + (int.Parse(cos_id.Value) + 1);
                    var module_cos = this.Mod.ModDB.GetModuleTblByValue(@"module\.\d+\.cos", cos_key);

                    string module_id_key = "";
                    Item module_id = new();
                    var module_lang = "";
                    string module_lang_str = "";
                    string module_lang_key = "";
                    string module_lang_value = "";
                    Item module_id_str = new();

                    if (module_cos.Count > 0)
                    {
                        module_id_key = "module." + module_cos[0].Parameter[1] + ".id";
                        module_id = this.Mod.ModDB.GetModuleTblByKey(module_id_key);
                        module_lang = this.Mod.GetArrayStr(config, item_subid.Value, module_id.Value, "");
                        module_id_str = item_subid.Value == "10" ? 
                            this.Mod.GmModule.GetGmModuleByValue(@"module\.\d+\.id", module_id.Value) :
                            new() ;

                        module_lang_str = string.IsNullOrEmpty(module_lang) ? string.Empty : module_lang.Split(".")[0];
                        module_lang_key = string.IsNullOrEmpty(module_lang) ? string.Empty : module_lang.Split(".")[1];
                        module_lang_value = string.IsNullOrEmpty(module_lang) ? string.Empty : module_lang.Split(".")[2];
                        module_lang_str = string.IsNullOrEmpty(module_lang) ? string.Empty : 
                            $"{module_lang_str}.{module_lang_key}.{module_id.Value} = \"{module_lang_value}\"";
                    }

                    if (item_name.Value == "アイヴィーラビットヘア")
                    {
                        ;
                    }

                    sb.Append(string.Join("\t",
                        this.Mod.Name,
                        this.Chara_Name,
                        cos_id.Value,
                        module_id_str.ParameterStr(),
                        module_id.Value,
                        item_no.Value,
                        item_subid.Value,
                        item_name.Value,
                        module_lang_value
                    ) + "\n");
                }
                // 紐づくモジュールなし(カスタマイズアイテムなど)
                else
                {
                    sb.Append(string.Join("\t",
                        this.Mod.Name,
                        this.Chara_Name,
                        "",
                        "",
                        "",
                        item_no.Value,
                        item_subid.Value,
                        item_name.Value,
                        "",
                        ""
                    ) + "\n");
                }
            }

            return sb.ToString();
        }
    }
}
