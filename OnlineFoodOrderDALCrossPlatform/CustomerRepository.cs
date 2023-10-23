using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OnlineFoodOrderDALCrossPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineFoodOrderDALCrossPlatform
{
    public class CustomerRepository
    {
        #region Do not modify the signature

        OnlineFoodOrderDBContext context;
        public CustomerRepository(OnlineFoodOrderDBContext onlineFoodOrderDBContext)
        {
            context = onlineFoodOrderDBContext;
        }

        #endregion

        #region GetAllItems
        public List<Item> GetAllItems()
        {
            List<Item> listOfItems = null;
            try
            {
                listOfItems = context.Items.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return listOfItems;
        }
        #endregion

        #region GetItemDetails
        public List<ItemDetail> GetItemDetails(string categoryName)
        {
            List<ItemDetail> itemDetails = null;
            try
            {
                SqlParameter prmCategoryName = new SqlParameter("@CategoryName", categoryName);
                itemDetails = context.ItemDetails.FromSqlRaw("SELECT * FROM ufn_FetchItemDetails(@CategoryName)", prmCategoryName).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return itemDetails;
        }
        #endregion

        #region GetItemPrice
        public decimal GetItemPrice(string itemId)
        {
            decimal itemPrice = 0;
            try
            {
                itemPrice = (from u in context.Items select OnlineFoodOrderDBContext.ufn_FetchItemPrice(itemId)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return itemPrice;
        }
        #endregion

        #region PlaceOrder
        public int PlaceOrder(int customerId, string itemId, int quantity, 
            string deliveryAddress, DateTime orderDate, out decimal totalPrice, 
            out int orderId)
        {
            int usp_ReturnValue = 0;
            try
            {
                SqlParameter prmCustomerId = new SqlParameter("@CustomerId", customerId);
                SqlParameter prmItemId = new SqlParameter("@ItemId", itemId);
                SqlParameter prmQuantity = new SqlParameter("@Quantity", quantity);
                SqlParameter prmDeliveryAddress = new SqlParameter("@DeliveryAddress", deliveryAddress);
                SqlParameter prmOrderDate = new SqlParameter("@OrderDate", orderDate);

                SqlParameter prmTotalPrice = new SqlParameter("@TotalPrice", System.Data.SqlDbType.Money);
                prmTotalPrice.Direction = System.Data.ParameterDirection.Output;

                SqlParameter prmOrderId = new SqlParameter("@OrderId", System.Data.SqlDbType.Int);
                prmOrderId.Direction = System.Data.ParameterDirection.Output;

                SqlParameter prmReturnValue = new SqlParameter("@ReturnValue", System.Data.SqlDbType.Int);
                prmReturnValue.Direction = System.Data.ParameterDirection.Output;

                context.Database.ExecuteSqlRaw(
                    "EXEC @ReturnValue=usp_AddOrderDetails @CustomerId, @ItemId, @Quantity, @DeliveryAddress, @OrderDate, @TotalPrice OUT, @OrderId OUT", prmReturnValue, prmCustomerId, prmItemId, prmQuantity, prmDeliveryAddress, prmOrderDate, prmTotalPrice, prmOrderId);

                usp_ReturnValue = Convert.ToInt32(prmReturnValue.Value);
                totalPrice = Convert.ToDecimal(prmTotalPrice.Value);
                orderId = Convert.ToInt32(prmOrderId.Value);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                totalPrice = -1;
                orderId = -1;
                usp_ReturnValue = -99;
            }
            return usp_ReturnValue;
        }  
        #endregion
    }
}
