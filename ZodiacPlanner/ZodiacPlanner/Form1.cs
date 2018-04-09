using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZodiacPlanner
{
    public partial class Form1 : Form
    {

        LicenceCollection collection;
        Button selectedCell;
        Point? selectedPoint;

        public Form1()
        {
            InitializeComponent();
            collection = new LicenceCollection();
            updateList();
        }

        //SEARCH
        private void searchBox_TextChanged(object sender, EventArgs e)
        {
            updateList();
        }

        private void updateList()
        {
            licenceList.Items.Clear();
            var list = collection.Search(searchBox.Text);
            foreach (var l in list)
                licenceList.Items.Add(l.GetListViewItem());
        }

        //Clicking list
        private void licenceList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (licenceList.SelectedItems.Count == 1) {
                var l = licenceList.SelectedItems[0].Tag as Licence;
                if (l.contents.Length > 0)
                    listLicence1.Text = l.contents[0];
                if (l.contents.Length > 1)
                    listLicence1.Text = l.contents[1];
                if (l.contents.Length > 2)
                    listLicence1.Text = l.contents[2];
                if (l.contents.Length > 3)
                    listLicence1.Text = l.contents[3];
            }
        }

        //Clicking board
        private void boardPanel_Click(object sender, EventArgs e)
        {
                        
            selectedPoint = GetRowColIndex(
                boardPanel,
                boardPanel.PointToClient(Cursor.Position));
            if (selectedPoint.HasValue && selectedPoint.Value.X > 0 && selectedPoint.Value.Y > 0
                && selectedPoint.Value.X < 25 && selectedPoint.Value.Y < 25)
            {
                Console.WriteLine($"{selectedPoint.Value.X} {selectedPoint.Value.Y}");

                selectedCellText.Text = $"[{(char)(selectedPoint.Value.X + 64)}{selectedPoint.Value.Y}] ";

                var control = boardPanel.GetControlFromPosition(selectedPoint.Value.X, selectedPoint.Value.Y);
                if(control != null)
                {
                    selectedCell = control as Button;

                    var ev = e as MouseEventArgs;
                    if (rightClick.Checked && ev.Button == MouseButtons.Right)
                    {
                        clearButton_Click(sender, e);
                    }

                    var currentLicence = selectedCell.Tag as Licence;
                    selectedCellText.Text += currentLicence.name;
                    selectedLPCost.Text = currentLicence.lpCost.ToString();
                    if (currentLicence.contents.Length > 0)
                        selectedContent1.Text = currentLicence.contents[0];
                    if (currentLicence.contents.Length > 1)
                        selectedContent2.Text = currentLicence.contents[1];
                    if (currentLicence.contents.Length > 2)
                        selectedContent3.Text = currentLicence.contents[2];
                    if (currentLicence.contents.Length > 3)
                        selectedContent4.Text = currentLicence.contents[3];
                }
                else
                {
                    selectedCell = null;
                }
            }
        }

        Point? GetRowColIndex(TableLayoutPanel tlp, Point point)
        {
            if (point.X > tlp.Width || point.Y > tlp.Height)
                return null;

            int w = tlp.Width;
            int h = tlp.Height;
            int[] widths = tlp.GetColumnWidths();

            int i;
            for (i = widths.Length - 1; i >= 0 && point.X < w; i--)
                w -= widths[i];
            int col = i + 1;

            int[] heights = tlp.GetRowHeights();
            for (i = heights.Length - 1; i >= 0 && point.Y < h; i--)
                h -= heights[i];

            int row = i + 1;

            return new Point(col, row);
        }

        //Clear cell
        private void clearButton_Click(object sender, EventArgs e)
        {
            if(selectedCell != null)
            {
                selectedCell.Parent.Controls.Remove(selectedCell);
                (selectedCell.Tag as Licence).Clear();
                selectedCell = null;
                updateList();
            }
        }

        //Insert licence
        private void insertButton_Click(object sender, EventArgs e)
        {
            if(licenceList.SelectedItems.Count == 1 && selectedPoint != null && selectedCell == null)
            {
                var l = licenceList.SelectedItems[0].Tag as Licence;
                if (!l.inserted)
                {
                    var btn = l.GetCell(boardPanel);
                    btn.MouseClick += (s, ev) => boardPanel_Click(s, ev);
                    btn.MouseDoubleClick += (s, ev) => boardPanel_DoubleClick(s, ev);
                    boardPanel.Controls.Add(btn, selectedPoint.Value.X, selectedPoint.Value.Y);
                    selectedCell = btn;
                    l.Insert();

                    selectedCellText.Text += l.name;
                    selectedLPCost.Text = l.lpCost.ToString();
                    if (l.contents.Length > 0)
                        selectedContent1.Text = l.contents[0];
                    if (l.contents.Length > 1)
                        selectedContent2.Text = l.contents[1];
                    if (l.contents.Length > 2)
                        selectedContent3.Text = l.contents[2];
                    if (l.contents.Length > 3)
                        selectedContent4.Text = l.contents[3];

                    var index = licenceList.SelectedIndices[0];
                    licenceList.Items.RemoveAt(index);
                    licenceList.Items.Insert(index, l.GetListViewItem());
                }
            }
        }

        private void boardPanel_DoubleClick(object sender, EventArgs e)
        {
            if (doubleClick.Checked)
                insertButton_Click(sender, e);
        }

        private void Load()
        {
            
        }
    }
}
