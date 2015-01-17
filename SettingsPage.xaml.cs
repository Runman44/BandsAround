using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using System.Globalization;

namespace BandsinTown
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            if (!Settings.GetValueOrDefault<Boolean>("location", true))
            {
                ToggleSwitch_location.IsChecked = false;
            }
            
            string when = Settings.GetValueOrDefault<string>("when", "Next+30+days");
            if (!when.Equals("Next+30+days"))
            {
                
                    ToggleSwitch.IsChecked = false;
                    string startDay = when.Substring(6, 2);
                    string startMonth = when.Substring(4, 2);
                    string startYear = when.Substring(0, 4);

                    string endDay = when.Substring(17, 2);
                    string endMonth = when.Substring(15, 2);
                    string endYear = when.Substring(11, 4);
                    // when substringen enzo...2015011000-20150110..
                    EndDate.Value = DateTime.ParseExact(endDay + "/" + endMonth + "/" + endYear, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                     StartDate.Value = DateTime.ParseExact(startDay+"/"+startMonth+"/"+startYear, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    

            }
            else
            {
                ToggleSwitch.IsChecked = true;
            }

            DistanceSlider.Value = Double.Parse(Settings.GetValueOrDefault<string>("distance", "20"));
        }
        
        private void switch_Checked(object sender, RoutedEventArgs e)
        {
            // set dates disabled. 
            StartDate.IsEnabled = false;
            EndDate.IsEnabled = false;

            // set a value to the next 30 days
            try
            {
                Settings.AddOrUpdateValue("when", "Next+30+days");
                Settings.save();
            }
            catch (ArgumentException ex)
            {
                
            }
        }

        private void switch_Unchecked(object sender, RoutedEventArgs e)
        {
            // set dates enabled
            StartDate.IsEnabled = true;
            EndDate.IsEnabled = true;
            StartDate.Focus(); 

            // set a value to the dates that are selected. 
            DateTime startDate = (DateTime)StartDate.Value;
            DateTime endDate = (DateTime)EndDate.Value;

            Boolean result = StartAndEndDates(startDate, endDate);

            if (result)
            {
                //2012042500-2012042700
                string date = startDate.ToString("yyyyMMdd");
                string end = endDate.ToString("yyyyMMdd");
                Settings.AddOrUpdateValue("when", date + "00-" + end + "00");
                Settings.save();
            }
            else
            {
                EndDate.Value = (DateTime)StartDate.Value;
                string date = startDate.ToString("yyyyMMdd");
                string end = endDate.ToString("yyyyMMdd");
                Settings.AddOrUpdateValue("when", date + "00-" + end + "00");
                Settings.save();
            }
        }

        private void Distance_Value_Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                Settings.AddOrUpdateValue("distance", Math.Round(((Slider)sender).Value).ToString());
                Settings.save();
            }
            catch (ArgumentException ex)
            {

            }
        }

        private void StartDate_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            DateTime startDate = (DateTime)StartDate.Value;
            DateTime endDate = (DateTime)EndDate.Value;

            Boolean result = StartAndEndDates(startDate, endDate);

            if (result)
            {
                //2012042500-2012042700
                string date = startDate.ToString("yyyyMMdd");
                string end = endDate.ToString("yyyyMMdd");
                Settings.AddOrUpdateValue("when", date + "00-" + end + "00");
                Settings.save();
            }
            else
            {
                EndDate.Value = (DateTime)StartDate.Value;
                string date = startDate.ToString("yyyyMMdd");
                string end = endDate.ToString("yyyyMMdd");
                Settings.AddOrUpdateValue("when", date + "00-" + end + "00");
                Settings.save();

                //MessageBox.Show("EndDate must allways be after the StartDate");
            }

        }

        private Boolean StartAndEndDates(DateTime date1, DateTime date2)
        {
            int result = DateTime.Compare(date1, date2);

            if (result < 0)
                return true;
            else if (result == 0)
                return true;
            else
                return false;
        }

        private void EndDate_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            DateTime startDate = (DateTime)StartDate.Value;
            DateTime endDate = (DateTime)EndDate.Value;

            Boolean result = StartAndEndDates(startDate, endDate);

            if (result)
            {
                //2012042500-2012042700
                string date = startDate.ToString("yyyyMMdd");
                string end = endDate.ToString("yyyyMMdd");
                Settings.AddOrUpdateValue("when", date + "00-" + end + "00");
                Settings.save();

            }
            else
            {

                StartDate.Value = EndDate.Value;
                string date = startDate.ToString("yyyyMMdd");
                string end = endDate.ToString("yyyyMMdd");
                Settings.AddOrUpdateValue("when", date + "00-" + end + "00");
                Settings.save();
                
            }
        }
        
        private void switch_Checked_location(object sender, RoutedEventArgs e)
        {
            //set Location on !
            Settings.AddOrUpdateValue("location", true);
            Settings.save();
        }

        private void switch_Unchecked_location(object sender, RoutedEventArgs e)
        {
            // set Location off !
            Settings.AddOrUpdateValue("location", false);
            Settings.save();
        }
    }
}