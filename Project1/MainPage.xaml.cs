using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Windows.Navigation;

namespace Project1
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            using (Data context = new Data(App.DataconnectionString))
            {
                if (!context.DatabaseExists())
                {
                    context.CreateDatabase();
                
                }

                var user_exist = (from user in context.User select user).FirstOrDefault();
                if(user_exist == null)
                {
                    User user = new User();
                    user.name = "User";
                    context.User.InsertOnSubmit(user);
                    context.SubmitChanges();
                }

                String name = (from user in context.User select user.name).FirstOrDefault();
                var exist_cycle = (from cycle in context.Cycle select cycle).FirstOrDefault();
                if (exist_cycle == null)
                {
                    Cycle cycle = new Cycle();
                    cycle.percentage = 20;
                    context.Cycle.InsertOnSubmit(cycle);
                    context.SubmitChanges();
                    MessageBox.Show(name + ", The first cycle was created by default.\nYou can change the username in settings!");
                }
            }
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            using (Data context = new Data(App.DataconnectionString))
            {
                int current_cycle = (from cycle in context.Cycle select cycle.ID).Max();
                IQueryable<Rubro> list = from rubro in context.Rubro where rubro.cycle == current_cycle select rubro;
                List<Rubro> RubroItems = list.ToList();
                listBox1.ItemsSource = RubroItems;
            }
        }

        private void NewRubro(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/NewRubro.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Menu(object sender, RoutedEventArgs e)
        {
            if (menu.Visibility == Visibility.Collapsed)
            {
                menu.Visibility = Visibility.Visible;
            }
            else
            {
                menu.Visibility = Visibility.Collapsed;
            }
        }

        private void EditRubro(object sender, EventArgs e) 
        {
            if (listBox1.SelectedIndex != -1) 
            {
                Rubro r = (Rubro)listBox1.SelectedItem;
                NavigationService.Navigate(new Uri("/EditRubro.xaml?id=" + r.ID, UriKind.RelativeOrAbsolute));
            }
            else
            {
                using (Data context = new Data(App.DataconnectionString))
                {
                    String username = (from user in context.User select user.name).FirstOrDefault();
                    MessageBox.Show(username + ", A Rubro must be selected in order to edit it!");
                }
            }
        }

        private void NewCycle(object sender, EventArgs e)
        {
            using (Data context = new Data(App.DataconnectionString))
            {
                Cycle new_cycle = new Cycle();
                new_cycle.percentage = 20;
                context.Cycle.InsertOnSubmit(new_cycle);
                context.SubmitChanges();
            }
            NavigationService.Navigate(new Uri("/NewCycle.xaml", UriKind.RelativeOrAbsolute));
        }

        private void ViewRecord(object sender, EventArgs e)
        { 
            NavigationService.Navigate(new Uri("/Record.xaml", UriKind.RelativeOrAbsolute));
        }

        private void ShowSettings(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}