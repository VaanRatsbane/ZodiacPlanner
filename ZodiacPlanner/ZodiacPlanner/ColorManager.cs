using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZodiacPlanner
{
    public class ColorManager
    {
        Dictionary<char, Color> colors;

        const string COLORFILE = "colors.json";

        public ColorManager()
        {
            try
            {
                var json = File.ReadAllText(COLORFILE);
                colors = JsonConvert.DeserializeObject<Dictionary<char, Color>>(json);
            }
            catch
            {
                Initialize();
            }
        }

        public void Save()
        {
            try
            {
                var json = JsonConvert.SerializeObject(colors, Formatting.Indented);
                File.WriteAllText(COLORFILE, json);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error saving the color settings.\n{e.ToString()}");
            }
        }

        public Color Get(char c)
        {
            return colors[c];
        }

        public void Set(char c, Color color)
        {
            colors[c] = color;
        }

        public void Initialize()
        {
            colors = new Dictionary<char, Color>();
            colors['0'] = System.Drawing.ColorTranslator.FromHtml("#ffff0000");
            colors['1'] = System.Drawing.ColorTranslator.FromHtml("#fff20000");
            colors['2'] = System.Drawing.ColorTranslator.FromHtml("#ff400000");
            colors['3'] = System.Drawing.ColorTranslator.FromHtml("#ffff9180");
            colors['4'] = System.Drawing.ColorTranslator.FromHtml("#ff241a33");
            colors['5'] = System.Drawing.ColorTranslator.FromHtml("#ff664133");
            colors['6'] = System.Drawing.ColorTranslator.FromHtml("#ffa64200");
            colors['7'] = System.Drawing.ColorTranslator.FromHtml("#ffa68d7c");
            colors['8'] = System.Drawing.ColorTranslator.FromHtml("#ffff8800");
            colors['9'] = System.Drawing.ColorTranslator.FromHtml("#ffffc480");
            colors['A'] = System.Drawing.ColorTranslator.FromHtml("#ff8c5e00");
            colors['B'] = System.Drawing.ColorTranslator.FromHtml("#ff332200");
            colors['C'] = System.Drawing.ColorTranslator.FromHtml("#ffffcc00");
            colors['D'] = System.Drawing.ColorTranslator.FromHtml("#fffff2bf");
            colors['E'] = System.Drawing.ColorTranslator.FromHtml("#ff403d30");
            colors['F'] = System.Drawing.ColorTranslator.FromHtml("#ff595300");
            colors['G'] = System.Drawing.ColorTranslator.FromHtml("#ffeeff00");
            colors['H'] = System.Drawing.ColorTranslator.FromHtml("#ff8da629");
            colors['I'] = System.Drawing.ColorTranslator.FromHtml("#ff858c69");
            colors['J'] = System.Drawing.ColorTranslator.FromHtml("#ff88ff00");
            colors['K'] = System.Drawing.ColorTranslator.FromHtml("#ffd9ffbf");
            colors['L'] = System.Drawing.ColorTranslator.FromHtml("#ff285916");
            colors['M'] = System.Drawing.ColorTranslator.FromHtml("#ff149900");
            colors['N'] = System.Drawing.ColorTranslator.FromHtml("#ff00ff88");
            colors['O'] = System.Drawing.ColorTranslator.FromHtml("#ff00a66f");
            colors['P'] = System.Drawing.ColorTranslator.FromHtml("#ff005947");
            colors['Q'] = System.Drawing.ColorTranslator.FromHtml("#ff00ffee");
            colors['R'] = System.Drawing.ColorTranslator.FromHtml("#ff8fbfbc");
            colors['S'] = System.Drawing.ColorTranslator.FromHtml("#ff00ccff");
            colors['T'] = System.Drawing.ColorTranslator.FromHtml("#ff1d6273");
            colors['U'] = System.Drawing.ColorTranslator.FromHtml("#ff0077b3");
            colors['V'] = System.Drawing.ColorTranslator.FromHtml("#ff0088ff");
            colors['W'] = System.Drawing.ColorTranslator.FromHtml("#ff13324d");
        }
    }

}
