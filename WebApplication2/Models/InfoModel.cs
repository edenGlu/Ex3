using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class InfoModel
    {
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
        public string ip { get; set; }
        public string port { get; set; }
        public int time { get; set; }
        private const string SCENARIO_FILE = "~/App_Data/{0}.txt";
        private string savePath;
        private string loadPath;
        private string[] dataLine;
        private int caunter = 0;

       public void setFile(string name)
        {
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
        

        public void LoadData()
        {
            this.dataLine = System.IO.File.ReadAllLines(loadPath);
        }


        public double getNextValue()
        {
            if (caunter < dataLine.Length)
            {
                string line = this.dataLine[caunter];
                caunter++;
                double val = Convert.ToDouble(line);
                return val;
            }
            return 404;
        }
    }
}
 