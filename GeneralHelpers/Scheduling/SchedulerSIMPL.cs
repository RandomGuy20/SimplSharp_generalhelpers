using Crestron.SimplSharp;
using GeneralHelpers.EventScheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralHelpers.Scheduling
{
    public class SchedulerSIMPL
    {

        #region Fields

        EventScheduler schedule;
        private int index = 0;
        private bool[] days = new bool[] { false, false, false, false, false, false, false};

        #endregion

        #region properties

        #endregion

        #region Delegates

 
        public delegate void DaysEditedEventHandler(ushort day, ushort dayState);
        public DaysEditedEventHandler onDay { get; set; }

        public delegate void ScheduleHourChange(ushort hour);
        public ScheduleHourChange onHourChange { get; set; }

        public delegate void ScheduleMinuteChange(ushort minute);
        public ScheduleMinuteChange onminuteChange { get; set; }

        public delegate void UpdatedScheduleSavedEventHandler(ushort saved);
        public UpdatedScheduleSavedEventHandler onScheduleSave { get; set; }

        public delegate void DateMatchIndexEventHandler(ushort index,ushort state);
        public DateMatchIndexEventHandler onDateMatch { get; set; }

        public delegate void OnScheduleNumberChangeEventHandler(ushort number);
        public OnScheduleNumberChangeEventHandler onScheduleNumberChange { get; set; }

        public delegate void ScheduleActiveStateChange(ushort active);
        public ScheduleActiveStateChange onActiveState { get; set; }

        public delegate void ScheduleEventNameChanged(SimplSharpString name);
        public ScheduleEventNameChanged onScheduleName { get; set; }



        #endregion

        #region Events








   

        #endregion

        #region Constructors

        public void Initialize(string filelocation)
        {
            try
            {
                if (filelocation.Length < 1)
                    filelocation = "eventScheduler.json";


                schedule = new EventScheduler(filelocation);

                schedule.onActiveChange += Schedule_onActiveChange; 
                schedule.onDateMatch += Schedule_onDateMatch1;
                schedule.onDaysChange += Schedule_onDaysChange;
                schedule.onHoursChange += Schedule_onHoursChange;
                schedule.onMinutesChange += Schedule_onMinutesChange; 
                schedule.onNameChange += Schedule_onNameChange;
                schedule.onScheduleEdit += Schedule_onScheduleEdit1;
                schedule.onScheduleSave += Schedule_onScheduleSave;

            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("\nError in Scheduler Initialize is: " + e);
            }

        }

        #endregion

        #region Internal


        private void Schedule_onScheduleSave(bool state)
        {
            onScheduleSave(Convert.ToUInt16(state));
        }

        private void Schedule_onScheduleEdit1(EventData data)
        {
            try
            {
                onScheduleName(data.EventName);
                onScheduleNumberChange(Convert.ToUInt16(data.EventID));
                onActiveState(Convert.ToUInt16(data.Active));
                onminuteChange(Convert.ToUInt16(data.Minute));
                onHourChange(Convert.ToUInt16(data.Hour));
                for (ushort i = 1; i < data.Days.Length + 1; i++)
                    onDay(i, Convert.ToUInt16(data.Days[(int)i - 1]));

            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("\nSchedulerSIMPL Schedule_onScheduleEdit1 error is: " + e);
            }
        }

        private void Schedule_onNameChange(string name)
        {
            onScheduleName(name);
        }

        private void Schedule_onMinutesChange(int minutes)
        {
            onminuteChange(Convert.ToUInt16(minutes));
        }

        private void Schedule_onHoursChange(int hours)
        {
            onHourChange(Convert.ToUInt16(hours));
        }

        private void Schedule_onDaysChange(bool[] days)
        {
            for (ushort i = 1; i < days.Length + 1; i++)
                    onDay(i, Convert.ToUInt16(days[(int)i - 1]));
               
        }

        private void Schedule_onDateMatch1(int eventIndex, string eventName, bool isMatch)
        {
            onDateMatch(Convert.ToUInt16(eventIndex),Convert.ToUInt16(isMatch));
        }

        private void Schedule_onActiveChange(bool active)
        {
            onActiveState(Convert.ToUInt16(active));
        }

        #endregion

        #region Public

        public void SetDebugSIMPL(ushort val)
        {
            schedule.Debug = Convert.ToBoolean(val);
        }


        public void SelectSchedule(ushort val)
        {
            try
            {
                if (val == 0)
                {
                    onScheduleName("");
                    onScheduleNumberChange(0);
                    onActiveState(0);
                    onminuteChange(0);
                    onHourChange(0);
                    for (ushort i = 1; i < 8; i++)
                        onDay(i, Convert.ToUInt16(0));
                    index = val;
                }
                else if (val > 0 && val < 21)
                {
                    schedule.ScheduleID = val;
                }
            }
            catch (Exception e)
            {
                schedule.SendDebug(string.Format("SchedulerSIMPL SelectSchedule(ushort val) error is: " + e));
            }


        }


        public void ScheduleName(string name)
        {
            try
            {
                schedule.ScheduleName = name;
            }
            catch (Exception e)
            {
                schedule.SendDebug(string.Format("Scheduler SIMPl Set sched name error is: " + e));
            }
        }

        public void SelectDays(ushort day, ushort status)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            try
            {
                days[Convert.ToInt16(day - 1)] = Convert.ToBoolean(status);
            }
            catch (Exception e)
            {
                schedule.SendDebug(string.Format("SchedulerSIMPl SelectDays(ushort day, ushort status) error is: " + e));
            }        
        }

        // If I get the base class (Schedule) collection of events and try to edjust here, it will stop the program ONLY in Simpl windows program
        //Error trapping is done in base class, no errors get thrown here, just stops
        public void SetHour(ushort hour)
        {
            schedule.Hour = hour;
        }

        public void SetMinute(ushort minute)
        {
            try
            {
                schedule.Minute = minute;
            }
            catch (Exception e)
            {
                schedule.SendDebug(string.Format("SchedulerSIMPl SetMinute(ushort minute) error is: " + e));
            }
        }

        public void SetScheduleActiveState(ushort state)
        {
            try
            {
                schedule.ScheduleActive = Convert.ToBoolean(state);
                CrestronConsole.PrintLine("\nSchedulerSIMPL SetScheduleActiveState(ushort state) active state is: " + Convert.ToBoolean(state));
            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("\nScheduleSIMPl SetScheduleActiveState(ushort state) Error is: " + e);
            }

        }

        public void Save()
        {
            try
            {
                schedule.Days = days;
                schedule.SaveSchedule();
            }
            catch (Exception e)
            {
                schedule.SendDebug(string.Format("Error SIMPLSchedule Save() is:{} " , e));
            }

        }

        #endregion



    }
}
