namespace Module_ID_Overlap_Checker.DIVA
{
    public class Item
    {
        public Mod Mod { get; set; }

        public string[] Parameter { get; set; }
        public string Value { get; set; }
        public string ValueView()
        {
            if(this == null || string.IsNullOrEmpty(this.Value))
            {
                return "";
            }
            else
            {
                return this.Value;
            }
        }

        public string GetParameterStr()
        {
            if (this.Parameter == null)
            {
                return "";
            }
            return string.Join(".", Parameter);
        }

        public Item()
        {
        }

        public Item(Mod mod, string[] parameter, string value)
        {
            this.Mod = mod;
            this.Parameter = parameter;
            this.Value = value;
        }

        public string GetItemValue(string key)
        {
            string ret = "";

            var param_key = string.Join(".", this.Parameter);
            if (key == param_key)
            {
                ret = Value;
            };

            return ret;
        }
    }
}
