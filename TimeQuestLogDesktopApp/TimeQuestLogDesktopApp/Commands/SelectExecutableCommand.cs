using CredentialManagement;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeQuestLogDesktopApp.Services;
using TimeQuestLogDesktopApp.ViewModels;

namespace TimeQuestLogDesktopApp.Commands
{
	internal class SelectExecutableCommand : CommandBase
	{
		private AddGameViewModel _addGameViewModel;


        public SelectExecutableCommand(AddGameViewModel addGameViewModel)
        {
            _addGameViewModel = addGameViewModel;
        }

		public override void Execute(object? parameter)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();

			openFileDialog.Filter = "Executable Files (*.exe)|*.exe|All Files (*.*)|*.*";

			openFileDialog.Title = "Select an Executable File";
			openFileDialog.InitialDirectory = @"C:\Program Files (x86)\Steam\steamapps\common";
			openFileDialog.Multiselect = false;
			openFileDialog.ShowDialog();
			string selectedFileName = openFileDialog.SafeFileName;
			if (!string.IsNullOrEmpty(selectedFileName))
			{
				_addGameViewModel.ExeName = selectedFileName;
			}
		}
	}
}
