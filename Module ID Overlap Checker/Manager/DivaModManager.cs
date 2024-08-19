using Microsoft.Extensions.Configuration;
using Module_ID_Overlap_Checker.DIVA;
using Module_ID_Overlap_Checker.Util;
using System.Text;

namespace Module_ID_Overlap_Checker.Manager
{
    public class DivaModManager
    {
        const string CONFIGS = "Configs";
        const string GAME_NAME = "Project DIVA Mega Mix\u002B";
        const string CURRENT_LOAD_OUT = "CurrentLoadout";
        const string LOAD_OUTS = "Loadouts";
        const string MOD_FOLDER = "ModsFolder";

        public List<Mod> Mods { get; }

        public List<string> Song_no_cnt { get; }


        public DivaModManager()
        {
            this.Song_no_cnt = [];
        }
        public DivaModManager(AppConfig Config) : this()
        {
            // DMMのConfig.json読み込み
            var builder = new ConfigurationBuilder()
                .AddJsonFile(Config.DivaModManager.ConfigPath, optional: true);

            var build = builder.Build();

            var current_loadout_join = string.Join(":", CONFIGS, GAME_NAME, CURRENT_LOAD_OUT);
            var CurrentLoadout = build[current_loadout_join];

            const string NAME = "name";
            const string ENABLED = "enabled";


            var i = 0;
            var name_join = string.Join(":", CONFIGS, GAME_NAME, LOAD_OUTS, CurrentLoadout, i.ToString(), NAME);
            var enabled_join = string.Join(":", CONFIGS, GAME_NAME, LOAD_OUTS, CurrentLoadout, i.ToString(), ENABLED);
            var mod_folder_join = string.Join(":", CONFIGS, GAME_NAME, MOD_FOLDER);

            List<Mod> mods = new List<Mod>();

            var mod_name = build[name_join];
            var mod_enabled = build[enabled_join];
            var mod_folder = build[mod_folder_join] + "\\" + mod_name;
            var pv_db_priority = 0;

            while (mod_name != null)
            {
                Mod mod = new Mod(i, mod_name, mod_enabled, mod_folder);

                if (mod.Enabled)
                {
                    mods.Add(mod);
                }

                i++;
                name_join = string.Join(":", CONFIGS, GAME_NAME, LOAD_OUTS, CurrentLoadout, i.ToString(), NAME);
                enabled_join = string.Join(":", CONFIGS, GAME_NAME, LOAD_OUTS, CurrentLoadout, i.ToString(), ENABLED);
                mod_folder = build[mod_folder_join] + "\\" + mod_name;

                mod_name = build[name_join];
                mod_enabled = build[enabled_join];
                mod_folder = build[mod_folder_join] + "\\" + mod_name;
            }

            this.Mods = mods;
        }

        public void LoadPvData(AppConfig appConfig)
        {
            var now_pv_db_priority = 0;
            var is_create_stage_data = false;
            for (var i = 0; i < Mods.Count; i++)
            {
                if (Mods[i].Enabled)
                {
                    //Mods[i].LoadPvDb(this.AddDbAnotherSong, this.Song_no_cnt, now_pv_db_priority);
                    //if (appConfig.Config.MergePvField)
                    //{
                    //    Mods[i].LoadPvField(this.allFieldSong);

                    //}
                    //if (appConfig.Config.MergeStageData)
                    //{
                    //    if (is_create_stage_data == false)
                    //    {
                    //        is_create_stage_data = Mods[i].InitStageData(appConfig);
                    //    }
                    //    else
                    //    {
                    //        Mods[i].LoadStageData(appConfig);
                    //    }
                    //}
                    //if (Mods[i].Db_Priority >= 0)
                    //{
                    //    now_pv_db_priority++;
                    //}

                }
            }
        }

        public string ToStringMods()
        {
            StringBuilder sb = new();

            if (this.Mods.Count > 0)
            {
                sb.AppendLine(ToolUtil.CONSOLE_PREFIX + "Load Mods Folder");

                foreach (var item in this.Mods)
                {
                    sb.AppendLine(ToolUtil.CONSOLE_PREFIX + "- " + item.Name);
                }
            }

            return sb.ToString();
        }
    }
}
