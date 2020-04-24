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
        protected static HashSet<SubstationEntity> substationEntities = new HashSet<SubstationEntity>(); // 67
        public static HashSet<SubstationEntity> GetSubstations => substationEntities;

        protected static HashSet<NodeEntity> nodeEntities = new HashSet<NodeEntity>(); // 2043
        public static HashSet<NodeEntity> GetNodes => nodeEntities;

        protected static HashSet<SwitchEntity> switchEntities = new HashSet<SwitchEntity>(); // 2282
        public static HashSet<SwitchEntity> GetSwitches => switchEntities;

        protected static HashSet<LineEntity> lineEntities = new HashSet<LineEntity>(); // 2336
        public static HashSet<LineEntity> GetLines => lineEntities;

        protected static HashSet<Point> pointsFromLines = new HashSet<Point>(); // 8747
        public static HashSet<Point> GetPoints => pointsFromLines;

        protected static AllPurpuseEntity[,] entityMatrix = new AllPurpuseEntity[MainWindow.CanvasData().Item1, MainWindow.CanvasData().Item2];
        public static AllPurpuseEntity[,] EntityMatrix => entityMatrix;

        protected static HashSet<AllPurpuseEntity> allPurpuseEntities = new HashSet<AllPurpuseEntity>();
        public static HashSet<AllPurpuseEntity> AllPurpuseEntities => allPurpuseEntities;

        protected static HashSet<Func<AllPurpuseEntity, IEnumerable<AllPurpuseEntity>>> allShortestPathFunction
            = new HashSet<Func<AllPurpuseEntity, IEnumerable<AllPurpuseEntity>>>();
        public static HashSet<Func<AllPurpuseEntity, IEnumerable<AllPurpuseEntity>>> AllShortestPathFunction => allShortestPathFunction;
        
        protected static HashSet<Coord> allCords = new HashSet<Coord>();
        public static HashSet<Coord> GetAllCord => allCords;
    }
}
