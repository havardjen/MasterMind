
using MasterMindResources.Commands;
using MasterMindResources.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Windows.Input;

namespace MasterMindResources.ViewModels
{
	public class MainWindowViewModel : Notifier
	{
		public MainWindowViewModel()
		{
			_client = new HttpClient();
			_baseURL = "https://localhost:44351/api/MasterMind";
			_gameId = -1;
			Attempts = new ObservableCollection<Attempt>();


			GetValidCharacters();
			CreateGameCommand = new GeneralNoParameterCommand(CreateGame);
			AttemptGameCommand = new GeneralNoParameterCommand(() =>
			{
				ValueOne = string.IsNullOrWhiteSpace(ValueOne) ? "A" : ValueOne.ToUpper();
				ValueTwo = string.IsNullOrWhiteSpace(ValueTwo) ? "A" : ValueTwo.ToUpper();
				ValueThree = string.IsNullOrWhiteSpace(ValueThree) ? "A" : ValueThree.ToUpper();
				ValueFour = string.IsNullOrWhiteSpace(ValueFour) ? "A" : ValueFour.ToUpper();
				OnPropertyChanged("ValueOne");
				OnPropertyChanged("ValueTwo");
				OnPropertyChanged("ValueThree");
				OnPropertyChanged("ValueFour");

				string completeAttempt = $"{_gameId}_{ValueOne}{ValueTwo}{ValueThree}{ValueFour}";
				AttemptGame(completeAttempt);
			});
		}

		private string _baseURL;
		private HttpClient _client;
		private List<string> _validCharacters;
		private int _gameId;
		private string _hints;
		private string _valueOne;

		#region Properties

		public string ValueOne 
		  { 
		   get { return _valueOne; }
		   set
			{
				_valueOne = value;
				OnPropertyChanged("ValueOne");
			}
		  }
		public string ValueTwo { get; set; }
		public string ValueThree { get; set; }
		public string ValueFour { get; set; }

		public bool CanGuessGame { get { return _gameId > 0; } }
		public ObservableCollection<Attempt> Attempts { get; set; }

		public ICommand CreateGameCommand { get; set; }
		public ICommand AttemptGameCommand { get; set; }

		#endregion


		public async void GetValidCharacters()
		{
			string getValidCharactersURL = $"{_baseURL}/ValidCharacters";
			HttpResponseMessage response = await _client.GetAsync(getValidCharactersURL);

			_validCharacters = new List<string>();
			if (response.IsSuccessStatusCode)
				_validCharacters = JsonConvert.DeserializeObject<List<string>>(await response.Content.ReadAsStringAsync());
		}

		public async void GetGameType(int gameTypeId)
		{
			string gameType = string.Empty;
			string getGameTypeURL = $"{_baseURL}/GameType/{gameTypeId}";
			HttpResponseMessage response = await _client.GetAsync(getGameTypeURL);

			if(response.IsSuccessStatusCode)
				gameType = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());
		}

		public async void CreateGameType(string gameType)
		{
			string getGameTypeURL = $"{_baseURL}/GameType/Create/{gameType}";

			StringContent queryString = new StringContent(gameType);
			HttpResponseMessage response = await _client.PostAsync(new Uri(getGameTypeURL), queryString);

			int gameTypeId = 0;
			if (response.IsSuccessStatusCode)
			{
				string responseBody = await response.Content.ReadAsStringAsync();
				gameTypeId = int.Parse(responseBody);
			}
		}

		public async void CreateGame()
		{
			string createGameURL = $"{_baseURL}/Game/Create";
			HttpResponseMessage response = await _client.GetAsync(createGameURL);

			if (response.IsSuccessStatusCode)
				_gameId = JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync());

			OnPropertyChanged("CanGuessGame");
		}

		public async void AttemptGame(string completeAttempt)
		{
			string attemptGameURL = $"{_baseURL}/Game/Attempt/{completeAttempt}";

			StringContent queryString = new StringContent(completeAttempt);
			HttpResponseMessage response = await _client.PostAsync(new Uri(attemptGameURL), queryString);

			_hints = string.Empty;
			if (response.IsSuccessStatusCode)
			{
				_hints = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());
			}

			Attempt newAttempt = new Attempt
			{
				ValueOne = ValueOne,
				ValueTwo = ValueTwo,
				ValueThree = ValueThree,
				ValueFour = ValueFour,
				Hints = _hints
			};

			Attempts.Add(newAttempt);
		}
	}
}
