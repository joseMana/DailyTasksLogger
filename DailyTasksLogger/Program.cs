using Cron;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DailyTasksLogger
{
    class Program
    {
        public static CronDaemon cron = new CronDaemon();
        public static Random Random = new Random();
        public static DayOfWeek Today = System.DateTime.Now.DayOfWeek;
        private static Thread _delagatorThread;
        static void Main()
        {
            string time = DateTime.Now.TimeOfDay.ToString()[0].ToString() + DateTime.Now.TimeOfDay.ToString()[1].ToString();
            var dayOfWeek = (DayOfWeek)System.DateTime.Now.DayOfWeek;

            #region Cron Implementation
            if ((time == "17" || time == "18" || time == "19" ||
                time == "20" || time == "21" || time == "22" ||
                time == "23" || time == "00" || time == "01" ||
                time == "02")
                &&
                (dayOfWeek != DayOfWeek.Saturday && dayOfWeek != DayOfWeek.Sunday))
            {
                //cron.Add("59 * * * *", () => {
                //    var form = new Form1();
                //    Application.Run(form);
                //})


                //cron.Start();
            } 
            #endregion

            InitializeDelegator();
            InitializeForm(new Form1(Today));
            Console.ReadLine();
        }
        private static void InitializeDelegator()
        {
            _delagatorThread = new Thread(() =>
            {
                while (1==1)
                {
                    if (Helper.IsDelegateTriggered)
                    {
                        InitializeForm(Helper.FormToDelegate);
                    }
                    Thread.Sleep(1000);
                }
            });
            _delagatorThread.SetApartmentState(ApartmentState.STA);
            _delagatorThread.Start();
        }
        public static void InitializeForm(Form form)
        {

            if (form is Form1)
            {
                Application.Run(form);
            }
            else if(form is Form2)
            {
                Application.Run(new Form2());
            }
            else if (form is Form3)
            {
                Application.Run(new Form3());
            }

            Helper.IsDelegateTriggered = false;
        }
    }
}
