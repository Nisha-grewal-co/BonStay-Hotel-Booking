using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineFoodOrderDALCrossPlatform;
using OnlineFoodOrderDALCrossPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineFoodOrderWebService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommonController : Controller
    {
        #region Do not modify signature

        readonly CommonRepository repository;
        public CommonController(CommonRepository commonRepository)
        {
            // To-do: Implement necessary code here
            repository = commonRepository;
        }

        #endregion

        #region CheckDeliveryStatus
        [HttpGet]
        public JsonResult CheckDeliveryStatus(int orderId)
        {
            string response;
            try
            {
                int status = repository.CheckDeliveryStatus(orderId);
                if(status == 0)
                {
                    response = "Delivered!";
                }
                else if (status == 1) {
                    response = "Not Delivered!";
                }
                else
                {
                    response = "Invalid !";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                response = ex.Message;
            }
            return Json(response);
        }
        #endregion

        #region DeleteOrderDetails
        [HttpDelete]  
        public JsonResult DeleteOrderDetails(int orderId)
        {
            string response;
            try
            {
                bool status = repository.DeleteOrderDetails(orderId);
                if (status)
                {
                    response = "Order was cancelled successfully!";
                }
                else
                {
                    response = "Order Id: " + orderId + " does not exist!";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                response = ex.Message;
            }
            return Json(response);
        }
        #endregion

        #region GetAllOrderDetails
        [HttpGet]
        public JsonResult GetAllOrderDetails(int orderId)
        {
            List<OrderDetail> orderDetails = null;
            try
            {
                orderDetails = repository.GetAllOrderDetails(orderId);
                if(orderDetails.Count == 0)
                {
                    return Json("No order with order Id: " + orderId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Json(orderDetails);
        }
        #endregion
    }
}
