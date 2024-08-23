namespace Module_ID_Overlap_Checker.DIVA
{
    public class ItemTbl
    {
        public Mod Mod { get; set; }

        public List<Item> Items;

        public ItemTbl()
        {
        }

        public ItemTbl(Mod mod)
        {
            this.Mod = mod;
            this.Items = new List<Item>();
        }

        public void SetItems(string path)
        {
            List<Item> ret = new();

            if (File.Exists(path))
            {
                foreach (var line in File.ReadAllLines(path))
                {
                    if (string.IsNullOrEmpty(line.Trim()) || line.StartsWith("#"))
                    {
                        continue;
                    }
                    var foo = line.Split("=")[0].Split(".");
                    var bar = string.Concat(line.Split("=")[1]);
                    Item item = new Item(this.Mod, foo, bar);

                    ret.Add(item);
                }

                this.Items = ret;
            }
            else
            {
                this.Items = null;
            }
        }

        public void SetItems(string path, Conditions conditions)
        {
            List<Item> ret = new();

            if (File.Exists(path))
            {
                foreach (var line in File.ReadAllLines(path))
                {
                    if (string.IsNullOrEmpty(line.Trim()) || line.StartsWith("#"))
                    {
                        continue;
                    }
                    var foo = line.Split("=")[0].Split(".");
                    var bar = string.Concat(line.Split("=")[1]);
                    if (conditions(foo, bar))
                    {
                        Item item = new Item(this.Mod, foo, bar);
                        ret.Add(item);
                    }

                }

                this.Items = ret;
            }
            else
            {
                this.Items = null;
            }
        }

        public delegate bool Conditions(string[] parameters, string value);

        public string GetItemValue(string key)
        {
            foreach (var item in this.Items)
            {
                var val = item.GetItemValue(key);
                if (!string.IsNullOrEmpty(val))
                {
                    return val;
                }
            }

            return null;
        }
    }
}