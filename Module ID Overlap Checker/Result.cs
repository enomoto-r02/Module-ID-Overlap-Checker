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

            var costbl_cos_ids = this.Mod.ModDB.GetItemTblByRegex(@"cos\.\d+\.id");

            int index = 0;

            foreach (var costbl_cos_id in costbl_cos_ids)
            {
                var cosId_id = this.Mod.ModDB.GetItemTblByValueRegex(@"cos\.\d+\.id", costbl_cos_id.Value);
                if (cosId_id.Count == 0)
                {
                    continue;
                }

                var cosId_length = this.Mod.ModDB.GetItemTblByRegex(@"cos." + cosId_id[0].Parameter[1] + ".item.length");
                if (cosId_length.Count == 0)
                {
                    continue;
                }

                List<Item> cos_parts = new List<Item>();
                for (int i=0; i<cosId_length.Count; i++)
                {
                    cos_parts.Add(this.Mod.ModDB.GetItemTblByKey(@"cos." + cosId_id[0].Parameter[1] + ".item."+i));
                }
                if (cos_parts.Count == 0)
                {
                    continue;
                }

                for (int i = 0; i < int.Parse(cosId_length[0].Value); i++)
                {
                    if (cosId_length.Count == 0) continue;

                    try
                    {
                        var item_index = this.Mod.ModDB.GetItemTblByValueRegex(@"item\.\d+\.no", cos_parts[i].Value);
                        if (item_index.Count == 0) continue;
                        var itemno = this.Mod.ModDB.GetItemTblByKey("item." + item_index[0].Parameter[1] + ".no");
                        if (itemno == null) continue;
                        var subid = this.Mod.ModDB.GetItemTblByKey("item." + item_index[0].Parameter[1] + ".sub_id");
                        if (subid == null) continue;
                        var name = this.Mod.ModDB.GetItemTblByKey("item." + item_index[0].Parameter[1] + ".name");
                        if (name == null) continue;
                        var module_id = this.Mod.ModDB.GmModuleItemTblByKey(@"module." + cosId_id[0].Parameter[1] + ".id");
                        if (module_id == null) continue;
                        var lang_val = this.Mod.GetArrayStr(config, subid.Value, cosId_id[0].Value, "");

                        sb.Append(string.Join("\t",
                            this.Mod.Name,
                            this.Chara_Name,
                            module_id.Value,        // Module ID
                            itemno.Value,           // Item No
                            subid.Value,            // Sub ID
                            name.Value,
                            lang_val
                        ) + "\n");

                        index++;
                    }
                    catch(Exception e)
                    {
                        ;
                    }
                }
            }

            return sb.ToString();
        }
    }
}
