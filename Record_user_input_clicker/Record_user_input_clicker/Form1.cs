using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace Record_user_input_clicker
{
    public partial class Form1 : Form
    {
        public static Capture_handle ch_instantce;
        public bool stop, capture, reset;


        public Form1()
        {

            //class specific variables
            stop = false;
            capture = false;
            reset = false;

            ch_instantce = new Capture_handle();
            ch_instantce._hookID = ch_instantce.SetHook(ch_instantce._proc);

            InitializeComponent();

        }//end of constructor


        private void listView1_SelectedIndexChanged(object sender, EventArgs e){}

        private void capture_btn_Click(object sender, EventArgs e)
        {
           
            if (capture == true)
                return;

            capture = true;
            ch_instantce.update_list = true;
        }//end of capture_btn_Click


        private void stop_btn_Click(object sender, EventArgs e)
        {
            if (capture == false || stop == true)
                return;

            stop = true;

            this.richTextBox1.AppendText("[With Seperation]");
            this.richTextBox1.AppendText(Environment.NewLine);
            for (int i = 0; i < ch_instantce.record_input.Count; i++)
                this.richTextBox1.AppendText(ch_instantce.record_input[i] + " -> ");

            this.richTextBox1.AppendText(Environment.NewLine);
            this.richTextBox1.AppendText(Environment.NewLine + "[No Seperation]");
            this.richTextBox1.AppendText(Environment.NewLine);
            for (int i = 0; i < ch_instantce.record_input.Count; i++)
                this.richTextBox1.AppendText(ch_instantce.record_input[i]);

            this.richTextBox1.AppendText(Environment.NewLine);
            this.richTextBox1.AppendText(Environment.NewLine + "[Input count]");
            this.richTextBox1.AppendText(Environment.NewLine);

            ch_instantce.calculate_character_occurance();
            for (int i = 0; i < ch_instantce.input_occurance.Count; i++)
                this.richTextBox1.AppendText(Environment.NewLine + ch_instantce.input_occurance[i]);
        }//end of stop method


        //options, display stuff like how to assign sounds to 'clicks'
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            // System.Media.SystemSounds.Asterisk.Play();
            //Options_form frm = new Options_form();
            // frm.Show();
            Options_form.GetForm.Show();
        }





        private void Form1_Load(object sender, EventArgs e) {}


        //help info
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To use tool press capture to begin recording input and then stop to end, reset to reset.\n" +
                            "For more information about this tool please contact Paul Adair at Newcastle based Leica Biosystems\n");
        }

        private void reset_btn_Click(object sender, EventArgs e)
        {
            capture = false;
            stop = false;

            this.richTextBox1.Clear();
            ch_instantce.record_input.Clear();
            ch_instantce.input_occurance.Clear();
        }//end of reset



    }//end of form definition class
}//end of namespace
