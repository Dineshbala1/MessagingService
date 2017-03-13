using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagingServiceExtension;
using MessagingServices_Mvvm_BL.Model;

namespace MessagingServices_Mvvm_BL.ViewModel
{
    public class StudentViewModel : INotifyPropertyChanged
    {
        //private IList<StudentModel>
        public IList<StudentModel> StudentModelCollection
        {
            get;
            set;
        }

        public StudentViewModel()
        {
            StudentModelCollection = new ObservableCollection<StudentModel>();
            MessagingService.SubscribeToMessage<StudentModel>(this, "NewStudent", (Sender) =>
            {
                StudentModelCollection.Add(Sender);
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
