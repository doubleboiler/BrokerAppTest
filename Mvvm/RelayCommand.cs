using Prism.Commands;
using System;
using System.Windows.Input;

namespace BrokerAppTest.Mvvm
{
    class RelayCommand : DelegateCommand
    {
        public RelayCommand(Action executeMethode) : base(executeMethode)
        {

        }

        public RelayCommand(Action executeMethode, Func<bool> canExecuteMethode) : base(executeMethode, canExecuteMethode)
        {

        }

        public override event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
