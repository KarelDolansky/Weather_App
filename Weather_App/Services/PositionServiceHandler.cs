﻿namespace Weather_App.Services
{
    public interface IPositionServiceHandler
    {
        Task<PositionData> CallApi(string position);
    }
    public class PositionServiceHandler : IPositionServiceHandler
    {
        private readonly HttpClient _httpClient;
        private readonly IPositionDataTransformations _positionDataTransformations;
        public PositionServiceHandler(HttpClient httpClient, IPositionDataTransformations positionDataTransformations)
        {
            _httpClient = httpClient;
            _positionDataTransformations = positionDataTransformations;
        }

        public async Task<PositionData> CallApi(string position)
        {
            string url = $"https://geocoding-api.open-meteo.com/v1/search?name={position}&count=1&language=en&format=json";
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();

                return _positionDataTransformations.JsonToPositionData(jsonString);
            }
            else
            {
                throw new ExceptionApiCall("Api call failed " + response.StatusCode);
            }
        }
    }
}
