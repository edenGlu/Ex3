﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Services;
using System.Web.Services;
using System.Xml;
using Ex3.Models;

namespace Ex3.Controllers
{
    public class FirstController : Controller
    {
        private const string LON = "get /position/longitude-deg \r\n";
        private const string LAT = "get /position/latitude-deg \r\n";
        private const string THROTTLE = "get /controls/engines/current-engine/throttle \r\n";
        private const string RUDDER = "get /controls/flight/rudder \r\n";
        private const double ERROR = 404;

        // GET: First difult 
        public ActionResult Index()
        {
            return View();
        }

        // GET: Display
        [HttpGet]
        public ActionResult ShowLocation(string ip, int port)
        {
            Client client = new Client(); 
            client.Connect(port, ip); // connect to the server of the plan.
            // get the value of the place from the server. 
            ViewBag.Lon = client.Send(LON);
            ViewBag.Lat = client.Send(LAT);
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
            double lon = InfoModel.Instance.Client.Send(LON);
            double lat = InfoModel.Instance.Client.Send(LAT);
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
            // the 2 value to pass 
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
            // Preserving the information in the model
            InfoModel.Instance.newClient();
            InfoModel.Instance.ip = ip;
            InfoModel.Instance.port = port.ToString();
            InfoModel.Instance.time = rate;

            InfoModel.Instance.Client.Connect(port, ip);
            // save the data in the View
            Session["rate"] = rate;
            ViewBag.Lon = InfoModel.Instance.Client.Send(LON);
            ViewBag.Lat = InfoModel.Instance.Client.Send(LAT);
        }


        [HttpPost]
        public string GetValuesAndSave()
        {
            // take param from the plan
            double lon = InfoModel.Instance.Client.Send(LON);
            double lat = InfoModel.Instance.Client.Send(LAT);
            double throttle = InfoModel.Instance.Client.Send(THROTTLE);
            double rudder = InfoModel.Instance.Client.Send(RUDDER);
            // save them in xml to send the plan
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
            if(lon == ERROR) // error ther is no more data in the 
            {
                return ToXml(ERROR, ERROR);
            }
            double lat = InfoModel.Instance.getNextValue();
            InfoModel.Instance.getNextValue();
            InfoModel.Instance.getNextValue();
            return ToXml(lon, lat);
        }

    }
}
