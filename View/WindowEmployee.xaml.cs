using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Lab_rab_4._2_KhasanovaNG_BPI_23_01.Helper;
using Lab_rab_4._2_KhasanovaNG_BPI_23_01.Model;
using Lab_rab_4._2_KhasanovaNG_BPI_23_01.ViewModel;

namespace Lab_rab_4._2_KhasanovaNG_BPI_23_01.View
{
    /// <summary>
    /// Логика взаимодействия для WindowEmployee.xaml
    /// </summary>
    public partial class WindowEmployee : Window
    {
        public WindowEmployee()
        {
            InitializeComponent();
            PersonViewModel vmPerson = new PersonViewModel();
            RoleViewModel vmRole = new RoleViewModel();

            List<Role> roles = new List<Role>();
            foreach (Role r in vmRole.ListRole)
            {
                roles.Add(r);
            }

            ObservableCollection<PersonDpo> persons = new ObservableCollection<PersonDpo>();
            FindRole finder;

            foreach (var p in vmPerson.ListPerson)
            {
                finder = new FindRole(p.RoleId);
                Role rol = roles.Find(new Predicate<Role>(finder.RolePredicate));
                persons.Add(new PersonDpo
                {
                    Id = p.Id,
                    RoleName = rol.NameRole,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Birthday = p.Birthday
                });
            }

            lvEmployee.ItemsSource = persons;
        }
    }
}
