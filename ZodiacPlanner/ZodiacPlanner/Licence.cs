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
            this.lpCost = lpCost;
            this.licenceType = licenceType;
            listViewItemContent = new string[] { pair1 + pair2, name, lpCost.ToString(), ParseType(licenceType) };
            inserted = false;
            color = Program.colors.Get(licenceType);
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

        public void ChangeCell(Button btn)
        {
            btn.BackColor = color;
            btn.Tag = this;
        }

        public void UpdateColor()
        {
            color = Program.colors.Get(licenceType);
        }

        static Dictionary<char, string> dict = new Dictionary<char, string>(33)
        {
            {'0', "Quickening"},    {'1', "Summon"},        {'2', "Essentials"},
            {'3', "Swords"},        {'4', "Greatswords"},   {'5', "Katanas"},
            {'6', "Ninja Swords"},  {'7', "Spears"},        {'8', "Poles"},
            {'9', "Bows"},          {'A', "Crossbows"},     {'B', "Guns"},
            {'C', "Axes & Hammers"},{'D', "Dagger"},        {'E', "Rods"},
            {'F', "Staves"},        {'G', "Maces"},         {'H', "Measures"},
            {'I', "Hand Bombs"},    {'J', "Shields"},       {'K', "Heavy Armor"},
            {'L', "Light Armor"},   {'M', "Mystic Armor"},  {'N', "Accessories"},
            {'O', "White Magicks"}, {'P', "Black Magicks"}, {'Q', "Time Magicks"},
            {'R', "Green Magicks"}, {'S', "Arcane Magicks"},{'T', "Augments"},
            {'U', "Gambit Slots"},  {'V', "Technicks"},     {'W', "Second Board"}
        };

        static string ParseType(char t)
        {
            return dict[t];
        }

    }
}
