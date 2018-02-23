using System;
using System.Collections.Generic;
using FaceMeApp.DependencyServices;
using FaceMeApp.Model;
using Xamarin.Forms;

namespace FaceMeApp.Views
{
    public partial class AttendanceSummaryPage : ContentPage
    {
        public AttendanceSummaryPage(EmployeeDetail details)
        {
            InitializeComponent();

            lblEmpName.Text = details.FirstName + " " + details.LastName;
            lblDesignation.Text = details.Technology;
            lblCheckinTime.Text =Convert.ToDateTime( details.InTime).ToString("t");
            lblCheckoutTime.Text = Convert.ToDateTime(details.OutTime).ToString("t");

            var duration = CalculateAttackDuration(Convert.ToDateTime(details.InTime), Convert.ToDateTime(details.OutTime));
            lblTotalHours.Text= (duration < 0) ? "NA" : duration.ToString();

            var path = DependencyService.Get<IPersistStoreService>().GetImageFromLocalDirectory();
            imgUser.Source = ImageSource.FromFile(path);
        }

        public int CalculateAttackDuration(DateTime startTime, DateTime endTime)
        {
            TimeSpan span = endTime - startTime;
            var duration = ((int)span.TotalHours);
            return duration;
        }
    }
}
