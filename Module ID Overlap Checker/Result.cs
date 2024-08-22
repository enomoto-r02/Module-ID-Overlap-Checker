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
        public List<Item> Sub_Id = new();

        public List<KeyValuePair<string, int>> Overlap_Cnt;


        public Result(Mod mod, string chara_name, List<Item> no, List<Item> name, List<Item> sub_id)
        {
            this.Mod = mod;
            this.Chara_Name = chara_name;
            this.No = no;
            this.Name = name;
            this.Sub_Id = sub_id;
            this.Overlap_Cnt = new();
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

                sb.Append(string.Join("\t",
                    this.Mod.Name,
                    this.Chara_Name,
                    this.No[i].GetParameterStr(),
                    this.No[i].Value,
                    this.Name[i].GetParameterStr(),
                    this.Name[i].Value,
                    this.Mod.GetArrayStr(config, this.Sub_Id[i].Value, this.No[i].Value, Name[i].Value)

                    ) + "\n");
            }

            return sb.ToString();
        }
    }
}
