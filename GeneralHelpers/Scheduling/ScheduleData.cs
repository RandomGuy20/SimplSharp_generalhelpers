using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralHelpers.Scheduling
{
    [Serializable]
    internal class ScheduleData //:  INotifyPropertyChanged
    {

       /* private int _eventID;
        private string _eventName;
        private List<bool> _days;
        private List<string> _DayString;
        private int _hour;
        private int _minute;
        private bool _active;*/



        /*public int EventID 
        {
            get;
        }*/

        
        public string EventName
        {
            get;set;
            //get { return this.EventName; } set { EventName = value; PropertyChanged(this, new PropertyChangedEventArgs("EventName")); }
            /*get
            {
                return _eventName;
            }
            set
            {
                _eventName = value;
               // PropertyChanged(this, new PropertyChangedEventArgs("EventName"));
            }*/
        }

        public bool[] Days
        {
            get; set;
           /* get
            {
                return _days; ;
            }
            set
            {
                this._days = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Days"));
            }*/
        }

        public string[] DaysString
        {
            get; set;
           /* get
            {
                return _DayString; 
            }
            set
            {
                this._DayString = value;
                PropertyChanged(this, new PropertyChangedEventArgs("DayString"));
            }*/
        }

        public int Hour
        {
            get; set;
           /* get
            {
                return _hour;
            }
            set
            {
                _hour = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Hour"));
            }*/
        }

        public int Minute
        {
            get; set;
            /*get
            {
                return _minute;
            }
            set
            {
                this._minute = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Minute"));
            }*/
        }

        public bool Active
        {
            get; set;
           /* get
            {
                return _active;
            }
            set
            {
                this._active = value;
                if (value)
                    StartScheduler();
                else
                    StopScheduler();
                PropertyChanged(this, new PropertyChangedEventArgs("Active"));
            }*/
        }


        [JsonIgnore]
        public JTimerCallback CallBack { get; set; }
        [JsonIgnore]
        private JTimer timer;
        //private JTimerCallback _callback;


        //public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Event Scheduler Will have the following defaults
        /// Active - False
        /// Days - Mon - Fri
        /// Days string - Empty
        /// Event name - empty
        /// hour - 12
        /// minute - 12
        /// isRunning - false
        /// </summary>
        /*public ScheduleData(JTimerCallback callBack, int id)
        {

            Days = new List<bool>();
            DaysString = new List<string>();
            Active = false;
            EventName = "";
            Hour = 12;
            Minute = 12;
            EventID = id;
            for (int j = 0; j < 7; j++)
            {
                Days.Add(false);
                DaysString.Add(dayArray[j]);
            }
            _callback = callBack;
        }*/


        public ScheduleData()
        {

        }
        private void JTimerCallbackMethod()
        {

        }

        public void StartScheduler()
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }

            timer = new JTimer(CallBack, 1000, 1000);
            Active = true;
        }

        public void StopScheduler()
        {
            timer.Stop();
            Active = false;
        }

    }
}
