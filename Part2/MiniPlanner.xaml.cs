using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Part2
{
    /// <summary>
    /// Interaction logic for MiniPlanner.xaml
    /// </summary>
    public partial class MiniPlanner : Window
    {
        public MiniPlanner()
        {
            InitializeComponent();
            TimePicker.Value = new DateTime() + DateTime.Now.TimeOfDay;
            Calendar.DisplayDateStart = DateTime.Today;
        }

        private void AddEventButton_Click(object sender, RoutedEventArgs e)
        {
            var time = TimePicker.Value;
            if (!time.HasValue)
                return;
            var date = Calendar.SelectedDate;
            if (!date.HasValue)
                return;
            var eventValue = EventValueTextBox.Text;
            if (string.IsNullOrEmpty(eventValue))
                return;
            var fullDateTime = date.Value.Add(time.Value.TimeOfDay);
            var value = $"{fullDateTime} - {eventValue}";
            var values = new List<string>
            {
                value
            };
            values.AddRange(EventsList.Items.Cast<string>());
            values.Sort();
            EventsList.Items.Clear();
            foreach (var v in values) EventsList.Items.Add(v);
        }
    }
}
