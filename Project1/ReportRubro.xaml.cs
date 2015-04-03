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
    public partial class ReportRubro : PhoneApplicationPage
    {
        public ReportRubro()
        {
            InitializeComponent();
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            using (Data context = new Data(App.DataconnectionString))
            {
                IQueryable<Rubro> list = from rubro in context.Rubro select rubro;
                List<Rubro> RubroItems = list.ToList();
                for (int i = 0; i < RubroItems.Count - 1; i++)
                {
                    Rubro rubro1 = RubroItems.ElementAt(i);
                    int j = i + 1;
                    while (j < RubroItems.Count)
                    {
                        Rubro rubro2 = RubroItems.ElementAt(j);
                        if (rubro1.name == rubro2.name)
                            RubroItems.RemoveAt(j);
                        else
                            j++;
                    }
                }
                listBox1.ItemsSource = RubroItems;
            }
        }

        private void ViewReport(object sender, RoutedEventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                Rubro r = (Rubro)listBox1.SelectedItem;
                using (Data context = new Data(App.DataconnectionString))
                {
                    IQueryable<Rubro> list = from rubro in context.Rubro where rubro.name == r.name select rubro;
                    List<Rubro> RubroItems = list.ToList();
                    String message = "\tRubro " + r.name + "\n";
                    int i = 0;
                    while (i < RubroItems.Count)
                    {
                        Rubro rubro = RubroItems.ElementAt(i);
                        message += "\nCiclo " + rubro.cycle + "."
                            + "\nExpected Value: " + rubro.expected
                            + "\nCurrent Value: " + rubro.current
                            + "\n";
                        i++;
                    }

                    MessageBox.Show(message);
                }
            }
            else
            {
                using (Data context = new Data(App.DataconnectionString))
                {
                    String username = (from user in context.User select user.name).FirstOrDefault();
                    MessageBox.Show(username + ", A Rubro must be selected in order to inform you!");
                }
            }
        }
    }
}