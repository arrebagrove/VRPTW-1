using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WRPT_0._1.Models;

namespace WRPT_0._1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {                            ///////// Calculate Distance With Depot Distance if shoold go
            InitializeComponent();
            Initialising_Data();
            Copy();
        }

        private void Copy()
        {
           Request R= new Request();
            Request R2;

            R2 = ICopy.Clone<Request>(R); /*  ICopy.Clone<Request>(   */
           
         }
        void Initialising_Data()
        {
            Static_Data.Initialising_FromFile();

        }
        private void Search_Click(object sender, RoutedEventArgs e)
        {  
            
            Searching_ByDistancePriority_New();
            SolomonSolution();

       
        }
      




        /// <summary>
        /// </summary>
        /// 

        void Searching_ByDistancePriority_New()
        {
             for (int i = 0; i < Static_Data.List_Request.Count(); i++)
            {
                
                Static_Data.List_Request[i].GetDistanceTo_TechnicianPosition(Static_Data.List_Tech[0]);
                
                #region  Deleting Requ From Route without Skills 
                Boolean ToAdd = true;
              
                foreach (int Sk in Static_Data.List_Request[i].Skills)
                {  
                    if (Static_Data.List_Tech[0].Skills.Contains(Sk) == false)
                        ToAdd = false;

                          
                }
                #endregion
                #region  Searching The Need of Going Depot

                Boolean NeeDepot = false;
                foreach (int Tool in Static_Data.List_Request[i].List_Tools)
                {
                    if (Static_Data.List_Tech[0].List_Tools.Contains(Tool) == false)
                    { NeeDepot = true; break; }
                 }
                foreach (int Tool_Ex in Static_Data.List_Request[i].List_Tool_Exchange)
                {
                    if (Static_Data.List_Tech[0].List_Tool_Exchange.Contains(Tool_Ex) == false)
                    { NeeDepot = true; break; }
                }
                #endregion


                if (ToAdd) Static_Data.List_Tech[0].UnRouted.Add( ICopy.Clone<Request>(Static_Data.List_Request[i]) );
            }
        Static_Data.List_Tech[0].UnRouted=   ICopy.Clone<List<Request>>(  Static_Data.List_Tech[0].UnRouted.OrderByDescending(x => x.Distance_To_Tech).ToList());
        Static_Data.List_Tech.[0].Routes.Add(ICopy.Clone<Request>(Static_Data.List_Tech[0].UnRouted[0]));
        Static_Data.List_Tech[0].UnRouted.RemoveAt(0);
        }
        public static double CoefMu = 1.0;
        public static double CoefAlpha1 = 0.5;
        public static double CoefAlpha2 = 0.5;
        public static double CoefLambda = 1;
      async  void SolomonSolution ()
        {
            while (Static_Data.List_Tech[0].UnRouted.Count > 0)
            {
                var bestU = new List<int>();
                var c1Vals = new List<double>();
                foreach (Request Req in Static_Data.List_Tech[0].UnRouted)
                {
                    double minC1 = double.MaxValue;
                    int optimalU = 1;
                    for (int i = 0; i < Static_Data.List_Tech[0].Routes.Count - 1; ++i)
                    {
                        double c1 = 0;
                        await Task.Run(() => { c1 = Criteria_C1(i, Req, i + 1); }); // was Criteria_C11
                         if (c1 < minC1)
                        {
                            minC1 = c1;
                            optimalU = i;
                        }
                    }
                    bestU.Add(optimalU);
                    c1Vals.Add(minC1);

                }
                var c2Vals = new List<double>();

                int bestRequOfMin = 0;
                int bestRequOfMax = 0; 
                double minC2 = double.MaxValue;
                double maxC2 = double.MinValue;
                for (int i = 0; i < Static_Data.List_Tech[0].UnRouted.Count; ++i)
                {
                    double c2 =  Criteria_C2(Static_Data.List_Tech[0].UnRouted[i], c1Vals[i]);
                    if (c2 < minC2)
                    {
                        minC2 = c2;
                        bestRequOfMin = i;
                    }
                    if (maxC2 > c2)
                    {
                        maxC2 = c2;
                        bestRequOfMax = i;
                    }

                    c2Vals.Add(c2); // added
                }





                Request Requ_ToAdd =ICopy.Clone<Request>( Static_Data.List_Tech[0].UnRouted[bestRequOfMax]) ;
                //Test Feasibility Before Adding

                Boolean Feasibile = false;
                #region Feasibility
            
                if (Static_Data.List_Tech[0].UnRouted[bestRequOfMax + 1].BeginTime_AfterAddingU <= Static_Data.List_Tech[0].UnRouted[bestRequOfMax + 1].Win_Temp.Start_Time)
                    Feasibile = true;
                else
                    Feasibile = false;

                #endregion 
                if (Feasibile == true)
                {
                    Static_Data.List_Tech[0].UnRouted[bestRequOfMax].BeginTime = Static_Data.List_Tech[0].UnRouted[bestRequOfMax + 1].BeginTime_AfterAddingU;
                    Static_Data.List_Tech[0].Routes.Insert(bestU[bestRequOfMax], ICopy.Clone<Request>(Requ_ToAdd));
                    Static_Data.List_Tech[0].UnRouted.RemoveAt(bestRequOfMax);
                }
                
             

               

            }


        }
     public  double Criteria_C11(int i, Request Requ, int j)
        {
            Request RequI = Static_Data.List_Tech[0].Routes[i];
            Request RequJ = Static_Data.List_Tech[0].Routes[j];
            double distIu = 0;
            double distUj  = 0;
            double distJi = 0;
            if(Requ.NeedGoingDepot==false)
            {
                  distIu = RequI.GetDistanceTo_RequestPosition(Requ);
                  distUj = Requ.GetDistanceTo_RequestPosition(RequJ);
                  distJi = RequI.GetDistanceTo_RequestPosition(RequJ);
            }
            else
            {  distIu = RequI.GetDistanceTo_RequestPosition_WithDepot(Requ);
                distUj = Requ.GetDistanceTo_RequestPosition_WithDepot(RequJ);
                distJi = RequI.GetDistanceTo_RequestPosition_WithDepot(RequJ);
            }
          
            return distIu + distUj -( CoefMu * distJi );
        }
       public double Criteria_C12(int i, Request Requ, int j)
        {
            Request RequI = Static_Data.List_Tech[0].Routes[i];
            Request RequJ = Static_Data.List_Tech[0].Routes[j];
            double bI = Static_Data.List_Tech[0].Routes[i].BeginTime;
            //double bU =  NextServiceBeginTime(Requ, RequI, bI);
            //double bJu = NextServiceBeginTime(RequJ, Requ, bU);
            double bU = 0;
            double bJu = 0;
           #region initilising bU
           if(Requ.NeedGoingDepot==false)
           {
               double EndTimeI = RequI.Win_Temp.End_Time;
               double Distance_RequI_U = RequI.GetDistanceTo_RequestPosition(Requ);
               bU = EndTimeI + Distance_RequI_U;
               Requ.BeginTime = bU;

               
           }
           else
           {
               double EndTimeI = RequI.Win_Temp.End_Time;
               double Distance_RequI_U = RequI.GetDistanceTo_RequestPosition_WithDepot(Requ);
               bU = EndTimeI + Distance_RequI_U;
           }
           #endregion
              if(RequJ.NeedGoingDepot==true)
               {
                    bJu= Requ.Win_Temp.End_Time + Requ.GetDistanceTo_RequestPosition_WithDepot(RequJ);
                 RequJ.BeginTime_AfterAddingU=bJu;
               }else
               {
                   bJu = Requ.Win_Temp.End_Time + Requ.GetDistanceTo_RequestPosition(RequJ);
                   RequJ.BeginTime_AfterAddingU = bJu;

               }


              double bJ = RequJ.BeginTime;
            return bJu - bJ;
        }


       public double Criteria_C1(int i, Request Requ, int j)
        {
            return CoefAlpha1 * Criteria_C11(i, Requ, i) + CoefAlpha2 * Criteria_C12(i, Requ, j);
        }


         double Criteria_C2(Request Requ, double c1Value)
        {
            double d0U = 0;
            if(Requ.NeedGoingDepot==false)
            { d0U = Static_Data.List_Tech[0].GetDistanceTo_RequestPosition(Requ);  }
       else {
           d0U = Static_Data.List_Tech[0].GetDistanceTo_DepotPosition(Static_Data._Depot) + Requ.GetDistanceTo_DepotPosition(Static_Data._Depot);
            
            }
            return CoefLambda * d0U - c1Value;
        }



        public double NextServiceBeginTime(Request newRequest, Request prevRequest, double prevBeginTime)
        {
            double travelTime = prevRequest.GetDistanceTo_RequestPosition(newRequest);
            double serviceTime = prevRequest.Service_Time;
            double readyTime = newRequest.Win_Temp.Start_Time;
            return Math.Min(readyTime, prevBeginTime + serviceTime + travelTime); /// to Check : Start From PrevBeginTime Or From PrevReadyTime
        }


















        /// ///////// Oldddddddddddddddddddddddd


        List<Technician> Sorted_ListOfTechnician_ToXRequest = new List<Technician>();
        void Searching_ByDistancePriority()
        {
 

          
                for (int i = 0; i < Static_Data.List_Tech.Count() ;i++)
                {
                    Static_Data.List_Tech[i].GetDistanceTo_RequestPosition(Static_Data.List_Request[0]);

                    #region  Deleting Technicians without Skills For this Request
                    Boolean ToAdd = true;
                    foreach (int Sk in Static_Data.List_Request[0].Skills)
                    { //
                        if (Static_Data.List_Tech[i].Skills.Contains(Sk) == false)
                            ToAdd = false;

                    }
                    #endregion

                    if (ToAdd)  Sorted_ListOfTechnician_ToXRequest.Add(Static_Data.List_Tech[i]);

                }


                //OrderByDescending  By Distance        /To Reverse  .Reverse();
         Sorted_ListOfTechnician_ToXRequest=   Sorted_ListOfTechnician_ToXRequest.OrderByDescending(x => x.Distance_To_Request).ToList();
             
 // -----> We Got the List of The Farthest Technician who Have Skills for this Request


        }


        List<Technician> AvailableTechnicians = new List<Technician>(); // Technician That Have Skills and Available for XRequest
        void Searching_ByTimePriority()
        {

            Request Requ= Static_Data.List_Request[0];
            Depot Dep=Static_Data._Depot;
            foreach(Technician Item in Sorted_ListOfTechnician_ToXRequest )
            {
                #region Testing if The Technician Shoold Go to Depot Before Request
                Boolean isGoingDepot = false;
                foreach (int Tool in Static_Data.List_Request[0].List_Tools)
                {  
                    if (Item.List_Tools.Contains(Tool) == false)
                        isGoingDepot = true ;
                }
                foreach (int SparePart in Static_Data.List_Request[0].List_Tool_Exchange)
                {
                    if (Item.List_Tool_Exchange.Contains(SparePart) == false)
                        isGoingDepot = true;
                }
                #endregion
                if (isGoingDepot = false)
                {
                    if((Item.Win_Temp.Start_Time + Item.Distance_To_Request <= Requ.Win_Temp.Start_Time ) &&   ( Requ.Win_Temp.Start_Time +  Requ.Service_Time  +    Item.Distance_To_Request <= Item.Win_Temp.End_Time ))
                    {
                        AvailableTechnicians.Add(Item);
                    }

                }
                    else
                { //////////// dont Forget Time pass = 0 verified
                    if ((Item.Win_Temp.Start_Time + Item.GetDistanceTo_DepotPosition(Dep) + Requ.GetDistanceTo_DepotPosition(Dep) <= Requ.Win_Temp.Start_Time) && (Requ.Win_Temp.Start_Time + Requ.Service_Time + Item.Distance_To_Request <= Item.Win_Temp.End_Time))
                    {
                        AvailableTechnicians.Add(Item);
                    }
                }

            }


        }
       

       
    }
}
