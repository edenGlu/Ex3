using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Services;
using System.Web.Services;
using System.Xml;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class FirstController : Controller
    {
        

        // GET: First
        public ActionResult Index()
        {
            return View();
        }

        // GET: Display
        [HttpGet]
        public ActionResult ShowLocation(string ip, int port)
        {
            Client client = new Client();
            client.Connect(port, ip);
            ViewBag.Lon = client.Send("get /position/longitude-deg \r\n");
            ViewBag.Lat = client.Send("get /position/latitude-deg \r\n");
            client.CLoseClient();
            return View();
        }

        [HttpGet]
        public ActionResult ShowRoute(string ip, int port, int rate)
        {
            saveInfo(ip, port, rate);
            return View();
        }


        [HttpPost]
        public string GetValues()
        {
            double lon = InfoModel.Instance.Client.Send("get /position/longitude-deg \r\n");
            double lat = InfoModel.Instance.Client.Send("get /position/latitude-deg \r\n");
            return ToXml(lon, lat);
        }

        private string ToXml(double lon, double lat)
        {
            //Initiate XML stuff
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("Value");

            writer.WriteElementString("Lon", lon.ToString());
            writer.WriteElementString("Lat", lat.ToString());

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }

        [HttpGet]
        public ActionResult Save(string ip, int port, int rate, int time, string file)
        {
            saveInfo(ip, port, rate);
            Session["time"] = time;
            InfoModel.Instance.setFile(file);
            
            return View();
        }

        public void saveInfo(string ip, int port, int rate)
        {
            InfoModel.Instance.newClient();
            InfoModel.Instance.ip = ip;
            InfoModel.Instance.port = port.ToString();
            InfoModel.Instance.time = rate;

            InfoModel.Instance.Client.Connect(port, ip);

            Session["rate"] = rate;
            ViewBag.Lon = InfoModel.Instance.Client.Send("get /position/longitude-deg \r\n");
            ViewBag.Lat = InfoModel.Instance.Client.Send("get /position/latitude-deg \r\n");
        }


        [HttpPost]
        public string GetValuesAndSave()
        {
            double lon = InfoModel.Instance.Client.Send("get /position/longitude-deg \r\n");
            double lat = InfoModel.Instance.Client.Send("get /position/latitude-deg \r\n");
            double throttle = InfoModel.Instance.Client.Send("get /controls/engines/current-engine/throttle \r\n");
            double rudder = InfoModel.Instance.Client.Send("get /controls/flight/rudder \r\n");
            InfoModel.Instance.SaveData(lon, lat, throttle, rudder);
            return ToXml(lon, lat);
        }

        [HttpGet]
        public ActionResult LoadFile(string file, int rate)
        {
            InfoModel.Instance.newClient();
            InfoModel.Instance.setLoadFile(file);
            InfoModel.Instance.LoadData();
            Session["rate"] = rate;
            return View();
        }

        [HttpPost]
        public string GetLoadValues()
        {
            double lon = InfoModel.Instance.getNextValue();
            if(lon == 404)
            {
                return ToXml(404, 404);
            }
            double lat = InfoModel.Instance.getNextValue();
            InfoModel.Instance.getNextValue();
            InfoModel.Instance.getNextValue();
            return ToXml(lon, lat);
        }

    }
}
