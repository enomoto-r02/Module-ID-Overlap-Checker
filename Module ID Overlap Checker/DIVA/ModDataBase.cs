using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module_ID_Overlap_Checker.DIVA
{
    public class ModDataBase
    {
        private List<Item> _No;
        public Item No(string key)
        {
            return this.GetItem(key, _No);
        }
        public List<Item> _Name { get; private set; }
        public Item Name(string key)
        {
            return this.GetItem(key, _Name);
        }
        public List<Item> _SubId { get; private set; }
        public Item SubId(string key)
        {
            return this.GetItem(key, _SubId);
        }
        public List<Item> _CosId { get; private set; }
        public Item CosId(string key)
        {
            return this.GetItem(key, _CosId);
        }
        public List<Item> _Customize { get; private set; }
        public Item Customize(string key)
        {
            return this.GetItem(key, _Customize);
        }

        public ModDataBase(List<Item> no, List<Item> name, List<Item> sub_id, List<Item> cosId, List<Item> customize)
        {
            this._No = no;
            this._Name = name;
            this._SubId = sub_id;
            this._CosId = cosId;
            this._Customize = customize;
        }

        public Item GetItem(string key, List<Item> DB)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            foreach (var item in DB)
            {
                if(key == item.GetParameterStr())
                {
                    return item;
                }
            }

            return null;
        }
    }
}
