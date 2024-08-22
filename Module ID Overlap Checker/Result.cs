using Module_ID_Overlap_Checker.DIVA;
using System.Text;

namespace Module_ID_Overlap_Checker
{
    public class Result
    {
        public Mod Mod { get; set; }
        public string Chara_Name { get; set; }

        public List<Item> No = new();
        public List<Item> Name = new();
        public List<Item> SubId = new();
        public List<Item> CosId = new();

        public Result(Mod mod, string chara_name, List<Item> no, List<Item> name, List<Item> sub_id, List<Item> cosId)
        {
            this.Mod = mod;
            this.Chara_Name = chara_name;
            this.No = no;
            this.Name = name;
            this.SubId = sub_id;
            this.CosId = cosId;
        }

        public string ToString(AppConfig config)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < this.No.Count; i++)
            {
                if (this.No[i].Parameter == null || this.Name[i].Parameter == null)
                {
                    ;
                }

                var id = this.Mod.gmModule.GetGmModuleData(config, "module." + No[i].Value +".id", "");
                string lang_val = "";

                if (this.CosId != null && this.CosId.Count > 0)
                {
                    lang_val = this.Mod.GetArrayStr(config, this.SubId[i].Value, this.CosId[i].Value, "");
                }

                sb.Append(string.Join("\t",
                    this.Mod.Name,
                    this.Chara_Name,
                    this.No[i].GetParameterStr(),
                    this.No[i].Value,
                    this.Name[i].GetParameterStr(),
                    this.Name[i].Value,
                    lang_val
                ) + "\n");
            }

            return sb.ToString();
        }
    }
}
