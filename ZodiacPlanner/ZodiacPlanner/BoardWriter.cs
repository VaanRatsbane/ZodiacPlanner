using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZodiacPlanner
{
    class BoardWriter
    {

        static string[] prefix = new string[] { "6C", "69", "63", "64", "18", "00", "18", "00" };
        public static string filePath;

        public static string[] Load(string filePath)
        {
            FileStream fs = null;
            try
            {
                if (new System.IO.FileInfo(filePath).Length != 1160)
                    return null;
                fs = new FileStream(filePath, FileMode.Open);
                int hexIn;
                var list = new List<string>();
                for (int i = 0; (hexIn = fs.ReadByte()) != -1; i++)
                    if(i>=8)
                        list.Add(string.Format("{0:X2}", hexIn));

                if (list.Count != (24 * 24 * 2))
                    return null;

                BoardWriter.filePath = filePath;
                return list.ToArray();
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if(fs != null)
                    fs.Close();
            }
        }

        public static void Write(TableLayoutPanel table)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                using (BinaryWriter r = new BinaryWriter(fs))
                {
                    foreach(var s in prefix)
                        r.Write(Convert.ToByte(s, 16));

                    for (int y = 1; y < 25; y++)
                        for (int x = 1; x < 25; x++)
                        {
                            var control = table.GetControlFromPosition(x, y);
                            if (control.Tag == null)
                            {
                                r.Write(Convert.ToByte("FF", 16));
                                r.Write(Convert.ToByte("FF", 16));
                            }
                            else
                            {
                                var btn = control as Button;
                                var l = btn.Tag as Licence;
                                var binaryval = Convert.ToByte(l.pair1, 16);
                                var binaryval2 = Convert.ToByte(l.pair2, 16);
                                r.Write(binaryval);
                                r.Write(binaryval2);
                            }
                        }
                    r.Flush();
                }
            }

            
        }

    }
}
