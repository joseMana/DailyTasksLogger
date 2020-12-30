using System;
using System.Windows.Forms;
namespace DailyTasksLogger
{
    public partial class Form1 : Form
    {
        private string _latestTasksValueFromDB;
        private bool _hasChanges 
        { 
            get => (_latestTasksValueFromDB.Equals(this.textBox1.Text)) ? false : true;
        }
        private DayOfWeek _dayOfWeek;
        public Form1(DayOfWeek dayOfWeek)
        {
            InitializeComponent();
            _dayOfWeek = dayOfWeek;
            this.label1.Text = this.label1.Text.Replace("[Today]", _dayOfWeek.ToString());

            var dailyTasks = Helper.SQLLiteDBHelper.GetTasksForTheDay(dayOfWeek);

            this.textBox1.Text = dailyTasks.TasksForTheDay;
            _latestTasksValueFromDB = dailyTasks.TasksForTheDay;

            this.CenterToParent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Helper.FormToDelegateSetter = new Form2();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Helper.FormToDelegateSetter = new Form3();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are You Sure To Clear All Data?", "Delete All Data?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
                Helper.SQLLiteDBHelper.ResetDailyTasks();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_hasChanges)
            {
                Helper.SQLLiteDBHelper.UpdateTasksForDay(new DailyTasks
                {
                    Day = _dayOfWeek,
                    TasksForTheDay = this.textBox1.Text
                });
            }

            Environment.Exit(0);
        }

    }
}
