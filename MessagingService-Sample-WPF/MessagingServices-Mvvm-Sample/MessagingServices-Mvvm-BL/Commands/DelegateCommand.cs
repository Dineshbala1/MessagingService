using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MessagingServices_Mvvm_BL.Commands
{
    internal class DelegateCommand : ICommand
    {
        private Action<object> delegatAction;

        public DelegateCommand(Action<object> action)
        {
            delegatAction = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            delegatAction(parameter);
        }

        public event EventHandler CanExecuteChanged;
    }
}
