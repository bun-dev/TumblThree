﻿using System;
using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Windows;

using TumblThree.Applications.ViewModels;
using TumblThree.Applications.Views;

namespace TumblThree.Presentation.Views
{
    /// <summary>
    ///     Interaction logic for SettingsView.xaml
    /// </summary>
    [Export(typeof(ISettingsView))][PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class SettingsView : Window, ISettingsView
    {
        private readonly Lazy<SettingsViewModel> viewModel;

        public SettingsView()
        {
            InitializeComponent();
            viewModel = new Lazy<SettingsViewModel>(() => ViewHelper.GetViewModel<SettingsViewModel>(this));
        }

        private SettingsViewModel ViewModel
        {
            get { return viewModel.Value; }
        }

        public void ShowDialog(object owner)
        {
            Owner = owner as Window;
            ShowDialog();
        }

        private void closeWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }

		private void CheckBox_Checked(object sender, RoutedEventArgs e)
		{

		}
	}
}
