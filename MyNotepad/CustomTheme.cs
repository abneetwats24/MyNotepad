using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyNotepad
{
    public partial class CustomTheme : Form
    {
        public CustomTheme(MyNotepad pMain)
        {
            InitializeComponent();
            _CustomThemeForm2 = pMain;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private MyNotepad _CustomThemeForm2 = new MyNotepad();

        


        private void MenuBgColorButton_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
                _CustomThemeForm2._MenuBgColor = colorDialog1.Color;
        }

        private void MenuForeColorButton_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
                _CustomThemeForm2._MenuForeColor = colorDialog1.Color;
        }

        private void TextBoxBgColorButton_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            _CustomThemeForm2._TextBoxBgColor = colorDialog1.Color;
            
                  
        }

        private void TextBoxForeColorButton_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            _CustomThemeForm2._TextBoxForeColor = colorDialog1.Color;
        }
    }
}
