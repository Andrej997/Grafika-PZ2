using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using PZ2.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using Brushes = System.Drawing.Brushes;
using Pen = System.Drawing.Pen;
using Point = PZ2.Model.Point;
using Size = System.Drawing.Size;

namespace PZ2.Controller
{
    public class XMLLoader
    {
        public static double noviX, noviY;
        /// <summary>
        /// Automaticly load Geographic.xml
        /// </summary>
        /// 
        public static void LoadXml()
        {
            #region Path to .xml
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("../../GeographicData/Geographic.xml");
            XmlNodeList nodeList;
            #endregion

            #region Substations
            List<SubstationEntity> substationEntities = new List<SubstationEntity>(); // 67
            SubstationEntity sub = new SubstationEntity();

            nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Substations/SubstationEntity");
            foreach (XmlNode node in nodeList)
            {
                sub.Id = long.Parse(node.SelectSingleNode("Id").InnerText);
                sub.Name = node.SelectSingleNode("Name").InnerText;
                sub.X = double.Parse(node.SelectSingleNode("X").InnerText);
                sub.Y = double.Parse(node.SelectSingleNode("Y").InnerText);

                ToLatLon(sub.X, sub.Y, 34, out noviX, out noviY);

                substationEntities.Add(sub);
            }
            #endregion 

            #region Nodes
            List<NodeEntity> nodeEntities = new List<NodeEntity>(); // 2043
            NodeEntity n = new NodeEntity();

            nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Nodes/NodeEntity");
            foreach (XmlNode node in nodeList)
            {
                n.Id = long.Parse(node.SelectSingleNode("Id").InnerText);
                n.Name = node.SelectSingleNode("Name").InnerText;
                n.X = double.Parse(node.SelectSingleNode("X").InnerText);
                n.Y = double.Parse(node.SelectSingleNode("Y").InnerText);

                ToLatLon(n.X, n.Y, 34, out noviX, out noviY);

                nodeEntities.Add(n);
            }
            #endregion

            #region Switches
            List<SwitchEntity> switchEntities = new List<SwitchEntity>(); // 2282
            SwitchEntity s = new SwitchEntity();

            nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Switches/SwitchEntity");
            foreach (XmlNode node in nodeList)
            {
                s.Id = long.Parse(node.SelectSingleNode("Id").InnerText);
                s.Name = node.SelectSingleNode("Name").InnerText;
                s.X = double.Parse(node.SelectSingleNode("X").InnerText);
                s.Y = double.Parse(node.SelectSingleNode("Y").InnerText);

                ToLatLon(s.X, s.Y, 34, out noviX, out noviY);

                switchEntities.Add(s);
            }
            #endregion

            #region Lines
            List<LineEntity> lineEntities = new List<LineEntity>(); // 2336
            LineEntity l = new LineEntity();
            List<Point> pointsFromLines = new List<Point>(); // 8747

            nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Lines/LineEntity");
            foreach (XmlNode node in nodeList)
            {
                l.Id = long.Parse(node.SelectSingleNode("Id").InnerText);
                l.Name = node.SelectSingleNode("Name").InnerText;
                if (node.SelectSingleNode("IsUnderground").InnerText.Equals("true"))
                {
                    l.IsUnderground = true;
                }
                else
                {
                    l.IsUnderground = false;
                }
                l.R = float.Parse(node.SelectSingleNode("R").InnerText);
                l.ConductorMaterial = node.SelectSingleNode("ConductorMaterial").InnerText;
                l.LineType = node.SelectSingleNode("LineType").InnerText;
                l.ThermalConstantHeat = long.Parse(node.SelectSingleNode("ThermalConstantHeat").InnerText);
                l.FirstEnd = long.Parse(node.SelectSingleNode("FirstEnd").InnerText);
                l.SecondEnd = long.Parse(node.SelectSingleNode("SecondEnd").InnerText);

                lineEntities.Add(l);

                foreach (XmlNode pointNode in node.ChildNodes[9].ChildNodes) // 9 posto je Vertices 9. node u jednom line objektu
                {
                    Point p = new Point();

                    p.X = double.Parse(pointNode.SelectSingleNode("X").InnerText);
                    p.Y = double.Parse(pointNode.SelectSingleNode("Y").InnerText);

                    ToLatLon(p.X, p.Y, 34, out noviX, out noviY);

                    pointsFromLines.Add(p);
                }
            }
            #endregion
        }

        //From UTM to Latitude and longitude in decimal
        public static void ToLatLon(double utmX, double utmY, int zoneUTM, out double latitude, out double longitude)
        {
            bool isNorthHemisphere = true;

            var diflat = -0.00066286966871111111111111111111111111;
            var diflon = -0.0003868060578;

            var zone = zoneUTM;
            var c_sa = 6378137.000000;
            var c_sb = 6356752.314245;
            var e2 = Math.Pow((Math.Pow(c_sa, 2) - Math.Pow(c_sb, 2)), 0.5) / c_sb;
            var e2cuadrada = Math.Pow(e2, 2);
            var c = Math.Pow(c_sa, 2) / c_sb;
            var x = utmX - 500000;
            var y = isNorthHemisphere ? utmY : utmY - 10000000;

            var s = ((zone * 6.0) - 183.0);
            var lat = y / (c_sa * 0.9996);
            var v = (c / Math.Pow(1 + (e2cuadrada * Math.Pow(Math.Cos(lat), 2)), 0.5)) * 0.9996;
            var a = x / v;
            var a1 = Math.Sin(2 * lat);
            var a2 = a1 * Math.Pow((Math.Cos(lat)), 2);
            var j2 = lat + (a1 / 2.0);
            var j4 = ((3 * j2) + a2) / 4.0;
            var j6 = ((5 * j4) + Math.Pow(a2 * (Math.Cos(lat)), 2)) / 3.0;
            var alfa = (3.0 / 4.0) * e2cuadrada;
            var beta = (5.0 / 3.0) * Math.Pow(alfa, 2);
            var gama = (35.0 / 27.0) * Math.Pow(alfa, 3);
            var bm = 0.9996 * c * (lat - alfa * j2 + beta * j4 - gama * j6);
            var b = (y - bm) / v;
            var epsi = ((e2cuadrada * Math.Pow(a, 2)) / 2.0) * Math.Pow((Math.Cos(lat)), 2);
            var eps = a * (1 - (epsi / 3.0));
            var nab = (b * (1 - epsi)) + lat;
            var senoheps = (Math.Exp(eps) - Math.Exp(-eps)) / 2.0;
            var delt = Math.Atan(senoheps / (Math.Cos(nab)));
            var tao = Math.Atan(Math.Cos(delt) * Math.Tan(nab));

            longitude = ((delt * (180.0 / Math.PI)) + s) + diflon;
            latitude = ((lat + (1 + e2cuadrada * Math.Pow(Math.Cos(lat), 2) - (3.0 / 2.0) * e2cuadrada * Math.Sin(lat) * Math.Cos(lat) * (tao - lat)) * (tao - lat)) * (180.0 / Math.PI)) + diflat;
        }
    }
}
