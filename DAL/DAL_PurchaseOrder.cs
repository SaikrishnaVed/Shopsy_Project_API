using NHibernate;
using Shopsy_Project.Interfaces;
using Shopsy_Project.Models;
using Shopsy_Project.Models.RequestModels;
using System.Data;
using System.Data.SqlClient;

namespace Shopsy_Project.DAL
{
    public class DAL_PurchaseOrder : IDAL_PurchaseOrder
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly HttpContextAccessor _httpContextAccessor;

        public DAL_PurchaseOrder()
        {
            var config = ConfigurationManager.SetConfiguration();
            _sessionFactory = config.BuildSessionFactory();
        }

        public void AddPurchaseOrder(AddPurchaseOrderRequest addPurchaseOrderRequest)
        {
            if (addPurchaseOrderRequest == null)
                throw new ArgumentNullException(nameof(addPurchaseOrderRequest), "Purchase order details cannot be null.");

            // Create a list of purchase orders
            List<PurchaseOrder> purchaseOrders = new List<PurchaseOrder>();

            using (var session = _sessionFactory.OpenSession())
            {
                try
                {
                    // Query cart details for the given user
                    var cartDetails = session.Query<Cart>()
                                              .Where(x => x.userId == addPurchaseOrderRequest.userId)
                                              .ToList();

                    // Populate the purchaseOrders list
                    foreach (var cart in cartDetails)
                    {
                        var purchaseOrder = new PurchaseOrder
                        {
                            Product_Id = cart.product_Id,
                            Quantity = Convert.ToInt32(cart.Quantity),
                            userId = cart.userId,
                            DateCreated = DateTime.UtcNow
                        };

                        // Get the product price for the cart item
                        var product = session.Query<Products>()
                                             .Where(x => x.Product_Id == cart.product_Id)
                                             .FirstOrDefault();

                        // Assign the price from the product
                        purchaseOrder.price = product != null ? Convert.ToInt32(product.List_Price) : 0;

                        purchaseOrders.Add(purchaseOrder);
                    }

                    // Get the connection string from appsettings
                    var configuration = new ConfigurationBuilder()
                                            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                                            .AddJsonFile("appsettings.json")
                                            .Build();
                    string connectionString = configuration.GetConnectionString("ShopsyDatabase");

                    // Now, pass the list of purchase orders to the stored procedure for bulk insertion
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // Create a DataTable to pass to the stored procedure
                        DataTable purchaseOrderTable = new DataTable();
                        purchaseOrderTable.Columns.Add("product_Id", typeof(int));
                        purchaseOrderTable.Columns.Add("Quantity", typeof(int));
                        purchaseOrderTable.Columns.Add("userId", typeof(int));
                        purchaseOrderTable.Columns.Add("price", typeof(int));
                        purchaseOrderTable.Columns.Add("DateCreated", typeof(DateTime));

                        foreach (var order in purchaseOrders)
                        {
                            purchaseOrderTable.Rows.Add(order.Product_Id, order.Quantity, order.userId, order.price, order.DateCreated);
                        }

                        using (var command = new SqlCommand("dbo.AddPurchaseOrders", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            var parameter = new SqlParameter("@PurchaseOrders", SqlDbType.Structured)
                            {
                                Value = purchaseOrderTable
                            };
                            command.Parameters.Add(parameter);

                            command.ExecuteNonQuery();
                        }

                        using (var transaction = session.BeginTransaction())
                        {
                            var cartItems = session.Query<Cart>().Where(x => x.userId == addPurchaseOrderRequest.userId).ToList();
                            foreach (var cartItem in cartItems)
                            {
                                session.Delete(cartItem);
                            }
                            transaction.Commit();
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions appropriately
                    throw new Exception("An error occurred while adding the purchase orders.", ex);
                }
            }
        }

        public void AddNewPurchaseOrder(List<PurchaseOrder> purchaseOrders)
        {
            if (purchaseOrders.Count == 0)
                throw new ArgumentNullException(nameof(PurchaseOrder), "Purchase order details cannot be null.");

            using (var session = _sessionFactory.OpenSession())
            {
                try
                {
                    // Get the connection string from appsettings
                    var configuration = new ConfigurationBuilder()
                                            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                                            .AddJsonFile("appsettings.json")
                                            .Build();
                    string connectionString = configuration.GetConnectionString("ShopsyDatabase");

                    // list of purchase orders to the stored procedure for bulk insertion
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // Create a DataTable to pass to the stored procedure
                        DataTable purchaseOrderTable = new DataTable();
                        purchaseOrderTable.Columns.Add("product_Id", typeof(int));
                        purchaseOrderTable.Columns.Add("Quantity", typeof(int));
                        purchaseOrderTable.Columns.Add("userId", typeof(int));
                        purchaseOrderTable.Columns.Add("price", typeof(int));
                        purchaseOrderTable.Columns.Add("DateCreated", typeof(DateTime));

                        foreach (var order in purchaseOrders)
                        {
                            purchaseOrderTable.Rows.Add(order.Product_Id, order.Quantity, order.userId, order.price, order.DateCreated);
                        }

                        using (var command = new SqlCommand("dbo.AddPurchaseOrders", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            var parameter = new SqlParameter("@PurchaseOrders", SqlDbType.Structured)
                            {
                                Value = purchaseOrderTable
                            };
                            command.Parameters.Add(parameter);

                            command.ExecuteNonQuery();
                        }

                        using (var transaction = session.BeginTransaction())
                        {
                            var cartItems = session.Query<Cart>().Where(x => x.userId == purchaseOrders[0].userId).ToList();
                            foreach (var cartItem in cartItems)
                            {
                                session.Delete(cartItem);
                            }
                            transaction.Commit();
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions appropriately
                    throw new Exception("An error occurred while adding the purchase orders.", ex);
                }
            }
        }

    }
}