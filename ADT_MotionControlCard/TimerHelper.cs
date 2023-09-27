using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Control_Card
{
    internal class TimerHelper
    {
        private Timer timer;
        public Action elapsedAction;

        public TimerHelper(double interval)
        {
            timer=new Timer(interval);
            timer.Elapsed += Timer_Elapsed;
        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        public void SetElapseAction(Action action)
        {
            elapsedAction = action;
        }

        private void Timer_Elapsed(object sender, EventArgs e)
        {
            elapsedAction?.Invoke();
        }

    }
}
