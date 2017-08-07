using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WRPT_0._1.Models
{
  public  class Technician
    {
        public int Id { get; set; }
        public Position Pos { get; set; }
        public List<int> List_Tools { get; set; }
        public List<int> List_Tool_Exchange { get; set; }
        public List<int> Skills { get; set; }
        public Window_Temp Win_Temp { get; set; }

        public List<Request> Routes = new List<Request>();
        public List<Request> UnRouted = new List<Request>();

        public double Distance_To_Request { get; set; }
       public double GetDistanceTo_RequestPosition(Request _Request)
        {
            double dX = this.Pos.X_Pos - _Request.Pos.X_Pos;
            double dY = this.Pos.Y_Pos - _Request.Pos.Y_Pos;
            double multi = dX * dX + dY * dY;
            double Dist = Math.Sqrt(multi);
            Distance_To_Request = Dist;
            return Dist;
        }

       public double GetDistanceTo_DepotPosition(Depot _Depot)
       {
           double dX = this.Pos.X_Pos - _Depot.Pos.X_Pos;
           double dY = this.Pos.Y_Pos - _Depot.Pos.Y_Pos;
           double multi = dX * dX + dY * dY;
           double Dist = Math.Sqrt(multi);
           Distance_To_Request = Dist;
           return Dist;
       }

      

        
    }
}
