using MasterMindResources.ViewModels;
using System.Windows;

namespace MasterMind
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			_vm = new MainWindowViewModel();
			DataContext = _vm;
		}

		MainWindowViewModel _vm;
	}
}
