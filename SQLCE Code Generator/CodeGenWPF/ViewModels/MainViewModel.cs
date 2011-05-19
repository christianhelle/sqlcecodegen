using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using CodeGenWPF.Views;

namespace CodeGenWPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
        }

        public void ShowAboutBox()
        {
            var about = new AboutBox(null);
            about.ShowDialog();
        }
    }
}
