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

        public static StorageFile File
        {
            get
            {
                return file;
            }
        }

        public static async Task<int> openfile()
        {
            FileOpenPicker openFile = new FileOpenPicker();
            openFile.SuggestedStartLocation = PickerLocationId.Downloads;
            openFile.ViewMode = PickerViewMode.List;
            openFile.FileTypeFilter.Add(".xls");

            file = await openFile.PickSingleFileAsync();
            if (file != null)
            {
                readXls(file);
            }
            return 0;
        }

        public static async void readXls(StorageFile file)
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

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    string s = System.Text.Encoding.Unicode.GetString(strs[str_id[i + 2, j + 2]]);
                    res[i, j] = s == null ? "" : s;
                }
            }
        }

    }
}
