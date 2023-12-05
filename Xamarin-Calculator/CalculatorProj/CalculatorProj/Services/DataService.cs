namespace CalculatorProj.Services
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using CalculatorProj.Model;
    using Newtonsoft.Json;

    public class DataService : IDataService
    {
        private const string _ApiKey = "fd475729fe46557c605001bb666014aa";
        private const string _ApiBaseAddress = "http://data.fixer.io/api/";
        public List<Currency> Items { get; private set; }
        private HttpClient _httpClient;
        private HttpClient HttpClient
        {
            get
            {
                if (this._httpClient == null)
                {
                    this._httpClient = new HttpClient
                    {
                        BaseAddress = new Uri(_ApiBaseAddress)
                    };

                   
                }
                return this._httpClient;
            }
        }


        public async Task<List<Currency>> GetCurrencies()
        {
            Items = new List<Currency>();
            
            try
            {
                var response = await HttpClient.GetAsync("symbols?access_key=" + _ApiKey);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    DataResponceRates deserializedResponce = new DataResponceRates();
                     deserializedResponce = JsonConvert.DeserializeObject<DataResponceRates>(content);
                    if (deserializedResponce.Symbols.Count > 0)
                    {
                        foreach( var item in deserializedResponce.Symbols)
                        {
                            Items.Add(new Currency(item.Key, item.Value));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"				ERROR {0}", ex.Message);
            }
            return Items;
        }        
        public async Task<string> GetConvertion(string from, string to, float ammount)
        {
           
            float converted=0;
            string getString = "latest?access_key=" + _ApiKey + "&symbols=" + from + "," + to + "&format=1";
            try
            {
                var response = await HttpClient.GetAsync(getString);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    DataResponceConvertion DataResponceConvertion = new DataResponceConvertion();
                    DataResponceConvertion = JsonConvert.DeserializeObject<DataResponceConvertion>(content);
                    string firstRateString = DataResponceConvertion.Rates[from];
                    string secondRateString = DataResponceConvertion.Rates[to];
                    converted = calcConvertion(firstRateString, secondRateString, ammount);

                }                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"				ERROR {0}", ex.Message);
            }
            return converted.ToString();
        }
        private float calcConvertion(string firstRateString, string secondRateString, float ammount)
        {
           
            float firstRate = float.Parse(firstRateString);
            
            float secondRate = float.Parse(secondRateString);
            float temp = ammount / firstRate;
            float converted = temp * secondRate;
            return converted;

        }
    }
}
