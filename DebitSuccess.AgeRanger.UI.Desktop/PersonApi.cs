using DebitSuccess.AgeRanger.Api;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;


using System.Text;
using System.Threading.Tasks;

namespace DebitSuccess.AgeRanger.UI.Desktop {
    internal class PersonApi {

        private string _personApiBaseAddress;
        private ILogger _logger;
        private HttpClient _httpClient;

        public PersonApi() {

            _logger = ContainerFactory.ResolveInstance<ILogger>();

            _personApiBaseAddress = ConfigurationManager.AppSettings["PersonApiBaseAddress"];
             
            InitialiseHttpClient();

        }

        private void InitialiseHttpClient() {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_personApiBaseAddress);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
        }

        public async Task Save(IPersonEntity person) {
           

            var data = JsonConvert.SerializeObject(person);

            HttpContent httpContent = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response;

            // Update or Create?
            if (person.ID > 0) {
                response = await _httpClient.PutAsync("api/Person", httpContent);
            } else {
                response = await _httpClient.PostAsync("api/Person", httpContent);
            }

            if (response.IsSuccessStatusCode) {
                _logger.Write($"Person ID {person.ID} was successfully updated", enLogType.Info);
            } else {
                _logger.Write($"Person ID {person.ID} failed to be updated: {response.ReasonPhrase}", enLogType.Error);
            }


        }

        public async Task<IEnumerable<IPersonEntity>> List(IPeopleFilter filter = null) {

            IEnumerable<IPersonEntity> people = new List<IPersonEntity>();

            var uriAppendix = $"api/Person{GetFilterQuery(filter)}";

            HttpResponseMessage response = await _httpClient.GetAsync(uriAppendix);
            
            if (response.IsSuccessStatusCode) {
                var data = await response.Content.ReadAsStringAsync();

                // var settings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };
                people = JsonConvert.DeserializeObject<List<PersonEntity>>(data);

            } else {
                _logger.Write($"Failed to GET People from the Web Api Service at {_personApiBaseAddress}", enLogType.Error);
            }

            return people;
           
        }

        private string GetFilterQuery(IPeopleFilter filter) {

           

            if (filter == null) return string.Empty;

            var query = string.Empty;

            if (!string.IsNullOrEmpty(filter.FirstName)) { 
                query += $"FirstName={filter.FirstName}&";
            }

            if (!string.IsNullOrEmpty(filter.LastName)) {
                query += $"LastName={filter.LastName}&";
            }

            if (filter.MinAge.HasValue) {
                query += $"MinAge={filter.MinAge}&";
            }

            if (filter.MaxAge.HasValue) {
                query += $"MaxAge={filter.MaxAge}&";
            }

            query = query.TrimEnd('&');

            if (!string.IsNullOrEmpty(query)) {
                query = $"?{query}";
            }

            return query;

        }

    }
}
