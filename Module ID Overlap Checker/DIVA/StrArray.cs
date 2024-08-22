using Nett;

namespace Module_ID_Overlap_Checker.DIVA
{
    public class StrArray
    {
        public static readonly string FILE_STR_ARRAY_MOD = "mod_str_array.toml";
        public static readonly string DIR_NAME = "lang2";

        public string Mod_Name { get; set; }
        public string Mod_Path { get; set; }

        public TomlTable Str_Array_Toml { get; set; }

        public StrArray(Mod mod)
        {
            this.Mod_Name = mod.Name;
            this.Mod_Path = mod.Path;
            this.Load();
        }

        private void Load()
        {
            var path = string.Join("/", this.Mod_Path, "rom", DIR_NAME, FILE_STR_ARRAY_MOD);

            if (File.Exists(path))
            {
                this.Str_Array_Toml = Toml.ReadFile(path);
            }
            else
            {
                this.Str_Array_Toml = null;
            }
        }
    }
}
