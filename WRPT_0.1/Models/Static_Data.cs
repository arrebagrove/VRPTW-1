using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WRPT_0._1.Models
{
    public static class Static_Data
    {
        public static List<Technician> List_Tech = new List<Technician>();

        public static Depot _Depot = new Depot();

        public static List<Request> List_Request = new List<Request>();


      public   static void Initialising_Tech()
        {    // Initialising_Tech Number 1
            List<int> Tools = new List<int>() { 1, 2, 3 };
            List<int> Tools_Exchange = new List<int>() { 1, 2, 3  };
            List<int> Skill = new List<int>() { 1, 2, 3 };
            Window_Temp Wind_Temp = new Window_Temp() { Start_Time = 50, End_Time = 100 };
            Position P = new Position() { X_Pos = 1, Y_Pos = 1235 };


            Technician _Tech1 = new Technician()
            {
                Id = 1,
                Skills = Skill,
                Win_Temp = Wind_Temp,
                List_Tool_Exchange = Tools_Exchange,
                List_Tools = Tools,
                Pos = P
            };
            // Initialising_Tech Number 2
              Tools = new List<int>() { 1, 2, 3 };
             Tools_Exchange = new List<int>() { 1, 2, 3 };
              Skill = new List<int>() { 1, 2, 3 };
              Wind_Temp = new Window_Temp() { Start_Time = 50, End_Time = 100 };
              P = new Position() { X_Pos = 1, Y_Pos = 1235 };


            Technician _Tech2 = new Technician()
            {
                Id = 1,
                Skills = Skill,
                Win_Temp = Wind_Temp,
                List_Tool_Exchange = Tools_Exchange,
                List_Tools = Tools,
                Pos = P
            };

          // Adding Technician To List Of Data
            List_Tech.Add(_Tech1);
            List_Tech.Add(_Tech2);

        }
      public static void Initialising_Request()
        {   
            List<int> Tools = new List<int>() { 1, 2, 3 };
            List<int> Tools_Exchange = new List<int>() { 1, 2, 3 };
            List<int> Skill  = new List<int>() { 1, 2, 3 };
            Window_Temp Wind_Temp = new Window_Temp() {Start_Time=50,End_Time=100 };
            Position P = new Position() { X_Pos = 1, Y_Pos = 1235 };


            Request _Request = new Request()
            {   Id=1,
                Skills =Skill,
                Win_Temp=Wind_Temp,
                List_Tool_Exchange = Tools_Exchange,
                List_Tools = Tools,
                Pos = P
            };
        }
      public static void Initialising_Depot()
        {


            List<int>  Tools =new List<int>() {};
            List<int>  Tools_Exchange = new List<int>() {};
            Position P = new Position() { X_Pos = 40, Y_Pos = 50 };

            _Depot = new Depot()
            {
                List_Tool_Exchange = Tools_Exchange,
                List_Tools = Tools,
                Pos = P
            };

        }
      public static void Initialising_FromFile()
      {
 
          string[] lines = System.IO.File.ReadAllLines(@"C:\C101.txt");

         int  i = 0;
              foreach (string line in lines)
              {
                  string line1 = line.Replace("   ", "/").Replace("     ", "/").Replace("    ", "/").Replace(" ", "/").ToString();
                  line1 = line1.Replace("///", "/").Replace("//", "/");
                  try
                  {
                      if (i == 8)//Depot
                      {
                           Initialising_Depot();

                      }
                      if (i>8 && i<34) //Technician
                      {
                          int _ID = int.Parse(line1.Split('/')[0]);
                          int _X = int.Parse(line1.Split('/')[1]);
                          int _Y = int.Parse(line1.Split('/')[2]);
                          int _TWS = int.Parse(line1.Split('/')[3]);
                          int _TWE = int.Parse(line1.Split('/')[4]);
                          string _Serv = line1.Split('/')[5];

                          //Getting Skills From File
                          string[] _Skill = line1.Split('/')[6].Split('[')[1].Split(']')[0].Split(',');
                          int[] _Skill_int = Array.ConvertAll(_Skill, c => int.Parse(c));
                          List<int> Skill = new List<int>() { };
                          foreach (int Item in _Skill_int)
                              Skill.Add(Item);
 
                             

                          //Getting Tools From File
                          List<int> Tools = new List<int>() { };

                          try
                          {
                              string[] _Tools = line1.Split('/')[7].Split('[')[1].Split(']')[0].Split(',');
                              int[] __Tools_int = Array.ConvertAll(_Tools, c => int.Parse(c));
                              foreach (int Item in __Tools_int)
                                  Tools.Add(Item);
                          }
                          catch { }
                       

                          //Getting SpareParts From File
                          string[] _SparePart = line1.Split('/')[8].Split('[')[1].Split(']')[0].Split(',');
                          int[] _SparePart_int = Array.ConvertAll(_SparePart, c => int.Parse(c));
                          List<int> Tools_Exchange = new List<int>() { };
                          foreach (int Item in _SparePart_int)
                              Tools_Exchange.Add(Item);

                          Window_Temp Wind_Temp = new Window_Temp() { Start_Time=_TWS,End_Time=_TWE};
                          Position P = new Position() {X_Pos=_X,Y_Pos=_Y };


                          Technician _Tech = new Technician()
                          {
                              Id = _ID,
                              Skills = Skill,
                              Win_Temp = Wind_Temp,
                              List_Tool_Exchange = Tools_Exchange,
                              List_Tools = Tools,
                              Pos = P
                          };
                          List_Tech.Add(_Tech);

                      }
                      if (i >= 34) //Requests
                      {
                          int _ID = int.Parse(line1.Split('/')[0]);
                          int _X = int.Parse(line1.Split('/')[1]);
                          int _Y = int.Parse(line1.Split('/')[2]);
                          int _TWS = int.Parse(line1.Split('/')[3]);
                          int _TWE = int.Parse(line1.Split('/')[4]);
                          string _Serv = line1.Split('/')[5];

                          //Getting Skills From File
                          string[] _Skill = line1.Split('/')[6].Split('[')[1].Split(']')[0].Split(',');
                          int[] _Skill_int = Array.ConvertAll(_Skill, c => int.Parse(c));
                          List<int> Skill = new List<int>() { };
                          foreach (int Item in _Skill_int)
                              Skill.Add(Item);



                          //Getting Tools From File
                          List<int> Tools = new List<int>() { };

                          try
                          {
                              string[] _Tools = line1.Split('/')[7].Split('[')[1].Split(']')[0].Split(',');
                              int[] __Tools_int = Array.ConvertAll(_Tools, c => int.Parse(c));
                              foreach (int Item in __Tools_int)
                                  Tools.Add(Item);
                          }
                          catch { }


                          //Getting SpareParts From File
                          string[] _SparePart = line1.Split('/')[8].Split('[')[1].Split(']')[0].Split(',');
                          int[] _SparePart_int = Array.ConvertAll(_SparePart, c => int.Parse(c));
                          List<int> Tools_Exchange = new List<int>() { };
                          foreach (int Item in _SparePart_int)
                              Tools_Exchange.Add(Item);

                          Window_Temp Wind_Temp = new Window_Temp() { Start_Time = _TWS, End_Time = _TWE };
                          Position P = new Position() { X_Pos = _X, Y_Pos = _Y };

                          Request _Request = new Request()
                          {
                              Id = _ID,
                              Skills = Skill,
                              Win_Temp = Wind_Temp,
                              List_Tool_Exchange = Tools_Exchange,
                              List_Tools = Tools,
                              Pos = P
                          };
                          List_Request.Add(_Request);
                      }



                  }
                  catch  {}
                  i++;
              }
        
      }

    }
}
