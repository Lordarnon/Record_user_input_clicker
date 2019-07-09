using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;

namespace Record_user_input_clicker
{

    //program wide variables//settings
    static class options_settings
    {
        public static string sound_delimiter { get; set; }
    }



    static class main
    {
        [STAThread]
        static void Main()
        {
            Application.Run(new Form1());
            
            UnhookWindowsHookEx(Form1.ch_instantce._hookID);

        }//end of main


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    }//end of class main
}//end of namespace
