using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using TimeQuestLogDesktopApp.Commands;
using TimeQuestLogDesktopApp.Services;

namespace TimeQuestLogDesktopApp.ViewModels
{
	public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
	{
		public event PropertyChangedEventHandler? PropertyChanged;
		private bool _disposed = false;

		public virtual void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					DisposeManagedResources();
				}

				DisposeUnmanagedResources();

				_disposed = true;
			}
		}

		protected virtual void DisposeManagedResources() { }

		protected virtual void DisposeUnmanagedResources() { }


		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		~ViewModelBase()
		{
			Dispose(false);
		}
	}
}
