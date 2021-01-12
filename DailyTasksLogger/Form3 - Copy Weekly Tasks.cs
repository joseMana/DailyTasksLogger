using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DailyTasksLogger
{
    public partial class Form3 : Form
    {
        private TextBox multilineTxtBox;
        Dictionary<string, string> dayDateValuePair = new Dictionary<string, string>();
        Tuple<string, string, string> dayDateTuple = new Tuple<string, string, string>("Qwe", "qwe", "123");
        public Form3()
        {
            InitializeComponent();

            multilineTxtBox = new TextBox();
            multilineTxtBox.Location = new System.Drawing.Point(13, 30);
            multilineTxtBox.Multiline = true;
            multilineTxtBox.Size = new System.Drawing.Size(418, 173);
            multilineTxtBox.TabIndex = 0;

            List<DailyTasks> tasksForTheWeek = Helper.SQLLiteDBHelper.GetTasksForTheWeek();


            foreach (DailyTasks dailyTask in tasksForTheWeek)
            {
                dayDateValuePair.Add(dailyTask.Day.ToString(), dailyTask.Day.ToString() + "(" + Helper.DateTimeHelper.GetDateTimeString(dailyTask.Day) + ")");
                
                multilineTxtBox.Text += dailyTask.Day.ToString() + "("+ Helper.DateTimeHelper.GetDateTimeString(dailyTask.Day) +")" + Environment.NewLine;
                multilineTxtBox.Text += dailyTask.TasksForTheDay + Environment.NewLine + Environment.NewLine;
            }


            const int padding = 3;
            int numLines = multilineTxtBox.GetLineFromCharIndex(multilineTxtBox.TextLength) + 1;
            int border = multilineTxtBox.Height - multilineTxtBox.ClientSize.Height;
            multilineTxtBox.Height = multilineTxtBox.Font.Height * numLines + padding + border;

            this.Controls.Add(multilineTxtBox);
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            string tasks;
            string dayDate;
            foreach(var dayDateValue in dayDateValuePair.Keys)
            {
                if (dayDateValuePair.TryGetValue(dayDateValue, out dayDate) && !dayDate.Contains("Friday"))
                {
                    tasks = multilineTxtBox.Text.Split(new string[1] { dayDate }, 1000, StringSplitOptions.None)[1];
                    tasks = tasks.Substring(2, tasks.Length - 2);

                    string dayDate2;
                    dayDateValuePair.TryGetValue(GetNextDay(dayDate), out dayDate2);
                    tasks = tasks.Split(new string[1] { dayDate2 }, 1000, StringSplitOptions.None)[0];
                    
                    tasks = tasks.Substring(0, tasks.Length - 4);
                }
                else
                {
                    //Friday
                    tasks = multilineTxtBox.Text.Split(new string[1] { dayDate }, 1000, StringSplitOptions.None)[1];
                    tasks = tasks.Substring(2, tasks.Length - 2);
                    tasks = tasks.Substring(0, tasks.Length - 4);
                }

                Helper.SQLLiteDBHelper.UpdateTasksForDay(
                new DailyTasks
                {
                    Day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), dayDateValue),
                    TasksForTheDay = tasks
                });
            }
        }

        private string GetNextDay(string dayDate)
        {
            if (dayDate.Contains("Monday"))
            {
                return "Tuesday";
            }
            else if (dayDate.Contains("Tuesday"))
            {
                return "Wednesday";
            }
            else if (dayDate.Contains("Wednesday"))
            {
                return "Thursday";
            }
            else if (dayDate.Contains("Thursday"))
            {
                return "Friday";
            }
            else if (dayDate.Contains("Friday"))
            {
                return "Tuesday";
            }
            return "";
        }
    }
}
