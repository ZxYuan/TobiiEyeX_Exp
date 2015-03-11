using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExperimentProgram
{
    public partial class ExperimentForm : Form
    {
        MessageForm xForm = new MessageForm();

        int PIC_WIDTH = 900; // to be modified hama
        int PIC_HEIGHT = 768; // to be modified hama

        public ExperimentForm()
        {
            InitializeComponent();
            int monitorIndex = 1;
            this.Left = Screen.AllScreens[monitorIndex].Bounds.Left;
            this.Top = Screen.AllScreens[monitorIndex].Bounds.Top;
            int right = Screen.AllScreens[monitorIndex].Bounds.Right - Screen.AllScreens[0].Bounds.Right;
            int bottom = Screen.AllScreens[monitorIndex].Bounds.Bottom;

            int picWidth = PIC_WIDTH;
            int picHeight = PIC_HEIGHT;
            pictureBox1.Width = picWidth;
            pictureBox1.Height = picHeight;
            pictureBox1.Location = new Point(right / 2 - picWidth/2, bottom / 2-picHeight/2);
        }
        public void showBackground()
        {
            pictureBox1.Visible = false;

            Graphics g = this.CreateGraphics();
            g.DrawLine(new Pen(Color.Black, 4), this.Width / 2 - 10, this.Height / 2, this.Width / 2 + 10, this.Height / 2); // hama
            g.DrawLine(new Pen(Color.Black, 4), this.Width / 2, this.Height / 2 - 10, this.Width / 2, this.Height / 2 + 10); // hama
            //this.Refresh();
        }

        // not used
        public void showInfo(string info)
        {
            xForm.setCueState(info, 0);
            xForm.Left = Screen.AllScreens[1].Bounds.Left;
            xForm.Top = Screen.AllScreens[1].Bounds.Top;
            xForm.ShowDialog();
        }

        // not used
        private void drawCross()
        {
            //this.Invalidate();
            //this.Update();
            Graphics g = this.CreateGraphics();
            g.DrawLine(new Pen(Color.Black, 2), this.Width / 2 - 15, this.Height / 2, this.Width / 2 + 15, this.Height / 2);
            g.DrawLine(new Pen(Color.Black, 2), this.Width / 2, this.Height / 2 - 15, this.Width / 2, this.Height / 2 + 15);

        }
        public void showStimuli()
        {
            pictureBox1.Visible = true; 
        }

        public void nowRefreshStimuli()
        {
            pictureBox1.Refresh();
        }

        public void setStimuli(Image img)
        {
            pictureBox1.Image = img;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
