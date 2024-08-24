using Module_ID_Overlap_Checker.DIVA;
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
                if (cos_item != null && cos_item.Count > 0)
                {
                    var cos_id = this.Mod.ModDB.GetItemTblByKey("cos." + cos_item[0].Parameter[1] + ".id");
                    var cos_key = "COS_" + (int.Parse(cos_id.Value) + 1);
                    var module_cos = this.Mod.ModDB.GetModuleTblByValue(@"module\.\d+\.cos", cos_key);

                    var module_id_str = "";
                    var module_lang = "";

                    if (module_cos.Count > 0)
                    {
                        var module_id = this.Mod.ModDB.GetModuleTblByKey("module." + module_cos[0].Parameter[1] + ".id");
                        module_id_str = module_id.Value;
                        module_lang = this.Mod.GetArrayStr(config, item_subid.Value, module_id.Value, "");
                    }

                    sb.Append(string.Join("\t",
                        this.Mod.Name,
                        this.Chara_Name,
                        cos_id.Value,
                        module_id_str,
                        item_no.Value,
                        item_subid.Value,
                        item_name.Value,
                        module_lang
                    ) + "\n");
                }
                else
                {
                    //ToolUtil.WarnLog($"{ToolUtil.LOG_PREFIX}[{Mod.Name}]cos.n.item.n={item_no.Value} is not Found.");
                }
            }

            return sb.ToString();
        }
    }
}
