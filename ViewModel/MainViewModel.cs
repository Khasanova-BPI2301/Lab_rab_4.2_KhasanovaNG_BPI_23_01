using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Lab_rab_4._2_KhasanovaNG_BPI_23_01.Helper;
using Lab_rab_4._2_KhasanovaNG_BPI_23_01.View;

namespace Lab_rab_4._2_KhasanovaNG_BPI_23_01.ViewModel
{
    public class MainViewModel
    {
        public RelayCommand OpenEmployeesCommand { get; }
        public RelayCommand OpenRolesCommand { get; }

        public MainViewModel()
        {
            OpenEmployeesCommand = new RelayCommand(_ => OpenWindow(new WindowEmployee()));
            OpenRolesCommand = new RelayCommand(_ => OpenWindow(new WindowRole()));
        }

        private void OpenWindow(Window window)
        {
            window.ShowDialog(); 
        }
    }
}
