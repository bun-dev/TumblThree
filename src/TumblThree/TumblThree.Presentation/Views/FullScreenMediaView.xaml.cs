﻿using System;
using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Input;

using TumblThree.Applications.ViewModels;
using TumblThree.Applications.Views;

namespace TumblThree.Presentation.Views
{
    /// <summary>
    ///     Interaction logic for FullScreenMediaView.xaml
    /// </summary>
    [Export(typeof(IFullScreenMediaView))][PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class FullScreenMediaView : Window, IFullScreenMediaView
    {
        private readonly Lazy<FullScreenMediaViewModel> viewModel;

        public FullScreenMediaView()
        {
            InitializeComponent();
            viewModel = new Lazy<FullScreenMediaViewModel>(() => ViewHelper.GetViewModel<FullScreenMediaViewModel>(this));
            PreviewKeyDown += new KeyEventHandler(HandleEsc);
        }

        private FullScreenMediaViewModel ViewModel
        {
            get { return viewModel.Value; }
        }

        public void ShowDialog(object owner)
        {
            Owner = owner as Window;
            ShowDialog();
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
	            Close();
            }
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
    }
}
