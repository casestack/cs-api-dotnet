using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.Web;
using RestSharp;
using RestSharp.Authenticators;

namespace cs_api_dotnet
{
    /// <summary>
    /// API Access into CaseStack Supply Chain Management Suite.
    /// For more infor please visit http://docs.casestack.io/
    /// </summary>
    public class CaseStackApi
    {
        private string _apiKey = "";
        private string _companyId = "";
        private string _apiEndpoint = "https://app.casestack.io";
        private string _apiVersion = "1.0.0";

        /// <summary>
        /// Creates and instance of CaseStackApi
        /// </summary>
        /// <param name="useStagingEndpoint">Set to true if you want to connect to casestack staging api</param>
        public CaseStackApi(bool useStagingEndpoint = false)
        {
            if (useStagingEndpoint == true)
            {
                _apiEndpoint = "https://staging.casestack.io";
            }
        }

        /// <summary>
        /// Available shipment statuses
        /// </summary>
        public enum ShipmentStatus
        {
            [DescriptionAttribute("Broker Approval Pending")] BrokerApprovalPending,

            [DescriptionAttribute("Quote Pending")] QuotePending,

            [DescriptionAttribute("Customer Approval Pending")] CustomerApprovalPending,

            [DescriptionAttribute("Customer Rejected")] CustomerRejected,

            [DescriptionAttribute("Ready to Tender")] ReadytoTender,

            [DescriptionAttribute("Tendered")] Tendered,

            [DescriptionAttribute("Tender Accepted by Carrier")] TenderAcceptedbyCarrier,

            [DescriptionAttribute("Tender Accepted by Rep")] TenderAcceptedbyRep,

            [DescriptionAttribute("Tender Rejected by Carrier")] TenderRejectedbyCarrier,

            [DescriptionAttribute("Tender Rejected by Rep")] TenderRejectedbyRep,

            [DescriptionAttribute("Pickup Appointment Scheduled")] PickupAppointmentScheduled,

            [DescriptionAttribute("Arrived at Pickup Location")] ArrivedatPickupLocation,

            [DescriptionAttribute("Picked Up")] PickedUp,

            [DescriptionAttribute("In Transit")] InTransit,

            [DescriptionAttribute("Delivery Appointment Scheduled")] DeliveryAppointmentScheduled,

            [DescriptionAttribute("Arrived at Delivery Location")] ArrivedatDeliveryLocation,

            [DescriptionAttribute("Out for Delivery")] OutforDelivery,

            [DescriptionAttribute("Delivered")] Delivered,

            [DescriptionAttribute("Delivery Exception")] DeliveryException,

            [DescriptionAttribute("Cancelled")] Cancelled,

            [DescriptionAttribute("Billable")] Billable,

            [DescriptionAttribute("Invoiced")] Invoiced,

            [DescriptionAttribute("Archived")] Archived,
        }

        public string ApiEndpoint { get { return _apiEndpoint; }}

        /// <summary>
        /// Authenticate API Access. Your credentials are available under the 'Settings > CaseStack API'.
        /// </summary>
        /// <param name="key">Your API Key</param>
        /// <param name="companyId">Your Company ID</param>
        public void Authenticate(String key, String companyId)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            if(string.IsNullOrEmpty(companyId))
                throw new ArgumentNullException("companyId");

            _apiKey = key;
            _companyId = companyId;
        }

      

        protected virtual IRestClient GetRestClient()
        {
            var client = new RestClient
            {
                BaseUrl = new Uri(_apiEndpoint),
                Authenticator = new HttpBasicAuthenticator(_companyId, _apiKey)
            };
            client.AddDefaultHeader("Accept-Version", _apiVersion);
            return client;
        }

        /// <summary>
        /// Get Carrier by ID
        /// </summary>
        /// <param name="carrierId">Carrier ID</param>
        /// <returns>Carrier Object</returns>
        public Carrier GetCarrier(string carrierId)
        {
            var request = CreateRestRequest<Carrier>(carrierId);

            var client = GetRestClient();

            var response = client.Execute<Carrier>(request).HandleResponse();
       
            response.Data.restClient = client;
            return response.Data;
        }

        /// <summary>
        /// Get Carrier by ID
        /// </summary>
        /// <param name="carrierId">Carrier ID</param>
        /// <returns>Task for get Carrier Object</returns>
        public Task<Carrier> GetCarrierAsync(string carrierId)
        {
            var request = CreateRestRequest<Carrier>(carrierId);

            var client = GetRestClient();

            var response = client.ExecuteTaskAsync<Carrier>(request).ContinueWith(
                previoustask =>
                {
                    var data = previoustask.Result.HandleResponse().Data;
                    previoustask.Result.Data.restClient = client;
                    return data;
                });

            return response;
        }

        /// <summary>
        /// Get Custom Fields for an object type
        /// </summary>
        /// <typeparam name="T">Classs must be of type Customizable</typeparam>
        /// <returns>Custom Fields Object</returns>
        public CustomFields GetCustomFields<T>() where T : Customizable
        {
            var parent = typeof (T).Name;
            var client = GetRestClient();
            var request = new RestRequest
            {
                Resource = "api/customfield/" + parent.ToLower(),
                RequestFormat = DataFormat.Json,
                RootElement = parent
            };

            return client.Execute<CustomFields>(request).HandleResponse().Data;
        }

        /// <summary>
        /// Get Custom Fields for an object type
        /// </summary>
        /// <typeparam name="T">Classs must be of type Customizable</typeparam>
        /// <returns>Custom Fields Object</returns>
        public Task<CustomFields> GetCustomFieldsAsync<T>() where T : Customizable
        {
            var parent = typeof(T).Name;
            var client = GetRestClient();
            var request = new RestRequest
            {
                Resource = "api/customfield/" + parent.ToLower(),
                RequestFormat = DataFormat.Json,
                RootElement = parent
            };

            return client.ExecuteTaskAsync<CustomFields>(request).ContinueWith(previousTask => previousTask.Result.HandleResponse().Data);

            
        }

        /// <summary>
        /// Get Customer by ID
        /// </summary>
        /// <param name="customerId">Customer ID</param>
        /// <returns>Customer Object</returns>
        public Customer GetCustomer(string customerId)
        {
            var request = CreateRestRequest<Customer>(customerId);

            var client = GetRestClient();

            var response = client.Execute<Customer>(request).HandleResponse();

            response.Data.restClient = client;
            return response.Data;
        }

        /// <summary>
        /// Get Customer by ID
        /// </summary>
        /// <param name="customerId">Customer ID</param>
        /// <returns>Task Customer Object</returns>
        public Task<Customer> GetCustomerAsync(string customerId)
        {
            var request = CreateRestRequest<Customer>(customerId);

            var client = GetRestClient();

            var responseTask = client.ExecuteTaskAsync<Customer>(request).ContinueWith(previousTask =>
            {
                var data = previousTask.Result.HandleResponse().Data;
                data.restClient = client;
                return data;
            });

            return responseTask;
        }

        /// <summary>
        /// Get Shipment by ID
        /// </summary>
        /// <param name="shipmentId">Shipment ID</param>
        /// <returns>Shipment Object</returns>
        public Task<Shipment> GetShipmentAsync(int shipmentId)
        {
            var request = CreateRestRequest<Shipment>(shipmentId);

            var client = GetRestClient();

            var response = client.ExecuteTaskAsync<Shipment>(request).ContinueWith(previousTask =>
                    {
                        var data = previousTask.Result.HandleResponse().Data;
                        data.restClient = client;
                        return data;
                    }
                );

            return response;
        }

        /// <summary>
        /// Get Shipment by ID
        /// </summary>
        /// <param name="shipmentId">Shipment ID</param>
        /// <returns>Shipment Object</returns>
        public Shipment GetShipment(int shipmentId)
        {
            var request = CreateRestRequest<Shipment>(shipmentId);
            
            var client = GetRestClient();
           
            var response =  client.Execute<Shipment>(request).HandleResponse();

            response.Data.restClient = client;

            return response.Data;
        }

        /// <summary>
        /// Lock a shipment by ID, makes it read-only from the TMS. 
        /// The shipment can still be updated via the API
        /// </summary>
        /// <param name="shipmentId">Shipment ID</param>
        /// <param name="isLocked">Lock or Unlock</param>
        [Obsolete(" Set readonly property on shipment object and use Shipment.Save() instead")]  
        public void LockShipment(int shipmentId, bool isLocked)
        {
            var client = GetRestClient();
            var request = new RestRequest
            {
                Resource = "api/shipment/readonly/" + shipmentId,
                RequestFormat = DataFormat.Json,
                Method = Method.PUT      
            };

            request.AddParameter("readonly", isLocked.ToString().ToLower());

            client.Execute(request).HandleResponse();
           
        }

        /// <summary>
        /// Set status of shipment
        /// </summary>
        /// <param name="shipmentId">Shipment ID</param>
        /// <param name="status">Shipment Status</param>
        /// 
        [Obsolete(" Set status property on shipment object and use Shipment.Save() instead")]      
        public void SetShipmentStatus(int shipmentId, ShipmentStatus status)
        {
            var client = GetRestClient();
            var request = new RestRequest
            {
                Resource = "api/shipment/status/" + shipmentId,
                RequestFormat = DataFormat.Json,
                Method = Method.PUT
            };

            request.AddParameter("status", status.ToFriendlyName());

            client.Execute(request).HandleResponse();
        }

        /// <summary>
        /// Get Address by ID
        /// </summary>
        /// <param name="addressId"></param>
        /// <returns>Address Object</returns>
        public Address GetAddress(string addressId)
        {
            var request = CreateRestRequest<Address>(addressId);
            var client = GetRestClient();
            
           return client.Execute<Address>(request).HandleResponse().Data;
        }

        /// <summary>
        /// Get Address by ID
        /// </summary>
        /// <param name="addressId"></param>
        /// <returns>Task Address Object</returns>
        public Task<Address> GetAddressAsync(string addressId)
        {
            var request = CreateRestRequest<Address>(addressId);
            var client = GetRestClient();

            return client.ExecuteTaskAsync<Address>(request).ContinueWith(prev=>prev.Result.HandleResponse().Data);
        }


        private static IRestRequest CreateRestRequest<T>(string id, Method method = Method.GET)
        {
            string type = typeof(T).Name;
            if (String.IsNullOrEmpty(id))
                throw new ArgumentNullException(type + " id");


            var request = new RestRequest
            {
                Resource = "api/" + type.ToLower() + "/" + id,
                RequestFormat = DataFormat.Json,
                RootElement = type,
                Method = method
            };

            return request;
        }

        private static IRestRequest CreateRestRequest<T>(int id, Method method = Method.GET)
        {
            return CreateRestRequest<T>(id.ToString(), method);
        }

        
    }
}
