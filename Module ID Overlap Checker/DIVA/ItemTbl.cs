namespace Module_ID_Overlap_Checker.DIVA
{
    public class ItemTbl
    {
        public string Mod_Name { get; set; }
        public string Mod_Path { get; set; }

        public List<Item> Items = new();

        public ItemTbl()
        {
        }

        public ItemTbl(Mod mod)
        {
            this.Mod_Name = mod.Name;
            this.Mod_Path = mod.Path;
            this.Items = new List<Item>();
        }
    }
}