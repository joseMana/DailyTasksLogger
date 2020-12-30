using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsInput.Native;
// TODO : Fix Messy Code of 'GetDateTimeString'
namespace DailyTasksLogger
{
    public static class Helper
    {
        public static string connectionString = @"Data Source=C:\Users\JosephM\source\repos\DailyTasksLogger\DailyTasksLogger\DB.db";

        #region Method Delegator
        public static bool IsDelegateTriggered;
        public static Form FormToDelegate;
        public static Form FormToDelegateSetter
        {
            set
            {
                FormToDelegate = value;
                IsDelegateTriggered = true;
            }
        }
        #endregion

        public static class TextBoxHelper
        {
            private static WindowsInput.InputSimulator _inputSimulator = new WindowsInput.InputSimulator();
            private static string bulletAndTab = "•\t";
            public static void generic_TextBox_KeyPress(object sender, KeyPressEventArgs e)
            {
                if (e.KeyChar == '\r')
                {
                    Helper.TextBoxHelper.CreateBulletByInputSimulator();
                }
            }
            public static void generic_TextBox_Click(object sender, EventArgs e)
            {
                if (string.IsNullOrEmpty(sender.GetType().GetProperty("Text").GetValue(sender).ToString()))
                {
                    Helper.TextBoxHelper.CreateBulletByInputSimulator();
                }
            }
            private static void CreateBulletByInputSimulator()
            {
                _inputSimulator.Keyboard.TextEntry(bulletAndTab);
                _inputSimulator.Keyboard.KeyDown(VirtualKeyCode.END);
            }

        }
        public static class SQLLiteDBHelper
        {
            public static DailyTasks GetTasksForTheDay(DayOfWeek day)
            {
                DailyTasks dailyTasks = new DailyTasks();

                using (var con = new SQLiteConnection(Helper.connectionString))
                {
                    con.Open();
                    using (var cmd = new SQLiteCommand(con))
                    {
                        cmd.CommandText = @"SELECT * FROM Tasks WHERE Day = '" + day.ToString() + "'";

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                dailyTasks.Day = day;
                                dailyTasks.TasksForTheDay = reader["TasksForTheDay"].ToString();
                            }
                        }
                    }
                    con.Close();
                }
                return dailyTasks;
            }
            public static List<DailyTasks> GetTasksForTheWeek()
            {
                List<DailyTasks> tasksForTheWeek = new List<DailyTasks>();

                foreach (var day in (DayOfWeek[])Enum.GetValues(typeof(DayOfWeek)))
                {
                    if (day == DayOfWeek.Sunday || day == DayOfWeek.Saturday) { continue; }

                    DailyTasks dailyTasks = new DailyTasks();
                    using (var con = new SQLiteConnection(Helper.connectionString))
                    {
                        con.Open();
                        using (var cmd = new SQLiteCommand(con))
                        {
                            cmd.CommandText = @"SELECT * FROM Tasks WHERE Day = @day";
                            cmd.Parameters.AddWithValue("@day", day.ToString());

                            using (var reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    Enum.TryParse(reader["Day"].ToString(), out DayOfWeek parsedDay);

                                    dailyTasks.Day = parsedDay;
                                    dailyTasks.TasksForTheDay = reader["TasksForTheDay"].ToString();
                                }
                            }
                        }
                        con.Close();
                    }

                    tasksForTheWeek.Add(dailyTasks);
                }

                return tasksForTheWeek;
            }
            public static void UpdateTasksForDay(DailyTasks dailyTasks)
            {
                using (var con = new SQLiteConnection(Helper.connectionString))
                {
                    con.Open();
                    using (var cmd = new SQLiteCommand(con))
                    {
                        cmd.CommandText = @"UPDATE Tasks SET TasksForTheDay = @tasks WHERE Day = @day";
                        cmd.Parameters.AddWithValue("@tasks", dailyTasks.TasksForTheDay);
                        cmd.Parameters.AddWithValue("@day", dailyTasks.Day.ToString());

                        var a = cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }
            }
            public static void ResetDailyTasks()
            {
                foreach (var day in (DayOfWeek[])Enum.GetValues(typeof(DayOfWeek)))
                {
                    using (var con = new SQLiteConnection(Helper.connectionString))
                    {
                        con.Open();
                        using (var cmd = new SQLiteCommand(con))
                        {
                            cmd.CommandText = @"UPDATE Tasks SET TasksForTheDay = ''";
                            cmd.ExecuteNonQuery();
                        }
                        con.Close();
                    }
                }
            }
        }
        public static class DateTimeHelper
        {
            public static string dateOfMonday = GetDateTimeString(DayOfWeek.Monday);
            public static string monthToday = DateTime.Now.Month.ToString();
            public static string GetDateTimeString(DayOfWeek dayOfWeek)
            {
                if(dayOfWeek == DayOfWeek.Monday)
                {
                    return GetDateTimeStringOfMonday(dayOfWeek);
                }
                else
                {
                    int dayOnDateOfMonday = int.Parse(dateOfMonday.Split('/')[1]);
                    string month = monthToday;
                    int newDay = dayOnDateOfMonday + (int)dayOfWeek - 1;

                    if (HasThirtyOneDays(monthToday) && newDay > 32)
                    {
                        newDay = newDay - 31;

                        if (month.Equals("12"))
                            month = (Convert.ToInt32(month) + 1).ToString();
                        else
                            month = "1";
                    }
                    else if (newDay > 30)
                    {
                        newDay = newDay - 31;

                        if (month.Equals("12"))
                            month = (Convert.ToInt32(month) + 1).ToString();
                        else
                            month = "1";
                    }

                    return month + "/" + newDay.ToString() + "/" + dateOfMonday.Split('/')[2];
                }
            }
            private static string GetDateTimeStringOfMonday(DayOfWeek dayOfWeek)
            {
                //reverse lookup
                bool DayOfWeekNotYetFound = true;
                int days = 0;
                do
                {
                    if (DateTime.Now.AddDays(-days).DayOfWeek == dayOfWeek)
                    {
                        return DateTime.Now.AddDays(-days).ToString().Split(' ')[0];
                    }
                    days++;

                    if (days > 5)
                        break;
                }
                while (DayOfWeekNotYetFound);


                //forward lookup
                DayOfWeekNotYetFound = true;
                days = 0;
                do
                {
                    if (DateTime.Now.AddDays(days).DayOfWeek == dayOfWeek)
                    {
                        return DateTime.Now.AddDays(days).ToString().Split(' ')[0];
                    }
                    days++;

                    if (days > 5)
                        break;
                }
                while (DayOfWeekNotYetFound);

                return "";
            }

            private static bool HasThirtyOneDays(string month)
            {
                if(month == "1" || month == "3" || month == "5" ||
                        month == "7" || month == "8" || month == "10" || month == "12")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        
    }
}
