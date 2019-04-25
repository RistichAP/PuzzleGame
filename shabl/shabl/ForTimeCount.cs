using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Input;

namespace shabl
{
    public class ForTimeCount: INotifyPropertyChanged
    {
        public DateTime startdate = new DateTime();
        protected String _tlcont;
        public String tlcont 
        {
            get { return _tlcont; }
            set
            { 
                _tlcont = value;
                if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("tlcont")); }           
            }
        }
        System.Windows.Threading.DispatcherTimer dispatcherTimer;      
        public void Timer()
        {
            startdate = DateTime.Now;
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }


        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
           long tick = DateTime.Now.Ticks - startdate.Ticks;
           DateTime StopWatch = new DateTime();
           StopWatch = StopWatch.AddTicks(tick);            
           tlcont = String.Format("{0:HH:mm:ss}", StopWatch);
        }

        public event PropertyChangedEventHandler PropertyChanged;


    }
   
}
