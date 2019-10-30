using System;
using System.Windows.Input;

namespace MasterMindResources.Commands
{
	public class GeneralNoParameterCommand : ICommand
	{
		public GeneralNoParameterCommand(Action handler)
		{
			this.handler = handler;
		}

		private Action handler;
		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			handler();
		}
	}

	public class GeneralParameterCommand : ICommand
	{
		public GeneralParameterCommand(Action<object> handler)
		{
			this.handler = handler;
		}

		private Action<object> handler;
		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			handler(parameter);
		}
	}
}
