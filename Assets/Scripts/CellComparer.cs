using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class CellComparer : IComparer<Cell>
    {
        public int Compare(Cell x, Cell y)
        {
            if ((int) ((x.DistanceLeft - y.DistanceLeft) * 1000f) != 0)
            {
                return (int) ((x.DistanceLeft - y.DistanceLeft) * 1000f);
            }
            else if (x.Distance - y.Distance != 0)
            {
                return (int) ((x.Distance - y.Distance) * 1000);
            }
            else if (x.Position.x - y.Position.x != 0)
            {
                return x.Position.x - y.Position.x;
            }
            else
            {
                return x.Position.y - y.Position.y;
            }
        }
    }
}