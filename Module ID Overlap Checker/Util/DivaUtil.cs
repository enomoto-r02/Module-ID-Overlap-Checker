using System.Collections.ObjectModel;

namespace Module_ID_Overlap_Checker.Util
{
    public class DivaUtil
    {
        public static readonly Dictionary<string, string> CHARA_STR = new()
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

        public static readonly Dictionary<string, string> CHARA_STR_EN = new()
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

        public static string GetChara(string target)
        {
            var ret = "";
            try
            {
                ret = CHARA_STR[target];
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
                ret = CHARA_STR_EN[target];
            }
            catch (KeyNotFoundException)
            {
                ret = target;
            }

            return ret;
        }
    }
}
