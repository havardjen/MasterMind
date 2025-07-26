
using MasterMindResources.Commands;
using MasterMindResources.Models;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Windows.Input;

namespace MasterMindResources.ViewModels
{
	public class MainWindowViewModel : Notifier
	{
		public MainWindowViewModel(HttpClient client, string baseUrl)
		{
			_client = client;
			_baseURL = baseUrl;
			_gameId = -1;
			Attempts = new ObservableCollection<Attempt>();

			GetValidCharacters();
			CreateGameCommand = new GeneralNoParameterCommand(CreateGame);
			AttemptGameCommand = new GeneralNoParameterCommand(() =>
			{
				ValueOne	= string.IsNullOrWhiteSpace(ValueOne)	? string.Empty : ValueOne;
                ValueTwo	= string.IsNullOrWhiteSpace(ValueTwo)	? string.Empty : ValueTwo;
                ValueThree	= string.IsNullOrWhiteSpace(ValueThree)	? string.Empty : ValueThree;
                ValueFour	= string.IsNullOrWhiteSpace(ValueFour)	? string.Empty : ValueFour;
                OnPropertyChanged("ValueOne");
				OnPropertyChanged("ValueTwo");
				OnPropertyChanged("ValueThree");
				OnPropertyChanged("ValueFour");

				AttemptGame();
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

        public int GameId { get { return _gameId; } }

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

		public async void CreateGame()
		{
			string createGameURL = $"{_baseURL}/Game/Create";
			HttpResponseMessage response = await _client.GetAsync(createGameURL);

			if (response.IsSuccessStatusCode)
				_gameId = JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync());

			OnPropertyChanged(nameof(CanGuessGame));
			OnPropertyChanged(nameof(GameId));
        }

        public async void AttemptGame()
        {
            var attempt = new List<string> { ValueOne, ValueTwo, ValueThree, ValueFour };

            var request = new AttemptRequest
            {
                GameId = _gameId,
                Attempt = attempt
            };

            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(new Uri(_baseURL + "/game/attempt"), content);

            _hints = string.Empty;
            if (response.IsSuccessStatusCode)
                _hints = await response.Content.ReadAsStringAsync();

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
