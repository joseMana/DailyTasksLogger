﻿using System;
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
        public Form3()
        {
            InitializeComponent();

            TextBox multilineTxtBox = new TextBox();
            multilineTxtBox.Location = new System.Drawing.Point(13, 30);
            multilineTxtBox.Multiline = true;
            multilineTxtBox.Size = new System.Drawing.Size(418, 173);
            multilineTxtBox.TabIndex = 0;

            List<DailyTasks> tasksForTheWeek = Helper.SQLLiteDBHelper.GetTasksForTheWeek();


            foreach (DailyTasks dailyTask in tasksForTheWeek)
            {
                multilineTxtBox.Text += dailyTask.Day.ToString() + "("+ Helper.DateTimeHelper.GetDateTimeString(dailyTask.Day) +")" + Environment.NewLine;
                multilineTxtBox.Text += dailyTask.TasksForTheDay + Environment.NewLine + Environment.NewLine;
            }


            const int padding = 3;
            int numLines = multilineTxtBox.GetLineFromCharIndex(multilineTxtBox.TextLength) + 1;
            int border = multilineTxtBox.Height - multilineTxtBox.ClientSize.Height;
            multilineTxtBox.Height = multilineTxtBox.Font.Height * numLines + padding + border;

            this.Controls.Add(multilineTxtBox);
        }
    }
}
