using OnlineFoodOrderDALCrossPlatform;
using System;
using System.Collections.Generic;
using OnlineFoodOrderDALCrossPlatform.Models;

namespace OnlineFoodOrderTestApplication
{
    class Program
    {
        static OnlineFoodOrderDBContext context;
        static CustomerRepository repository;

        static Program()
        {
            context = new OnlineFoodOrderDBContext();
           
        }
        static void Main(string[] args)
        {
            //TestGetAllItems();
            //TestGetItemDetails();
            //TestGetItemPrice();
            //TestPlaceOrder();
            //TestGetAllOrderDetails();
            //TestCheckDeliveryStatus();
            //TestDeleteOrder();
            //TestAddItem();
            //TestUpdatePrice();
            //TestGetAllCategoryOrderDetails();
        }

        #region TestGetAllItems
        static void TestGetAllItems()
        {
            CustomerRepository repository = new CustomerRepository(context);
            List<Item> itemsList = repository.GetAllItems();
            if (itemsList != null && itemsList.Count > 0)
            {
                Console.WriteLine("----------------------------------------------------------------------");
                Console.WriteLine(" {0, -12}{1, -30}{2, -20}{3}", "ItemId", "ItemName", "CategoryId", "Price");
                Console.WriteLine("----------------------------------------------------------------------");
                foreach (var item in itemsList)
                {
                    Console.WriteLine(" {0, -12}{1, -30}{2, -20}{3:0.00}", item.ItemId, item.ItemName, item.CategoryId, item.Price);
                }
                Console.WriteLine("----------------------------------------------------------------------");
            }
            else
            {
                Console.WriteLine("----------------------");
                Console.WriteLine(" No items to display ");
                Console.WriteLine("----------------------");
            }
        }
        #endregion

        #region TestGetItemDetails
        static void TestGetItemDetails()
        {
            CustomerRepository repository = new CustomerRepository(context);
            string categoryName = "Pizza";
            var itemDetails = repository.GetItemDetails(categoryName);
            
            if (itemDetails.Count == 0)
            {
                Console.WriteLine("------------------------------------------");
                Console.WriteLine("\n No items available in the given category\n");
                Console.WriteLine("------------------------------------------");
            }
            else
            {
                Console.WriteLine("---------------------------------------------------");
                Console.WriteLine(" {0, -12}{1, -30}{2}", "ItemId", "ItemName", "Price");
                Console.WriteLine("---------------------------------------------------");
                foreach (var item in itemDetails)
                {
                    //// UnComment below line of code
                    Console.WriteLine(" {0, -12}{1, -30}{2:0.00}", item.ItemId, item.ItemName, item.Price);
                }
                Console.WriteLine("---------------------------------------------------");
            }          
        }
        #endregion

        #region TestGetItemPrice
        static void TestGetItemPrice()
        {
            CustomerRepository repository = new CustomerRepository(context);
            string itemId = "CBR";
            decimal itemPrice = repository.GetItemPrice(itemId);
            if (itemPrice != -99)
            {
                Console.WriteLine("\n-----------------------------------\n");
                Console.WriteLine(" The price of item \"{0}\" is {1:0.00}", itemId, itemPrice);
                Console.WriteLine("\n-----------------------------------\n");
            }
            else
            {
                Console.WriteLine("\n-----------------------------------\n");
                Console.WriteLine(" The given item does not exist");
                Console.WriteLine("\n-----------------------------------\n");
            }
        }
        #endregion

        #region TestPlaceOrder
        static void TestPlaceOrder()
        {
            CustomerRepository repository = new CustomerRepository(context);
            int orderId = 0;
            decimal totalPrice = 0;
            int returnValue = repository.PlaceOrder(1001, "VDL", 1, "No.Infy. 9447123254",
                                                            DateTime.Today, out totalPrice, out orderId);
            Console.WriteLine("\n----------------------------------------------------------");
            if (returnValue == 1)
            {
                Console.WriteLine("Order placed successfully. " +
                    "\nYour OrderId = {0}" +
                    "\nTotal Price to pay = {1}", orderId, totalPrice);
            }
            else if (returnValue == -1)
            {
                Console.WriteLine(" CustomerID doesn't exist, please use existing CustomerId");
            }
            else if (returnValue == -2)
            {
                Console.WriteLine(" ItemID doesn't exist, plesae use existing ItemId");
            }
            else if (returnValue == -3)
            {
                Console.WriteLine(" The quantity should be greater than zero");
            }
            else if (returnValue == -4)
            {
                Console.WriteLine(" The delivery address should not be null ");
            }
            else if (returnValue == -5)
            {
                Console.WriteLine(" The date should not be less than today's date");
            }
            else
            {
                Console.WriteLine(" Some error occured, please try again.");
            }
            Console.WriteLine("----------------------------------------------------------");
        }
        #endregion

        #region TestGetAllOrderDetails
        static void TestGetAllOrderDetails()
        {
            CommonRepository repository = new CommonRepository(context);
            int orderId = 1;
            List<OrderDetail> orderList = repository.GetAllOrderDetails(orderId);
            if (orderList.Count == 0 || orderList == null)
            {
                Console.WriteLine("\n----------------------------------------------\n");
                Console.WriteLine(" No orders available under the given orderId");
                Console.WriteLine("\n----------------------------------------------\n");
            }
            else
            {
                Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("{0, -12}{1, -14}{2, -20}{3, -20}{4, -15}{5, -30}{6, -30}{7}", "OrderId", "CustomerId", "CustomerName", "ItemName", "TotalPrice", "DeliveryAddress", "OrderDate", "DeliveryStatus");
                Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------------------------------");

                foreach (var item in orderList)
                {  //// UnComment below line of code
                    Console.WriteLine("{0, -12}{1, -14}{2, -20}{3, -20}{4, -15}{5, -30}{6, -30}{7}", item.OrderId, item.CustomerId, item.CustomerName, item.ItemName, item.TotalPrice, item.DeliveryAddress, item.OrderDate, item.DeliveryStatus);
                }
                Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------------------------------");

            }
        }
        #endregion

        #region TestCheckDeliveryStatus
        static void TestCheckDeliveryStatus()
        {
            CommonRepository repository = new CommonRepository(context);
            int deliveryStatusResult = repository.CheckDeliveryStatus(2);
            if (deliveryStatusResult == 1)
            {
                Console.WriteLine("\n----------------- ");
                Console.WriteLine("| NOT DELIVERED |");
                Console.WriteLine("----------------- ");
            }
            else if (deliveryStatusResult == 0)
            {
                Console.WriteLine("\n------------- ");
                Console.WriteLine("| DELIVERED |");
                Console.WriteLine("------------- ");
            }
            else if (deliveryStatusResult == -1)
            {
                Console.WriteLine("\n------------------- ");
                Console.WriteLine("| Invalid OrderId |");
                Console.WriteLine("------------------- ");
            }
            else
            {
                Console.WriteLine("\n------------------------- ");
                Console.WriteLine("| Some error! Try again |");
                Console.WriteLine("------------------------- ");
            }
        }
        #endregion

        #region TestDeleteOrder
        static void TestDeleteOrder()
        {
            CommonRepository repository = new CommonRepository(context);
            bool orderStatus = repository.DeleteOrderDetails(6);
            if (orderStatus)
            {
                Console.WriteLine("\n-------------------------------------");
                Console.WriteLine("| Order details deleted successfully |");
                Console.WriteLine("-------------------------------------");
            }
            else
            {
                Console.WriteLine("\n----------------------------------------");
                Console.WriteLine("| Some error occurred. Check OrderId!! |");
                Console.WriteLine("----------------------------------------");
            }
        }
        #endregion

        #region TestAddItem
        static void TestAddItem()
        {
            AdminRepository repository = new AdminRepository(context);
            bool result = repository.AddItem("PBC", "Pepper Barbeque Chicken", 1, 120);
            if (result)
            {
                Console.WriteLine("\n-------------------------------");
                Console.WriteLine("| New Item added successfully |");
                Console.WriteLine("-------------------------------");
            }
            else
            {
                Console.WriteLine("\n------------------------------------");
                Console.WriteLine(" Something went wrong. Try again!!!");
                Console.WriteLine("------------------------------------");
            }
        }
        #endregion

        #region TestUpdatePrice
        static void TestUpdatePrice()
        {
            AdminRepository repository = new AdminRepository(context);
            string itemId = "ZPS";
            decimal itemPrice = 120;
            bool result = repository.UpdatePrice(itemId, itemPrice);
            if (result)
            {
                Console.WriteLine("\n----------------------------------------");
                Console.WriteLine("| The price of \"{0}\" is updated to {1:0,00} |", itemId, itemPrice);
                Console.WriteLine("----------------------------------------");
            }
            else
            {
                Console.WriteLine("\n-------------------------------------");
                Console.WriteLine("| Something went wrong. Try again!! |");
                Console.WriteLine("\n-------------------------------------");
            }
        }
        #endregion

        #region TestGetAllCategoryOrderDetails
        static void TestGetAllCategoryOrderDetails()
        {
            AdminRepository repository = new AdminRepository(context);
            int categoryId = 1;
            List<CategoryItemDetail> orderList = repository.GetAllCategoryOrderDetails(categoryId);
            if (orderList.Count == 0 || orderList == null)
            {
                Console.WriteLine("\n-----------------------------------------");
                Console.WriteLine("| No orders available for this category |");
                Console.WriteLine("-----------------------------------------");
            }
            else
            {
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("{0, -12}{1, -12}{2, -20}{3, -20}{4, -30}{5, -25}{6}", "OrderId", "CustomerId", "CustomerName", "ItemName", "DeliveryAddress", "OrderDate", "DeliveryStatus");
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------------");

                foreach (var item in orderList)
                {
                    // UnComment below line of code
                    Console.WriteLine("{0, -12}{1, -12}{2, -20}{3, -20}{4, -30}{5, -25}{6}", item.ItemId, item.CustomerId, item.CustomerName, item.ItemName, item.DeliveryAddress, item.OrderDate, item.DeliveryStatus);
                }
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------------");
            }
        } 
        #endregion
    }
}
 



  
    

