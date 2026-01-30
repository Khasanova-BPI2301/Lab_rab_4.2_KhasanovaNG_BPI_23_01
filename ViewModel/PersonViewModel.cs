using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Lab_rab_4._2_KhasanovaNG_BPI_23_01.Helper;
using Lab_rab_4._2_KhasanovaNG_BPI_23_01.Model;
using Lab_rab_4._2_KhasanovaNG_BPI_23_01.View;

namespace Lab_rab_4._2_KhasanovaNG_BPI_23_01.ViewModel
{
    public class PersonViewModel : INotifyPropertyChanged
    {
        private PersonDpo selectedPersonDPO;
        public PersonDpo SelectedPersonDpo
        {
            get => selectedPersonDPO;
            set
            {
                selectedPersonDPO = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Person> ListPerson { get; set; } = new();
        public ObservableCollection<PersonDpo> ListPersonDPO { get; set; } = new();

        public PersonViewModel()
        {
            
            ListPerson.Add(new Person(1, 1, "Иван", "Иванов", new DateTime(1980, 2, 28)));
            ListPerson.Add(new Person(2, 2, "Петр", "Петров", new DateTime(1981, 3, 20)));
            ListPerson.Add(new Person(3, 3, "Виктор", "Викторов", new DateTime(1982, 4, 15)));
            ListPerson.Add(new Person(4, 3, "Сидор", "Сидоров", new DateTime(1983, 5, 10)));

            // Преобразуем в PersonDPO для отображения
            foreach (var p in ListPerson)
            {
                var dpo = new PersonDpo().CopyFromPerson(p);
                ListPersonDPO.Add(dpo);
            }
        }

        public int MaxId()
        {
            int max = 0;
            foreach (var p in ListPerson)
                if (p.Id > max) max = p.Id;
            return max;
        }

        #region AddPerson
        private RelayCommand addPerson;
        public RelayCommand AddPerson => addPerson ??= new RelayCommand(_ =>
        {
            var window = new WindowNewEmployee { Title = "Новый сотрудник" };
            var dpo = new PersonDpo
            {
                Id = MaxId() + 1,
                Birthday = DateTime.Today
            };
            window.DataContext = dpo;
            if (window.ShowDialog() == true)
            {
                // Получаем выбранную должность
                var selectedRole = (Model.Role)window.CbRole.SelectedItem;
                dpo.RoleName = selectedRole?.NameRole ?? "Неизвестно";
                ListPersonDPO.Add(dpo);

                // Добавляем в исходный список
                var person = new Person().CopyFromPersonDPO(dpo);
                ListPerson.Add(person);
            }
        });
        #endregion

        #region EditPerson
        private RelayCommand editPerson;
        public RelayCommand EditPerson => editPerson ??= new RelayCommand(_ =>
        {
            var window = new WindowNewEmployee { Title = "Редактирование" };
            var temp = SelectedPersonDpo.ShallowCopy();
            window.DataContext = temp;

            // Передаём текущий список должностей в окно
            window.SetRoles(new RoleViewModel().ListRole);

            if (window.ShowDialog() == true)
            {
                var selectedRole = (Model.Role)window.CbRole.SelectedItem;
                SelectedPersonDpo.FirstName = temp.FirstName;
                SelectedPersonDpo.LastName = temp.LastName;
                SelectedPersonDpo.Birthday = temp.Birthday;
                SelectedPersonDpo.RoleName = selectedRole?.NameRole ?? "Неизвестно";

                // Обновляем в исходном списке
                foreach (var p in ListPerson)
                {
                    if (p.Id == SelectedPersonDpo.Id)
                    {
                        p.CopyFromPersonDPO(SelectedPersonDpo);
                        break;
                    }
                }
            }
        }, _ => SelectedPersonDpo != null);
        #endregion

        #region DeletePerson
        private RelayCommand deletePerson;
        public RelayCommand DeletePerson => deletePerson ??= new RelayCommand(_ =>
        {
            var result = MessageBox.Show(
                $"Удалить сотрудника:\n{SelectedPersonDpo.LastName} {SelectedPersonDpo.FirstName}?",
                "Подтверждение",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                ListPersonDPO.Remove(SelectedPersonDpo);


                var personToRemove = ListPerson.FirstOrDefault(p => p.Id == SelectedPersonDpo.Id);
                if (personToRemove != null)
                {
                    ListPerson.Remove(personToRemove);
                }
            }
        }, _ => SelectedPersonDpo != null);
        #endregion
        public event PropertyChangedEventHandler PropertyChanged;

        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
