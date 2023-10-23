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
    public class CustomerController : Controller
    {
        #region Do not modify signature

        readonly CustomerRepository repository;
        public CustomerController(CustomerRepository customerRepository)
        {
            repository = customerRepository;
        }

        #endregion

        #region GetAllItems

        [HttpGet]
        public JsonResult GetAllItems()
        {
            List<Item> items;
            try
            {
                items = repository.GetAllItems();
                if(items.Count == 0)
                {
                    return Json("No items are available!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                items = null;
            }
            return Json(items);
        }
        #endregion

        #region GetAllItemsByCategoryName

        [HttpGet]
        public JsonResult GetAllItemsByCategoryName(string categoryName)
        {
            List<ItemDetail> items;
            try
            {
                items = repository.GetItemDetails(categoryName);
                if (items.Count == 0) {
                    return Json("Invalid Category Name");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                items = null;
            }
            return Json(items);
        }
        #endregion

        #region GetItemPrice
        
        public JsonResult GetItemPrice(string itemId)
        {
            decimal response = 0;
            try
            {
                response = repository.GetItemPrice(itemId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Json(response);
        }
        #endregion

        #region PlaceOrder
        [HttpPost]
        public JsonResult PlaceOrder(Models.Order order)
        {
            string response;
            try
            {
                decimal totalPrice = 0;
                int orderId = 0;
                int status = repository.PlaceOrder(
                                            order.CustomerId, 
                                            order.ItemId, 
                                            order.Quantity, 
                                            order.DeliveryAddress, 
                                            order.OrderDate, 
                                            out totalPrice, 
                                            out orderId);

                if(status == 1)
                {
                    response = "Order Placed Successfully! Your orderId is " + orderId + " and Total Price to be paid is " + totalPrice;
                }
                else
                {
                    response = "Could not place your order now! Please try again after some time!";
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
    }
}
