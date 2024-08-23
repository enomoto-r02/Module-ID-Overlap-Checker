using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public Item SubIdByKey(string key)
        {
            return this.GetItem(key, _SubId);
        }
        public List<Item> SubIdByValue(string key_regex, string value)
        {
            return this.GetItemValue(key_regex, value, _SubId);
        }
        private List<Item> _CosId;
        public Item CosIdByKey(string key)
        {
            return this.GetItem(key, _CosId);
        }
        public List<Item> CosIdByValue(string key_regex, string value)
        {
            return this.GetItemValue(key_regex, value, _CosId);
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

        public List<Item> GetItemValue(string key_regex, string value, List<Item> DB)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            List<Item> ret = new List<Item>();
            foreach (var item in DB)
            {
                if (string.IsNullOrEmpty(key_regex))
                {
                    if (key_regex == item.Value)
                    {
                        return ret;
                    }
                }
                else
                {
                    if (Regex.IsMatch(item.GetParameterStr(), key_regex))
                    {
                        if (value == item.Value)
                        {
                            ret.Add(item);
                        }
                    }
                }
            }

            return ret;
        }
    }
}
