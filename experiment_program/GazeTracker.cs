using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using TETCSharpClient;
using TETCSharpClient.Data;
using System.Threading;

namespace ExperimentProgram
{
    public class GazeTracker : IGazeListener
    {


        List<EyePoint> listData = new List<EyePoint>();
        double millsec;
        string savefilename;
        string savedir;
        int index;
        PictureBox pbox = null;

        public GazeTracker()
        {
            GazeManager.Instance.Activate(GazeManager.ApiVersion.VERSION_1_0, GazeManager.ClientMode.Push);
            //savefilename = filename;
            // Connect client
            //GazeManager.Instance.Activate(GazeManager.ApiVersion.VERSION_1_0, GazeManager.ClientMode.Push);

            // Register this class for events
            //GazeManager.Instance.AddGazeListener(this);

            //Thread.Sleep(ms); // simulate app lifespan (e.g. OnClose/Exit event)

            // Disconnect client
            //GazeManager.Instance.Deactivate();
            //saveData(filename);
        }

        ~GazeTracker()
        {
            GazeManager.Instance.Deactivate();
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
        public void startTracking()
        {
            listData.Clear(); //
            // Connect client
            //GazeManager.Instance.Activate(GazeManager.ApiVersion.VERSION_1_0, GazeManager.ClientMode.Push);

            // Register this class for events
            GazeManager.Instance.AddGazeListener(this);
        }

        public void stopTracking()
        {
            // Disconnect client
            GazeManager.Instance.RemoveGazeListener(this);
            saveData(savedir+@"\"+savefilename.Split('.')[0] + ".txt"); // '.' maybe trivial
        }

        public void OnGazeUpdate(GazeData gazeData)
        {
            double gX = gazeData.SmoothedCoordinates.X;
            double gY = gazeData.SmoothedCoordinates.Y;
            bool isfixed = gazeData.IsFixated;
            Console.WriteLine(gX + ", " + gY + ", " + (isfixed ? "true" : "false"));
            listData.Add(new EyePoint((int)gX, (int)gY, isfixed));
            if (pbox != null && isfixed)
            {
                Graphics g = pbox.CreateGraphics();
                int panelX = (int)(gX - (1280 - 960) / 2) / 2;
                int panelY = (int)(gY - (1024 - 768) / 2) / 2;
                g.FillEllipse(new SolidBrush(Color.Blue), panelX - 5, panelY - 5, 10, 10);
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
            sw.WriteLine("Stimuli file: "+ savefilename);
            sw.WriteLine("Response time: "+ millsec);

            foreach (EyePoint p in listData)
            {
                sw.WriteLine(p.X + " " + p.Y + " " + (p.isfixed?"true":"false"));
            }
            sw.Close();
            //MessageBox.Show("Saved!");
        }

    }

    
}
