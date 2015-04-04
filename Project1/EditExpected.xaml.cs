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
    public partial class EditExpected : PhoneApplicationPage
    {
        public EditExpected()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            String id = NavigationContext.QueryString["id"];
            using (Data context = new Data(App.DataconnectionString))
            {
                var RubroLoaded = (from rubro in context.Rubro where rubro.ID == Int32.Parse(id) select rubro).FirstOrDefault();
                if (RubroLoaded != null)
                {
                    ID_txt.Text = RubroLoaded.ID.ToString();
                    name.Text = RubroLoaded.name.ToString();
                    type.Text = RubroLoaded.type;
                    expected.Text = RubroLoaded.expected.ToString();
                }
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            using (Data context = new Data(App.DataconnectionString))
            {
                var RubroLoaded = (from rubro in context.Rubro where rubro.ID == Int32.Parse(ID_txt.Text) select rubro).FirstOrDefault();
                if (RubroLoaded != null)
                {
                    RubroLoaded.expected = Int32.Parse(expected.Text);
                    context.SubmitChanges();
                }
            }
        }

        private void Edit_Expected(object sender, RoutedEventArgs e)
        {
            if (expected.Text.Length != 0)
                NavigationService.Navigate(new Uri("/NewCycle.xaml", UriKind.RelativeOrAbsolute));
            else
            {
                using (Data context = new Data(App.DataconnectionString))
                {
                    String username = (from user in context.User select user.name).FirstOrDefault();
                    MessageBox.Show(username + ", The Field expected value is empty, please provide them.");
                }
            }
        }
    }
}