using Nett;

namespace Module_ID_Overlap_Checker
{
    public class ConfigToml
    {
        public static string FILE_CONFIG = "config.toml";

        public bool Enabled { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public List<string> Dll { get; set; }
        public List<string> Include { get; set; }

        public ConfigToml()
        {
            var toml = Toml.ReadFile(FILE_CONFIG);

            //this.Enabled = toml.Get<bool>("enabled");
            //this.Name = toml.Get<string>("name");
            //this.Author = toml.Get<string>("author");
            //this.Version = toml.Get<string>("version");
            //this.Description = toml.Get<string>("description");

            try
            {
                this.Dll = toml.Get<List<string>>("dll");
            }
            catch
            {
                this.Dll = null;
            }

            //this.Include = toml.Get<List<string>>("include");
        }
    }
}
