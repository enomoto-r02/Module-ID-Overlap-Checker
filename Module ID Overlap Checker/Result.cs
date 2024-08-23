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

            var costbl_cos_ids = this.Mod.ModDB.CosIdByKeyRegex(@"cos\.\d+\.id");

            //foreach (var module in this.Mod.GmModule.Gm_Module_Item.Items)
            int cnt = -1;
            foreach (var costbl_cos_id in costbl_cos_ids)
            {
                cnt++;
                // xxxitm_tblからcos.xxxxx.id=cosidの添字を取得
                //var cosId_id = this.Mod.ModDB.NameByValue(@"module\.\d+\.id", module.Value);
                var cosId_id = this.Mod.ModDB.CosIdByValueRegex(@"cos\.\d+\.id", costbl_cos_id.Value);
                if (cosId_id.Count == 0)
                {
                    continue;
                }

                //var cosId_id2_int = int.Parse(cosId_cos[0].Value);
                //var cosId_id2 = this.Mod.ModDB.CosIdByValue(@"cos\.\d\.id", (cosId_id2_int-1).ToString());

                // xxxitm_tblからcos.n.id.length=xxxxxを取得
                var cosId_length = this.Mod.ModDB.CosIdByKey(@"cos." + cosId_id[0].Parameter[1] + ".item.length");
                if (cosId_length == null)
                {
                    continue;
                }

                List<Item> cos_parts = new List<Item>();
                for (int i=0; i<int.Parse(cosId_length.Value); i++)
                {
                    // xxxitm_tblからcos.n.id.item=xxxxxを取得
                    cos_parts.Add(this.Mod.ModDB.CosIdByKey(@"cos." + cosId_id[0].Parameter[1] + ".item."+i));
                }
                if (cos_parts.Count == 0)
                {
                    continue;
                }

                // cos.n.id.item数ループ
                for (int i = 0; i < cos_parts.Count; i++)
                {
                    var subid = this.Mod.ModDB.SubIdByKey("item."+ i +".sub_id");
                    if (subid == null) continue;
                    var name = this.Mod.ModDB.SubIdByKey("item." + i + ".name");
                    if (name == null) continue;
                    var module_id = this.Mod.ModDB.NameByKey(@"module."+ cnt +".id");
                    if (module_id == null) continue;
                    var lang_val = this.Mod.GetArrayStr(config, subid.Value, module_id.Value, "");
                    if (cos_parts[i] == null) continue;

                    sb.Append(string.Join("\t",
                        this.Mod.Name,
                        this.Chara_Name,
                        cosId_id[0].Value,
                        cos_parts[i].Value,
                        subid.Value,
                        name.Value,
                        lang_val
                    ) + "\n");
                }
            }

            return sb.ToString();
        }
    }
}
