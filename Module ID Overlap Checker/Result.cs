using Module_ID_Overlap_Checker.DIVA;
using Module_ID_Overlap_Checker.Util;
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

            var costbl_cos_ids = this.Mod.ModDB.GetItemTblByRegex(@"cos\.\d+\.id");

            int index = 0;

            foreach (var costbl_cos_id in costbl_cos_ids)
            {
                string key = @"cos.n.id="+costbl_cos_id.Value;
                var cosId_id = this.Mod.ModDB.GetItemTblByValueRegex(@"cos\.\d+\.id", costbl_cos_id.Value);
                if (cosId_id.Count == 0)
                {
                    ToolUtil.WarnLog("[" + key + "] count is 0 , continue;\n");
                    continue;
                }

                key = "cos." + cosId_id[0].Parameter[1] + ".item.length";
                var cosId_length = this.Mod.ModDB.GetItemTblByRegex("cos." + cosId_id[0].Parameter[1] + ".item.length");
                if (cosId_length.Count == 0)
                {
                    ToolUtil.WarnLog("[" + key + "] count is 0 , continue;\n");
                    continue;
                }

                List<Item> cos_parts = new List<Item>();
                for (int i=0; i<cosId_length.Count; i++)
                {
                    key = "cos." + cosId_id[0].Parameter[1] + ".item." + i;
                    cos_parts.AddRange(this.Mod.ModDB.GetItemTblByRegex(@"cos\." + cosId_id[0].Parameter[1] + @"\.item.\d+"));
                }
                if (cos_parts.Count == 0)
                {
                    ToolUtil.WarnLog("[" + key + "] count is 0 , continue;\n");
                    continue;
                }

                for (int i = 0; i < int.Parse(cosId_length[0].Value); i++)
                {                    
                    try
                    {
                        key = @"item.n.no=" + cos_parts[i].Value;
                        var item_index = this.Mod.ModDB.GetItemTblByValueRegex(@"item\.\d+\.no", cos_parts[i].Value);
                        Item itemno = null;
                        Item subid = null;
                        Item name = null;
                        if (item_index != null && item_index.Count > 0)
                        {
                            itemno = this.Mod.ModDB.GetItemTblByKey("item." + item_index[0].Parameter[1] + ".no");
                            subid = this.Mod.ModDB.GetItemTblByKey("item." + item_index[0].Parameter[1] + ".sub_id");
                            name = this.Mod.ModDB.GetItemTblByKey("item." + item_index[0].Parameter[1] + ".name");
                        }

                        key = @"module.n.id=" + cosId_id[0].Value;
                        var module_id = this.Mod.ModDB.GmModuleItemTblByValue(@"module\.\d+\.id", cosId_id[0].Value);
                        string module_val = "";
                        string lang_val = "";
                        if (module_id != null && module_id.Count == 1)
                        {
                            module_val = module_id[0].Value;
                            lang_val = this.Mod.GetArrayStr(config, subid.Value, module_id[0].Value, "");
                        }

                        sb.Append(string.Join("\t",
                            this.Mod.Name,
                            this.Chara_Name,
                            cosId_id[0].ValueView(),        // Cos ID
                            module_val,                     // Module ID
                            itemno.ValueView(),             // Item No
                            subid.ValueView(),              // Sub ID
                            name.ValueView(),
                            lang_val
                        ) + "\n");

                        index++;
                    }
                    catch(Exception e)
                    {
                        ToolUtil.ErrorLog(e.Message + "\n" + e.InnerException + "\n" + e.StackTrace);
                        continue;
                    }
                }
            }

            return sb.ToString();
        }
    }
}
