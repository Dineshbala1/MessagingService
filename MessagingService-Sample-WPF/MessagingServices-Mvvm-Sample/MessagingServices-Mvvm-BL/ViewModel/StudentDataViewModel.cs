using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MessagingServices_Mvvm_BL.Commands;
using MessagingServices_Mvvm_BL.Model;

namespace MessagingServices_Mvvm_BL.ViewModel
{
    public class StudentDataViewModel
    {
        public ICommand AddStudentDataCommand { get; set; }

        public StudentDataViewModel()
        {
            AddStudentDataCommand = new DelegateCommand(AddNewRecord);
        }

        private void AddNewRecord(object parameter)
        {
            var data = new StudentModel()
            {
                Name = "Random Student",
                Age = 10,
                FavouriteSubject = parameter?.ToString()
            };
            MessagingServiceExtension.MessagingService.PublishMessage(data, "NewStudent");
        }
    }
}
