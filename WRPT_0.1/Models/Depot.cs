using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WRPT_0._1.Models
{
    public class Depot
    {
        public Position Pos { get; set; }
        public List<int> List_Tools { get; set; }
        public List<int> List_Tool_Exchange { get; set; }
        public double GetDistanceTo_RequestPosition(Request Requ)
        {
            double dX = this.Pos.X_Pos - Requ.Pos.X_Pos;
            double dY = this.Pos.Y_Pos - Requ.Pos.Y_Pos;
            double multi = dX * dX + dY * dY;
            double Dist = Math.Sqrt(multi);

            return Dist;
        }
    }
}
