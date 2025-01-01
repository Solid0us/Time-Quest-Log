using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TimeQuestLogDesktopApp.Commands
{
	internal abstract class CommandBase : ICommand
	{
		public event EventHandler? CanExecuteChanged;

		public bool CanExecute(object? parameter)
		{
			return true;
		}

		public abstract void Execute(object? parameter);

		protected void OnCanExecuteChanged(object? parameter)
		{
			CanExecuteChanged?.Invoke(this, new EventArgs());
		}
	}
}
