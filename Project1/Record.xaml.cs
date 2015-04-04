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
    public partial class Record : PhoneApplicationPage
    {
        public Record()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            using (Data context = new Data(App.DataconnectionString))
            {
                IQueryable<Cycle> list = from cycle in context.Cycle select cycle;
                List<Cycle> CycleItems = list.ToList();
                listBox1.ItemsSource = CycleItems;
            }
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void ViewRubros(object sender, RoutedEventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                Cycle c = (Cycle)listBox1.SelectedItem;
                using (Data context = new Data(App.DataconnectionString))
                {
                    IQueryable<Rubro> list = from rubro in context.Rubro where rubro.cycle == c.ID select rubro;
                    List<Rubro> RubroItems = list.ToList();
                    String message = "Rubros of cycle " + c.ID + ":\n";
                    int i = 0;
                    while (i < RubroItems.Count)
                    {
                        Rubro rubro = RubroItems.ElementAt(i);
                        message += "\n" + rubro.name + "  Type: " + rubro.type + "  " + rubro.current + "/" + rubro.expected + "(C/E)";
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
                    MessageBox.Show(username + ", A Cycle must be selected in order to see it!");
                }
            }
        }

        private void ViewReport(object sender, RoutedEventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                Cycle c = (Cycle)listBox1.SelectedItem;
                using (Data context = new Data(App.DataconnectionString))
                {
                    int real_ingress = 0;
                    int real_egress = 0;
                    int expected_ingress = 0;
                    int expected_egress = 0;

                    var exist_ingress = (from rubro in context.Rubro where rubro.cycle == c.ID && rubro.type == "Ingress" select rubro).FirstOrDefault();
                    var exist_egress = (from rubro in context.Rubro where rubro.cycle == c.ID && rubro.type == "Egress" select rubro).FirstOrDefault();

                    if (exist_ingress != null)
                    {
                        real_ingress = (from rubro in context.Rubro where rubro.cycle == c.ID && rubro.type == "Ingress" select rubro.current).Sum();
                        expected_ingress = (from rubro in context.Rubro where rubro.cycle == c.ID && rubro.type == "Ingress" select rubro.expected).Sum();
                    }

                    if (exist_egress != null)
                    {
                        real_egress = (from rubro in context.Rubro where rubro.cycle == c.ID && rubro.type == "Egress" select rubro.current).Sum();
                        expected_egress = (from rubro in context.Rubro where rubro.cycle == c.ID && rubro.type == "Egress" select rubro.expected).Sum();
                    }
                    
                    String message = "\tBalance of cycle " + c.ID + "\n"
                        + "\nGeneral: " + real_ingress + " - " + real_egress + " = " + (real_ingress - real_egress)
                        + "\nIngress: " + expected_ingress + " - " + real_ingress + " = " + (expected_ingress - real_ingress)
                        + "\nEgress: " + expected_egress + " - " + real_egress + " = " + (expected_egress - real_egress)
                        + "\n";

                    IQueryable<Rubro> list = from rubro in context.Rubro where rubro.cycle == c.ID select rubro;
                    List<Rubro> RubroItems = list.ToList();
                    int i = 0;
                    while (i < RubroItems.Count)
                    {
                        Rubro rubro = RubroItems.ElementAt(i);
                        float percentage = (float.Parse(rubro.current.ToString()) / float.Parse(rubro.expected.ToString())) * 100;
                        
                        if (percentage > 100 + c.percentage) 
                            message += "\n" + rubro.name + " exceeded by " + (rubro.current - rubro.expected) + " (" + (Int32)percentage + "%)";
                        
                        if (percentage < 100 - c.percentage) 
                            message += "\n" + rubro.name + " didn't achieve its goal in " + (rubro.expected - rubro.current) + " (" + (Int32)percentage + "%)";
                        
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
                    MessageBox.Show(username + ", A Cycle must be selected in order to inform you!");
                }
            }
        }
    }
}