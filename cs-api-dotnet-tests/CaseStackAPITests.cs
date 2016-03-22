using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using cs_api_dotnet;
using FakeItEasy;
using NUnit.Framework;
using RestSharp;

namespace cs_api_dotnet_tests
{
    [TestFixture]
    public class CaseStackAPITests
    {
        private CaseStackApi _api;
        private CaseStackApiMock _apiMock;

        [SetUp]
        public void Setup()
        {
            _api = new CaseStackApi(useStagingEndpoint: true);
            _apiMock = new CaseStackApiMock();
        }

        #region Constructor Tests

        [Test]
        public void Construct_CaseStackAPi_Url_Is_Production()
        {
            _api = new CaseStackApi(true);
            Assert.True(_api.ApiEndpoint == "https://staging.casestack.io");
        }

        [Test]
        public void Construct_CaseStackApi_Url_Is_Staging()
        {
            Assert.True(_api.ApiEndpoint == "https://staging.casestack.io");
        }

        [Test]
        public void GetRestClient_ApiVersion_Correct()
        {
            var api = new CaseStackApiMock();
            api.Authenticate("foo", "foo");
            var apiVersion =
                api.GetBaseRestClient().DefaultParameters.FirstOrDefault(p => p.Name == "Accept-Version").Value as
                    string;

            Assert.AreEqual("1.0.0", apiVersion);
        }

        #endregion

        #region Authentication Tests

        [Test]
        public void Authenticate_Key_Is_Null_Throws_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _api.Authenticate(null, "foo"));
        }

        [Test]
        public void Authenticate_Key_Is_Empty_Throws_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _api.Authenticate(string.Empty, "foo"));
        }

        [Test]
        public void Authenticate_Company_Is_Null_Throws_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _api.Authenticate("foo", null));
        }

        [Test]
        public void Authenticate_Company_Is_Empty_Throws_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _api.Authenticate("foo", string.Empty));
        }

        [Test]
        public void Authenticate_Does_Not_Throw_Exception()
        {
            _api.Authenticate("foo", "foo");
        }

        #endregion

        #region Carrier Tests

        [Test]
        public void GetCarrier_CarrierId_Empty_Throws_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _api.GetCarrier(string.Empty));
        }

        [Test]
        public void GetCarrier_CarrierId_Null_Throws_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _api.GetCarrier(null));
        }

        [Test]
        public void GetCarrier_Data_Valid()
        {
            var api = new CaseStackApiMock();

            api.Authenticate("foo", "foo");
            var carrier = api.GetCarrier("foo");
            Assert.IsNotNull(carrier);
        }

        [Test]
        public async Task GetCarrierAsync_Data_Valid()
        {
            var api = new CaseStackApiMock();
            var carrier = await api.GetCarrierAsync("foo");
            //var carrier = carrierTask.Result;
            Assert.IsNotNull(carrier);
        }

        [Test]
        public void GetCarrier_RootElement_Carrier()
        {
           _apiMock.GetCarrier("foo");
           Assert.True(_apiMock.Request.RootElement == "Carrier");
        }

        [Test]
        public async Task GetCarrierAsync_RootElement_Carrier()
        {
            await _apiMock.GetCarrierAsync("foo");
            Assert.True(_apiMock.Request.RootElement == "Carrier");
        }


        [Test]
        public void GetCarrier_Resource_Correct()
        {
            _apiMock.GetCarrier("foo");

            Assert.True(_apiMock.Request.Resource == "api/carrier/foo");
        }

        [Test]
        public async Task GetCarrierAsync_Resource_Correct()
        {
            await _apiMock.GetCarrierAsync("foo");

            Assert.True(_apiMock.Request.Resource == "api/carrier/foo");
        }

        [Test]
        public void GetCarrier_Throws_HttpException()
        {
            var api = new CaseStackApiMock();

            api.Authenticate("foo", "foo");
            var exception = Assert.Throws<HttpException>(() => api.GetCarrier("err"));
            Assert.True(500 == exception.GetHttpCode());
        }

        [Test]
        public async Task GetCarrierAsync_Throws_HttpException()
        {
            var api = new CaseStackApiMock();

     
            var exception = Assert.ThrowsAsync<HttpException>(() => api.GetCarrierAsync("err"));
            Assert.True(500 == exception.GetHttpCode());
        }

        [Test]
        public void Carrier_Save_HttpException_Thrown()
        {
            var api = new CaseStackApiMock();

            api.Authenticate("foo", "foo");
            var carrier = api.GetCarrier("foo");

            carrier.carrier_id = "err";
            Assert.Throws<HttpException>(() => carrier.Save());
        }

        [Test]
        public async Task Carrier_SaveAsync_HttpException_Thrown()
        {
            var api = new CaseStackApiMock();

            api.Authenticate("foo", "foo");
            var carrier = api.GetCarrier("foo");

            carrier.carrier_id = "err";
            Assert.ThrowsAsync<HttpException>(() => carrier.SaveAsync());
        }

        [Test]
        public void Carrier_Save_400_Returned_HttpException_Thrown()
        {
            var api = new CaseStackApiMock();

            var carrier = api.GetCarrier("foo");

            carrier.carrier_id = "badgateway";
            Assert.Throws<HttpException>(() => carrier.Save());
        }

        [Test]
        public async Task Carrier_SaveAsync_400_Returned_HttpException_Thrown()
        {
            var api = new CaseStackApiMock();

            var carrier = api.GetCarrier("foo");

            carrier.carrier_id = "badgateway";
            Assert.ThrowsAsync<HttpException>(() => carrier.SaveAsync());
        }

        [Test]
        public void Carrier_Save_No_Error()
        {
            var api = new CaseStackApiMock();

            var carrier = api.GetCarrier("foo");
            carrier.Save();
        }

        [Test]
        public async Task Carrier_SaveAsync_No_Error()
        {
            var api = new CaseStackApiMock();

            var carrier = api.GetCarrier("foo");
            await carrier.SaveAsync();
        }

        #endregion

        #region Customer Tests

        [Test]
        public void GetCustomer_CustomerId_Empty_Throws_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _api.GetCustomer(string.Empty));
        }

        [Test]
        public async Task GetCustomerAsync_CustomerId_Empty_Throws_ArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _api.GetCustomerAsync(string.Empty));
        }

        [Test]
        public void GetCustomer_CustomerId_Null_Throws_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _api.GetCustomer(null));
        }

        [Test]
        public void GetCustomerAsync_CustomerId_Null_Throws_ArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _api.GetCustomerAsync(null));
        }

        [Test]
        public void GetCustomer_Customer_NotNull()
        {
            var api = new CaseStackApiMock();

            var customer = api.GetCustomer("foo");
            Assert.IsNotNull(customer);
        }

        [Test]
        public async Task GetCustomerAsync_Customer_NotNull()
        {
            var api = new CaseStackApiMock();
            var customer = await api.GetCustomerAsync("foo");
            Assert.IsNotNull(customer);
        }

        [Test]
        public void Customer_Save_HttpException_Thrown()
        {
            var api = new CaseStackApiMock();
            var customer = api.GetCustomer("foo");

            customer.customer_id = "err";
            Assert.Throws<HttpException>(() => customer.Save());
        }

        [Test]
        public void Customer_SaveAsync_HttpException_Thrown()
        {
            var api = new CaseStackApiMock();
            var customer = api.GetCustomer("foo");

            customer.customer_id = "err";
            Assert.ThrowsAsync<HttpException>(() => customer.SaveAsync());
        }

        [Test]
        public void Customer_Save_No_Error()
        {
            var api = new CaseStackApiMock();
            var customer = api.GetCustomer("foo");
            customer.Save();
        }

        [Test]
        public async Task Customer_SaveAsync_No_Error()
        {
            var api = new CaseStackApiMock();
            var customer = api.GetCustomer("foo");
            await customer.SaveAsync();
        }

        [Test]
        public void GetCustomer_Throws_HttpException()
        {
            var api = new CaseStackApiMock();

            api.Authenticate("foo", "foo");
            var exception = Assert.Throws<HttpException>(() => api.GetCustomer("err"));
            Assert.True(500 == exception.GetHttpCode());
        }

        [Test]
        public void GetCustomerAsync_Throws_HttpException()
        {
            var api = new CaseStackApiMock();
            var exception = Assert.ThrowsAsync<HttpException>(() => api.GetCustomerAsync("err"));
            Assert.True(500 == exception.GetHttpCode());
        }

        #endregion

        #region CustomField Tests

        [Test]
        public void GetCustomFields_Carrier_No_Errors()
        {
            var api = new CaseStackApiMock();
            api.GetCustomFields<Carrier>();
        }

        [Test]
        public async Task GetCustomFieldsAsync_Carrier_No_Errors()
        {
            var api = new CaseStackApiMock();
            await api.GetCustomFieldsAsync<Carrier>();
        }

        [Test]
        public void GetCustomFields_Throws_HttpException()
        {

            var api = new CaseStackApiMock();

            api.Authenticate("foo", "foo");
            var exception = Assert.Throws<HttpException>(() => api.GetCustomFields<TestError>());
            Assert.True(500 == exception.GetHttpCode());
        }

        [Test]
        public void GetCustomFieldsAsync_Throws_HttpException()
        {

            var api = new CaseStackApiMock();

            api.Authenticate("foo", "foo");
            var exception = Assert.ThrowsAsync<HttpException>(() => api.GetCustomFieldsAsync<TestError>());
            Assert.True(500 == exception.GetHttpCode());
        }

        #endregion

        #region Shipment Tests

        [Test]
        public void GetShipment_RootElement_Shipment()
        {
            _apiMock.GetShipment(0);
            Assert.True(_apiMock.Request.RootElement == "Shipment");
        }

        [Test]
        public async Task  GetShipmentAsync_RootElement_Shipment()
        {
            await _apiMock.GetShipmentAsync(0);
            Assert.True(_apiMock.Request.RootElement == "Shipment");
        }

        [Test]
        public void GetShipment_Resource_Correct()
        {
            _apiMock.GetShipment(0);

            Assert.True(_apiMock.Request.Resource == "api/shipment/0");
        }

        [Test]
        public async Task GetShipmentAsync_Resource_Correct()
        {
            await _apiMock.GetShipmentAsync(0);

            Assert.True(_apiMock.Request.Resource == "api/shipment/0");
        }

        [Test]
        public void GetShipment_Data_Valid()
        {
            var api = new CaseStackApiMock();
            var shipment = api.GetShipment(0);
            Assert.IsNotNull(shipment);
        }

        [Test]
        public async Task GetShipmentAsync_Data_Valid()
        {
            var api = new CaseStackApiMock();

            var shipment = await api.GetShipmentAsync(0);
            Assert.IsNotNull(shipment);
        }

        [Test]
        public void Save_No_Error()
        {
            var api = new CaseStackApiMock();
            var shipment = api.GetShipment(0);
            shipment.Save();
        }

        [Test]
        public async Task SaveAsync_No_Error()
        {
            var api = new CaseStackApiMock();
            var shipment = api.GetShipment(0);
            await shipment.SaveAsync();
        }

        [Test]
        public void Save_BadGatewaty_Throws_HttpException()
        {
            var api = new CaseStackApiMock();
            var shipment = api.GetShipment(0);
            shipment.shipment_id = "-2";
            Assert.Throws<HttpException>(()=> shipment.Save() );
        }

        [Test]
        public void SaveAsync_BadGatewaty_Throws_HttpException()
        {
            var api = new CaseStackApiMock();
            var shipment = api.GetShipment(0);
            shipment.shipment_id = "-2";
            Assert.ThrowsAsync<HttpException>(() => shipment.SaveAsync());
        }

        [Test]
        public void GetShipment_Throws_HttpException()
        {
            var api = new CaseStackApiMock();
            var exception = Assert.Throws<HttpException>(() => api.GetShipment(-1));
            Assert.True(500 == exception.GetHttpCode());
        }

        [Test]
        public void GetShipmentAsync_Throws_HttpException()
        {
            var api = new CaseStackApiMock();

            var exception = Assert.ThrowsAsync<HttpException>(() => api.GetShipmentAsync(-1));
            Assert.True(500 == exception.GetHttpCode());
        }

       
        #endregion

        #region SetShipmentStatus Tests

        [Test]
        public void SetShipmentStatus_Throws_HttpException()
        {
            var api = new CaseStackApiMock();
            Assert.Throws<HttpException>(() => api.SetShipmentStatus(-1, CaseStackApi.ShipmentStatus.Archived));
        }

        [Test]
        public void SetShipmentStatus_No_Error()
        {
            var api = new CaseStackApiMock();
            api.SetShipmentStatus(0, CaseStackApi.ShipmentStatus.Archived);
        }


        #endregion

        #region LockShipment Tests

        [Test]
        public void LockShipment_Throws_HttpException()
        {

            var api = new CaseStackApiMock();


            Assert.Throws<HttpException>(() => api.LockShipment(-1, true));
        }

        [Test]
        public void LockShipment_No_Error()
        {
            var api = new CaseStackApiMock();
            api.LockShipment(0, true);
        }


        #endregion

        #region Address

        [Test]
        public void GetAddress_HttpException_Thrown()
        {
            var api = new CaseStackApiMock();
            Assert.Throws<HttpException>(() => api.GetAddress("error"));
        }

        [Test]
        public void GetAddressAsync_HttpException_Thrown()
        {
            var api = new CaseStackApiMock();
            Assert.ThrowsAsync<HttpException>(() => api.GetAddressAsync("error"));
        }

        [Test]
        public void GetAddress_Error_Not_Thrown()
        {
            var api = new CaseStackApiMock();
            api.GetAddress("foo");
        }

        [Test]
        public async Task GetAddressAsync_Error_Not_Thrown()
        {
            var api = new CaseStackApiMock();
            await api.GetAddressAsync("foo");
        }

        #endregion

    }
}

