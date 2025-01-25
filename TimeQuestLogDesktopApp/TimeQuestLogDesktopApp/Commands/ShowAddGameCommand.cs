using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeQuestLogDesktopApp.ViewModels;
using TimeQuestLogDesktopApp.Views;

namespace TimeQuestLogDesktopApp.Commands
{
	internal class ShowAddGameCommand : CommandBase
	{
		public override void Execute(object? parameter)
		{
			AddGameView addGameView = new AddGameView
			{
				DataContext = new AddGameViewModel()
			};
			addGameView.ShowDialog();
		}
	}
}
