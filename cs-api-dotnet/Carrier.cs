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
    /// Carrier Object
    /// </summary>
    public class Carrier : Customizable
    {
        /// <summary>
        /// Carrier ID
        /// </summary>
        public string carrier_id { get; set; }

        /// <summary>
        /// Timestamp the carrier was created (unix epoch)
        /// </summary>
        public long created { get; set; }

        /// <summary>
        /// Name of carrier
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Whether the carrier is active or not
        /// </summary>
        public bool is_active { get; set; }

        /// <summary>
        /// URL to the logo of the carrier
        /// </summary>
        public string logo { get; set; }

        /// <summary>
        /// Homepage of carrier
        /// </summary>
        public string website { get; set; }


        /// <summary>
        /// Carrier Rep
        /// </summary>
        public string rep { get; set; }

        /// <summary>
        /// Any comments or special instructions for the carrier
        /// </summary>
        public string comments { get; set; }

        public CarrierBilling billing { get; set; }
        public Type type { get; set; }
        public Flags flags { get; set; }
        public CarrierIdentifiers identifiers { get; set; }
        public Address address { get; set; }
        public List<Insurance> insurances { get; set; }
        public List<CarrierContact> contacts { get; set; }
        public List<Terminal> terminals { get; set; }

        internal IRestClient restClient;

        public void Save()
        {
            var request = CreateRequest();

            restClient.Execute(request).HandleResponse();
        }

        public Task SaveAsync()
        {
            var request = CreateRequest();
            return restClient.ExecuteTaskAsync(request).ContinueWith(previousTask => previousTask.Result.HandleResponse());
        }

        private IRestRequest CreateRequest()
        {
            var request = new RestRequest
            {
                Method = Method.PUT,
                Resource = "api/carrier/" + carrier_id,
                RequestFormat = DataFormat.Json
            };

            var body = JsonConvert.SerializeObject(new { carrier = this }, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            request.AddParameter("application/json", body, ParameterType.RequestBody);
            return request;
        }

       
    }

    /// <summary>
    /// Carrier billing information
    /// </summary>
    public class CarrierBilling
    {
        /// <summary>
        /// Code to tie this carrier with your ERP/Accounting software
        /// </summary>
        public string accounting_code { get; set; }
        /// <summary>
        /// Payment Terms. This is a numeric field and can have one of the following options
        /// "00" => No Terms
        /// "99" => Prepaid
        /// "89" => Due on Receipt
        /// "02" => 2%, Net 15 Days
        /// "05" => Net 5 Days
        /// "07" => Net 7 Days
        /// "10" => Net 10 Days
        /// "15" => Net 15 Days
        /// "20" => Net 20 Days
        /// "30" => Net 30 Days
        /// "40" => Net 40 Days
        /// "45" => Net 45 Days
        /// "55" => Net 55 Days
        /// "60" => Net 60 Days
        /// "90" => Net 90 Days
        /// "20" => Net 120 Days
        /// </summary>
        public string payment_terms { get; set; }
    }

    /// <summary>
    /// Insurance certificate
    /// </summary>
    public class Insurance
    {
        /// <summary>
        /// Insurance type
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// Insurace company name
        /// </summary>
        public string company_name { get; set; }

        /// <summary>
        /// Company NAIC code
        /// </summary>
        public long company_naic { get; set; }

        /// <summary>
        /// Insurance Broker name
        /// </summary>
        public string broker_name { get; set; }

        /// <summary>
        /// Primary contact name
        /// </summary>
        public string contact_name { get; set; }

        /// <summary>
        /// Contact PHone
        /// </summary>
        public string contact_phone { get; set; }

        /// <summary>
        /// Contact Phone
        /// </summary>
        public string contact_fax { get; set; }

        /// <summary>
        /// Contact Email
        /// </summary>
        public string contact_email { get; set; }

        /// <summary>
        /// Boolean to denote if your company name is explicitly denoted on the insurance certificate
        /// </summary>
        public bool company_on_insurance { get; set; }

        /// <summary>
        /// Policy Number
        /// </summary>
        public string policy { get; set; }

        /// <summary>
        /// Coverage amount in USD
        /// </summary>
        public double coverage { get; set; }

        /// <summary>
        /// Timestamp of when the certificate will expire (unix epoch)
        /// </summary>
        public long expiration { get; set; }
    }

    /// <summary>
    /// Carrier contact
    /// </summary>
    public class CarrierContact
    {
        /// <summary>
        /// Type of contact. Can be one of the following options
        /// "dispatcher", "shipping", "receiving", "planner", "appointment_scheduler", 
        /// "general", "manager", "accounting", "sales", "claims", "safety", "after_hour_dispatch"
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// Full name of carrier contact
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Email of contact
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// Phone number (with extension)
        /// </summary>
        public string phone { get; set; }
        /// <summary>
        /// Fax Number
        /// </summary>
        public string fax { get; set; }
        /// <summary>
        /// Cell Phone Number
        /// </summary>
        public string mobile { get; set; }
        /// <summary>
        /// Notes for the contact
        /// </summary>
        public string notes { get; set; }
    }

    /// <summary>
    /// Cities in which the carrier has terminals in
    /// </summary>
    public class Terminal
    {
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
        /// <summary>
        /// Geo Location: [lon, lat]
        /// </summary>
        public List<double> loc { get; set; }
    }

    /// <summary>
    /// The differet modes the carrier currently services
    /// </summary>
    public class Type
    {
        /// <summary>
        /// Weather this carrier handles LTL shipments
        /// </summary>
        public bool ltl { get; set; }
        /// <summary>
        /// Weather this carrier handles Truckload shipments
        /// </summary>
        public bool truckload { get; set; }
        /// <summary>
        /// Weather this carrier handles Air Freight
        /// </summary>
        public bool air { get; set; }
        /// <summary>
        /// Weather this carrier is a broker/3PL
        /// </summary>
        public bool broker { get; set; }
        /// <summary>
        /// Weather this carrier handles intermodal freight
        /// </summary>
        public bool intermodal { get; set; }
        /// <summary>
        /// Weather this carrier is a freight forwarder (non-vessel operating common carrier)
        /// </summary>
        public bool nvocc { get; set; }
        /// <summary>
        /// Weather this carrier supports expedited shipments
        /// </summary>
        public bool expedited { get; set; }
        /// <summary>
        /// Weather this carrier has team drivers
        /// </summary>
        public bool teams { get; set; }
        /// <summary>
        /// Weather this carrier has White Glove services to handle sensitive shipments
        /// </summary>
        public bool whiteglove { get; set; }

    }

    /// <summary>
    /// Carrier flags
    /// </summary>
    public class Flags
    {
        /// <summary>
        /// Smartway approved carrier
        /// </summary>
        public bool smartway { get; set; }
        /// <summary>
        /// OOIDA (Owner Operator Independent Drivers Association) Member
        /// </summary>
        public bool ooida { get; set; }
        /// <summary>
        /// TIA (Transportation Intermediaries Association) Member
        /// </summary>
        public bool tia { get; set; }
        /// <summary>
        /// TCA (Truckload Carrier Association) Member 
        /// </summary>
        public bool tca { get; set; }
        /// <summary>
        /// Has drop trailer preveliges at walmart
        /// </summary>
        public bool walmart_drop_trailer { get; set; }
        /// <summary>
        /// Has certification to ship hazmat freight
        /// </summary>
        public bool hazmat { get; set; }
    }

    /// <summary>
    /// FMCSA Information
    /// </summary>
    public class Fmcsa
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long dot { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long mc { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long ff { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long mx { get; set; }
        public bool broker_authority { get; set; }
        public bool common_authority { get; set; }
        public bool contract_authority { get; set; }
    }

    /// <summary>
    /// Identifiers
    /// </summary>
    public class CarrierIdentifiers
    {
        public string scac { get; set; }
        public Fmcsa fmcsa { get; set; }
    }

}
