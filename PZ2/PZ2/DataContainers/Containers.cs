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

        protected static List<NodeEntity> nodeEntities = new List<NodeEntity>(); // 2043

        protected static List<SwitchEntity> switchEntities = new List<SwitchEntity>(); // 2282

        protected static List<LineEntity> lineEntities = new List<LineEntity>(); // 2336

        protected static List<Point> pointsFromLines = new List<Point>(); // 8747
    }
}
