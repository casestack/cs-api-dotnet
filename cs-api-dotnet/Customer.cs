using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace cs_api_dotnet
{
    /// <summary>
    /// Customer Object
    /// </summary>
    public class Customer : Customizable
    {
        /// <summary>
        /// Customer ID
        /// </summary>
        public string customer_id { get; set; }

        /// <summary>
        /// Timestamp the customer was created (unix epoch)
        /// </summary>
        public long created { get; set; }

        /// <summary>
        /// Name of customer
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Whether the customer is active or not
        /// </summary>
        public bool is_active { get; set; }

        /// <summary>
        /// URL to logo of customer
        /// </summary>
        public string logo { get; set; }

        /// <summary>
        /// Customer Rep
        /// </summary>
        public string rep { get; set; }

        /// <summary>
        /// Any comments or special instructions for the customer
        /// </summary>
        public string comments { get; set; }

        public CustomerBilling billing { get; set; }
        public CustomerIdentifiers identifiers { get; set; }
        public List<CustomerContact> contacts { get; set; }
        public Address address { get; set; }

        internal IRestClient restClient;

        public void Save()
        {
            var request = CreateRequest();

            restClient.Execute(request).HandleResponse();

        }


        public Task SaveAsync()
        {
            var request = CreateRequest();
            return
                restClient.ExecuteTaskAsync(request).ContinueWith(previousTask => previousTask.Result.HandleResponse());
        }

        private RestRequest CreateRequest()
        {
            var request = new RestRequest
            {
                Method = Method.PUT,
                Resource = "api/customer/" + customer_id,
                RequestFormat = DataFormat.Json
            };

            var body = JsonConvert.SerializeObject(new {customer = this}, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            request.AddParameter("application/json", body, ParameterType.RequestBody);
            return request;
        }

    }

    /// <summary>
    /// Customer billing info
    /// </summary>
    public class CustomerBilling
    {
        public string accounting_code { get; set; }
        public string payment_terms { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double credit_limit { get; set; }
    }

    /// <summary>
    /// Customer contact
    /// </summary>
    public class CustomerContact
    {
        public string type { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string fax { get; set; }
    }

    /// <summary>
    /// Customer company identifiers
    /// </summary>
    public class CustomerIdentifiers
    {
        /// <summary>
        /// Employer identification number (Tax ID)
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int ein { get; set; }

        /// <summary>
        /// Dun & Bradstreet Identification number
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int duns { get; set; }

        /// <summary>
        /// Industry
        /// </summary>
        public string industry { get; set; }

        /// <summary>
        /// Customer Type
        /// </summary>
        public string company_type { get; set; }
    }
}
