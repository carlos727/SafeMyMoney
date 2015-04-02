using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;

namespace Project1
{
    public partial class NewCycle : PhoneApplicationPage
    {
        public NewCycle()
        {
            InitializeComponent();
            using (Data context = new Data(App.DataconnectionString))
            {
                int current_cycle = (from cycle in context.Cycle select cycle.ID).Max();
                var isfirst = (from rubro in context.Rubro where rubro.cycle == current_cycle select rubro).FirstOrDefault();
                if (isfirst == null)
                {
                    IQueryable<Rubro> list = from rubro in context.Rubro where rubro.cycle == (current_cycle - 1) select rubro;
                    List<Rubro> RubroItems = list.ToList();
                    int i = 0;
                    while (i < RubroItems.Count)
                    {
                        Rubro r = RubroItems.ElementAt(i);
                        Rubro rubro = new Rubro();
                        rubro.name = r.name;
                        rubro.type = r.type;
                        rubro.expected = r.expected;
                        rubro.current = 0;
                        rubro.cycle = current_cycle;
                        context.Rubro.InsertOnSubmit(rubro);
                        context.SubmitChanges();
                        i++;
                    }
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
        
        private void EditExpected(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                Rubro r = (Rubro)listBox1.SelectedItem;
                NavigationService.Navigate(new Uri("/EditExpected.xaml?id=" + r.ID, UriKind.RelativeOrAbsolute));
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

        private void New_Cycle(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPAge.xaml", UriKind.RelativeOrAbsolute));
        }

    }
}