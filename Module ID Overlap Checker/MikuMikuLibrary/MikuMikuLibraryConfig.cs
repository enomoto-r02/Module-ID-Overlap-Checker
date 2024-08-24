namespace Module_ID_Overlap_Checker.MikuMikuLibrary
{
    public class MikuMikuLibraryConfig
    {
        public static string FILE_FARC_PACK = "FarcPack.exe";
        public string Path { get; set; }
        public string FarcPack_Path { get { return this.Path + "\\" + FILE_FARC_PACK; } }
    }

}
