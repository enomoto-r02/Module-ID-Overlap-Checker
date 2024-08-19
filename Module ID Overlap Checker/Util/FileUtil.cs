using System.Text;

namespace Module_ID_Overlap_Checker.Util
{
    public static class FileUtil
    {
        public static void CreateFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static void WriteFile(string str, string path, Boolean addFlg)
        {
            using (StreamWriter writer = new StreamWriter(
                path,
                addFlg,
                Encoding.UTF8
            ))
            {
                writer.Write(str);
                writer.Close();
            }
        }

        public static void WriteFile_UTF_8_NO_BOM(string str, string path, Boolean addFlg)
        {
            System.Text.Encoding enc = new System.Text.UTF8Encoding(false);

            // UTF-8 BOM無し
            using (StreamWriter writer = new StreamWriter(path, addFlg, enc))
            {
                writer.Write(str);
                writer.Close();
            }
        }

        public static string ReadFile(string path)
        {
            string ret = "";

            using (StreamReader sr = new StreamReader(
                path,
                Encoding.UTF8
            ))
            {
                ret = sr.ReadToEnd();
                sr.Close();
            }

            return ret;
        }

        public static string Backup(string file_name)
        {
            var new_file_name = "";
            if (File.Exists(file_name))
            {
                new_file_name = "./rom/" + Path.GetFileNameWithoutExtension(file_name) + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + Path.GetExtension(file_name);
                File.Move(file_name, new_file_name);
            }

            return new_file_name;
        }

        public static string Delete(string file_name)
        {
            var del_file_name = "";
            if (File.Exists(file_name))
            {
                File.Delete(file_name);
                del_file_name = file_name;
            }

            return del_file_name;
        }
    }
}
