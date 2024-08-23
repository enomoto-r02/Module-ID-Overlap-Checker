namespace Module_ID_Overlap_Checker.Util
{
    public class DivaUtil
    {
        public static readonly List<string> CHARA_STR = new() {

            "MIK",
            "RIN",
            "LEN",
            "LUK",
            "MEI",
            "KAI",
            "HAK",
            "NER",
            "SAK",
            "TET",
        };

        public static readonly Dictionary<string, string> CHARA_STR_TBL_JP = new()
        {
            {"MIK", "初音ミク"},
            {"RIN", "鏡音リン"},
            {"LEN", "鏡音レン"},
            {"LUK", "巡音ルカ"},
            {"MEI", "MEIKO"},
            {"KAI", "KAITO"},
            {"HAK", "弱音ハク"},
            {"NER", "亞北ネル"},
            {"SAK", "咲音メイコ"},
            {"TET", "重音テト"},
        };

        public static readonly Dictionary<string, string> CHARA_STR_TBL_EN = new()
        {
            {"MIK", "Miku"},
            {"RIN", "Rin"},
            {"LEN", "Len"},
            {"LUK", "Luka"},
            {"MEI", "MEIKO"},
            {"KAI", "KAITO"},
            {"HAK", "Haku"},
            {"NER", "Neru"},
            {"SAK", "Sakine"},
            {"TET", "Teto"},
        };

        public static readonly Dictionary<string, string> CHARA_ITM_TBL = new()
        {
            {"MIK", "mikitm_tbl.txt"},
            {"RIN", "rinitm_tbl.txt"},
            {"LEN", "lenitm_tbl.txt"},
            {"LUK", "lukitm_tbl.txt"},
            {"MEI", "meiitm_tbl.txt"},
            {"KAI", "kaiitm_tbl.txt"},
            {"HAK", "hakitm_tbl.txt"},
            {"NER", "neritm_tbl.txt"},
            {"SAK", "sakitm_tbl.txt"},
            {"TET", "tetitm_tbl.txt"},
        };

        public static string GetChara(string target)
        {
            var ret = "";
            try
            {
                ret = CHARA_STR_TBL_JP[target];
            }
            catch (KeyNotFoundException)
            {
                ret = target;
            }

            return ret;
        }

        public static string GetCharaEn(string target)
        {
            var ret = "";
            try
            {
                ret = CHARA_STR_TBL_EN[target];
            }
            catch (KeyNotFoundException)
            {
                ret = target;
            }

            return ret;
        }
    }
}
