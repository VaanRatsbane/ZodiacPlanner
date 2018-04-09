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

        string[] prefix = new string[] { "6C", "69", "63", "64", "18", "00", "18", "00" };

        public static string[] Load(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open);
            int hexIn;
            var list = new List<string>();
            for (int i = 8; (hexIn = fs.ReadByte()) != -1; i++)
                list.Add(string.Format("{0:X2}", hexIn));
            return list.ToArray();
        }

        public static void Write(string filePath, TableLayoutPanel table)
        {

        }

    }
}
