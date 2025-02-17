﻿using PZ2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Point = PZ2.Model.Point;

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
                lineEntitiesDict.Add(l.Id, l);

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
        
        #region Set all
        private static AllPurpuseEntity SetCoords(AllPurpuseEntity allPurpuseEntity)
        {
            ToLatLon(allPurpuseEntity.Entity.X, allPurpuseEntity.Entity.Y, 34, out double latitude, out double longitude);

            double XSpot = ((latitude - closestX) / (farthestX - closestX)) * (MainWindow.CanvasData().Item2 - 1);
            double YSpot = ((longitude - closestY) / (farthestY - closestY)) * (MainWindow.CanvasData().Item1 - 1);

            XSpot = XSpot - XSpot % MainWindow.CanvasData().Item3;
            YSpot = YSpot - YSpot % MainWindow.CanvasData().Item3;

            Coord coord = new Coord((int)XSpot, (int)YSpot);

            if(!allCords.Any(i => i.X == coord.X && i.Y == coord.Y))
            {
                allCords.Add(coord);
                
                allPurpuseEntity.X = (int)XSpot;
                allPurpuseEntity.Y = (int)YSpot;
            }
            else
            {
                bool foundedFreeSpot = false;
                bool end = false;
                int levelElements = 3;
                int level = 1;
                while (foundedFreeSpot == false && end == false)
                {
                    int newX = 0;
                    if (XSpot != 0)
                        newX = (int)XSpot - level;

                    int newY = 0;
                    if (YSpot != 0)
                        newY = (int)YSpot - level;

                    for (int i = 0; i < levelElements; i++)
                    {
                        coord.X = newX + i;
                        coord.Y = newY;
                        if (!allCords.Any(t => t.X == coord.X && t.Y == coord.Y))
                        {
                            allCords.Add(coord);

                            foundedFreeSpot = true;
                            allPurpuseEntity.X = newX + i;
                            allPurpuseEntity.Y = newY;
                            break;
                        }
                    }
                    if (foundedFreeSpot) break;

                    for (int i = 0; i < levelElements; i++)
                    {
                        coord.X = newX + i;
                        coord.Y = newY + levelElements - 1;
                        if (!allCords.Any(t => t.X == coord.X && t.Y == coord.Y))
                        {
                            allCords.Add(coord);
                            foundedFreeSpot = true;
                            allPurpuseEntity.X = newX + i;
                            allPurpuseEntity.Y = newY + levelElements - 1;
                            break;
                        }
                    }
                    if (foundedFreeSpot) break;

                    for (int i = 0; i < levelElements; i++)
                    {
                        coord.X = newX + levelElements - 1;
                        coord.Y = newY + i;
                        if (!allCords.Any(t => t.X == coord.X && t.Y == coord.Y))
                        {
                            allCords.Add(coord);
                            foundedFreeSpot = true;
                            allPurpuseEntity.X = newX + levelElements - 1;
                            allPurpuseEntity.Y = newY + i;
                            break;
                        }
                    }
                    if (foundedFreeSpot) break;

                    levelElements += 2;
                    level++;
                }
            }
            return allPurpuseEntity;
        }

        public static void SetAll()
        {
            GetClosestPoint(out closestX, out closestY);
            GetFarthestPoint(out farthestX, out farthestY);
            foreach (var entity in substationEntities)
                allPurpuseEntities.Add(SetCoords(new AllPurpuseEntity(entity.Id, entity, EntityType.Substation, System.Windows.Media.Brushes.Blue)));
            
            foreach (var entity in nodeEntities)
                allPurpuseEntities.Add(SetCoords(new AllPurpuseEntity(entity.Id, entity, EntityType.Node, System.Windows.Media.Brushes.Red)));

            foreach (var entity in switchEntities)
                allPurpuseEntities.Add(SetCoords(new AllPurpuseEntity(entity.Id, entity, EntityType.Switch, System.Windows.Media.Brushes.Green)));

            Graph = new Graph(allPurpuseEntities, lineEntities);

        }
        #endregion

        #region BFS

        /// <summary>
        /// Proverava da li su zauzete koordinate
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private static bool CheckCoords(int x, int y)
        {
            Coord coord = new Coord(x, y);
            if (!allCords.Any(i => i.X == coord.X && i.Y == coord.Y))
            {
                allCords.Add(coord);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Daje listu koordita izmedju 2 dva cvora
        /// </summary>
        /// <param name="startNode">prvi cvor</param>
        /// <param name="endNode">drugi cvor</param>
        /// <returns></returns>
        private static HashSet<AllPurpuseEntity> SetCoordsForLine(AllPurpuseEntity startNode, AllPurpuseEntity endNode)
        {
            HashSet<AllPurpuseEntity> retVal = new HashSet<AllPurpuseEntity>();
            // ako su u istoj koloni
            if (startNode.X == endNode.X)
            {
                if (startNode.Y > endNode.Y)
                {
                    for (int y = startNode.Y; y > endNode.Y; y--)
                    {
                        if (CheckCoords(startNode.X, y))
                        {
                            retVal.Add(new AllPurpuseEntity(startNode.X, y));
                        }
                    }
                }
                else if (startNode.Y < endNode.Y)
                {
                    for (int y = 0; y < endNode.Y; y++)
                    {
                        if (CheckCoords(startNode.X, y))
                        {
                            retVal.Add(new AllPurpuseEntity(startNode.X, y));
                        }
                    }
                }
            }

            // ako su u istom redu
            if (startNode.Y == endNode.Y)
            {
                if (startNode.X > endNode.X)
                {
                    for (int x = startNode.X; x > endNode.X; x--)
                    {
                        if (CheckCoords(x, startNode.Y))
                        {
                            retVal.Add(new AllPurpuseEntity(x, startNode.Y));
                        }
                    }
                }
                else if (startNode.X < endNode.X)
                {
                    for (int x = startNode.X; x < endNode.X; x++)
                    {
                        if (CheckCoords(x, startNode.Y))
                        {
                            retVal.Add(new AllPurpuseEntity(x, startNode.Y));
                        }
                    }
                }
            }

            // I kvadrant
            if (startNode.X < endNode.X && startNode.Y > endNode.Y)
            {
                int x = startNode.X;
                for (; x < endNode.X; x++)
                {
                    if (CheckCoords(x, startNode.Y))
                    {
                        retVal.Add(new AllPurpuseEntity(x, startNode.Y));
                    }
                }
                for (int y = startNode.Y; y > endNode.Y; y--)
                {
                    if (CheckCoords(x, y))
                    {
                        retVal.Add(new AllPurpuseEntity(x, y));
                    }
                }
            }

            // II kvadrant
            if (startNode.X > endNode.X && startNode.Y > endNode.Y)
            {
                int x = startNode.X;
                for (; x > endNode.X; x--)
                {
                    if (CheckCoords(x, startNode.Y))
                    {
                        retVal.Add(new AllPurpuseEntity(x, startNode.Y));
                    }
                }
                for (int y = startNode.Y; y > endNode.Y; y--)
                {
                    if (CheckCoords(x, y))
                    {
                        retVal.Add(new AllPurpuseEntity(x, y));
                    }
                }
            }

            // III kvadrant
            if (startNode.X > endNode.X && startNode.Y < endNode.Y)
            {
                int x = startNode.X;
                for (; x > endNode.X; x--)
                {
                    if (CheckCoords(x, startNode.Y))
                    {
                        retVal.Add(new AllPurpuseEntity(x, startNode.Y));
                    }
                }
                for (int y = startNode.Y; y < endNode.Y; y++)
                {
                    if (CheckCoords(x, y))
                    {
                        retVal.Add(new AllPurpuseEntity(x, y));
                    }
                }
            }

            // IV kvadrant
            if (startNode.X < endNode.X && startNode.Y < endNode.Y)
            {
                int x = startNode.X;
                for (; x < endNode.X; x++)
                {
                    if (CheckCoords(x, startNode.Y))
                    {
                        retVal.Add(new AllPurpuseEntity(x, startNode.Y));
                    }
                }
                for (int y = startNode.Y; y < endNode.Y; y++)
                {
                    if (CheckCoords(x, y))
                    {
                        retVal.Add(new AllPurpuseEntity(x, y));
                    }
                }
            }

            return retVal;
        }

        public static HashSet<Rectangle> SetLine(List<AllPurpuseEntity> path)
        {
            HashSet<Rectangle> rectangleLine = new HashSet<Rectangle>();

            int i = 0;
            HashSet<AllPurpuseEntity> pathLastLine = new HashSet<AllPurpuseEntity>();
            if (path.Count == 2)
            {
                pathLastLine = SetCoordsForLine(path[i], path[++i]);
                foreach (var item in pathLastLine)
                    rectangleLine.Add(CreateEllipse(item));
                return rectangleLine;
            }
            else
            {
                for (; i < path.Count - 1; i++)
                {
                    pathLastLine = SetCoordsForLine(path[i], path[i + 1]);
                    foreach (var item in pathLastLine)
                        rectangleLine.Add(CreateEllipse(item));
                }
            }

            return rectangleLine;
        }

        private static Rectangle CreateEllipse(AllPurpuseEntity allPurpuseEntity)
            => new Rectangle
            {
                Width = MainWindow.CanvasData().Item3,
                Height = MainWindow.CanvasData().Item3,
                Fill = System.Windows.Media.Brushes.Black,
                Stroke = System.Windows.Media.Brushes.Black,
                StrokeThickness = 1
            };

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
