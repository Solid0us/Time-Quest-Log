using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TimeQuestLogDesktopApp.Services
{
	internal class HttpService
	{
		private readonly HttpClient _httpClient;
		private readonly int HTTP_TIMEOUT = 30;
		private readonly JsonSerializerSettings _jsonSerializerSettings;

		public HttpService()
		{
			_httpClient = new HttpClient
			{
				Timeout = TimeSpan.FromSeconds(HTTP_TIMEOUT),
			};
			_jsonSerializerSettings = new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver()
			};

		}

		public async Task<HttpResponseMessage> GetAsync(string url)
		{
			try
			{
				var response = await _httpClient.GetAsync(url);
				return response;
			}
			catch (Exception ex)
			{
				throw new HttpRequestException($"Error making GET request to {url}: {ex.Message}", ex);
			}
		}

		public async Task<HttpResponseMessage> PostAsync<T>(string url, T payload)
		{
			try
			{
				string jsonPayload = JsonConvert.SerializeObject(payload, _jsonSerializerSettings);
				var jsonContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
				var response = await _httpClient.PostAsync(url, jsonContent);
				return response;
			}
			catch (Exception ex)
			{
				throw new HttpRequestException($"Error making POST request to {url}: {ex.Message}", ex);
			}
		}

		public async Task<HttpResponseMessage> PutAsync<T>(string url, T payload)
		{
			try
			{
				string jsonPayload = JsonConvert.SerializeObject(payload, _jsonSerializerSettings);
				var jsonContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
				var response = await _httpClient.PutAsync(url, jsonContent);
				return response;
			}
			catch (Exception ex)
			{
				throw new HttpRequestException($"Error making PUT request to {url}: {ex.Message}", ex);
			}
		}

		public async Task<HttpResponseMessage> DeleteAsync(string url)
		{
			try
			{
				var response = await _httpClient.DeleteAsync(url);
				return response;
			}
			catch (Exception ex)
			{
				throw new HttpRequestException($"Error making DELETE request to {url}: {ex.Message}", ex);
			}
		}

		public void Dispose()
		{
			_httpClient?.Dispose();
		}
	}
}
