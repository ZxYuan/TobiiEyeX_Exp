using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using EyeXFramework;
using Tobii.EyeX.Framework;

namespace ExperimentProgram
{
    class TobiiGazeTracker
    {
        EyeXHost eyeXHost;
        GazePointDataStream lightlyFilteredGazeDataStream;

        List<EyePoint> listData = new List<EyePoint>();
        double millsec;
        string savefilename;
        string savedir;
        int index;
        PictureBox pbox = null;

        int exp_truth = 0; // 1 for left, 2 for right
        int exp_result = 0; // 1 for left, 2 for right

        int SCREEN_WIDTH = 1280; // hama
        int SCREEN_HEIGHT = 1024; // hama
        int PIC_WIDTH = 960; // hama
        int PIC_HEIGHT = 768; // hama

        public TobiiGazeTracker()
        {
            eyeXHost = new EyeXHost();
            //lightlyFilteredGazeDataStream = eyeXHost.CreateGazePointDataStream(GazePointDataMode.LightlyFiltered);
            eyeXHost.Start();
        }

        ~TobiiGazeTracker()
        {
            eyeXHost.Dispose();
        }

        public void setPicBox(PictureBox pb)
        {
            pbox = pb;
        }

        public void setSaveFileName(string dir, string s, int i)
        {
            savedir = dir;
            savefilename = s;
            index = i;
        }

        public void setResponsetime(double ts)
        {
            millsec = ts;
        }

        public void setResult(int result)
        {
            exp_result = result;
        }

        public void startTracking()
        {
            listData.Clear(); //

            lightlyFilteredGazeDataStream = eyeXHost.CreateGazePointDataStream(GazePointDataMode.LightlyFiltered);

            // Register this class for events
            lightlyFilteredGazeDataStream.Next += OnGazeUpdate;
        }

        public void stopTracking()
        {
            // Disconnect client
            lightlyFilteredGazeDataStream.Dispose();
            saveData(savedir+@"\"+savefilename.Split('.')[0] + ".txt"); // '.' maybe trivial
        }

        public void OnGazeUpdate(Object s, GazePointEventArgs e)
        {
            double gX = e.X;
            double gY = e.Y;
            bool isfixed = true;
            //Console.WriteLine(gX + ", " + gY + ", " + (isfixed ? "true" : "false"));
            listData.Add(new EyePoint(e.Timestamp, (int)(gX - SCREEN_WIDTH), (int)gY, isfixed));
            if (pbox != null && isfixed)
            {
                Graphics g = pbox.CreateGraphics();
                int panelX = (int)(gX - SCREEN_WIDTH - (SCREEN_WIDTH - PIC_WIDTH) / 2) / 2; // raw X is 1280+x
                int panelY = (int)(gY - (SCREEN_HEIGHT - PIC_HEIGHT) / 2) / 2;
                g.FillEllipse(new SolidBrush(Color.Blue), panelX - 5, panelY - 5, 10, 10); // hama
            }
            /*
            if (isfixed)
            {
                listData.Add(new EyePoint((int)gX, (int)gY,isfixed));
            }
             * */
        }
        public List<EyePoint> getData()
        {
            return listData;
        }
        public void saveData(string filename)
        {
            StreamWriter sw = new StreamWriter(filename);

            sw.WriteLine("Index number: "+ index);
            sw.WriteLine("Stimuli file: " + savefilename + ", ground truth: " + exp_truth);
            sw.WriteLine("Response time: "+ millsec + ", subject result: " + exp_result);
            sw.WriteLine("Index TimeStamp X(pixel) Y(pixel) tracked");

            int num = 0;
            foreach (EyePoint p in listData)
            {
                num++;
                sw.WriteLine(num+" "+p.timeStamp+" "+ p.X + " " + p.Y + " " + (p.isfixed?"true":"false"));
            }
            
            sw.Close();
            //MessageBox.Show("Saved!");
        }

    }

    
   
}
