using System.Collections.Generic;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Deserializers;

namespace cs_api_dotnet
{
    /// <summary>
    /// Shipment Object
    /// </summary>
    public class Shipment : Customizable
    {
        internal IRestClient restClient;

        /// <summary>
        /// Shipment ID
        /// </summary>
        public string shipment_id { get; set; }
        
        /// <summary>
        /// User who created shipment
        /// </summary>
        public string user_id { get; set; }

        /// <summary>
        /// Customer who will be billed for shipment
        /// </summary>
        public string customer_id { get; set; }

        /// <summary>
        /// Carrier which will transport the shipment
        /// </summary>
        public string carrier_id { get; set; }

        /// <summary>
        /// Status of shipment
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// Timestamp the shipment was created (unix epoch)
        /// </summary>
        public long created { get; set; }

        /// <summary>
        /// Can shipment be edited through the web interface
        /// </summary>
        public bool read_only { get; set; }

        /// <summary>
        /// Carrier Rep for this shipment
        /// </summary>
        public string carrier_rep { get; set; }

        /// <summary>
        /// Customer Rep for this shipment
        /// </summary>
        public string customer_rep { get; set; }

        /// <summary>
        /// Any comments from customer
        /// </summary>
        public string customer_comments { get; set; }

        /// <summary>
        /// Value of shipment, as declared by customer
        /// </summary>
        public double shpment_value { get; set; }

        /// <summary>
        /// Insurance required in USD
        /// </summary>
        public double insurance_required { get; set; }

        /// <summary>
        /// Used by Customer to identify shipment
        /// </summary>
        public string customer_order_number { get; set; }

        /// <summary>
        /// Total mileage of trip as provided by customer
        /// </summary>
        public double customer_mileage { get; set; }

        /// <summary>
        /// Mode of Transportation for shipment
        /// </summary>
        public string mode { get; set; }
        
        /// <summary>
        /// Freight Charge Payment terms. Can be one of three options: "thirdparty", "collect" or "prepaid"
        /// </summary>
        public string payment_term { get; set; }

        /// <summary>
        /// Distance Matrix used to compute distance for trip
        /// </summary>
        public string distance_matrix { get; set; }

        /// <summary>
        /// Special Instructions to be printed on the Load Tender
        /// </summary>
        public string special_instructions { get; set; }

        /// <summary>
        /// Total Items as entered
        /// </summary>
        public int total_items { get; set; }

        /// <summary>
        /// Total Weight of shipment as entered
        /// </summary>
        public double total_weight { get; set; }

        /// <summary>
        /// Unit of measure used for this shipment
        /// </summary>
        public Uom uom { get; set; }

        /// <summary>
        /// Truckload options
        /// </summary>
        public Truckload truckload { get; set; }

        /// <summary>
        /// Third Party billing info
        /// </summary>
        public Thirdparty thirdparty { get; set; }

        /// <summary>
        /// Values dereived and cached, for quick lookup
        /// </summary>
        public ComputedValues computed_values { get; set; }

        /// <summary>
        /// Charges the carrier will be billing you
        /// </summary>
        public List<Charge> carrier_charges { get; set; }

        /// <summary>
        /// Charges you will be billing your customer
        /// </summary>
        public List<Charge> customer_charges { get; set; }

        /// <summary>
        /// List of Documents related to this shipment
        /// </summary>
        public List<Documents> documents { get; set; }

        /// <summary>
        /// Notes on this shipment as posted by users
        /// </summary>
        public List<Notes> notes { get; set; }

        /// <summary>
        /// All the items that are being transported
        /// </summary>
        public List<Item> items { get; set; }

        /// <summary>
        /// All the pickup and delivery stops
        /// </summary>
        public List<Stop> stops { get; set; }

        /// <summary>
        /// Truck location log
        /// </summary>
        public List<RouteLog> route_log { get; set; }

        public void Save()
        {
            var request = new RestRequest
            {
                Method = Method.PUT,
                Resource = "api/shipment/" + shipment_id,
                RequestFormat = DataFormat.Json
            };

            var body = JsonConvert.SerializeObject(new { shipment = this }, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            request.AddParameter("application/json", body, ParameterType.RequestBody);

            var response = restClient.Execute(request);
            if (response.ErrorException != null || response.StatusCode != HttpStatusCode.OK)
                throw new HttpException((int)response.StatusCode, "Error updating shipment", response.ErrorException);
        }
    }

   /// <summary>
   /// Unit of Measurements
   /// </summary>
    public class Uom
    {
        /// <summary>
        /// Unit of Measurement for all dimensions
        /// </summary>
        public string dimensions { get; set; }
        
        /// <summary>
        /// Unit of Measurement for all weights
        /// </summary>
        public string weight { get; set; }
    }

    /// <summary>
    /// Truckload Options
    /// </summary>
    public class Truckload
    {
        /// <summary>
        /// List of equipment required 
        /// </summary>
        public List<string> equipment_required { get; set; }

        /// <summary>
        /// Full or Partial truck required
        /// </summary>
        public Space space { get; set; }
    }

    /// <summary>
    /// Space on a truck
    /// </summary>
    public class Space
    {
        /// <summary>
        /// Shipment requires a Full truck 
        /// </summary>
        public bool full { get; set; }

        /// <summary>
        /// Shipment requires a Partial truck
        /// </summary>
        public bool partial { get; set; }
    }

    /// <summary>
    /// Third Party billing info
    /// </summary>
    public class Thirdparty
    {
        public string company { get; set; }
        public string line1 { get; set; }
        public string line2 { get; set; }
        public string zip { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string contact { get; set; }
        public string phone { get; set; }
    }

    /// <summary>
    /// Computed Values
    /// </summary>
    public class ComputedValues
    {
        /// <summary>
        /// Total count of all items
        /// </summary>
        public double total_items { get; set; }

        /// <summary>
        /// Summation of weights of all items
        /// </summary>
        public double total_weight { get; set; }
        
        /// <summary>
        /// Total stops
        /// </summary>
        public double total_stops { get; set; }

        /// <summary>
        /// Total duration of trip
        /// </summary>
        public double total_duration { get; set; }

        /// <summary>
        /// Total distance of trip
        /// </summary>
        public double total_distance { get; set; }

        /// <summary>
        /// Summation of all charges owed to carrier
        /// </summary>
        public double total_carrier_charges { get; set; }

        /// <summary>
        /// Summation of all charges to be charged to customer
        /// </summary>
        public double total_customer_charges { get; set; }

        /// <summary>
        /// Profit margin (as USD)
        /// </summary>
        public double margin_dollars { get; set; }
        
        /// <summary>
        /// Profilt margin (as percentage) 
        /// </summary>
        public double margin_percent { get; set; }

        /// <summary>
        /// Total miles completed thus far
        /// </summary>
        public double complete_miles { get; set; }

        /// <summary>
        /// Percent of trip completed
        /// </summary>
        public double complete_percent { get; set; }

        /// <summary>
        /// Percent of trip remaining
        /// </summary>
        public double remaining_percent { get; set; }
    }

    public class Charge
    {
        /// <summary>
        /// Type of charge. Can be one of the following:
        /// "Detention Charge", "Equipment Ordered Not Used", "Fuel Surcharge", "Shipping", "Other", "Disposal Charge", 
        /// "Excess Weight", "Freeze Protection", "Freight Charge", "Hazardous Materials", "Layover Charge", 
        /// "Loading/Unloading Charge", "Lumper Charge", "Mileage Rates", "Minimum Charge", "Pallet Exchange", 
        /// "Reconsignment Charge", "Refusal Charge", "Scale Charge", "Stop-off Charge", "Tarp Charge", 
        /// "Team Driver Service", "Temperature Control", "Tolls"
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// Quantity of charge
        /// </summary>
        public double qty { get; set; }

        /// <summary>
        /// Rate per item
        /// </summary>
        public double rate { get; set; }

        /// <summary>
        /// Total charge (qty * rate)
        /// </summary>
        public double total { get; set; }
    }

    public class Documents
    {
        /// <summary>
        /// Type of document
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// URL to where the document is stored
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// Size in bytes of file
        /// </summary>
        public double size { get; set; }

        /// <summary>
        /// Timestamp when document was uploaded (unix epoch)
        /// </summary>
        public long timestamp { get; set; }
    }

    public class Notes
    {
        /// <summary>
        /// Message
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// User who posted the message
        /// </summary>
        public string user { get; set; }

        /// <summary>
        /// Timestamp when the message was posted(unix epoch)
        /// </summary>
        public long timestamp { get; set; }
    }

    public class Item
    {

        /// <summary>
        /// Packaging Type. Can be one of the following:
        /// "FAK", "Bag", "Bale", "Barrel", "Basket", "Bin", "Box", "Bunch", "Bundle", "Cabinet", "Can", "Carboy", 
        /// "Carrier", "Carton", "Case", "Cask", "Container", "Coil", "Crate", "Cylinder", "Drum", "Each", "Loose", 
        /// "Other", "Package", "Pail", "Pallet", "Pieces", "Pipe Line", "Rack", "Reel", "Roll", "Skid", "Spool", 
        /// "Tank", "Tube", "Totes", "Unit", "Van Pack", "Wrapped"

        /// </summary>
        public string packaging { get; set; }

        /// <summary>
        /// Quantity of item
        /// </summary>
        public double qty { get; set; }

        /// <summary>
        /// Weight of item
        /// </summary>
        public double weight { get; set; }

        /// <summary>
        /// Length of item
        /// </summary>
        public double length { get; set; }

        /// <summary>
        /// Width of item
        /// </summary>
        public double width { get; set; }

        /// <summary>
        /// Height of item
        /// </summary>
        public double height { get; set; }

        /// <summary>
        /// Volume (cube) of item
        /// </summary>
        public double cube { get; set; }

        [DeserializeAs(Name = "class")]
        public string classs { get; set; }

        /// <summary>
        /// NMFC
        /// </summary>
        public double nmfc { get; set; }

        /// <summary>
        /// NMFC Sub
        /// </summary>
        public double nmfc_sub { get; set; }
        
        /// <summary>
        /// Is item Hazardous
        /// </summary>
        public bool hazmat { get; set; }

        /// <summary>
        /// Description of item
        /// </summary>
        public string description { get; set; }
    }

    public class Distance
    {
        /// <summary>
        /// Distance matrix used
        /// </summary>
        public string matrix { get; set; }

        /// <summary>
        /// Miles 
        /// </summary>
        public double miles { get; set; }

        /// <summary>
        /// Hours in drive time
        /// </summary>
        public double hours { get; set; }

        /// <summary>
        /// Minutes in drive time
        /// </summary>
        public double minutes { get; set; }
    }

    public class Stop
    {
        /// <summary>
        /// Type: Can be either pickup or delivery
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// ID of Address used for this stop
        /// </summary>
        public string address_id { get; set; }

        /// <summary>
        /// Name of company
        /// </summary>
        public string company { get; set; }

        /// <summary>
        /// Primary contact person
        /// </summary>
        public string contact { get; set; }

        /// <summary>
        /// Phone number
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        /// Fax
        /// </summary>
        public string fax { get; set; }

        /// <summary>
        /// Email address
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// Zio code of stop
        /// </summary>
        public string zip { get; set; }

        /// <summary>
        /// Country of stop
        /// </summary>
        public string country { get; set; }

        /// <summary>
        /// Address Line 1
        /// </summary>
        public string line1 { get; set; }

        /// <summary>
        /// Address Line 2
        /// </summary>
        public string line2 { get; set; }

        /// <summary>
        /// Geo Location: [lon, lat]
        /// </summary>
        public List<double> loc { get; set; }

        /// <summary>
        /// City of stop
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// State of stop
        /// </summary>
        public string state { get; set; }

        /// <summary>
        /// Timezone of stop
        /// </summary>
        public string timezone { get; set; }

        /// <summary>
        /// Timestamp of earliest pickup/delivery (unix epoch)
        /// </summary>
        public long earliest { get; set; }

        /// <summary>
        /// Timestamp of latest pickup/delivery (unix epoch)
        /// </summary>
        public long latest { get; set; }

        /// <summary>
        /// Distance from previous stop to this stop
        /// </summary>
        public Distance leg_distance { get; set; }

        /// <summary>
        /// Appointment required before picking up/delivering to this stop
        /// </summary>
        public bool apt_required { get; set; }

        /// <summary>
        /// Indexes of all items which will be picked up/delivered to this stop
        /// </summary>
        public List<long> items { get; set; }

        /// <summary>
        /// If carrier has reached this stop
        /// </summary>
        public bool carrier_reached { get; set; }
        
        /// <summary>
        /// Timestamp of actual arrival (unix epoch)
        /// </summary>
        public long actual_arrival { get; set; }

        /// <summary>
        /// Timestamp of actual departure (unix epoch)
        /// </summary>
        public long actual_depart { get; set; }

        /// <summary>
        /// Optional comments or special instructions for stop
        /// </summary>
        public string comments { get; set; }

        /// <summary>
        /// Bill of Lading for Stop
        /// </summary>
        public string bol { get; set; }

        /// <summary>
        /// Number used by carriers as a reference
        /// </summary>
        public string pro { get; set; }
    }

    /// <summary>
    /// Last seen location of Truck
    /// </summary>
    public class LogLocation
    {
        public string zip { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public List<double> loc { get; set; }
    }

    public class RouteLog
    {
        /// <summary>
        /// User who logged location
        /// </summary>
        public string user { get; set; }

        /// <summary>
        /// Actual location of truck
        /// </summary>
        public LogLocation location { get; set; }

        /// <summary>
        /// Timestamp when truck was seen (unix epoch)
        /// </summary>
        public long timestamp { get; set; }

        /// <summary>
        /// Timezone of timestamp
        /// </summary>
        public string timezone { get; set; }

        /// <summary>
        /// Distance and Duration from logged location to final destination
        /// </summary>
        public Distance remaining { get; set; }
    }
}
