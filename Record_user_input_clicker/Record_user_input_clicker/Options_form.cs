using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Record_user_input_clicker
{
    public partial class Options_form : Form
    {
         public Options_form()
         {
             InitializeComponent();
         }

        private static Options_form inst;
        public static Options_form GetForm
        {
            get
            {
                if (inst == null || inst.IsDisposed)
                {
                    inst = new Options_form();
                }
                return inst;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // set the delimiter
            options_settings.sound_delimiter = textBox1.Text;
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {}


    }
}
