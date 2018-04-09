using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZodiacPlanner
{
    class Licence
    {
        public string pair1 { get; private set; }
        public string pair2 { get; private set; }
        public int lpCost { get; private set; }
        public char licenceType { get; private set; }
        public string name { get; private set; }
        public string[] contents { get; private set; }

        public bool inserted { get; private set; }
        Color color;

        string[] listViewItemContent;

        public Licence(char licenceType, string pair1, string pair2, int lpCost, string name, string[] contents)
        {
            this.pair1 = pair1;
            this.pair2 = pair2;
            this.name = name;
            this.contents = contents;
            listViewItemContent = new string[] { pair1 + pair2, name, lpCost.ToString() };
            inserted = false;

            switch(licenceType)
            {
                case 'A':
                    color = Color.WhiteSmoke;
                    break;
                case 'B':
                    color = Color.Gold;
                    break;
                case 'C':
                    color = Color.LightGray;
                    break;
                case '0': //Handbombs/Guns
                    color = Color.Violet;
                    break;
                case '1': //Staves/Maces/Measures
                    color = Color.Blue;
                    break;
                case '2': //Shields
                    color = Color.SandyBrown;
                    break;
                case '3': //Weapons
                    color = Color.SteelBlue;
                    break;
                case '4': //Magicks
                    color = Color.Purple;
                    break;
                case '5': //Augments
                    color = Color.Pink;
                    break;
                case '6': //Accessories
                    color = Color.Green;
                    break;
                case '7': //Technicks
                    color = Color.MediumVioletRed;
                    break;
                case '8': //Gambit Slots
                    color = Color.Yellow;
                    break;
                case '9': //Summons
                    color = Color.OrangeRed;
                    break;
                default: throw new Exception($"Error in licenceinfo.txt, no such type as {licenceType}");
            }
        }

        public void Insert()
        {
            inserted = true;
        }

        public void Clear()
        {
            inserted = false;
        }

        public bool Query(string query)
        {
            query = query.ToLower();

            if (name.ToLower().Contains(query))
                return true;
            if (contents.Length > 0 && contents[0].ToLower().Contains(query))
                return true;
            if (contents.Length > 1 && contents[1].ToLower().Contains(query))
                return true;
            if (contents.Length > 2 && contents[2].ToLower().Contains(query))
                return true;
            if (contents.Length > 3 && contents[3].ToLower().Contains(query))
                return true;
            return false;
        }

        public ListViewItem GetListViewItem()
        {
            return new ListViewItem(listViewItemContent)
            {
                BackColor = inserted ? Color.Gold : Color.White,
                Tag = this
            };
        }

        public Button GetCell(Control parent)
        {
            var btn = new Button()
            {
                BackColor = color,
                Dock = DockStyle.Fill,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(0),
                Parent = parent,
                Tag = this
            };

            return btn;
        }

    }
}
