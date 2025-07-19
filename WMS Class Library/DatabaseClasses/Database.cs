using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS_Class_Library.DatabaseClasses
{

    public static class Database
    {
        public static readonly string ConnectionString = "Host=localhost;Username=postgres;Password=Koreanishard2424$$;Database=postgres"; // change to an environment variable some day

        public static Order CreateOrderById(int orderId)
        {
            Order newOrder = null;

            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                var sql = @"
                SELECT
                    po.from_org,
                    po.to_org,
                    po.status,
                    pod.device_id,
                    pod.quantity,
                    pod.shipment_type,
                    pod.required_inventory,
                    d.sku
                FROM
                    pending_orders po
                LEFT JOIN
                    pending_order_details pod ON po.order_id = pod.order_id
                LEFT JOIN
                    devices d ON pod.device_id = d.id
                WHERE
                    po.order_id = @orderId;";

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("orderId", orderId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        string fromOrg = null;
                        string toOrg = null;
                        string status = null;
                        List<OrderDevice> deviceList = new List<OrderDevice>();
                        bool orderFound = false;

                        while (reader.Read())
                        {
                            if (!orderFound)
                            {
                                fromOrg = reader.GetString(reader.GetOrdinal("from_org"));
                                toOrg = reader.GetString(reader.GetOrdinal("to_org"));
                                status = reader.GetString(reader.GetOrdinal("status"));
                                orderFound = true;
                            }
                            if (!reader.IsDBNull(reader.GetOrdinal("device_id")))
                            {

                                string sku = reader.IsDBNull(reader.GetOrdinal("sku")) ? string.Empty : reader.GetString(reader.GetOrdinal("sku"));
                                int qty = reader.GetInt32(reader.GetOrdinal("quantity"));
                                string shipmentType = reader.GetString(reader.GetOrdinal("shipment_type"));
                                string requiredInventory = reader.GetString(reader.GetOrdinal("required_inventory"));

                                OrderDevice newDevice = new OrderDevice(sku, qty, requiredInventory, shipmentType);
                                deviceList.Add(newDevice);
                            }
                        }

                        if (orderFound)
                        {
                            newOrder = new Order(fromOrg, toOrg, status, orderId, deviceList);
                        }
                    }
                }
            }
            return newOrder; 
        }
        public static List<int> GetPendingOrders()
        {
            List<int> returnList = new List<int>();
            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();
            var cmd = new NpgsqlCommand("SELECT order_id FROM pending_orders WHERE status = 'PENDING'", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int new_id = reader.GetInt32(0);  // Always index 0 because there's only one column
                returnList.Add(new_id);
            }

            return returnList;
        }
        public static (string locator, string sku, string subInventory) GetSerialDatabaseDetails(string serialNumber)
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();
            var cmd = new NpgsqlCommand(@"
                SELECT 
                    i.locator, 
                    d.sku, 
                    i.sub_inventory 
                FROM 
                    inventory i
                JOIN 
                    devices d ON i.device_id = d.id
                WHERE 
                    i.organization = 'NB1' AND 
                    i.serial = @serialNumber", conn);
            cmd.Parameters.AddWithValue("serialNumber", serialNumber);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string locator = reader.GetString(0);
                string device_id = reader.GetString(1);
                string sub_inventory = reader.GetString(2);

                return (locator, device_id, sub_inventory);
                
            }
            
            return ("Error", "Error", "Error");

        }
    }
}