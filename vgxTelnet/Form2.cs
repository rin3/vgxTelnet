using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace vgxTelnet
{
    public partial class Form2 : Form
    {     
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // fix if port is blank
            if (textBox6.Text.Length == 0)
            {
                textBox6.Text = "0";
            }
            if (textBox7.Text.Length == 0)
            {
                textBox7.Text = "0";
            }
            if (textBox8.Text.Length == 0)
            {
                textBox8.Text = "0";
            }
            if (textBox9.Text.Length == 0)
            {
                textBox9.Text = "0";
            }
            if (textBox10.Text.Length == 0)
            {
                textBox10.Text = "0";
            }

            // update server list
            Properties.Settings.Default.svr1adr = textBox1.Text;
            Properties.Settings.Default.svr2adr = textBox2.Text;
            Properties.Settings.Default.svr3adr = textBox3.Text;
            Properties.Settings.Default.svr4adr = textBox4.Text;
            Properties.Settings.Default.svr5adr = textBox5.Text;
            Properties.Settings.Default.svr1port = int.Parse(textBox6.Text);
            Properties.Settings.Default.svr2port = int.Parse(textBox7.Text);
            Properties.Settings.Default.svr3port = int.Parse(textBox8.Text);
            Properties.Settings.Default.svr4port = int.Parse(textBox9.Text);
            Properties.Settings.Default.svr5port = int.Parse(textBox10.Text);

            // update server selection
            if (radioButton1.Checked)
            {
                Properties.Settings.Default.svr = 1;
            }
            else if (radioButton2.Checked)
            {
                Properties.Settings.Default.svr = 2;
            }
            else if (radioButton3.Checked)
            {
                Properties.Settings.Default.svr = 3;
            }
            else if (radioButton4.Checked)
            {
                Properties.Settings.Default.svr = 4;
            }
            else if (radioButton5.Checked)
            {
                Properties.Settings.Default.svr = 5;
            }

            // save to the settings
            Properties.Settings.Default.Save();

            // set new, if any, server variables
            update_SelectedServer();

            // hide the dialog
            Hide();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            readPropertiesSettingsDefaults();

            // set caret at home, not select whole string
            textBox1.SelectionStart = 0;
        }

        private void readPropertiesSettingsDefaults()
        {
            textBox1.Text = Properties.Settings.Default.svr1adr;
            textBox2.Text = Properties.Settings.Default.svr2adr;
            textBox3.Text = Properties.Settings.Default.svr3adr;
            textBox4.Text = Properties.Settings.Default.svr4adr;
            textBox5.Text = Properties.Settings.Default.svr5adr;
            textBox6.Text = Properties.Settings.Default.svr1port.ToString();
            textBox7.Text = Properties.Settings.Default.svr2port.ToString();
            textBox8.Text = Properties.Settings.Default.svr3port.ToString();
            textBox9.Text = Properties.Settings.Default.svr4port.ToString();
            textBox10.Text = Properties.Settings.Default.svr5port.ToString();

            // set radiobuttons corresponding to the selected server
            switch (Form1.svr)
            {
                case 2:
                    radioButton2.Checked = true;
                    break;
                case 3:
                    radioButton3.Checked = true;
                    break;
                case 4:
                    radioButton4.Checked = true;
                    break;
                case 5:
                    radioButton5.Checked = true;
                    break;
                default:
                    radioButton1.Checked = true;
                    break;
            }
        }

        static public void update_SelectedServer()
        {
            // set the selected server from a saved value
            Form1.svr = Properties.Settings.Default.svr;

            // set server variables
            switch (Form1.svr) {
                case 2:
                    Form1.host = Properties.Settings.Default.svr2adr;
                    Form1.port = Properties.Settings.Default.svr2port;
                    break;
                case 3:
                    Form1.host = Properties.Settings.Default.svr3adr;
                    Form1.port = Properties.Settings.Default.svr3port;
                    break;
                case 4:
                    Form1.host = Properties.Settings.Default.svr4adr;
                    Form1.port = Properties.Settings.Default.svr4port;
                    break;
                case 5:
                    Form1.host = Properties.Settings.Default.svr5adr;
                    Form1.port = Properties.Settings.Default.svr5port;
                    break;
                default:
                    Form1.host = Properties.Settings.Default.svr1adr;
                    Form1.port = Properties.Settings.Default.svr1port;
                    break;
                }
        }

        private void fontButton_Click(object sender, EventArgs e)
        {
            // get current font
            fontDialog1.Font = Form1.form1Instance.rcvFont;

            DialogResult dr = fontDialog1.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                Form1.form1Instance.rcvFont = fontDialog1.Font;
                Properties.Settings.Default.fFont = fontDialog1.Font;
            }
        }

        private void backgrndButton_Click(object sender, EventArgs e)
        {
            // get current color
            //colorDialog1.Color = Form1.form1Instance.bckColor;  //not working

            DialogResult dr = colorDialog1.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                Form1.form1Instance.bckColor = colorDialog1.Color;
                Properties.Settings.Default.bColor = colorDialog1.Color;
            }
        }

        private void colorButton_Click(object sender, EventArgs e)
        {
            // get current color
            //colorDialog1.Color = Form1.form1Instance.fontColor;  //not working

            DialogResult dr = colorDialog1.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                Form1.form1Instance.fontColor = colorDialog1.Color;
                Properties.Settings.Default.fColor = colorDialog1.Color;
            }
        }
    }
}