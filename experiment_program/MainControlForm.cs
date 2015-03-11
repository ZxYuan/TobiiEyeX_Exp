using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using System.Threading;

namespace ExperimentProgram
{
    public partial class MainControlForm : Form
    {
        // Forms
        ExperimentForm expForm = new ExperimentForm();
        MessageForm msgForm = new MessageForm();

        // Tobii gaze tracker
        TobiiGazeTracker gt = new TobiiGazeTracker();

        // strings
        string stimuli_dir;
        List<string> image_filenames = new List<string>();
        string matlab_dir;
        string track_data_dir;

        Random randomGeneratoor = new Random();

        public MainControlForm(string stimuli_d, string matlab_d, string track_data_d)
        {
            InitializeComponent();

            stimuli_dir = System.Environment.CurrentDirectory + @"\" + stimuli_d;
            matlab_dir = System.Environment.CurrentDirectory + @"\" + matlab_d;
            track_data_dir = System.Environment.CurrentDirectory + @"\" + track_data_d;

            // get image file names
            DirectoryInfo dir = new DirectoryInfo(stimuli_dir);
            foreach (FileInfo nextFile in dir.GetFiles())
            {
                if (nextFile.Name.EndsWith(".png") || nextFile.Name.EndsWith(".jpg") || nextFile.Name.EndsWith(".jpeg"))
                {
                    this.image_filenames.Add(nextFile.Name);
                }
            }
            //MessageBox.Show(image_filenames.Count + " images detected in " + stimuli_dir);

            gt.setPicBox(pictureBox1);
            KeyPreview = true;
        }


        /// <summary>
        /// Called when the control form is loaded
        /// Show the experiment form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainControlForm_Load(object sender, EventArgs e)
        {
            expForm.Show(); // Show full-screen experiment form
        }

        // Experiment start
        private void button1_Click(object sender, EventArgs e)
        {
            oneBlock_shioiri();
        }

        void oneBlock_shioiri()
        {
            int TRIAL_NUM = 10; // change here hama
            DateTime timeStart, timeEnd;

            for (int iTrial = 1; iTrial <= TRIAL_NUM; iTrial++ )
            {
                expForm.showBackground();

                msgForm.setCueState("Press down key to start", 0);
                msgForm.ShowDialog();

                pictureBox1.Refresh();

                startTracking();
                gt.setSaveFileName(track_data_dir, iTrial.ToString() + ".stimuli", iTrial);
                timeStart = DateTime.Now;

                msgForm.setCueState("Press left/right key to stop", 1);
                msgForm.startClock();
                msgForm.ShowDialog();

                timeEnd = DateTime.Now;
                TimeSpan ts = timeEnd - timeStart;
                gt.setResponsetime(ts.TotalMilliseconds);
                gt.setResult(msgForm.getResult());
                stopTracking();
                //makeHeatmap(img_name); // may not be used during experiment
            }
            expForm.showBackground();
        }

        // Show images 
        void oneBlock_test()
        {
            int i = 1; // index
            DateTime timeStart, timeEnd;

            while (image_filenames.Count > 0)
            {
                expForm.showBackground();

                msgForm.setCueState("Press down key to start", 0);
                msgForm.ShowDialog();

                startTracking();
                //int idx = randomGeneratoor.Next(image_filenames.Count);
                string img_name = image_filenames[0];
                Image img = Image.FromFile(stimuli_dir + @"\" + img_name);
                gt.setSaveFileName(track_data_dir, img_name, i++);
                expForm.setStimuli(img);
                expForm.showStimuli();
                
                expForm.nowRefreshStimuli();
                timeStart = DateTime.Now;
                image_filenames.RemoveAt(0);
                Console.WriteLine(image_filenames.Count);

                pictureBox1.Image = img;
                pictureBox1.Refresh();

                msgForm.setCueState("Press left/right key to stop", 1);
                msgForm.startClock();
                msgForm.ShowDialog();

                timeEnd = DateTime.Now;
                TimeSpan ts = timeEnd - timeStart;
                gt.setResponsetime(ts.TotalMilliseconds);
                gt.setResult(msgForm.getResult());
                stopTracking();
                makeHeatmap(img_name); // may not be used during experiment
            }
            expForm.showBackground();
        }

        // Randomly show images
        void oneBlock_test_random()
        {
            int i = 1; // index
            DateTime timeStart, timeEnd;
            
            while (image_filenames.Count>0)
            {
                expForm.showBackground();

                msgForm.setCueState("Press down key to start", 0);
                msgForm.ShowDialog();
                
                startTracking();
                int idx = randomGeneratoor.Next(image_filenames.Count);
                string img_name = image_filenames[idx];
                Image img = Image.FromFile(stimuli_dir + @"\" + img_name);
                gt.setSaveFileName(track_data_dir, img_name, i++);
                expForm.setStimuli(img);
                expForm.showStimuli();
                expForm.nowRefreshStimuli();
                timeStart = DateTime.Now;
                image_filenames.RemoveAt(idx);
                Console.WriteLine(image_filenames.Count);

                pictureBox1.Image = img;
                pictureBox1.Refresh();

                msgForm.setCueState("Press left/right key to stop", 1);
                msgForm.startClock();
                msgForm.ShowDialog();
                
                timeEnd = DateTime.Now;
                TimeSpan ts = timeEnd - timeStart;
                gt.setResponsetime(ts.TotalMilliseconds);
                gt.setResult(msgForm.getResult());
                stopTracking();
                makeHeatmap(img_name); // may not be used during experiment
            }
            expForm.showBackground();
        }

        void startTracking()
        {
            MethodInvoker mi = new MethodInvoker(callStartTracking);
            mi.BeginInvoke(null, null);
        }
        public void callStartTracking()
        {
            Console.WriteLine("Tracking is to begin.");
            gt.startTracking();
            Console.WriteLine("Tracking begins done.");
        }
        void stopTracking()
        {
            MethodInvoker mi = new MethodInvoker(callStopTracking);
            mi.BeginInvoke(null, null);
        }
        public void callStopTracking()
        {
            Console.WriteLine("Tracking is to stop.");
            gt.stopTracking();
            Console.WriteLine("Tracking stopped.");
        }

        /// <summary>
        /// Make heat maps based on tracked eye data, by calling matlab scripts
        /// </summary>
        /// <param name="img_name"></param>
        void makeHeatmap(string img_name)
        {
            MLApp.MLApp matlab = new MLApp.MLApp();
            matlab.Execute(@"cd " + matlab_dir);

            // Define the output 
            object result = null;
            string stimuli_path = stimuli_dir + @"\" + img_name.Split('.')[0];
            string trackdata_path = track_data_dir + @"\" + img_name.Split('.')[0] + ".txt";
            // Call the MATLAB function myfunc
            matlab.Feval("genHeatmap", 1, out result, stimuli_path, trackdata_path);

            object[] res = result as object[];

            Console.WriteLine(res[0]);

            pictureBox2.Image = Image.FromFile(track_data_dir + @"\" + img_name.Split('.')[0] + @"_mapOnImg.jpg"); // to modify
        }
    }


}
