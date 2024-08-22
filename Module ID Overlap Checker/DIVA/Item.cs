using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module_ID_Overlap_Checker.DIVA
{
    public class Item
    {
        public string Mod_Name { get; set; }
        public string Mod_Path { get; set; }

        public string[] Parameter { get; set; }


        public string GetParameterStr()
        {
            if (this.Parameter == null)
            {
                return null;
            }
            return string.Join(".", Parameter);
        }

        public string Value { get; set; }

        public Item()
        {
        }

        public Item(Mod mod, string[] parameter, string value)
        {
            this.Mod_Name = mod.Name;
            this.Mod_Path = mod.Path;
            this.Parameter = parameter;
            this.Value = value;
        }
    }
}
