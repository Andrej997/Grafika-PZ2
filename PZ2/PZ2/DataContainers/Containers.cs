using PZ2.Model;
using System.Collections.Generic;

namespace PZ2.DataContainers
{
    public class Containers
    {
        protected static HashSet<SubstationEntity> substationEntities = new HashSet<SubstationEntity>(); // 67
        public static HashSet<SubstationEntity> GetSubstations => substationEntities;

        protected static HashSet<NodeEntity> nodeEntities = new HashSet<NodeEntity>(); // 2043
        public static HashSet<NodeEntity> GetNodes => nodeEntities;

        protected static HashSet<SwitchEntity> switchEntities = new HashSet<SwitchEntity>(); // 2282
        public static HashSet<SwitchEntity> GetSwitches => switchEntities;

        protected static HashSet<LineEntity> lineEntities = new HashSet<LineEntity>(); // 2336
        public static HashSet<LineEntity> GetLines => lineEntities;

        protected static Dictionary<long, LineEntity> lineEntitiesDict = new Dictionary<long, LineEntity>(); // 2336
        public static Dictionary<long, LineEntity> GetLinesDict => lineEntitiesDict;

        protected static HashSet<Point> pointsFromLines = new HashSet<Point>(); // 8747
        public static HashSet<Point> GetPoints => pointsFromLines;

        /// <summary>
        /// GetSubstations + GetNodes + GetSwitches
        /// </summary>
        protected static HashSet<AllPurpuseEntity> allPurpuseEntities = new HashSet<AllPurpuseEntity>();
        public static HashSet<AllPurpuseEntity> AllPurpuseEntities => allPurpuseEntities;
        
        /// <summary>
        /// Sve zauzete koordinate
        /// </summary>
        protected static HashSet<Coord> allCords = new HashSet<Coord>();
        public static HashSet<Coord> AllCord { get => allCords; set => allCords = value; }

        private static Graph graph;
        public static Graph Graph { get => graph; set => graph = value; }
    }
}
