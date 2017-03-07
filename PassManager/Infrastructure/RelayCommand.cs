using System;
using System.Windows.Input;

namespace PassManager.Infrastructure
{
	public class RelayCommand : ICommand
	{
		private readonly Action<object> _execute;
		private readonly Predicate<object> _canExecute;

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		#region Constructors

		public RelayCommand(Action<object> execute) : this(execute, null)
		{
			
		}

		public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
		{
			if(execute == null)
				throw new ArgumentNullException(nameof(execute));

			_execute = execute;
			_canExecute = canExecute;
		}
		#endregion

		public bool CanExecute(object parameter)
		{
			return _canExecute == null ? true : _canExecute.Invoke(parameter);
		}

		public void Execute(object parameter)
		{
			_execute.Invoke(parameter);
		}
	}
}
