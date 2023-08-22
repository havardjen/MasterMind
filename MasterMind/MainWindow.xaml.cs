using MasterMindResources.ViewModels;
using System.Net.Http;
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

			var baseUrl = "https://localhost:44351/api/MasterMind";
			var client = new HttpClient();

            _vm = new MainWindowViewModel(client, baseUrl);
			DataContext = _vm;
		}

		MainWindowViewModel _vm;
	}
}
