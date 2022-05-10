using Crestron.SimplSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
//using Crestron.SimplSharp.CrestronIO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralHelpers.Scheduling
{
    class Scheduler 
    {
        #region Fields


        private string fileName;
        private int events;
        private int schedID;
        private string fileDirectory;
        string dir = "\\Schedule\\";

        private string[] dayArray = new string[7] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };


        public List<ScheduleData> schedList = new List<ScheduleData>();
        internal List<ScheduleData> _schedList = new List<ScheduleData>();
        ScheduleData data = new ScheduleData();

        #endregion

        #region properties

        // The collection
       /* public IList<ScheduleData> EventList 
        {
            get 
            {
                try
                {
                    CrestronConsole.PrintLine("Scheduler IList get");
                    return schedList as IList<ScheduleData>;
                }
                catch (Exception e)
                {
                    CrestronConsole.PrintLine("Stack IList Schedule Data error is: : " + e);
                    return null;
                }

            }
            set 
            {
                try
                {
                    CrestronConsole.PrintLine("Scheduler IList set");
                    //_schedList = schedList;
                    FindChange((List<ScheduleData>)value);
                    //this.schedList = (List<ScheduleData>)value;

                }
                catch (StackOverflowException e)
                {
                    CrestronConsole.PrintLine("Stack overflow : " + e);
                }

            }
        }*/


        
        /// <summary>
        /// Sets what Schedule you want to edit, or Gets the current schedule you are editing 0 based
        /// </summary>
        public int ScheduleID 
        {
            
            get
            {
                try
                {
                    _schedList = DeserializeObject(fileDirectory);
                    ConfigureList();
                    return schedID;
                }
                catch (Exception e)
                {
                    CrestronConsole.PrintLine("Error getting schedul ID is: " + e);
                    return schedID;
                }

            }
            set 
            {
                try
                {
                    if (value >= 0 && value <= events)
                    {
                        schedID = value;
                        SetToCurrentIndex(schedID);
                        onScheduleEdit(data);
                    }
                    else
                        throw new IndexOutOfRangeException("Value is higher than there are schedules");
                }
                catch (Exception e)
                {
                    SendDebug("Scheduler ScheduleID SEt Error is: " + e);
                }

            }
        }
       

        /// <summary>
        /// Can Set the current schedule name, or get the current Schedules name
        /// </summary>
        public string ScheduleName 
        { 
            get
            {
                return data.EventName;
            }
            set
            {
                try
                {
                    data.EventName = value;
                    onScheduleEdit(schedList[ScheduleID]);
                }
                catch (Exception e)
                {
                    CrestronConsole.PrintLine("Error Scheduler Schedule name is: " + e);
                }

            }
        }

        /// <summary>
        /// Set which days you want the schedule to be active on
        /// Get which days it is running on
        /// </summary>
        public bool[] Days
        { 
            get 
            {  
                return data.Days.ToArray();
            } 
            set 
            {
                data.Days = value;
                onScheduleEdit(data);
            } 
        }

        /// <summary>
        /// Set / Get the current hour ( in 24 hr format)
        /// </summary>
        public int Hour 
        {
            get
            {
                return data.Hour;
            } 
            set
            {
                try
                {
                    if (value < 24 && value >= 0)
                    {
                        data.Hour = value;
                        onScheduleEdit(data);
                        CrestronConsole.PrintLine("Getting Schedule ID:{0} and hour is : {1} ", ScheduleID, value);
                    }
                }
                catch (Exception e)
                {
                    CrestronConsole.PrintLine("Error Scheduler setting schedule hour is: " + e);
                }
            }
        }

        public int Minute 
        {
            get
            {
                return data.Minute;
            }
            set
            {
                try
                {
                    if (value < 60 && value >= 0)
                    {
                        data.Minute = value;
                        onScheduleEdit(data);
                        CrestronConsole.PrintLine("Getting Schedule ID:{0} and minute is : {1} ", ScheduleID, value);
                    }
                }
                catch (Exception e)
                {
                    CrestronConsole.PrintLine("Error Scheduler setting schedule minute is: " + e);
                }


            }
        }

        /// <summary>
        /// Set Schedule to Run or get run state
        /// </summary>
        public bool ScheduleActive 
        {
            get
            {
                return schedList[ScheduleID].Active;
            }
            set
            {
                try
                {
                    schedList[ScheduleID].Active = value;
                    onScheduleEdit(schedList[ScheduleID]);
                    CrestronConsole.PrintLine("Getting Schedule ID:{0} and active is : {1} ", ScheduleID, value);
                }
                catch (Exception e)
                {
                    CrestronConsole.PrintLine("Error Scheduler setting schedule active is: " + e);
                }

            }
        }
        
        public bool Debug { private get;  set; }

        #endregion

        #region Delegates

        public delegate void ScheduleMatchEventHandler(int eventIndex, string eventName);

        public delegate void ScheduleChangeEventHandler(ScheduleData data);

        #endregion

        #region Events

        public event ScheduleMatchEventHandler onDateMatch;

        public event ScheduleChangeEventHandler onScheduleEdit;


        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FileName">File name</param>
        /// <param name="maxEvents">how many events are in the schedule</param>
        public Scheduler(string FileName,int maxEvents)
        {
            // When constructor is called, see if file exists and if it does read from that right away, else then configure a blank template
            try
            {
                fileName = FileName.Contains(".json") ? FileName : FileName + ".json";
                events = maxEvents;
                if (schedList.Count < maxEvents)
                {
                    for (int i = 0; i < maxEvents; i++)
                    {
                        schedList.Add(new ScheduleData());
                        schedList[i].Active = false;
                        schedList[i].Days = new bool[] { false, false, false, false, false, false, false };
                        schedList[i].DaysString = dayArray;
                        schedList[i].EventName = "";
                        schedList[i].Hour = 0;
                        schedList[i].Minute = 0;
                    }

                    SaveSchedule();
                }
            }
            catch (Exception e)
            {
                SendDebug("Error in Scheduler COnstructor is message: " + e);
            }
        }

        #endregion

        #region Internal Methods
        private void SetToCurrentIndex(int index)
        {
            try
            {
                data.Active = schedList[index].Active;
                data.Days = schedList[index].Days;
                data.DaysString = schedList[index].DaysString;
                data.EventName = schedList[index].EventName;
                data.Hour = schedList[index].Hour;
                data.Minute = schedList[index].Minute;
            }
            catch (Exception e)
            {
                SendDebug("Error in SetToCurrentINdex is: " + e);
            }

        }

        private void ConfigureList()
        {
            schedList = _schedList;
            foreach (var sched in schedList)
            {
                if (sched.CallBack == null)
                    sched.CallBack = CheckScheduleCallback;
            }
            
        }

        internal void SendDebug(string data)
        {
            //if (Debug)
            //{
                CrestronConsole.PrintLine("\nError Scheduler is: " + data);
                ErrorLog.Error("\nError Scheduler is: " + data);
            //}

        }

        private List<ScheduleData> DeserializeObject(string filePath)
        {
            try
            {
                string jsonString = "";
                FileOperations.ReadFromFile(out jsonString, filePath);

                var jsonSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                var list = JsonConvert.DeserializeObject<List<ScheduleData>>(jsonString,jsonSettings);

                events = list.Count;
                
                return list;
            }
            catch (Exception e)
            {
                SendDebug("DeserializeObject error is: " + e);
                return new List<ScheduleData>(); ;
            }

        }

        private void SerializeObject(List<ScheduleData> list, string filepath)
        {
            try
            {
                dir = "\\Schedule\\";
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                fileDirectory = dir + filepath;

                var settings = new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                FileOperations.WriteToFile(JsonConvert.SerializeObject(list,settings), fileDirectory, true) ;
            }
            catch (Exception e)
            {
                SendDebug("Error in SerializeObject is: " + e);
            }

        }

        private void CheckScheduleCallback(object obj)
        {
            try
            {
                // var xmlList = new List<Scheduler>();
                // xmlList = _obj as List<Scheduler>;

                //var daymatch = xmlList.Any(m => m.Active == true && m.Day == DateTime.Now.DayOfWeek.ToString() && m.Hour == DateTime.Now.Hour && m.Minute == DateTime.Now.Minute);
                // if (daymatch) onDateMatch();
                //&& Convert.ToInt32(DateTime.Now.DayOfWeek) == j);(m => m.Active == true && m.Days[j] == true &&  m.Hour = DateTime.Now.Hour && m.Minute == DateTime.Now.Minute);

                if (!File.Exists(fileDirectory))
                    SaveSchedule();

                var jsonList = DeserializeObject(fileDirectory);
                int val = Convert.ToInt32(obj);
               // for (int i = 0; i < schedList.Count; i++)
               // {
                //  for (int j = 0; j < 7; j++)
                // {
                // var match = jsonList.Any(m => m.Active == true && m.Days[j] == true && Convert.ToBoolean(Convert.ToInt32(DateTime.Now.DayOfWeek) == m) && 
                // Convert.ToBoolean(m.Hour = DateTime.Now.Hour) == true && Convert.ToBoolean(m.Minute == DateTime.Now.Minute));

                // Active must be true
                //Hour Must match
                //Minute muts match
                // Day must be active
                var matchIndex = jsonList.FindIndex(m => m.Active == true && m.Days[jsonList.FindIndex(d => d.DaysString.Contains(DateTime.Now.DayOfWeek.ToString()))] == true &&
                                            Convert.ToBoolean(m.Hour = DateTime.Now.Hour) == true && Convert.ToBoolean(m.Minute == DateTime.Now.Minute));
                /* var match = jsonList.Any(m => m.Active == true && m.Days[jsonList.FindIndex(d => d.DaysString.Contains(DateTime.Now.DayOfWeek.ToString()))] == true &&
                                         Convert.ToBoolean(m.Hour = DateTime.Now.Hour) == true && Convert.ToBoolean(m.Minute == DateTime.Now.Minute));*/
                if (matchIndex == val)
                    onDateMatch(matchIndex, jsonList[val].EventName);
               // }
                    
            }
            catch (Exception e)
            {
                SendDebug("CheckDayCallBAck error is: " + e);
            }

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// If you do not save the schedule, it will not run. You need to save and Start it
        /// </summary>
        internal void SaveSchedule()
        {
            try
            {
                schedList[ScheduleID] = data;
                SerializeObject(schedList, fileName);
                _schedList = DeserializeObject(fileDirectory);
                schedList = _schedList;
                ConfigureList();
            }
            catch (Exception e)
            {
                SendDebug("Save Schedule Error is: " + e.Message);
            }

        }

        public void StartSchedulerGlobal()
        {
            try
            {
                foreach (var sched in schedList)
                {
                    sched.StartScheduler();
                }
            }
            catch (Exception e)
            {
               CrestronConsole.PrintLine("Scheduler Error Starting All Schedules is: " + e);
            }
        }

        public void StopSchedulerGlobal()
        {
            try
            {
                foreach (var sched in schedList)
                {
                    sched.StopScheduler();
                }
            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("Scheduler Error Stopping All Schedules is: " + e);
            }
        }

        private void SchedulerStatus(bool state,int currentSchedule)
        {
            if (currentSchedule <= schedList.Count)
            {
                if(state)
                    schedList[currentSchedule].StartScheduler();
                else
                    schedList[currentSchedule].StopScheduler();

            }
        }

        public void SetScheduleRunningState(bool running, int currentSchedule)
        {
            if (currentSchedule <= schedList.Count)
            {
                schedList[currentSchedule].Active = running;
            }
        }

        public void SetScheduleRunningDays(bool[] runningDays, int currentSchedule)
        {
            if (currentSchedule <= schedList.Count)
            {
                schedList[currentSchedule].Days = runningDays;
            }
        }

        /// <summary>
        /// 24 hour format 0 - 24
        /// </summary>
        /// <param name="running"></param>
        /// <param name="currentSchedule"></param>
        public void SetScheduleHour(int time, int currentSchedule)
        {
            if (currentSchedule <= schedList.Count && (time >= 0 && time < 24))
            {
                schedList[currentSchedule].Hour = time;
            }
        }

        public void SetScheduleMinute(int time, int currentSchedule)
        {
            if (currentSchedule <= schedList.Count && (time >= 0 && time < 59))
            {
                schedList[currentSchedule].Minute = time;
            }
        }
        public void SetScheduleName(string name, int currentSchedule)
        {
            if (currentSchedule <= schedList.Count )
            {
                schedList[currentSchedule].EventName = name;
            }
        }



        #endregion

    }
}
