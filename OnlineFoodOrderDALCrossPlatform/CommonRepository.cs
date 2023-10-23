using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OnlineFoodOrderDALCrossPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineFoodOrderDALCrossPlatform
{
    public class CommonRepository
    {
        #region Do not modify the signature

        OnlineFoodOrderDBContext context;
        public CommonRepository(OnlineFoodOrderDBContext onlineFoodOrderDBContext)
        {
            context = onlineFoodOrderDBContext;
        }

        #endregion

        #region CheckDeliveryStatus
        public int CheckDeliveryStatus(int orderId)
        {
            int resultForDeliverStatus = 0;
            try
            {
                resultForDeliverStatus = (from u in context.Orders select OnlineFoodOrderDBContext.ufn_CheckDeliveryStatus(orderId)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                resultForDeliverStatus = -99;
            }
            return resultForDeliverStatus;
        }
        #endregion

        #region DeleteOrderDetails
        public bool DeleteOrderDetails(int orderId)
        {
            bool isDeleted = false;
            try
            {
                Order order = context.Orders.Find(orderId);
                if (order != null)
                {
                    context.Orders.Remove(order);
                    context.SaveChanges();
                    isDeleted = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return isDeleted;
        }
        #endregion

        #region GetAllOrderDetails
        public List<OrderDetail> GetAllOrderDetails(int orderId)
        {
            List<OrderDetail> orderDetails = null;
            try
            {
                SqlParameter prmOrderId = new SqlParameter("@OrderId", orderId);
                orderDetails = context.OrderDetails.FromSqlRaw("SELECT * FROM ufn_GetOrderDetails(@OrderId)", prmOrderId).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return orderDetails;
        }
        #endregion
    }
}
