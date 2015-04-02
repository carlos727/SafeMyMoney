﻿using System;
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
    public partial class Page1 : PhoneApplicationPage
    {
        public Page1()
        {
            InitializeComponent();
            option1.IsChecked = true;
        }

        private void New_Rubro(object sender, RoutedEventArgs e)
        {
            if (name.Text.Length != 0 && expected.Text.Length != 0)
            {
                using (Data context = new Data(App.DataconnectionString))
                {
                    String username = (from user in context.User select user.name).FirstOrDefault();
                    int current_cycle = (from cycle in context.Cycle select cycle.ID).Max();
                    Rubro rubro = new Rubro();
                    rubro.name = name.Text;
                    if (option1.IsChecked == true)
                    {
                        rubro.type = option1.Content.ToString();
                    }
                    else
                    {
                        rubro.type = option2.Content.ToString();
                    }

                    rubro.expected = Int32.Parse(expected.Text);
                    rubro.current = Int32.Parse(current.Text);
                    rubro.cycle = current_cycle;
                    context.Rubro.InsertOnSubmit(rubro);
                    context.SubmitChanges();
                    MessageBox.Show(username + ", The Rubro Was Added Successfully");
                }
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
            }
            else
            {
                using (Data context = new Data(App.DataconnectionString))
                {
                    String username = (from user in context.User select user.name).FirstOrDefault();
                    MessageBox.Show(username + ", The Field name or expected value empty, please provide them.");
                }
            }
        }
    }
}