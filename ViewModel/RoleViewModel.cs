using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

using Lab_rab_4._2_KhasanovaNG_BPI_23_01.Helper;
using Lab_rab_4._2_KhasanovaNG_BPI_23_01.Model;
using Lab_rab_4._2_KhasanovaNG_BPI_23_01.View;

namespace Lab_rab_4._2_KhasanovaNG_BPI_23_01.ViewModel
{
    public class RoleViewModel : INotifyPropertyChanged
    {
        private readonly string _roleDataPath = @"C:\\Users\\Нургиза\\source\\repos\\Lab_rab_4.2_KhasanovaNG_BPI_23_01\\Lab_rab_4.2_KhasanovaNG_BPI_23_01\\DataModels\\RoleData.json";
        private string _jsonRoles = string.Empty;
        public string Error { get; set; }

        private Role selectedRole;
        public Role SelectedRole
        {
            get => selectedRole;
            set
            {
                selectedRole = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Role> ListRole { get; set; } = new();

        public RoleViewModel()
        {
            ListRole = LoadRole();
        }

        public ObservableCollection<Role> LoadRole()
        {
            try
            {
                if (File.Exists(_roleDataPath))
                {
                    _jsonRoles = File.ReadAllText(_roleDataPath);
                    if (!string.IsNullOrEmpty(_jsonRoles))
                    {
                        var roles = JsonConvert.DeserializeObject<List<Role>>(_jsonRoles);
                        if (roles != null)
                        {
                            return new ObservableCollection<Role>(roles);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Error = "Ошибка загрузки JSON: " + ex.Message;
            }

            return InitializeDefaultRoles();
        }
        private ObservableCollection<Role> InitializeDefaultRoles()
        {
            var roles = new ObservableCollection<Role>();
            roles.Add(new Role { Id = 1, NameRole = "Директор" });
            roles.Add(new Role { Id = 2, NameRole = "Бухгалтер" });
            roles.Add(new Role { Id = 3, NameRole = "Менеджер" });
            return roles;
        }
        public int MaxId()
        {
            int max = 0;
            foreach (var r in this.ListRole)
            {
                if (max < r.Id) max = r.Id;
            }
            return max;
        }

        #region AddRole
        private RelayCommand addRole;
        public RelayCommand AddRole => addRole ??= new RelayCommand(obj =>
        {
            var wnRole = new WindowNewRole { Title = "Новая должность" };
            var role = new Role { Id = MaxId() + 1 };
            wnRole.DataContext = role;
            if (wnRole.ShowDialog() == true)
            {
                ListRole.Add(role);
                SelectedRole = role;
                SaveChanges(ListRole);
            }
        });
        #endregion

        #region EditRole
        private RelayCommand editRole;
        public RelayCommand EditRole => editRole ??= new RelayCommand(obj =>
        {
            var wnRole = new WindowNewRole { Title = "Редактирование должности" };
            var tempRole = SelectedRole.ShallowCopy();
            wnRole.DataContext = tempRole;
            if (wnRole.ShowDialog() == true)
            {
                SelectedRole.NameRole = tempRole.NameRole;
                SaveChanges(ListRole);
            }
        }, _ => SelectedRole != null);
        #endregion

        #region DeleteRole
        private RelayCommand deleteRole;
        public RelayCommand DeleteRole => deleteRole ??= new RelayCommand(obj =>
        {
            var result = MessageBox.Show($"Удалить данные по должности: {SelectedRole.NameRole}", "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                ListRole.Remove(SelectedRole);
                SelectedRole = null;
                SaveChanges(ListRole);
            }
        }, _ => SelectedRole != null);
        #endregion
        private void SaveChanges(ObservableCollection<Role> listRole)
        {
            try
            {
                var jsonRole = JsonConvert.SerializeObject(listRole, Formatting.Indented);
                Directory.CreateDirectory(Path.GetDirectoryName(_roleDataPath)!);
                File.WriteAllText(_roleDataPath, jsonRole);
            }
            catch (IOException e)
            {
                Error = "Ошибка записи json файла\n" + e.Message;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
