using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Project1
{
    public partial class Settings : PhoneApplicationPage
    {
        public Settings()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            using (Data context = new Data(App.DataconnectionString))
            {
                String username = (from user in context.User select user.name).FirstOrDefault();
                int current_cycle = (from cycle in context.Cycle select cycle.ID).Max();
                var CycleLoaded = (from cycle in context.Cycle where cycle.ID == current_cycle select cycle).FirstOrDefault();
                if (CycleLoaded != null)
                {
                    tolerance.Text = CycleLoaded.percentage.ToString();
                    name.Text = username;
                }
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            using (Data context = new Data(App.DataconnectionString))
            {
                int current_cycle = (from cycle in context.Cycle select cycle.ID).Max();
                var CycleLoaded = (from cycle in context.Cycle where cycle.ID == current_cycle select cycle).FirstOrDefault();
                if (CycleLoaded != null)
                {
                    CycleLoaded.percentage = Int32.Parse(tolerance.Text);
                }
                var UserLoaded = (from user in context.User select user).FirstOrDefault();
                UserLoaded.name = name.Text;
                context.SubmitChanges();
            }
        }

        private void SaveSettings(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}