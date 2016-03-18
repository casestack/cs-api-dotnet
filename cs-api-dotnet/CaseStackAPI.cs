using System;
using System.ComponentModel;
using System.Net;
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

            if (String.IsNullOrEmpty(carrierId))
                throw new ArgumentNullException("carrierId");

            var client = GetRestClient();
            var request = new RestRequest
            {
                Resource = "api/carrier/" + carrierId,
                RequestFormat = DataFormat.Json,
                RootElement = "Carrier"
            };

            var response = client.Execute<Carrier>(request);
            if (response.StatusCode != HttpStatusCode.OK || response.ErrorException != null)
                throw new HttpException((int)response.StatusCode, "Error retrieving Carrier");

            var carrier = response.Data;
            carrier.restClient = client;
            return carrier;
        }

        /// <summary>
        /// Get Custom Fields for an object type
        /// </summary>
        /// <typeparam name="T">Classs must be of type Customizable</typeparam>
        /// <returns>Custom Fields Object</returns>
        public CustomFields GetCustomFields<T>() where T : Customizable
        {
            var parent = typeof (T).Name.ToLower();
            var client = GetRestClient();
            var request = new RestRequest
            {
                Resource = "api/customfield/" + parent,
                RequestFormat = DataFormat.Json,
                RootElement = "Carrier"
            };

            var response = client.Execute<CustomFields>(request);
            if (response.ErrorException != null)
            {
                throw new HttpException((int)response.StatusCode, "Error retrieving custom fields", response.ErrorException);
              
            }

            var customFields = response.Data;
            return customFields;
        }

        /// <summary>
        /// Get Customer by ID
        /// </summary>
        /// <param name="customerId">Customer ID</param>
        /// <returns>Customer Object</returns>
        public Customer GetCustomer(string customerId)
        {
            if (String.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException("customerId");
            }

            var client = GetRestClient();
            var request = new RestRequest
            {
                Resource = "api/customer/" + customerId,
                RequestFormat = DataFormat.Json,
                RootElement = "Customer"
            };

            var response = client.Execute<Customer>(request);
            if (response.ErrorException != null)
                throw new HttpException((int)response.StatusCode, "Error retrieving Customer");

            var customer = response.Data;
            customer.restClient = client;
            return customer;
        }

        /// <summary>
        /// Get Shipment by ID
        /// </summary>
        /// <param name="shipmentId">Shipment ID</param>
        /// <returns>Shipment Object</returns>
        public Shipment GetShipment(int shipmentId)
        {
            var client = GetRestClient();
            var request = new RestRequest
            {
                Resource = "api/shipment/" + shipmentId,
                RequestFormat = DataFormat.Json,
                RootElement = "Shipment"
            };

            var response = client.Execute<Shipment>(request);
            if (response.ErrorException != null)
                throw new HttpException((int)response.StatusCode, "Error retrieving Shipment", response.ErrorException);

           

            var shipment = response.Data;
            shipment.restClient = client;
            return shipment;
        }

        /// <summary>
        /// Lock a shipment by ID, makes it read-only from the TMS. 
        /// The shipment can still be updated via the API
        /// </summary>
        /// <param name="shipmentId">Shipment ID</param>
        /// <param name="isLocked">Lock or Unlock</param>
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

            var response = client.Execute(request);
            if (response.ErrorException != null)
                throw new HttpException((int)response.StatusCode, "Error locking/unlocking Shipment", response.ErrorException);
        }

        /// <summary>
        /// Set status of shipment
        /// </summary>
        /// <param name="shipmentId">Shipment ID</param>
        /// <param name="status">Shipment Status</param>
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

            var response = client.Execute(request);
            if (response.ErrorException != null)
                throw new HttpException((int)response.StatusCode, "Error updating status of Shipment", response.ErrorException);
        }

        /// <summary>
        /// Get Address by ID
        /// </summary>
        /// <param name="addressId"></param>
        /// <returns>Address Object</returns>
        public Address GetAddress(string addressId)
        {
            var client = GetRestClient();
            var request = new RestRequest
            {
                Resource = "api/address/" + addressId,
                RequestFormat = DataFormat.Json,
                RootElement = "Address"
            };

            var response = client.Execute<Address>(request);
            if (response.ErrorException != null || response.StatusCode != HttpStatusCode.OK)
                throw new HttpException((int)response.StatusCode, "Error retrieving Address", response.ErrorException);

            var address = response.Data;
            return address;
        }
    }
}
