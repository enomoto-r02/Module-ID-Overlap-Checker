using Module_ID_Overlap_Checker.DIVA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module_ID_Overlap_Checker
{
    public class Result
    {
        public Mod Mod { get; set; }
        public string Chara_Name { get; set; }

        public Item No = new();
        public Item Name = new();

        public Result(Mod mod, string chara_name, Item no, Item name)
        {
            this.Mod = mod;
            this.Chara_Name = chara_name;
            this.No = no;
            this.Name = name;
        }

        public string ToString(AppConfig config)
        {
            if (this.No.Parameter == null || this.Name.Parameter == null)
            {
                return "";
            }

            return string.Join("\t", 
                this.Mod.Name,
                this.Chara_Name,
                this.No.GetParameterStr(), 
                this.No.Value,
                this.Name.GetParameterStr(), 
                this.Name.Value,
                this.Mod.GetArrayStr(config, this.No.Value, Name.Value)

                ) + "\n";
            
        }
    }
}
