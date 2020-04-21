using PZ2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ2.DataContainers
{
    public class Containers
    {
        protected static List<SubstationEntity> substationEntities = new List<SubstationEntity>(); // 67
        public static List<SubstationEntity> GetSubstations => substationEntities;

        protected static List<NodeEntity> nodeEntities = new List<NodeEntity>(); // 2043
        public static List<NodeEntity> GetNodes => nodeEntities;

        protected static List<SwitchEntity> switchEntities = new List<SwitchEntity>(); // 2282
        public static List<SwitchEntity> GetSwitches => switchEntities;

        protected static List<LineEntity> lineEntities = new List<LineEntity>(); // 2336
        public static List<LineEntity> GetLines => lineEntities;

        protected static List<Point> pointsFromLines = new List<Point>(); // 8747
        public static List<Point> GetPoints => pointsFromLines;

        protected static AllPurpuseEntity[,] entityMatrix = new AllPurpuseEntity[MainWindow.CanvasData().Item1, MainWindow.CanvasData().Item2];
        public static AllPurpuseEntity[,] EntityMatrix => entityMatrix;
    }
}
