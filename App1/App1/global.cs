using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using System.Text.RegularExpressions;
using System.Xml;

namespace App1
{
    static class global
    {
        static StorageFile file;
        public static string[,] res = new string[6, 7];

        //public static StringMap setting;
        public static StorageFile File
        {
            get
            {
                return file;
            }
            set
            {
                file = value;
            }
        }

        internal static subjectDisplay[,] Subjects
        {
            get
            {
                return subjects;
            }
            
        }

        public static int state = 0;
        public static XmlDocument doc;

        static subjectDisplay[,] subjects = new subjectDisplay[6, 7];


        public static async Task<int> openfile()
        {
            FileOpenPicker openFile = new FileOpenPicker();
            openFile.SuggestedStartLocation = PickerLocationId.Downloads;
            openFile.ViewMode = PickerViewMode.List;
            openFile.FileTypeFilter.Add(".xls");

            file = await openFile.PickSingleFileAsync();
            if (file != null)
            {
                StorageFolder folder = ApplicationData.Current.LocalFolder;
                StorageFile a = await folder.TryGetItemAsync("saved.xls") as StorageFile;
                if (a == null) a = await folder.CreateFileAsync("saved.xls");
                await file.CopyAndReplaceAsync(a);
                int r = await readXls();
                return r;
            }
            return -3;
        }

        public static async Task<int> readXls()
        {
            byte b1, b2 = 0;
            int num_of_str = 0;
            byte[][] strs = new byte[100][];
            int[,] str_id = new int[100, 100];

            for (int i = 0; i < 6; i++)
                for (int j = 0; j < 7; j++)
                    Subjects[i, j] = new subjectDisplay();

            try
            {
                using (var read = new BinaryReader(await file.OpenStreamForReadAsync()))
                {
                    while (true)
                    {
                        b1 = read.ReadByte();

                        if (b1 == 0 && b2 == 0xfc)
                        {
                            //read strs
                            read.ReadInt16();
                            read.ReadInt32();
                            num_of_str = read.ReadInt32();

                            for (int i = 0; i < num_of_str; i++)
                            {
                                int length = read.ReadInt16();
                                int long_char = read.ReadByte();
                                length = length << long_char;
                                strs[i] = read.ReadBytes(length);
                            }

                            b2 = 0;
                            continue;
                        }
                        if (b1 == 0 && b2 == 0xfd)
                        {
                            b2 = 0;
                            read.ReadInt16();
                            int row = read.ReadInt16();
                            int col = read.ReadInt16();
                            int s = read.ReadInt16();

                            int id = read.ReadInt32();
                            str_id[row, col] = id;

                            continue;
                        }
                        b2 = b1;
                    }
                }
            }
            catch (UnauthorizedAccessException e)
            {
                return -1;
            }
            catch (EndOfStreamException e)
            {

            }
            catch (Exception e)
            {
                return -2;
            }

            string course_patern = "</br>(?<course>[\\s\\S]+?)</br>";
            string teacher_patern = "(?<teacher>[\\u4E00-\\uFA29\\s,]*)";
            string week_patern = "\\[(?<week>[,\\-\\s\\d\\u5468\\u5355\\u53cc]+)\\]";
            string teacher_week_patern = teacher_patern + week_patern + "(," + teacher_patern + week_patern + ")*";
            string location_patern = "(</br>)?(?<location>[\\w\\s\\-()]+)?";
            string time_patern = "(?<time>\\u7B2C[\\d,]+\\u8282)+";
            string location_time_patern = location_patern + time_patern;
            string patern = course_patern + teacher_week_patern + location_time_patern + "(," + teacher_week_patern + location_time_patern + ")*";

            Regex rg = new Regex(@patern);
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    string s = System.Text.Encoding.Unicode.GetString(strs[str_id[i + 2, j + 2]]);
                    if (s == null || s == "")
                    {
                        res[i, j] = "";
                    }
                    else
                    {
                        string target_s = s.Replace("，", ",");
                        target_s = target_s.Replace("\n", "");
                        target_s = "</br>" + target_s;
                        MatchCollection matchCollection = rg.Matches(target_s);
                        string s2show = "";
                        for (int i_in = 0; i_in < matchCollection.Count; i_in++)
                        {
                            Match match = matchCollection[i_in];
                            string name = match.Groups["course"].ToString();
                            if (name.IndexOf('\u0010') >= 0)
                            {
                                name = name.Substring(2);
                            }
                            string sub_patern = teacher_week_patern + location_time_patern + "(," + teacher_week_patern + location_time_patern + ")*" + "?";
                            Regex sub_rg = new Regex(@sub_patern);
                            MatchCollection sub_matchCollection = sub_rg.Matches(match.Value);
                            for (int j_in = 0; j_in < sub_matchCollection.Count; j_in++)
                            {
                                Match sub_match = sub_matchCollection[j_in];
                                string time = sub_match.Groups["time"].ToString();
                                string location = sub_match.Groups["location"].ToString();

                                string t_w_patern = teacher_patern + week_patern;
                                Regex teacher_week_rg = new Regex(@t_w_patern);
                                MatchCollection teacher_week_matchCollection = teacher_week_rg.Matches(sub_match.Value);

                                for (int k = 0; k < teacher_week_matchCollection.Count; k++)
                                {
                                    Match sub_t_w_match = teacher_week_matchCollection[k];
                                    string teacher = sub_t_w_match.Groups["teacher"].Value;
                                    string week = sub_t_w_match.Groups["week"].Value;
                                    if (teacher[0] == ',')
                                    {
                                        teacher = teacher.Substring(1);
                                    }
                                    s2show += i + " " + j + name + week + teacher + time + location + "\n";

                                    week = week.Replace("周", "");
                                    List<int> week_int = new List<int>();
                                    
                                    foreach (string str in week.Split(','))
                                    {
                                        if (str.IndexOf("-") > 0)
                                        {
                                            string[] str2 = str.Split('-');
                                            if (str.IndexOf("单") > 0)
                                            {
                                                int start = int.Parse(str2[0]);
                                                start = start % 2 == 1 ? start : start + 1;
                                                int end = int.Parse(str2[1].Replace("单", ""));
                                                end = end % 2 == 1 ? end : end - 1;
                                                Subjects[i, j].pushBack(name, teacher, location, start, end, 1);
                                            }
                                            else if (str.IndexOf("双") > 0)
                                            {
                                                int start = int.Parse(str2[0]);
                                                start = start % 2 == 0 ? start : start + 1;
                                                int end = int.Parse(str2[1].Replace("双", ""));
                                                end = end % 2 == 0 ? end : end - 1;
                                                Subjects[i, j].pushBack(name, teacher, location, start, end, 2);
                                            }
                                            else
                                            {
                                                int start = int.Parse(str2[0]);
                                                int end = int.Parse(str2[1]);
                                                Subjects[i, j].pushBack(name, teacher, location, start, end, 0);
                                            }
                                        }
                                        else
                                        {
                                            int start = int.Parse(str);
                                            Subjects[i, j].pushBack(name, teacher, location, start, start, 0);
                                        }
                                    }

                                }
                            }
                        }
                        res[i, j] = s2show;
                    }
                }
            }
            global.state = 1;
            return 0;
        }

        public static string getSetting(string name)
        {
            XmlNode root = doc.ChildNodes[0];
            foreach (XmlNode n in root.ChildNodes)
            {
                if (n.Name == name)
                {
                    return n.InnerText;
                }
            }
            return "";
        }

        public static void setSetting(string name, string value)
        {
            XmlNode root = doc.ChildNodes[0];
            foreach (XmlNode n in root.ChildNodes)
            {
                if (n.Name == name)
                {
                    n.InnerText = value;
                    return;
                }
            }
            XmlNode newNode = doc.CreateElement(name);
            newNode.InnerText = value;
            doc.ChildNodes[0].AppendChild(newNode);
        }

        public static async Task<int> saveSetting()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile setting_file = await folder.TryGetItemAsync("setting.xml") as StorageFile;
            if (setting_file == null)
            {
                setting_file = await folder.CreateFileAsync("setting.xml");
            }
            using (Stream f = await setting_file.OpenStreamForWriteAsync())
                doc.Save(f);
            return 0;
        }

        public static async Task<int> readSetting()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile setting_file = await folder.TryGetItemAsync("setting.xml") as StorageFile;
            if (setting_file != null)
            {
                doc = new XmlDocument();
                using (Stream f = await setting_file.OpenStreamForReadAsync())
                    doc.Load(f);
                //temp_message.Text = doc.ChildNodes[0].ChildNodes[0].InnerText;
            }
            else
            {
                setting_file = await folder.CreateFileAsync("setting.xml");
                doc = new XmlDocument();
                doc.CreateXmlDeclaration("1.0", "utf-8", "yes");
                XmlNode rootNode = doc.CreateElement("Settings");
                doc.AppendChild(rootNode);
                using (Stream f = await setting_file.OpenStreamForWriteAsync())
                    doc.Save(f);
            }
            return 0;
        }

        public static async Task<int> readSaved()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile saved = await folder.TryGetItemAsync("saved.xls") as StorageFile;
            if (saved != null)
            {
                file = saved;
                return await readXls();
            }
            else
            {
                return -3;
            }
        }

        public static int getWeekOfToday()
        {
            try
            {
                long temp = Convert.ToInt64(global.getSetting("firstMondayFileTime"));
                DateTime date = DateTime.FromFileTime(temp);
                return ((DateTime.Now - date).Days / 7 + 1);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

    }
}
