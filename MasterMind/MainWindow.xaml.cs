using MasterMindResources.ViewModels;
using Microsoft.Extensions.Configuration;
using System.IO;
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

            var config = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.Build();

			var baseUrl = config["BaseUrl"];
            _vm = new MainWindowViewModel(new HttpClient(), baseUrl);
			DataContext = _vm;
		}

		MainWindowViewModel _vm;
	}
}
