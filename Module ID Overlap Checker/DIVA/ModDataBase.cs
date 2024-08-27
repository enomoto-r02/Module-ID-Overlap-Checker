using System.Text.RegularExpressions;

namespace Module_ID_Overlap_Checker.DIVA
{
    public class ModDataBase
    {
        private List<Item> _Item_Tbl;
        public Item Item_Tbl(string key)
        {
            return this.GetItemKey(key, _Item_Tbl);
        }
        public Item GetItemTblByKey(string key)
        {
            return this.GetItemKey(key, _Item_Tbl);
        }
        public List<Item> GetItemTblByValue(string key, string value)
        {
            return this.GetItemValue(key, value, _Item_Tbl);
        }
        public List<Item> GetItemTblByValueRegex(string key_regex, string value)
        {
            return this.GetItemValue(key_regex, value, _Item_Tbl);
        }

        public List<Item> GetItemTblByRegex(string regex)
        {
            return this.GetItemRegex(regex, _Item_Tbl);
        }
        public List<Item> _Gm_Module_ItemTbl { get; private set; }
        public Item GetModuleTblByKey(string key)
        {
            return this.GetItemKey(key, _Gm_Module_ItemTbl);
        }
        public List<Item> GetModuleTblByValue(string key_regex, string value)
        {
            return this.GetItemValue(key_regex, value, _Gm_Module_ItemTbl);
        }

        public List<Item> _GmCustomizeModule { get; private set; }
        public Item GetCustomizeModuleByKey(string key)
        {
            return this.GetItemKey(key, _GmCustomizeModule);
        }
        public List<Item> GetCustomizeModuleByValueRegex(string key_regex, string value)
        {
            return this.GetItemValue(key_regex, value, _GmCustomizeModule);
        }

        public ModDataBase(List<Item> Item_Tbl, List<Item> Gm_Module_ItemTbl, List<Item> GmCustomizeModule)
        {
            this._Item_Tbl = Item_Tbl;
            this._Gm_Module_ItemTbl = Gm_Module_ItemTbl;
            this._GmCustomizeModule = GmCustomizeModule;
        }

        private Item GetItemKey(string key, List<Item> DB)
        {
            if (string.IsNullOrEmpty(key))
            {
                return new();
            }

            foreach (var item in DB)
            {
                if (key == item.ParameterStr())
                {
                    return item;
                }
            }

            return new();
        }

        private List<Item> GetItemRegex(string key_regex, List<Item> DB)
        {
            if (string.IsNullOrEmpty(key_regex))
            {
                return new();
            }

            List<Item> ret = new List<Item>();
            foreach (var item in DB)
            {
                if (Regex.IsMatch(item.ParameterStr(), key_regex))
                {
                    ret.Add(item);
                }
            }

            return ret;
        }

        private List<Item> GetItemValue(string key_regex, string value, List<Item> DB)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new();
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
                    if (Regex.IsMatch(item.ParameterStr(), key_regex))
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
