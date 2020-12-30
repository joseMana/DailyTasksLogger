using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace DailyTasksLogger
{
    public partial class Form2 : Form
    {
        private string[] daysArray = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        private int daysArrayIndex = 0;
        public Form2()
        {
            InitializeComponent(); 

            List<DailyTasks> tasksForTheWeek = Helper.SQLLiteDBHelper.GetTasksForTheWeek();

            int yPointValueToAppend = 0;

            foreach(DailyTasks dailyTask in tasksForTheWeek)
            {
                Label label = new Label();
                label.Text = dailyTask.Day.ToString();
                label.AutoSize = true;
                label.Location = new System.Drawing.Point(13, 13 + yPointValueToAppend);
                label.Size = new System.Drawing.Size(46, 17);
                label.TabIndex = 0;

                TextBox multilineTxtBox = new TextBox();
                multilineTxtBox.Name = daysArray[daysArrayIndex]; 
                multilineTxtBox.Location = new System.Drawing.Point(13, 30 + yPointValueToAppend);
                multilineTxtBox.Multiline = true;
                multilineTxtBox.Size = new System.Drawing.Size(418, 173);
                multilineTxtBox.TabIndex = 0;
                multilineTxtBox.Text = dailyTask.TasksForTheDay;
                multilineTxtBox.Click += new System.EventHandler(Helper.TextBoxHelper.generic_TextBox_Click);
                multilineTxtBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(Helper.TextBoxHelper.generic_TextBox_KeyPress);

                const int padding = 3;
                int numLines = multilineTxtBox.GetLineFromCharIndex(multilineTxtBox.TextLength) + 1;
                int border = multilineTxtBox.Height - multilineTxtBox.ClientSize.Height;
                multilineTxtBox.Height = multilineTxtBox.Font.Height * numLines + padding + border;


                this.Controls.Add(label);
                this.Controls.Add(multilineTxtBox);

                yPointValueToAppend = yPointValueToAppend + 100;

                daysArrayIndex++;
            }

            #region padding
            Label formPadding = new Label();
            formPadding.Location = new System.Drawing.Point(13, 30 + yPointValueToAppend - 50);
            formPadding.Size = new System.Drawing.Size(46, 17);
            this.Controls.Add(formPadding);
            #endregion
        }
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (Control v in this.Controls)
            {
                if(v is TextBox)
                {
                    Helper.SQLLiteDBHelper.UpdateTasksForDay(new DailyTasks
                    {
                        Day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), v.Name),
                        TasksForTheDay = v.Text
                    });
                }
            }
        }
    }
}
