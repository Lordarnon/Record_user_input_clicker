using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Record_user_input_clicker
{

    public class Capture_handle
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        public LowLevelKeyboardProc _proc;
        public IntPtr _hookID;

        public bool update_list;
        public List<string> record_input;
        public List<string> input_occurance;

        

        public Capture_handle()
        {
            update_list = false;
            _proc = HookCallback;
            record_input = new List<string>();
            input_occurance = new List<string>();
            _hookID = IntPtr.Zero;
        }



        public IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        public IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (update_list)
            {
                if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
                {
                    int vkCode = Marshal.ReadInt32(lParam);

                    record_input.Add(KeyCodeToUnicode((Keys)vkCode));

                    if((KeyCodeToUnicode((Keys)vkCode).Length <= 1) && 
                        (KeyCodeToUnicode((Keys)vkCode) == options_settings.sound_delimiter))
                    {
                        System.Media.SystemSounds.Asterisk.Play();
                    }
                    //Console.WriteLine((Keys)vkCode);

                }//end of if

                return CallNextHookEx(_hookID, nCode, wParam, lParam);
            }//end of if
            return IntPtr.Zero;
        }//end of HookCallback


        public string KeyCodeToUnicode(Keys key)
        {
            byte[] keyboardState = new byte[255];
            bool keyboardStateStatus = GetKeyboardState(keyboardState);

            if (!keyboardStateStatus)
                return "";

            if (key == Keys.Space)
                return "[SPACE]";

            uint virtualKeyCode = (uint)key;
            uint scanCode = MapVirtualKey(virtualKeyCode, 0);
            IntPtr inputLocaleIdentifier = GetKeyboardLayout(0);

            StringBuilder result = new StringBuilder();
            ToUnicodeEx(virtualKeyCode, scanCode, keyboardState, result, (int)5, (uint)0, inputLocaleIdentifier);

            return result.ToString();
        }//end of KeyCodeToUnicode


        public void calculate_character_occurance()
        {
            List<string> temp_record = record_input;
            List<string> noDupes = temp_record.Distinct().ToList();

            for (int i = 0; i < noDupes.Count; i++)
            {

                string occur = noDupes[i];
                int count = record_input.Where(x => x.Equals(occur)).Count();

                input_occurance.Add(noDupes[i] + ": " + count.ToString());
            }

        }



        //DLLimports for string conversion 
        [DllImport("user32.dll")]
        static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        static extern uint MapVirtualKey(uint uCode, uint uMapType);

        [DllImport("user32.dll")]
        static extern IntPtr GetKeyboardLayout(uint idThread);

        [DllImport("user32.dll")]
        static extern int ToUnicodeEx(uint wVirtKey, uint wScanCode, byte[] lpKeyState, [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwszBuff, int cchBuff, uint wFlags, IntPtr dwhkl);

        //DLLimports for capture

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }//end of class
}//end of namespace
