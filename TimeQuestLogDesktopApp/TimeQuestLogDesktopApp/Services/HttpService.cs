using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Reflection.PortableExecutable;
using System.Net.Http.Headers;
using System.Net;
using TimeQuestLogDesktopApp.Models.DTOs;

namespace TimeQuestLogDesktopApp.Services
{
	internal class HttpService
	{
		private static readonly HttpService _httpService = new HttpService();
		private static CredentialManagerService _credentialService;
		private readonly HttpClient _httpClient;
		private readonly int HTTP_TIMEOUT = 30;
		private readonly JsonSerializerSettings _jsonSerializerSettings;
		private EnvironmentVariableService EnvironmentVariableService;

		private HttpService()
		{
			_httpClient = new HttpClient
			{
				Timeout = TimeSpan.FromSeconds(HTTP_TIMEOUT),
			};
			_jsonSerializerSettings = new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver()
			};
			_credentialService = CredentialManagerService.GetInstance();
			_credentialService.LoadCredentials();
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _credentialService.GetPassword(CredentialManagerService.CredentialType.JWT));
			EnvironmentVariableService = new EnvironmentVariableService();
		}

		public static HttpService GetInstance()
		{
			return _httpService;
		}

		public void SetHeaders(Dictionary<string, string> headers)
		{
			_httpClient.DefaultRequestHeaders.Clear();
			foreach (var header in headers)
			{
				_httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
			}

		}

		public async Task<HttpResponseMessage> GetAsync(string url, Dictionary<string, string>? headers = null)
		{
			using var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
			AddHeadersToRequest(requestMessage, headers);

			return await _httpClient.SendAsync(requestMessage);
		}

		public async Task<HttpResponseMessage> PostAsync<T>(string url, T payload, Dictionary<string, string>? headers = null)
		{
			string jsonPayload = JsonConvert.SerializeObject(payload, _jsonSerializerSettings);
			var jsonContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

			using var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
			{
				Content = jsonContent
			};

			AddHeadersToRequest(requestMessage, headers);

			return await _httpClient.SendAsync(requestMessage);
		}

		public async Task<HttpResponseMessage> PutAsync<T>(string url, T payload, Dictionary<string, string>? headers = null)
		{

			string jsonPayload = JsonConvert.SerializeObject(payload, _jsonSerializerSettings);
			var jsonContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
			using var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
			{
				Content = jsonContent
			};
			AddHeadersToRequest(requestMessage, headers);

			return await _httpClient.SendAsync(requestMessage);
		}

		public async Task<HttpResponseMessage> DeleteAsync(string url, Dictionary<string, string>? headers = null)
		{
			using var requestMessage = new HttpRequestMessage(HttpMethod.Delete, url);
			AddHeadersToRequest(requestMessage, headers);

			return await _httpClient.SendAsync(requestMessage);
		}

		private void AddHeadersToRequest(HttpRequestMessage requestMessage, Dictionary<string, string>? headers)
		{
			if (headers != null)
			{
				foreach (var header in headers)
				{
					requestMessage.Headers.Add(header.Key, header.Value);
				}
			}
		}

		public async Task<HttpResponseMessage> SendAndRepeatAuthorization(Task<HttpResponseMessage> httpRequest)
		{
			HttpResponseMessage response = await httpRequest;
			if (response.StatusCode == HttpStatusCode.Unauthorized)
			{
				string url = $"{EnvironmentVariableService.ApiBaseUrl}users/refresh";
				RefreshTokenRequest refreshTokenRequest = new RefreshTokenRequest
				{
					refreshToken = _credentialService.GetPassword(CredentialManagerService.CredentialType.REFRESH),
					username = _credentialService.GetUsername(CredentialManagerService.CredentialType.REFRESH),
				};
				HttpResponseMessage refreshTokenResponse = await PostAsync(url, refreshTokenRequest);
				if (refreshTokenResponse.IsSuccessStatusCode)
				{
					string message = await refreshTokenResponse.Content.ReadAsStringAsync();
					RefreshTokenRequest json = JsonConvert.DeserializeObject<RefreshTokenRequest>(message);
					_credentialService.SetPassword(CredentialManagerService.CredentialType.JWT, json.refreshToken);
					_credentialService.LoadCredentials();
					response = await httpRequest;
				}
			}

			return response;
		}

		public void Dispose()
		{
			_httpClient?.Dispose();
		}
	}
}
