using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WRPT_0._1.Models
{
   public class Request
    {
       public Request()
       {
           this.Service_Time = 90;
       }
        public int Id { get; set; }
        public Position Pos { get; set; }
        public List<int> List_Tools { get; set; }
        public List<int> List_Tool_Exchange { get; set; }
        public List<int> Skills { get; set; }
        public Window_Temp Win_Temp { get; set; }
        public double Distance_To_Tech { get; set; }
        public double BeginTime { get; set; }

        public double BeginTime_AfterAddingU { get; set; }

        public Boolean NeedGoingDepot { get; set; }

        public int Service_Time { get; set; }
        public double GetDistanceTo_DepotPosition(Depot _Depot)
        {
            double dX = this.Pos.X_Pos - _Depot.Pos.X_Pos;
            double dY = this.Pos.Y_Pos - _Depot.Pos.Y_Pos;
            double multi = dX * dX + dY * dY;
            double Dist = Math.Sqrt(multi);
            
            return Dist;
        }
        public double GetDistanceTo_TechnicianPosition(Technician Tech)
        {
            double dX = this.Pos.X_Pos - Tech.Pos.X_Pos;
            double dY = this.Pos.Y_Pos - Tech.Pos.Y_Pos;
            double multi = dX * dX + dY * dY;
            double Dist = Math.Sqrt(multi);
            Distance_To_Tech = Dist;
            return Dist;
        }
        public double GetDistanceTo_RequestPosition(Request Requ)
        {
            double dX = this.Pos.X_Pos - Requ.Pos.X_Pos;
            double dY = this.Pos.Y_Pos - Requ.Pos.Y_Pos;
            double multi = dX * dX + dY * dY;
            double Dist = Math.Sqrt(multi);
            Distance_To_Tech = Dist;
            return Dist;
        }
        public double GetDistanceTo_RequestPosition_WithDepot(Request Requ)
        {

          double ThisToDepot=   GetDistanceTo_DepotPosition(Static_Data._Depot);
          double DepotToRequ = Static_Data._Depot.GetDistanceTo_RequestPosition(Requ);
           
          return (ThisToDepot + DepotToRequ);
        }
    }
}
