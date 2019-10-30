
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MasterMindResources.ViewModels
{
	public class MainWindowViewModel : Notifier
	{
		public MainWindowViewModel()
		{
			_client = new HttpClient();
			_baseURL = "https://localhost:44351/api/MasterMind";

			//GetGameType(2);
			//CreateGameType("Viskelærbelte");
			GetValidCharacters();
		}

		private string _baseURL;
		HttpClient _client;
		List<string> _validCharacters;

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
	}
}
