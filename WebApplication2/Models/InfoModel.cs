using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Ex3.Models
{
    // a singeltone class
    public class InfoModel 
    {
        private const double ERROR = 404; // sign to error msg

        private static InfoModel s_instace = null;

        public static InfoModel Instance
        {
            get {
                if (s_instace == null)
                {
                    s_instace = new InfoModel();
                }
                return s_instace;
             }
        }

        public Client Client { get; private set; }
        public string ip { get; set; } // to connect the server
        public string port { get; set; } 
        public int time { get; set; } // the rate 
        private const string SCENARIO_FILE = "~/App_Data/{0}.txt"; // how the file save
        private string savePath; // the path of the save data file
        private string loadPath; // the path of the load data file
        private string[] dataLine; // the data that load
        private int caunter = 0; // a point on the current data to send in the dataline

       public void setFile(string name)
        {
            // a format to save the file in the app_data folder
            this.savePath = HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, name));
        }

        public void setLoadFile(string name)
        {
            this.loadPath = HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, name));
        }

        public InfoModel()
        {
            Client = new Client();
        }
        // Close the previous client before connecting with a new client
        public void newClient()
        {
            Client.CLoseClient();
        }

        

        public void SaveData(double lon, double lat, double throttle, double rudder)
        {
            if (!File.Exists(this.savePath))
            {
                using (StreamWriter file = new StreamWriter(savePath, true))
                {
                    file.WriteLine(lon.ToString());
                    file.WriteLine(lat.ToString());
                    file.WriteLine(throttle.ToString());
                    file.WriteLine(rudder.ToString());
                    file.Close();
                }
            }
            else
            {
                // add line in the end of the file.
                using (StreamWriter tw = File.AppendText(savePath))
                {
                    tw.WriteLine(lon.ToString());
                    tw.WriteLine(lat.ToString());
                    tw.WriteLine(throttle.ToString());
                    tw.WriteLine(rudder.ToString());
                    tw.Close();
                }
            }
        }
        
        // get all the data from the file
        public void LoadData()
        {
            this.dataLine = System.IO.File.ReadAllLines(loadPath);
        }

        // return the next line
        public double getNextValue()
        {
            // If you do not exceed the number of lines
            if (caunter < dataLine.Length) 
            {
                string line = this.dataLine[caunter];
                caunter++;
                double val = Convert.ToDouble(line);
                return val;
            }
            return ERROR;
        }
    }
}
 