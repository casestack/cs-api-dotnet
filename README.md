# cs-api-dotnet

[![NuGet version](https://badge.fury.io/nu/CaseStackAPI.svg)](https://badge.fury.io/nu/CaseStackAPI)

This library allows you to push/pull data out of the CaseStack Supply Chain Management System from your .NET apps.

For complete documentation visit [http://docs.casestack.io/](http://docs.casestack.io/)

## Installation

Install via nuget

	PM> Install-Package CaseStackAPI

You can also directly add the source into your Project Dependencies.

## Usage

**Authentication**

Before you can make any API calls, you'll have to authenticate and pick your environment. Get your keys under Settings > CaseStack API.

```C#
// establish connection
CaseStackApi api = new CaseStackApi();

// establish connection to staging
CaseStackApi api = new CaseStackApi(useStagingEndpoint:true);

api.Authenticate("<put your API Key here>", "<put your Company ID here>");
```

**Getting objects by their ID**
	
```C#
// get a customer by ID
const string customerId = "38fc6a35";
Customer customer = api.GetCustomer(customerId);
Console.WriteLine("Customer with ID '{0}' has name '{1}' accounting code '{2}'", customerId, customer.name, customer.billing.accounting_code);

// get a customer by ID asynchronously
const string customerId = "38fc6a35";
Customer customer = await api.GetCustomerAsync(customerId);
Console.WriteLine("Customer with ID '{0}' has name '{1}' accounting code '{2}'", customerId, customer.name, customer.billing.accounting_code);

// get a shipment by ID
const int shipmentId = 1;
Shipment shipment = api.GetShipment(shipmentId);
Console.WriteLine("Shipment with ID '{0}' has status '{1}'", shipmentId, shipment.status);

// get a shipment by ID asynchronously
const int shipmentId = 1;
Shipment shipment = await api.GetShipmentAsync(shipmentId);
Console.WriteLine("Shipment with ID '{0}' has status '{1}'", shipmentId, shipment.status);

// get a carrier by ID
const string carrierId = "ec60fa20";
Carrier carrier = api.GetCarrier(carrierId);
Console.WriteLine("Carrier with ID '{0}' has name '{1}' and accounting code '{2}'", carrierId, carrier.name, carrier.billing.accounting_code);

// get a carrier by ID asynchronously
const string carrierId = "ec60fa20";
Carrier carrier = await api.GetCarrier(carrierId);
Console.WriteLine("Carrier with ID '{0}' has name '{1}' and accounting code '{2}'", carrierId, carrier.name, carrier.billing.accounting_code);

```

**Updating objects**

```C#
// update a customer
customer.name = "New name";
customer.custom_fields["c5dca8e0"] = "50";
customer.Save();

// update a customer asynchronously
customer.name = "New name";
customer.custom_fields["c5dca8e0"] = "50";
await customer.SaveAsync();

// update a carrier
carrier.name = "New name";
carrier.custom_fields["f571fcc8"] = "value";
carrier.Save();

// update a carrier asynchronously
carrier.name = "New name";
carrier.custom_fields["f571fcc8"] = "value";
await carrier.SaveAsync();
```

**Locking shipment with ID '1'**

```C#
const int shipmentId = 1;    
Console.WriteLine("Locking Shipment with ID '{0}'", shipmentId);
api.LockShipment(shipmentId, true);
```

**Changing status of shipment with ID '1'**

```C#
const int shipmentId = 1;    
Console.WriteLine("Setting status of Shipment with ID '{0}' to 'Delivery Apt Scheduled'", shipmentId);
api.SetShipmentStatus(shipmentId, CaseStackApi.ShipmentStatus.DeliveryAppointmentScheduled);
```    

**Getting all Custom Fields.**

This only has to be done once. It can be retrieved and cached through the life-cycle of the application. 

```C#
CustomFields carrierCustomFields = api.GetCustomFields<Carrier>();
CustomFields customerCustomFields = api.GetCustomFields<Customer>();
CustomFields shipmentCustomFields = api.GetCustomFields<Shipment>();

//asynchronously
CustomFields carrierCustomFields = await api.GetCustomFieldsAsync<Carrier>();
CustomFields customerCustomFields = await api.GetCustomFieldsAsync<Customer>();
CustomFields shipmentCustomFields = await api.GetCustomFieldsAsync<Shipment>();
```

Note: An object bust be of type "Customizable" in order to retrieve custom fields.

**Stitching together custom fields IDs and their labels**

```C#
// get a carrier by ID
const string carrierId = "ec60fa20";
Carrier carrier = api.GetCarrier(carrierId);
Console.WriteLine("Carrier with ID '{0}' has name '{1}' and accounting code '{2}'", carrierId, carrier.name, carrier.billing.accounting_code);

Console.WriteLine("Custom Fields for Carrier:");
foreach (string key in carrier.custom_fields.Keys)
{
    Field field = carrierCustomFields.fields.Single(s => s.id == key);
    String value = carrier.custom_fields[key];
    Console.WriteLine("\t => {0}: {1} ({2})", field.name, value, field.type);
}
```	

**Getting resources in paralell**
```C#
var shipmentIds = new[] {1, 2, 3, 4, 5};

var tasks = new List<Task<Shipment>>();

foreach (var shipmentId in shipmentIds)
{
    tasks.Add(sdk.GetShipmentAsync(shipmentId));
}

Task.WaitAll(tasks.ToArray());

List<Shipment> shipments =  tasks.Select(t => t.Result).ToList();
```

**Exception Handling**

```C#
//get a customer with error handling

const string customerId = "38fc6a35";

Customer customer = null;

try
{
    customer = api.GetCustomer(customerId);
}
catch (HttpException exception)
{
    if (exception.GetHttpCode() == 404)
    {
        Console.WriteLine("Customer with ID '{0}' was not found", customerId);
    }
}

//get a customer async with error handling
Customer customer = null;

try
{
    customer = await api.GetCustomer(customerId);
}
catch (HttpException exception)
{
    if (exception.GetHttpCode() == 404)
    {
        Console.WriteLine("Customer with ID '{0}' was not found", customerId);
    }
}

```
