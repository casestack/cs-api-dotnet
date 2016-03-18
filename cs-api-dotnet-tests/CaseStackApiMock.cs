using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using cs_api_dotnet;
using FakeItEasy;
using RestSharp;

namespace cs_api_dotnet_tests
{
    public class CaseStackApiMock : CaseStackApi
    {
        private IRestClient _restClient;
        

        protected override IRestClient GetRestClient()
        {
            var restClient = A.Fake<IRestClient>();
            var apiCarrierErr = "api/carrier/err";
            var apiCarrierBadGateway = "api/carrier/badgateway";
            var apiCarrierGood = "api/carrier/foo";
            var apiCustomerErr = "api/customer/err";
            var apiCustomerGood = "api/customer/foo";
            var apiCustomFieldsErr = "api/customfield/testerror";
            var apiShipmentGood = "api/shipment/0";
            var apiShipmentBadgateway = "api/shipment/-2";
            var apiShipmentErr = "api/shipment/-1";
            var apiShipStatusErr = "api/shipment/status/-1";
            var apiLockShipmentErr = "api/shipment/readonly/-1";
            var apiShipStatusGood = "api/shipment/status/0";
            var apiLockShipmentGood = "api/shipment/readonly/0";
            string apiGetAddressError = "api/address/error";
            string apiGetAddressGood = "api/address/foo";
           


           
            //SHIPMENTS
            A.CallTo(() => restClient.Execute(A<IRestRequest>.That.Matches(r => r.Resource == apiShipStatusErr ))).Returns(new RestResponse<Shipment> { StatusCode = HttpStatusCode.InternalServerError, ErrorException = new Exception("test") });
            A.CallTo(() => restClient.Execute(A<IRestRequest>.That.Matches(r => r.Resource == apiLockShipmentErr))).Returns(new RestResponse<Shipment> { StatusCode = HttpStatusCode.InternalServerError, ErrorException = new Exception("test") });
            A.CallTo(() => restClient.Execute(A<IRestRequest>.That.Matches(r => r.Resource == apiShipStatusGood))).Returns(new RestResponse<Shipment> { Data = new Shipment() });
            A.CallTo(() => restClient.Execute(A<IRestRequest>.That.Matches(r => r.Resource == apiLockShipmentGood ))).Returns(new RestResponse<Shipment> { Data = new Shipment() });
            A.CallTo(() => restClient.Execute<Shipment>(A<IRestRequest>.That.Matches(r=>r.Resource == apiShipmentGood))).Returns(new RestResponse<Shipment>{StatusCode = HttpStatusCode.OK, Data = new Shipment{shipment_id = "0"}});
            A.CallTo(() => restClient.Execute<Shipment>(A<IRestRequest>.That.Matches(r => r.Resource == apiShipmentErr))).Returns(new RestResponse<Shipment>{StatusCode = HttpStatusCode.InternalServerError, ErrorException = new Exception("test")});
            A.CallTo(() => restClient.Execute(A<IRestRequest>.That.Matches(r => r.Resource == apiShipmentGood)))
                .Returns(new RestResponse<Shipment> {StatusCode = HttpStatusCode.OK});
            A.CallTo(() => restClient.Execute(A<IRestRequest>.That.Matches(r => r.Resource == apiShipmentBadgateway)))
                .Returns(new RestResponse<Carrier>() {StatusCode = HttpStatusCode.BadGateway});

            //CustomFields
            A.CallTo(() => restClient.Execute<CustomFields>(A<IRestRequest>.That.Not.Matches(r => r.Resource.StartsWith(apiCustomFieldsErr)))).Returns(new RestResponse<CustomFields>() {Data = new CustomFields()});

            //CUSTOMERS
            A.CallTo(() => restClient.Execute(A<IRestRequest>.That.Matches(r => r.Resource == apiCustomerGood))).Returns(new RestResponse<Customer>() {StatusCode  = HttpStatusCode.OK, Data = new Customer{customer_id = "foo"} });
            A.CallTo(() => restClient.Execute(A<IRestRequest>.That.Matches(r => r.Resource == apiCustomerErr))).Returns(new RestResponse<Customer> { StatusCode = HttpStatusCode.InternalServerError, ErrorException = new Exception("test") });
            A.CallTo(() => restClient.Execute<Customer>(A<IRestRequest>.That.Matches(r => r.Resource == apiCustomerErr))).Returns(new RestResponse<Customer>() { StatusCode = HttpStatusCode.InternalServerError, ErrorException = new Exception("test") });
            A.CallTo(() => restClient.Execute<Customer>(A<IRestRequest>.That.Matches(r => r.Resource == apiCustomerGood))).Returns(new RestResponse<Customer>() { StatusCode = HttpStatusCode.OK, Data = new Customer{ customer_id = "foo"}});
            
            //CARRIERS
            A.CallTo(() => restClient.Execute(A<IRestRequest>.That.Matches(r => r.Resource == apiCarrierGood))).Returns(new RestResponse<Carrier>() { StatusCode = HttpStatusCode.OK, Data = new Carrier { carrier_id = "foo" } });
            A.CallTo(() => restClient.Execute(A<IRestRequest>.That.Matches(r => r.Resource == apiCarrierErr))).Returns(new RestResponse<Carrier> { StatusCode = HttpStatusCode.InternalServerError, ErrorException = new Exception("test") });
            A.CallTo(() => restClient.Execute<Carrier>(A<IRestRequest>.That.Matches(r => r.Resource == apiCarrierErr))).Returns(new RestResponse<Carrier>() { StatusCode = HttpStatusCode.InternalServerError, ErrorException = new Exception("test")});
            A.CallTo(() => restClient.Execute<Carrier>(A<IRestRequest>.That.Matches(r=>r.Resource==apiCarrierGood))).Returns(new RestResponse<Carrier>(){StatusCode = HttpStatusCode.OK, Data = new Carrier{carrier_id = "foo"}});
            A.CallTo(
                () => restClient.Execute<Carrier>(A<IRestRequest>.That.Matches(r => r.Resource == apiCarrierBadGateway)))
                .Returns(new RestResponse<Carrier>() {StatusCode = HttpStatusCode.BadGateway});

            //ADDRESSES
            A.CallTo(() => restClient.Execute<Address>(A<IRestRequest>.That.Matches(r => r.Resource == apiGetAddressError)))
                .Returns(new RestResponse<Address>{StatusCode = HttpStatusCode.InternalServerError});
            A.CallTo(() => restClient.Execute<Address>(A<IRestRequest>.That.Matches(r => r.Resource == apiGetAddressGood))).Returns(new RestResponse<Address>{StatusCode = HttpStatusCode.OK, Data = new Address()});
            
            return restClient; 
        }

        public IRestClient GetBaseRestClient()
        {
            return base.GetRestClient();
        }


       
    }
}
