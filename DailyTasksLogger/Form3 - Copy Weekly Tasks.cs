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
            string mondayDate = ""; dayDateValuePair.TryGetValue("Monday", out mondayDate);
            string tuesdayDate = ""; dayDateValuePair.TryGetValue("Tuesday", out tuesdayDate);
            string wednesdayDate = ""; dayDateValuePair.TryGetValue("Wednesday", out wednesdayDate);
            string thursdayDate = ""; dayDateValuePair.TryGetValue("Thursday", out thursdayDate);
            string fridayDate = ""; dayDateValuePair.TryGetValue("Friday", out fridayDate);

            //Monday
            var mondayTasks = multilineTxtBox.Text.Split(new string[1] { mondayDate }, 1000, StringSplitOptions.None)[1];
            mondayTasks = mondayTasks.Substring(2, mondayTasks.Length - 2);
            mondayTasks = mondayTasks.Split(new string[1] { tuesdayDate }, 1000, StringSplitOptions.None)[0];
            mondayTasks = mondayTasks.Substring(0, mondayTasks.Length - 4);

            //Tuesday
            var tuesdayTasks = multilineTxtBox.Text.Split(new string[1] { tuesdayDate }, 1000, StringSplitOptions.None)[1];
            tuesdayTasks = tuesdayTasks.Substring(2, tuesdayTasks.Length - 2);
            tuesdayTasks = tuesdayTasks.Split(new string[1] { wednesdayDate }, 1000, StringSplitOptions.None)[0];
            tuesdayTasks = tuesdayTasks.Substring(0, tuesdayTasks.Length - 4);

            //Wednesday
            var wednesdayTasks = multilineTxtBox.Text.Split(new string[1] { wednesdayDate }, 1000, StringSplitOptions.None)[1];
            wednesdayTasks = wednesdayTasks.Substring(2, wednesdayTasks.Length - 2);
            wednesdayTasks = wednesdayTasks.Split(new string[1] { thursdayDate }, 1000, StringSplitOptions.None)[0];
            wednesdayTasks = wednesdayTasks.Substring(0, wednesdayTasks.Length - 4);

            //Thursday
            var thursdayTasks = multilineTxtBox.Text.Split(new string[1] { thursdayDate }, 1000, StringSplitOptions.None)[1];
            thursdayTasks = thursdayTasks.Substring(2, thursdayTasks.Length - 2);
            thursdayTasks = thursdayTasks.Split(new string[1] { fridayDate }, 1000, StringSplitOptions.None)[0];
            thursdayTasks = thursdayTasks.Substring(0, thursdayTasks.Length - 4);

            //Friday
            var fridayTasks = multilineTxtBox.Text.Split(new string[1] { fridayDate }, 1000, StringSplitOptions.None)[1];
            fridayTasks = fridayTasks.Substring(2, fridayTasks.Length - 2);
            fridayTasks = fridayTasks.Substring(0, fridayTasks.Length - 4);

            Helper.SQLLiteDBHelper.UpdateTasksForDay(
                new DailyTasks 
                { 
                    Day = DayOfWeek.Monday, 
                    TasksForTheDay = mondayTasks 
                });
            Helper.SQLLiteDBHelper.UpdateTasksForDay(
                new DailyTasks
                {
                    Day = DayOfWeek.Tuesday,
                    TasksForTheDay = tuesdayTasks
                });
            Helper.SQLLiteDBHelper.UpdateTasksForDay(
                new DailyTasks
                {
                    Day = DayOfWeek.Wednesday,
                    TasksForTheDay = wednesdayTasks
                });
            Helper.SQLLiteDBHelper.UpdateTasksForDay(
                new DailyTasks
                {
                    Day = DayOfWeek.Thursday,
                    TasksForTheDay = thursdayTasks
                });
            Helper.SQLLiteDBHelper.UpdateTasksForDay(
                new DailyTasks
                {
                    Day = DayOfWeek.Friday,
                    TasksForTheDay = fridayTasks
                });
        }
    }
}
