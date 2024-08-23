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

            foreach (var module in this.Mod.gmModule.Gm_Module_Item.Items)
            {



                var subid = Mod.modDB.SubId("item." + module.Parameter[1] + ".sub_id");
                if (subid == null || subid.Value != "10")
                {
                    continue;
                }

                var lang_val = this.Mod.GetArrayStr(config, subid.Value, module.Value, "");
                var name = Mod.modDB.Name("module." + module.Parameter[1] + ".name");

                //var no = Mod.modDB.No("item."+ module.Parameter[1] + ".no");


                //var id = this.Mod.gmModule.GetGmModuleData(config, "module." + no.Value +".id", "");

                //var cosid = Mod.modDB.CosId("cos." + module.Parameter[1] + ".id");


                //else if (subid.Value == "1")
                //{
                //    name = Mod.modDB.Customize("cstm_item." + module.Parameter[1] + ".name");
                //}
                //else
                //{
                //    name = new();
                //}


                //if (this.CosId != null && this.CosId.Count > 0)
                //{
                //    lang_val = this.Mod.GetArrayStr(config, this.SubId[i].Value, this.No[i].Value, "");
                //}

                //sb.Append(string.Join("\t",
                //    this.Mod.Name,
                //    this.Chara_Name,
                //    no.GetParameterStr(),
                //    no.Value,
                //    name.GetParameterStr(),
                //    name.Value,
                //    lang_val
                //) + "\n");

                sb.Append(string.Join("\t",
                    this.Mod.Name,
                    this.Chara_Name,
                    module.Value,
                    subid.Value,
                    name.Value,
                    lang_val
                ) + "\n");
            }

            return sb.ToString();
        }
    }
}
