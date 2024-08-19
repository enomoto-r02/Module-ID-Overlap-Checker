namespace Module_ID_Overlap_Checker.DIVA
{
    public class Mod
    {
        public string Name { get; set; }
        public string Folder_Path { get; set; }
        public int Mod_Priority { get; set; }
        public bool Enabled { get; set; }

        public ItmTbl itmTbl { get; set; }

        public Mod()
        {
            this.Mod_Priority = -1;
            this.itmTbl = new(this);
        }

        public Mod(int priority, string name, string enabled, string folder_path) : this()
        {
            this.Mod_Priority = priority;
            this.Name = name;
            this.Enabled = bool.Parse(enabled);
            this.Folder_Path = folder_path;
            this.itmTbl = new(this);
        }
    }
}
