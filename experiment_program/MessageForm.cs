using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;


namespace ExperimentProgram
{
    public partial class MessageForm : Form
    {
        int expState = 0; // 0 for stopped, 1 for started
        int leftOrRight = 0; // 1 for left, 2 for right

        public MessageForm()
        {
            InitializeComponent();
        }

        public void setCueState(string cueLabel, int state)
        {
            label1.Text = cueLabel;
            expState = state;
        }

        public void startClock()
        {
            timer1.Start();
        }

        public int getResult()
        {
            return leftOrRight;
        }

        private void MessageForm_KeyDown(object sender, KeyEventArgs e)
        {
            //Console.WriteLine("key down");
            if (expState==0 && e.KeyData == Keys.Down)
            {
                this.Close();
            }
            else if (expState == 1 && e.KeyData == Keys.Left)
            {
                leftOrRight = 1;
                Console.WriteLine("orientation => left");
                timer1.Stop();
                this.Close();
            }
            else if (expState == 1 && e.KeyData == Keys.Right)
            {
                leftOrRight = 2;
                Console.WriteLine("orientation => right");
                timer1.Stop();
                this.Close();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("No action");
            timer1.Stop();
            this.Close();
            //NativeMethods.keybd_event((Byte)40, 0x45, 0x1, (UIntPtr)0);
            //Console.WriteLine("want keydown");
        }
        public class NativeMethods
        {
            [DllImport("User32.dll")]
            public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
        }
        
    }
    
}
