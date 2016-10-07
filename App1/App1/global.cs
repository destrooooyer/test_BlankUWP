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
        public static int state=0;

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
                if(a==null) a=await folder.CreateFileAsync("saved.xls");
                await file.CopyAndReplaceAsync(a);
                readXls();
            }
            return 0;
        }

        public static async Task<int> readXls()
        {
            byte b1, b2 = 0;
            int num_of_str = 0;
            byte[][] strs = new byte[100][];
            int[,] str_id = new int[100, 100];
            using (Stream file1 = await file.OpenStreamForReadAsync())
            {
                using (BinaryReader read = new BinaryReader(file1))
                {
                    //read.Read(data, 0, 10000);
                    try
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
                    catch (Exception exce)
                    {

                    }
                }
            }
            String course_patern = "\\</br\\>(?<course>[\\u4E00-\\uFA29|\\s|\\S]+?)\\<\\/br\\>";
            String tail_patern = "(?<teacher>[\\u4E00-\\uFA29|\\s|\\,]*)\\[(?<week>[\\,|\\-|\\s|\\d|\\u4E00-\\uFA29]+)](\\<\\/br\\>)?(?<location>[\\w|\\s|\\-|\\(||\\)]+)?(?<time>\\u7B2C[\\d|\\,]+\\u8282)(,(?<teacher>[\\u4E00-\\uFA29|\\s]*)\\[(?<week>[\\,|\\-|\\s|\\d|\\u4E00-\\uFA29]+)]\\<\\/br\\>(?<location>[\\w|\\s|\\-|\\(||\\)]+)?(?<time>\\u7B2C[\\d|\\,]+\\u8282))*";
            String patern = course_patern + tail_patern;
            System.Text.RegularExpressions.Regex rg = new System.Text.RegularExpressions.Regex(@patern);
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    string s = System.Text.Encoding.Unicode.GetString(strs[str_id[i + 2, j + 2]]);
                    if (s == null||s=="")
                    {
                        res[i, j] = "";
                    }else
                    {
                        String target_s = s.Replace("，", ",");
                        target_s = target_s.Replace("\n", "");
                        target_s = "</br>" + target_s;
                        System.Text.RegularExpressions.MatchCollection matchCollection = rg.Matches(target_s);
                        String s2show = "";
                        for (int i_in = 0; i_in < matchCollection.Count; i_in++)
                        {
                            System.Text.RegularExpressions.Match match = matchCollection[i_in];
                            s2show=s2show+match.Groups["course"]+"\n";
                            s2show += "----------------------------------\n";
                            String sub_patern = tail_patern + "?";
                            System.Text.RegularExpressions.Regex sub_rg = new System.Text.RegularExpressions.Regex(@sub_patern);
                            System.Text.RegularExpressions.MatchCollection sub_matchCollection = sub_rg.Matches(match.Value);
                            for (int j_in = 0; j_in < sub_matchCollection.Count; j_in++)
                            {
                                System.Text.RegularExpressions.Match sub_match = sub_matchCollection[j_in];
                                String teacher = sub_match.Groups["teacher"].Value;
                                if ( teacher[0] == ','){
                                    teacher = teacher.Substring(1);
                                }
                                s2show = s2show + "teacher:"+teacher+"\n";
                                s2show = s2show + "week:"+sub_match.Groups["week"]+"\n";
                                s2show = s2show + "time:"+sub_match.Groups["time"] + "\n";
                                s2show = s2show + "location:"+sub_match.Groups["location"] + "\n";
                                if (j_in < sub_matchCollection.Count - 1)
                                {
                                    s2show = s2show + "------------\n";
                                }
                            }
                            if (i_in < matchCollection.Count - 1)
                            {
                                s2show += "----------------------------------\n";
                            }
                        }
                        res[i, j] = s2show;
                    }
                }
            }
            return 0;
        }

    }
}
