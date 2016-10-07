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
        static byte[][] strs = new byte[100][];
        static int[,] str_id = new int[100, 100];

        public static StorageFile File
        {
            get
            {
                return file;
            }
        }

        public static string Strs(int a)
        {
            return System.Text.Encoding.Unicode.GetString(strs[a]);
        }

        public static int[,] Str_id
        {
            get
            {
                return str_id;
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
                byte b1, b2 = 0;
                int num_of_str = 0;

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
            }
            return 0;
        }



    }
}
