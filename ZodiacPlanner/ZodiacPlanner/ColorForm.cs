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
    public partial class ColorForm : Form
    {

        private ColorManager colors;
        private bool hasChanges;

        public ColorForm()
        {
            InitializeComponent();
            this.colors = Program.colors;
            LoadColors();
            hasChanges = false;
        }

        private void LoadColors()
        {
            quickenings.BackColor = colors.Get('0');
            summons.BackColor = colors.Get('1');
            essentials.BackColor = colors.Get('2');
            swords.BackColor = colors.Get('3');
            greatswords.BackColor = colors.Get('4');
            katanas.BackColor = colors.Get('5');
            ninjaswords.BackColor = colors.Get('6');
            spears.BackColor = colors.Get('7');
            poles.BackColor = colors.Get('8');
            bows.BackColor = colors.Get('9');
            crossbows.BackColor = colors.Get('A');
            guns.BackColor = colors.Get('B');
            axes.BackColor = colors.Get('C');
            daggers.BackColor = colors.Get('D');
            rods.BackColor = colors.Get('E');
            staves.BackColor = colors.Get('F');
            maces.BackColor = colors.Get('G');
            measures.BackColor = colors.Get('H');
            handbombs.BackColor = colors.Get('I');
            shields.BackColor = colors.Get('J');
            heavy.BackColor = colors.Get('K');
            light.BackColor = colors.Get('L');
            mystic.BackColor = colors.Get('M');
            accessories.BackColor = colors.Get('N');
            whm.BackColor = colors.Get('O');
            blm.BackColor = colors.Get('P');
            time.BackColor = colors.Get('Q');
            green.BackColor = colors.Get('R');
            arcane.BackColor = colors.Get('S');
            augments.BackColor = colors.Get('T');
            gambits.BackColor = colors.Get('U');
            technicks.BackColor = colors.Get('V');
            secondboard.BackColor = colors.Get('W');
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            if (hasChanges)
            {
                colors.Save();
                (Owner as Form1).redraw = true;
            }
            Close();
        }

        private void changeColor(Control control, char type)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                control.BackColor = colorDialog1.Color;
                colors.Set(type, colorDialog1.Color);
                hasChanges = true;
            }
        }

        private void quickenings_Click(object sender, EventArgs e)
        {
            changeColor(quickenings, '0');
        }

        private void summons_Click(object sender, EventArgs e)
        {
            changeColor(summons, '1');
        }

        private void essentials_Click(object sender, EventArgs e)
        {
            changeColor(essentials, '2');
        }

        private void swords_Click(object sender, EventArgs e)
        {
            changeColor(swords, '3');
        }

        private void greatswords_Click(object sender, EventArgs e)
        {
            changeColor(greatswords, '4');
        }

        private void katanas_Click(object sender, EventArgs e)
        {
            changeColor(katanas, '5');
        }

        private void ninjaswords_Click(object sender, EventArgs e)
        {
            changeColor(ninjaswords, '6');
        }

        private void spears_Click(object sender, EventArgs e)
        {
            changeColor(spears, '7');
        }

        private void poles_Click(object sender, EventArgs e)
        {
            changeColor(poles, '8');
        }

        private void bows_Click(object sender, EventArgs e)
        {
            changeColor(bows, '9');
        }

        private void crossbows_Click(object sender, EventArgs e)
        {
            changeColor(crossbows, 'A');
        }

        private void guns_Click(object sender, EventArgs e)
        {
            changeColor(guns, 'B');
        }

        private void axes_Click(object sender, EventArgs e)
        {
            changeColor(axes, 'C');
        }

        private void daggers_Click(object sender, EventArgs e)
        {
            changeColor(daggers, 'D');
        }

        private void rods_Click(object sender, EventArgs e)
        {
            changeColor(rods, 'E');
        }

        private void staves_Click(object sender, EventArgs e)
        {
            changeColor(staves, 'F');
        }

        private void maces_Click(object sender, EventArgs e)
        {
            changeColor(maces, 'G');
        }

        private void measures_Click(object sender, EventArgs e)
        {
            changeColor(measures, 'H');
        }

        private void handbombs_Click(object sender, EventArgs e)
        {
            changeColor(handbombs, 'I');
        }

        private void shields_Click(object sender, EventArgs e)
        {
            changeColor(shields, 'J');
        }

        private void heavy_Click(object sender, EventArgs e)
        {
            changeColor(heavy, 'K');
        }

        private void light_Click(object sender, EventArgs e)
        {
            changeColor(light, 'L');
        }

        private void mystic_Click(object sender, EventArgs e)
        {
            changeColor(mystic, 'M');
        }

        private void accessories_Click(object sender, EventArgs e)
        {
            changeColor(accessories, 'N');
        }

        private void whm_Click(object sender, EventArgs e)
        {
            changeColor(whm, 'O');
        }

        private void blm_Click(object sender, EventArgs e)
        {
            changeColor(blm, 'P');
        }

        private void time_Click(object sender, EventArgs e)
        {
            changeColor(time, 'Q');
        }

        private void green_Click(object sender, EventArgs e)
        {
            changeColor(green, 'R');
        }

        private void arcane_Click(object sender, EventArgs e)
        {
            changeColor(arcane, 'S');
        }

        private void augments_Click(object sender, EventArgs e)
        {
            changeColor(augments, 'T');
        }

        private void gambits_Click(object sender, EventArgs e)
        {
            changeColor(gambits, 'U');
        }

        private void technicks_Click(object sender, EventArgs e)
        {
            changeColor(technicks, 'V');
        }

        private void secondboard_Click(object sender, EventArgs e)
        {
            changeColor(secondboard, 'W');
        }

        private void revertDefaults_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Revert colors to their default values?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                colors.Initialize();
                LoadColors();
                hasChanges = true;
            }
        }
    }
}
