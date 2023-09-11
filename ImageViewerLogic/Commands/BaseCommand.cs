using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageViewerLogic.Commands
{
    public class BaseCommand : ICommand
    {
        private Action<object> _execute;
        private Func<object, bool> _canExecute;

        public BaseCommand(Action<object> executeDelegate, Func<object, bool> canExecuteDelegate)
        {
            _execute = executeDelegate;
            _canExecute = canExecuteDelegate;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute.Invoke(parameter);
        }

        public void Execute(object parameter)
        {
            _execute.Invoke(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
