using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Threading.Tasks;

namespace GeneralHelpers
{

    public delegate void JTimerCallback(object obj);
    public class JTimer 
    {

        private System.Timers.Timer timer;
        private JTimerCallback callBack;
        private object _obj;
        private bool isRunning;

        /// <summary>
        /// Returns if Timer is running or not
        /// Setting to true automatically starts timer, setting to false auto stops
        /// </summary>
        public bool IsRunning 
        {
            get => isRunning; 
            set
            {
                if (value)
                    StartTimer();
                else
                    Stop();
            }
        }


        public bool IsDisposed  { get; set; }


        /// <summary>
        /// Default is automatically start with .5 second delay
        /// </summary>
        /// <param name="CallBackMethod">Method To Be called back</param>
        public JTimer(JTimerCallback CallBackMethod )
        {
            IsDisposed = false;
            _obj = null;
            callBack = CallBackMethod;
            timer = new System.Timers.Timer(500);
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
            timer.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CallBackMethod">Callback Method</param>
        /// <param name="startDelay">Delay to start in MS</param>
        /// <param name="interval">interval in MS</param>
        public JTimer(JTimerCallback CallBackMethod, int startDelay, int interval)
        {
            _obj = null;
            callBack = CallBackMethod;
            timer = new System.Timers.Timer(500);
            timer.Elapsed += Timer_Elapsed;
            IsDisposed = false;

            SetDelay(startDelay);
           
        }

        /// <summary>
        /// Will auto start timer
        /// </summary>
        /// <param name="CallBackMethod"></param>
        /// <param name="startDelay"></param>
        /// <param name="interval"></param>
        /// <param name="obj"></param>
        public JTimer(JTimerCallback CallBackMethod, int startDelay, int interval, object obj)
        {
            _obj = obj;
            callBack = CallBackMethod;
            timer = new System.Timers.Timer(500);
            timer.Elapsed += Timer_Elapsed;

            IsDisposed = false;
            SetDelay(startDelay);

        }

        /// <summary>
        /// Will Not auto start timer
        /// </summary>
        /// <param name="CallBackMethod"></param>
        /// <param name="startDelay"></param>
        /// <param name="interval"></param>
        /// <param name="obj"></param>
        public JTimer(JTimerCallback CallBackMethod, int interval , object obj)
        {
            _obj = obj;
            callBack = CallBackMethod;
            timer = new System.Timers.Timer(500);
            timer.Elapsed += Timer_Elapsed;
            IsDisposed = false;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            callBack(_obj);
        }

        public void Stop()
        {
            timer.Stop();
            isRunning = false;
            IsDisposed = false;
        }

        public void Reset()
        {
            if (!timer.Enabled)
            {
                StartTimer();
                isRunning = true;
                IsDisposed = false;
            }
        }

        public void Reset(int delay)
        {
            if (!timer.Enabled)
            {
                Stop();
                SetDelay(delay);
                StartTimer();
                isRunning = true;
                IsDisposed = false;
            }
        }

        public void Dispose()
        {

            timer.Dispose();
            isRunning = false;
            IsDisposed = true;
        }


        internal void SetDelay(int delay)
        {
            var myTask = Task.Run(async () =>
            {
                await Task.Delay(delay);
                StartTimer();
            });
        }

        internal void StartTimer()
        {
            timer.AutoReset = true;
            timer.Start();
            isRunning = true;
            IsDisposed = true;
        }

    }

}
