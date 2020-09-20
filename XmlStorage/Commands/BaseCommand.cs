using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace XmlStorage.Commands
{
    class BaseCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action<object> _Method;

        public BaseCommand(Action<object> m)
        {
            _Method = m;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _Method.Invoke(parameter);
        }
    }
}
