namespace Module_ID_Overlap_Checker.DIVA
{
    public class Item
    {
        public Mod Mod { get; set; }

        public string[] Parameter { get; set; }
        public string Value { get; set; }

        public string GetParameterStr()
        {
            if (this.Parameter == null)
            {
                return null;
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
            string ret = null;

            var param_key = string.Join(".", this.Parameter);
            if (key == param_key)
            {
                ret = Value;
            };

            return ret;
        }
    }
}
