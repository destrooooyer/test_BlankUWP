using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;

namespace App1
{
    class ParseXls
    {
        public static async Task<Course[,,]> parse(StorageFile file)
        {
            string[,] strs = await read(file);
            Course[,,] res = new Course[6,7,20];
            for(int i = 0; i < 6; i++)
            {
                for(int j = 0; j < 7; j++)
                {
                    string t = strs[i, j];
                    int p = 0;
                    for (int k=0;k<20;k++)
                    {
                        Course c = new Course();
                        c.name = t;
                        res[i, j, k] = c;
                    }

                    //TODO:parse
                    Regex r1 = new Regex(@"^(.+)</br>(.+)\[(.+)周](.*)\s+第(.+)节");
                    Match m1 = r1.Match(t);
                    ;
                }
            }
            return res;
        }

        public static async Task<string[,]> read(StorageFile file)
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
            string[,] res = new string[6, 7];
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    string s = System.Text.Encoding.Unicode.GetString(strs[str_id[i+2, j+2]]);
                    res[i, j] = s == null ? "" : s;
                }
            }
            return res;
        }
    }
}
