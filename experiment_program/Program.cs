using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ExperimentProgram
{
    static class Program
    {
        // configuration
        static string configFilename = "config.txt";
        static string STIMULI = "stimuli dir";
        static string MATLABSCRIPT = "matlab script dir";
        static string OUTPUT = "output dir";

        // dirs
        static string stimuli_d;
        static string matlab_d;
        static string track_data_d;

        /// <summary>
        /// Main entry point
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (readParams() == -1)
            {
                MessageBox.Show("The config file " + configFilename + " is not correct.\nAbort!");
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainControlForm mainForm = new MainControlForm(stimuli_d, matlab_d, track_data_d);
            Application.Run(mainForm);
        }

        /// <summary>
        /// Check and read params from config file
        /// </summary>
        /// <returns>Return -1 if error occurs; otherwise 0</returns>
        static int readParams()
        {
            StreamReader sr = new StreamReader(configFilename);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (line.Contains(STIMULI))
                {
                    stimuli_d = line.Split('=')[1].Split('"')[1];
                }
                else if (line.Contains(MATLABSCRIPT))
                {
                    matlab_d = line.Split('=')[1].Split('"')[1];
                }
                else if (line.Contains(OUTPUT))
                {
                    track_data_d = line.Split('=')[1].Split('"')[1];
                }
                else if (line.StartsWith("#") || line.Replace(" ", "") == "")
                {
                    continue;
                }
                else
                {
                    return -1;
                }
            }
            return 0;
        }
    }
}
