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
        bool hasChanges;
        ListViewItem listViewSelected;
        int sortColumn;

        public bool redraw;

        public Form1()
        {
            InitializeComponent();
            
            for (int y = 1; y < 25; y++)
                for (int x = 1; x < 25; x++)
                {
                    var btn = new DoubleClickButton()
                    {
                        Dock = DockStyle.Fill,
                        Margin = new Padding(0),
                        FlatStyle = FlatStyle.Flat,
                        BackColor = Color.Transparent
                    };
                    btn.MouseClick += boardPanel_MouseClick;
                    btn.DoubleClick += boardPanel_DoubleClick;
                    btn.TabStop = false;
                    boardPanel.Controls.Add(btn, x, y);
                }

            Program.Log("Initialized licence board.", logLabel);

            collection = new LicenceCollection();
            updateList();
            selectedCell = boardPanel.GetControlFromPosition(1, 1) as Button;
            selectedCell.Select();
            boardPanel_MouseClick(null, null);
            NoChanges();
            redraw = false;

            if (Program.settings.location == new Point(-1, -1))
            {
                Program.settings.location = Location;
                Program.settings.width = Width;
                Program.settings.height = Height;
            }
            else
            {
                Location = Program.settings.location;
                Width = Program.settings.width;
                Height = Program.settings.height;
                rightClick.Checked = Program.settings.rightClick;
                doubleClick.Checked = Program.settings.doubleClick;
                if (Program.settings.darkMode)
                {
                    BackColor = SystemColors.ControlDarkDark;
                    doubleClick.ForeColor = SystemColors.ButtonHighlight;
                    rightClick.ForeColor = SystemColors.ButtonHighlight;
                }
            }
            Rainbow();
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
            if (listViewSelected == null)
                listViewSelected = licenceList.Items[0];
            listViewSelected.Selected = true;
        }

        //Clicking list
        private void licenceList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewSelected != null)
            {
                if ((listViewSelected.Tag as Licence).inserted)
                {
                    listViewSelected.BackColor = Color.Gold;
                    insertButton.Enabled = false;
                }
                else
                {
                    listViewSelected.BackColor = Color.Transparent;
                    insertButton.Enabled = true;
                }
            }

            if (licenceList.SelectedItems.Count == 1) {
                listViewSelected = licenceList.SelectedItems[0];
                var l = listViewSelected.Tag as Licence;
                listLicence1.Text = l.contents.Length > 0 ? l.contents[0] : "";
                listLicence2.Text = l.contents.Length > 1 ? l.contents[1] : "";
                listLicence3.Text = l.contents.Length > 2 ? l.contents[2] : "";
                listLicence4.Text = l.contents.Length > 3 ? l.contents[3] : "";
                listViewSelected.BackColor = Color.LightBlue;
            }
        }

        //Clicking board
        private void boardPanel_MouseClick(object sender, MouseEventArgs e)
        {
            selectedPoint = GetRowColIndex(
                boardPanel,
                boardPanel.PointToClient(Cursor.Position));
                        
            if (selectedPoint.HasValue && selectedPoint.Value.X > 0 && selectedPoint.Value.Y > 0
                && selectedPoint.Value.X < 25 && selectedPoint.Value.Y < 25)
            {
                Console.WriteLine($"{selectedPoint.Value.X} {selectedPoint.Value.Y}");

                selectedCellText.Text = $"[{(char)(selectedPoint.Value.X + 64)}{selectedPoint.Value.Y}] ";
                selectedLPCost.Text = "";
                selectedContent1.Text = "";
                selectedContent2.Text = "";
                selectedContent3.Text = "";
                selectedContent4.Text = "";

                var control = boardPanel.GetControlFromPosition(selectedPoint.Value.X, selectedPoint.Value.Y);
                if (control != null)
                {
                    if (selectedCell != null)
                    {
                        selectedCell.FlatAppearance.BorderColor = Color.Black;
                        selectedCell.FlatAppearance.BorderSize = 1;
                    }
                    selectedCell = control as Button;
                    //selectedCell.FlatAppearance.BorderColor = Color.Red;
                    selectedCell.FlatAppearance.BorderSize = 2;

                    if (rightClick.Checked && e.Button == MouseButtons.Right)
                    {
                        clearButton_Click(sender, e);
                    }

                    if (selectedCell.Tag != null)
                    {
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

                    selectedCell.PerformClick();
                }
                else
                {
                    selectedCell = null;
                }
            }
        }

        private Point? GetRowColIndex(TableLayoutPanel tlp, Point point)
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
            if(selectedCell != null && selectedCell.Tag != null)
            {
                (selectedCell.Tag as Licence).Clear();
                selectedCell.Tag = null;
                selectedCell.BackColor = Color.Transparent;
                updateList();
                Changes();
            }
        }

        //Insert licence
        private void insertButton_Click(object sender, EventArgs e)
        {
            if(selectedCell.Tag != null && selectedCell.Tag != listViewSelected.Tag)
            {
                if (MessageBox.Show("Are you sure?", "Overwrite current licence?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    var index = licenceList.Items.IndexOf(listViewSelected);
                    clearButton_Click(sender, e);
                    licenceList.Items[index].Selected = true;
                }
                else
                    return;
            }

            if(licenceList.SelectedItems.Count == 1 && selectedPoint != null && selectedCell.Tag == null)
            {
                var l = licenceList.SelectedItems[0].Tag as Licence;
                if (!l.inserted)
                {
                    var btn = boardPanel.GetControlFromPosition(selectedPoint.Value.X, selectedPoint.Value.Y) as Button;
                    l.ChangeCell(btn);
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

                    Changes();
                }
            }
        }

        private void boardPanel_DoubleClick(object sender, EventArgs e)
        {
            if (doubleClick.Checked)
                insertButton_Click(sender, e);
        }

        //Initialize
        private void newButton_Click(object sender, EventArgs e)
        {
            UnsavedChanges();

            for (int y = 1; y < 25; y++)
                for (int x = 1; x < 25; x++)
                {
                    var control = boardPanel.GetControlFromPosition(x, y) as Button;
                    if (control.Tag != null)
                    {
                        control.BackColor = Color.Transparent;
                        control.Tag = null;
                    }

                }

            collection.Clear();
            updateList();
            licenceList.Items[0].Selected = true;
            Text = "Zodiac Planner";
            Program.Log("Initialized licence board.", logLabel);
        }

        //LOAD
        private void loadBtn_Click(object sender, EventArgs e)
        {
            UnsavedChanges();

            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var s = BoardWriter.Load(openFileDialog1.FileName);
                if (s == null)
                {
                    MessageBox.Show("Failed to load.");
                    Program.Log("Initialized licence board.", logLabel);
                }
                else
                {
                    Text = $"Zodiac Planner - {openFileDialog1.FileName}";
                    collection.Clear();
                    bool dupes = false;
                    int j = 0;
                    for (int y = 1; y < 25; y++)
                        for (int x = 1; x < 25; x++)
                        {
                            string hex = s[j] + s[j + 1];
                            j += 2;
                            var btn = boardPanel.GetControlFromPosition(x, y) as Button;
                            if (hex == "FFFF")
                            {
                                if (btn.Tag != null)
                                {
                                    btn.BackColor = Color.Transparent;
                                    btn.Tag = null;
                                }
                                continue;
                            }
                            var l = collection.GetLicence(hex);
                            if (!l.inserted)
                            {
                                l.ChangeCell(btn);
                                l.Insert();
                            }
                            else
                            {
                                dupes = true;
                                Changes();
                                Program.Log($"Found dupe [X:{x}|Y:{y}] code {hex}.", logLabel);
                            }
                        }
                    updateList();
                    licenceList.Items[0].Selected = true;
                    if (dupes)
                    {
                        MessageBox.Show("There were duplicate licences in the file. They have been removed. Save to keep the fixes.");
                        Program.Log("Removed duplicates.", logLabel);
                    }
                    Program.Log("Loaded.", logLabel);
                }
            }
        }

        //SAVE
        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                BoardWriter.filePath = saveFileDialog1.FileName;
                BoardWriter.Write(boardPanel);
                MessageBox.Show("Saved.");
                Program.Log("Saved licence board.", logLabel);
                NoChanges();
            }
        }



        //QUIT
        private void quitBtn_Click(object sender, EventArgs e)
        {
            UnsavedChanges();
            hasChanges = false;
            Program.settings.location = Location;
            Program.settings.width = Width;
            Program.settings.height = Height;
            Close();// Environment.Exit(0);
        }

        private void UnsavedChanges()
        {
            if (hasChanges)
            {
                if (MessageBox.Show("You have unsaved changes. Would you like to save them?", "Save?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (BoardWriter.filePath == null)
                    {
                        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            BoardWriter.filePath = saveFileDialog1.FileName;
                        }
                        else
                        {
                            return;
                        }
                    }

                    BoardWriter.Write(boardPanel);
                    MessageBox.Show("Saved.");
                    Program.Log("Saved licence board.", logLabel);
                    NoChanges();
                }
            }
        }

        private void aboutBtn_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Zodiac Planner made by Vaan - Latest Version 2018/04/10\nOpen Steam Profile?","About", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                System.Diagnostics.Process.Start("https://steamcommunity.com/id/Vaan");
            }
        }

        //Dark mode
        private void toggleColor_Click(object sender, EventArgs e)
        {
            BackColor = (BackColor == SystemColors.Control) ? SystemColors.ControlDarkDark : SystemColors.Control;
            doubleClick.ForeColor = (doubleClick.ForeColor == SystemColors.ControlText) ? SystemColors.ButtonHighlight : SystemColors.ControlText;
            rightClick.ForeColor = (rightClick.ForeColor == SystemColors.ControlText) ? SystemColors.ButtonHighlight : SystemColors.ControlText;
            Program.settings.darkMode = !Program.settings.darkMode;
        }

        //sort list
        private void licenceList_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column != sortColumn)
            {
                // Set the sort column to the new column.
                sortColumn = e.Column;
                // Set the sort order to ascending by default.
                licenceList.Sorting = SortOrder.Ascending;
            }
            else
            {
                // Determine what the last sort order was and change it.
                if (licenceList.Sorting == SortOrder.Ascending)
                    licenceList.Sorting = SortOrder.Descending;
                else
                    licenceList.Sorting = SortOrder.Ascending;
            }

            // Call the sort method to manually sort.
            licenceList.Sort();
            // Set the ListViewItemSorter property to a new ListViewItemComparer
            // object.
            this.licenceList.ListViewItemSorter = new ListViewItemComparer(e.Column,
                                                              licenceList.Sorting);
        }

        private void updateColors()
        {
            for(int x = 1; x < 25; x++)
                for (int y = 1; y < 25; y++)
                {
                    var control = boardPanel.GetControlFromPosition(x, y);
                    if(control != null && control is Button && control.Tag != null)
                    {
                        var l = control.Tag as Licence;
                        l.UpdateColor();
                        l.ChangeCell(control as Button);
                    }
                }
            Program.Log("Colors have been updated.", logLabel);
        }

        private void colorSettings_Click(object sender, EventArgs e)
        {
            var form = new ColorForm();
            form.Owner = this;
            form.StartPosition = FormStartPosition.CenterParent;
            form.ShowDialog();
            if(redraw)
            {
                updateColors();
                redraw = false;
            }
        }

        private void outputLogCheck_CheckedChanged(object sender, EventArgs e)
        {
            Program.settings.saveLog = outputLogCheck.Checked;
        }

        private void showLogBtn_Click(object sender, EventArgs e)
        {
            var form = new LogForm();
            form.Owner = this;
            form.ShowDialog();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnsavedChanges();
            Program.settings.location = Location;
            Program.settings.width = Width;
            Program.settings.height = Height;
        }

        private void Changes()
        {
            if (!hasChanges)
                Text += '*';
            hasChanges = true;
        }

        private void NoChanges()
        {
            if (Text.EndsWith("*"))
                Text = Text.TrimEnd('*');
            hasChanges = false;
        }

        private async void Rainbow()
        {
            double i = 0;
            while (true)
            {
                await Task.Delay(100);
                if (selectedCell != null)
                {
                    i += 0.025;
                    if (i >= 1) i = 0.025;
                    selectedCell.FlatAppearance.BorderColor = Helpers.HSL2RGB(i, 0.5, 0.5);
                }
            }
        }

        private void doubleClick_CheckedChanged(object sender, EventArgs e)
        {
            Program.settings.doubleClick = doubleClick.Checked;
        }

        private void rightClick_CheckedChanged(object sender, EventArgs e)
        {
            Program.settings.rightClick = rightClick.Checked;
        }
    }
}
