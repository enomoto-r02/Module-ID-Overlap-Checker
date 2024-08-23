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

            foreach (var module in this.Mod.GmModule.Gm_Module_Item.Items)
            {
                // xxxitm_tblからcos.xxxxx.id=cosidの添字を取得
                var cosId_id = this.Mod.ModDB.CosIdByValue(@"cos.\d+.id", module.Value);
                if (cosId_id.Count == 0)
                {
                    continue;
                }
                // xxxitm_tblからcos.n.id.length=xxxxxを取得
                var cosId_length = this.Mod.ModDB.CosIdByKey(@"cos." + cosId_id[0].Parameter[1] + ".item.length");
                List<Item> cos_parts = new List<Item>();
                for (int i=0; i<int.Parse(cosId_length.Value); i++)
                {
                    // xxxitm_tblからcos.n.id.item=xxxxxを取得
                    cos_parts.Add(this.Mod.ModDB.CosIdByKey(@"cos." + cosId_id[0].Parameter[1] + ".item."+i));
                }

                // cos.n.id.item数ループ
                for (int i = 0; i < cos_parts.Count; i++)
                {
                    var subid = this.Mod.ModDB.SubIdByKey("item."+i+".sub_id");
                    var name = this.Mod.ModDB.SubIdByKey("item." + i + ".name");
                    var lang_val = this.Mod.GetArrayStr(config, subid.Value, module.Value, "");

                    sb.Append(string.Join("\t",
                        this.Mod.Name,
                        this.Chara_Name,
                        module.Value,
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
