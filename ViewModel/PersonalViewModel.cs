using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module05Exercise01.Model;
using Module05Exercise01.Service;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Module05Exercise01.ViewModel
{
    public class PersonalViewModel : INotifyPropertyChanged
    {
        private readonly PersonalService _personalService;
        public ObservableCollection<Personal> PersonalList { get; set; }

        private bool _isBusy;
        public bool isBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(); }
        }

        private Personal _selectedEmployee;
        public Personal SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                if (_selectedEmployee != null)
                {
                    NewEmployeeName = _selectedEmployee.Name;
                    NewEmployeeEmail = _selectedEmployee.Email;
                    NewEmployeeAddress = _selectedEmployee.Address;
                    NewEmployeeContactNo = _selectedEmployee.ContactNo;
                    IsEmployeeSelected = true;
                }
                else
                {
                    IsEmployeeSelected = false;
                }
                OnPropertyChanged();
            }
        }

        private bool _isEmployeeSelected;
        public bool IsEmployeeSelected
        {
            get => _isEmployeeSelected;
            set
            {
                _isEmployeeSelected = value;
                OnPropertyChanged();
            }
        }

        private string _statusMessage;
        public string statusMessage
        {
            get => _statusMessage;
            set { _statusMessage = value; OnPropertyChanged(); }
        }

        private string _newEmployeeName;
        public string NewEmployeeName
        {
            get => _newEmployeeName;
            set
            {
                _newEmployeeName = value;
                OnPropertyChanged();
            }
        }

        private string _newEmployeeEmail;
        public string NewEmployeeEmail
        {
            get => _newEmployeeEmail;
            set
            {
                _newEmployeeEmail = value;
                OnPropertyChanged();
            }
        }

        private string _newEmployeeAddress;
        public string NewEmployeeAddress
        {
            get => _newEmployeeAddress;
            set
            {
                _newEmployeeAddress = value;
                OnPropertyChanged();
            }
        }

        private string _newEmployeeContactNo;
        public string NewEmployeeContactNo
        {
            get => _newEmployeeContactNo;
            set
            {
                _newEmployeeContactNo = value;
                OnPropertyChanged();
            }

        }

        public ICommand LoadDataCommand { get; }
        public ICommand InsertEmployeeCommand { get; }
        public ICommand SelectedEmployeeCommand { get; }
        public ICommand DeleteEmployeeCommand { get; }
        public PersonalViewModel()
        {
            _personalService = new PersonalService();
            PersonalList = new ObservableCollection<Personal>();
            LoadDataCommand = new Command(async () => await LoadData());
            LoadDataCommand = new Command(async () => await LoadData());
            InsertEmployeeCommand = new Command(async () => await InsertEmployee());
            SelectedEmployeeCommand = new Command<Personal>(person => SelectedEmployee = person);
            DeleteEmployeeCommand = new Command(async () => await DeleteEmployee(), () => SelectedEmployee != null);

            LoadData();
        }

        public async Task LoadData()
        {
            if (isBusy) return;
            isBusy = true;
            statusMessage = "Loading employee data...";
            try
            {
                var personals = await _personalService.GetAllPersonalsAsync();
                PersonalList.Clear();
                foreach (var personal in personals)
                {
                    PersonalList.Add(personal);
                }
                statusMessage = "Data Load Successfully";
            }
            catch (Exception ex)
            {
                statusMessage = $"Failed to Load data:{ex.Message}";
            }
            finally
            {
                isBusy = false;
            }
        }

        private async Task InsertEmployee()
        {
            if (isBusy || string.IsNullOrWhiteSpace(_newEmployeeName) || string.IsNullOrWhiteSpace(_newEmployeeAddress) || string.IsNullOrWhiteSpace(_newEmployeeEmail) ||string.IsNullOrWhiteSpace(_newEmployeeContactNo))
            {
                statusMessage = "Please fill in all the fields before adding";
                return;
            }
            isBusy = true;
            statusMessage = "Adding new employee...";

            try
            {
                var newPerson = new Personal
                {
                    Name = NewEmployeeName,
                    Email = NewEmployeeEmail,
                    Address = NewEmployeeAddress,
                    ContactNo = NewEmployeeContactNo
                };
                var isSuccess = await _personalService.InsertEmployeeAsync(newPerson);
                if (isSuccess)
                {
                    NewEmployeeName = string.Empty;
                    NewEmployeeEmail = string.Empty;
                    NewEmployeeAddress = string.Empty;
                    NewEmployeeContactNo = string.Empty;
                    statusMessage = "New Employee added successfully!";
                }
                else
                {
                    statusMessage = "Failed to add new Employee...";
                }
            }
            catch (Exception ex)
            {
                statusMessage = $"Failed to add new Employee: {ex.Message}";
            }

            finally
            {
                isBusy = false;
                await LoadData();
            }
        }

        private async Task DeleteEmployee ()
        {
            if (SelectedEmployee == null) return;
            var answer = await Application.Current.MainPage.DisplayAlert("Confirm Delete", $"Are you sure you want to delete {SelectedEmployee.Name}?", "Yes", "No");

            if (!answer) return;
            isBusy = true;
            statusMessage = "Deleting employee...";

            try
            {
                var success = await _personalService.DeleteEmployeeAsync(SelectedEmployee.Id);
                statusMessage = success ? "Employee has been deleted successfully!" : "Failed to delete Employee record...";

                if (success)
                {
                    PersonalList.Remove(SelectedEmployee);
                    SelectedEmployee = null;
                }
            }
            catch (Exception ex)
            {
                statusMessage = $"Error deleting person: {ex.Message}";

            }
            finally
            {
                isBusy = false;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}

