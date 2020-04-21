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

using PZ2.DataContainers;
using System.Windows.Shapes;

namespace PZ2.Controller
{
    /// <summary>
    /// nasledio sam je, jer sam stavio da 
    /// liste budu protekted kako ne bi moglo
    /// da se pristupi iz neke druge klase.
    /// </summary>
    public class XMLLoader : Containers 
    {
        private static XmlDocument xmlDoc;

        /// <summary>
        /// tacka najbliza koordinatnom pocetku
        /// </summary>
        private static double closestX;
        private static double closestY;

        /// <summary>
        /// tacka najdalja koordinatnom pocetku
        /// </summary>
        private static double farthestX;
        private static double farthestY;

        /// <summary>
        /// Automaticly load Geographic.xml
        /// </summary>
        public static void LoadXml()
        {
            #region Path to .xml
            xmlDoc = new XmlDocument();
            xmlDoc.Load("../../GeographicData/Geographic.xml");
            XmlNodeList nodeList;
            #endregion

            #region Substations
            nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Substations/SubstationEntity");
            LoadSubstationEntities(nodeList);
            #endregion 

            #region Nodes
            nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Nodes/NodeEntity");
            LoadNodeEntities(nodeList);
            #endregion

            #region Switches
            nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Switches/SwitchEntity");
            LoadSwitcheEntities(nodeList);
            #endregion

            #region Lines
            nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Lines/LineEntity");
            LoadLineEntities(nodeList);
            #endregion
        }

        #region Entity Loaders
        private static void LoadSubstationEntities(XmlNodeList nodeList)
        {
            foreach (XmlNode node in nodeList)
            {
                SubstationEntity sub = new SubstationEntity();
                sub.Id = long.Parse(node.SelectSingleNode("Id").InnerText);
                sub.Name = node.SelectSingleNode("Name").InnerText;
                sub.X = double.Parse(node.SelectSingleNode("X").InnerText);
                sub.Y = double.Parse(node.SelectSingleNode("Y").InnerText);

                substationEntities.Add(sub);
            }
        }
        private static void LoadNodeEntities(XmlNodeList nodeList)
        {
            foreach (XmlNode node in nodeList)
            {
                NodeEntity n = new NodeEntity();
                n.Id = long.Parse(node.SelectSingleNode("Id").InnerText);
                n.Name = node.SelectSingleNode("Name").InnerText;
                n.X = double.Parse(node.SelectSingleNode("X").InnerText);
                n.Y = double.Parse(node.SelectSingleNode("Y").InnerText);

                nodeEntities.Add(n);
            }
        }
        private static void LoadSwitcheEntities(XmlNodeList nodeList)
        {
            foreach (XmlNode node in nodeList)
            {
                SwitchEntity s = new SwitchEntity();
                s.Id = long.Parse(node.SelectSingleNode("Id").InnerText);
                s.Name = node.SelectSingleNode("Name").InnerText;
                s.Status = node.SelectSingleNode("Status").InnerText;
                s.X = double.Parse(node.SelectSingleNode("X").InnerText);
                s.Y = double.Parse(node.SelectSingleNode("Y").InnerText);

                switchEntities.Add(s);
            }
        }
        private static void LoadLineEntities(XmlNodeList nodeList)
        {
            foreach (XmlNode node in nodeList)
            {
                LineEntity l = new LineEntity();
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

                    pointsFromLines.Add(p);
                }
            }
        }
        #endregion

        #region Get closest and farthest point from (0,0)
        private static void GetClosestPoint(out double closestX, out double closestY)
        {
            closestX = 1000;
            closestY = 1000;
            double latitude;
            double longitude;
            foreach (var item in substationEntities)
            {
                ToLatLon(item.X, item.Y, 34, out latitude, out longitude);
                if (latitude < closestX)
                    closestX = latitude;
                if (longitude < closestY)
                    closestY = longitude;
            }
            foreach (var item in nodeEntities)
            {
                ToLatLon(item.X, item.Y, 34, out latitude, out longitude);
                if (latitude < closestX)
                    closestX = latitude;
                if (longitude < closestY)
                    closestY = longitude;
            }
            foreach (var item in switchEntities)
            {
                ToLatLon(item.X, item.Y, 34, out latitude, out longitude);
                if (latitude < closestX)
                    closestX = latitude;
                if (longitude < closestY)
                    closestY = longitude;
            }
        }
        private static void GetFarthestPoint(out double farthestX, out double farthestY)
        {
            farthestX = 0;
            farthestY = 0;
            double latitude;
            double longitude;
            foreach (var item in substationEntities)
            {
                ToLatLon(item.X, item.Y, 34, out latitude, out longitude);
                if (latitude > farthestX)
                    farthestX = latitude;
                if (longitude > farthestY)
                    farthestY = longitude;
            }
            foreach (var item in nodeEntities)
            {
                ToLatLon(item.X, item.Y, 34, out latitude, out longitude);
                if (latitude > farthestX)
                    farthestX = latitude;
                if (longitude > farthestY)
                    farthestY = longitude;
            }
            foreach (var item in switchEntities)
            {
                ToLatLon(item.X, item.Y, 34, out latitude, out longitude);
                if (latitude > farthestX)
                    farthestX = latitude;
                if (longitude > farthestY)
                    farthestY = longitude;
            }
        }
        #endregion

        #region Insert in entityMatrix
        private static void InsertInEM(double x, double y, AllPurpuseEntity allPurpuseEntity)
        {
            double latitude = 0;
            double longitude = 0;
            ToLatLon(x, y, 34, out latitude, out longitude);

            double XSpot = ((latitude - closestX) / (farthestX - closestX)) * (MainWindow.CanvasData().Item2 - 1);
            double YSpot = ((longitude - closestY) / (farthestY - closestY)) * (MainWindow.CanvasData().Item1 - 1);

            XSpot = XSpot - XSpot % MainWindow.CanvasData().Item3;
            YSpot = YSpot - YSpot % MainWindow.CanvasData().Item3;

            if (entityMatrix[(int)XSpot, (int)YSpot] == null)
            {
                entityMatrix[(int)XSpot, (int)YSpot] = allPurpuseEntity;
            }
            else
            {
                bool find = false;
                bool end = false;
                int levelElements = 3;
                int level = 1;

                while (find == false && end == false)
                {

                    int searchX = 0;
                    if (XSpot != 0)
                    {
                        searchX = (int)XSpot - level;
                    }
                    int searchY = 0;
                    if (YSpot != 0)
                    {
                        searchY = (int)YSpot - level;
                    }

                    for (int i = 0; i < levelElements; i++)
                    {
                        if (entityMatrix[searchX + i, searchY] == null)
                        {
                            find = true;
                            entityMatrix[searchX + i, searchY] = allPurpuseEntity;
                            break;
                        }
                    }

                    if (find)
                        break;

                    for (int i = 0; i < levelElements; i++)
                    {
                        if (entityMatrix[searchX, searchY + i] == null)
                        {
                            find = true;
                            entityMatrix[searchX, searchY + i] = allPurpuseEntity;
                            break;
                        }
                    }

                    if (find)
                        break;

                    for (int i = 0; i < levelElements; i++)
                    {
                        if (entityMatrix[searchX + i, searchY + levelElements - 1] == null)
                        {
                            find = true;
                            entityMatrix[searchX + i, searchY + levelElements - 1] = allPurpuseEntity;
                            break;
                        }
                    }

                    if (find)
                        break;

                    for (int i = 0; i < levelElements; i++)
                    {
                        if (entityMatrix[searchX + levelElements - 1, searchY + i] == null)
                        {
                            find = true;
                            entityMatrix[searchX + levelElements - 1, searchY + i] = allPurpuseEntity;
                            break;
                        }
                    }

                    if (find)
                        break;


                    levelElements += 2;
                    level++;
                }
            }
        }
        private static long GetID(AllPurpuseEntity allPurpuseEntity)
        {
            if (allPurpuseEntity.Entity != null)
                return allPurpuseEntity.Entity.Id;
            else
                return allPurpuseEntity.LineEntity.Id;
        }
        private static void InsertLinesInEM()
        {
            foreach (var line in lineEntities)
            {
                long startID = line.FirstEnd;
                long finishID = line.SecondEnd;
                int startRow = 0; // X je red
                int startCol = 0; // Y je kolona
                int finishRow = 0;
                int finishCol = 0;

                for (int i = 0; i < MainWindow.CanvasData().Item1; i++)
                {
                    for (int j = 0; j < MainWindow.CanvasData().Item2; j++)
                    {
                        if (EntityMatrix[i, j] != null)
                        {
                            if (GetID(EntityMatrix[i, j]) == startID)
                            {
                                startRow = i;
                                startCol = j;
                            }
                        }
                        if (EntityMatrix[i, j] != null)
                        {
                            if (GetID(EntityMatrix[i, j]) == finishID)
                            {
                                finishRow = i;
                                finishCol = j;
                            }
                        }
                    }
                }

                if (startRow <= finishRow && startCol >= finishCol) // red | a kolona --
                {
                    for (int i = startRow; i <= finishRow; i++)
                    {
                        if (EntityMatrix[i, startCol] == null)
                        {
                            AllPurpuseEntity allPurpuseEntity = new AllPurpuseEntity(line, EntityType.Line, System.Windows.Media.Brushes.Black);
                            EntityMatrix[i, startCol] = allPurpuseEntity;
                        }
                    }

                    for (int i = finishCol; i <= startCol; i++)
                    {
                        if (EntityMatrix[finishRow, i] == null)
                        {
                            AllPurpuseEntity allPurpuseEntity = new AllPurpuseEntity(line, EntityType.Line, System.Windows.Media.Brushes.Black);
                            EntityMatrix[finishRow, i] = allPurpuseEntity;
                        }
                    }
                }

                if (startRow <= finishRow && startCol < finishCol) // red | a kolona --
                {
                    for (int i = startRow; i <= finishRow; i++)
                    {
                        if (EntityMatrix[i, finishCol] == null)
                        {
                            AllPurpuseEntity allPurpuseEntity = new AllPurpuseEntity(line, EntityType.Line, System.Windows.Media.Brushes.Black);
                            EntityMatrix[i, finishCol] = allPurpuseEntity;
                        }
                    }

                    for (int i = startCol; i <= finishCol; i++)
                    {
                        if (EntityMatrix[startRow, i] == null)
                        {
                            AllPurpuseEntity allPurpuseEntity = new AllPurpuseEntity(line, EntityType.Line, System.Windows.Media.Brushes.Black);
                            EntityMatrix[startRow, i] = allPurpuseEntity;
                        }
                    }
                }

                if (startRow > finishRow && startCol < finishCol) // red | a kolona --
                {
                    for (int i = finishRow; i <= startRow; i++)
                    {
                        if (EntityMatrix[i, startCol] == null)
                        {
                            AllPurpuseEntity allPurpuseEntity = new AllPurpuseEntity(line, EntityType.Line, System.Windows.Media.Brushes.Black);
                            EntityMatrix[i, startCol] = allPurpuseEntity;
                        }
                    }

                    for (int i = startCol; i <= finishCol; i++)
                    {
                        if (EntityMatrix[finishRow, i] == null)
                        {
                            AllPurpuseEntity allPurpuseEntity = new AllPurpuseEntity(line, EntityType.Line, System.Windows.Media.Brushes.Black);
                            EntityMatrix[finishRow, i] = allPurpuseEntity;
                        }
                    }
                }

                if (startRow > finishRow && startCol >= finishCol) // red | a kolona --
                {
                    for (int i = finishRow; i <= startRow; i++)
                    {
                        if (EntityMatrix[i, startCol] == null)
                        {
                            AllPurpuseEntity allPurpuseEntity = new AllPurpuseEntity(line, EntityType.Line, System.Windows.Media.Brushes.Black);
                            EntityMatrix[i, startCol] = allPurpuseEntity;
                        }
                    }

                    for (int i = finishCol; i <= startCol; i++)
                    {
                        if (EntityMatrix[finishRow, i] == null)
                        {
                            AllPurpuseEntity allPurpuseEntity = new AllPurpuseEntity(line, EntityType.Line, System.Windows.Media.Brushes.Black);
                            EntityMatrix[finishRow, i] = allPurpuseEntity;
                        }
                    }
                }
            }
        }
        #endregion

        #region Set all entities in one big canvas matrix
        public static void SetAll()
        {
            GetClosestPoint(out closestX, out closestY);
            GetFarthestPoint(out farthestX, out farthestY);
            foreach (var item in substationEntities)
            {
                InsertInEM(
                    item.X,
                    item.Y,
                    new AllPurpuseEntity(
                        item,
                        EntityType.Substation,
                        System.Windows.Media.Brushes.Blue
                        )
                    );
            }
            //foreach (var item in nodeEntities)
            //{
            //    InsertInEM(
            //        item.X,
            //        item.Y,
            //        new AllPurpuseEntity(
            //            item,
            //            EntityType.Node,
            //            System.Windows.Media.Brushes.Red
            //            )
            //        );
            //}
            //foreach (var item in switchEntities)
            //{
            //    InsertInEM(
            //        item.X,
            //        item.Y,
            //        new AllPurpuseEntity(
            //            item,
            //            EntityType.Switch,
            //            System.Windows.Media.Brushes.Green
            //            )
            //        );
            //}
            InsertLinesInEM();
        }
        #endregion

        #region Converter
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
        #endregion
    }
}
