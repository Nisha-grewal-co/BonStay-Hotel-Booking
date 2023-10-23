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
    public class AdminController : Controller
    {
        #region Do not modify signature
        
        AdminRepository repository;
        public AdminController(AdminRepository adminRepository)
        {
            repository = adminRepository;
        }
        #endregion

        #region AddItem

        [HttpPost]
        public JsonResult AddItem(Models.Item item)
        {
            string response;
            try
            {
                bool status = repository.AddItem(item.ItemId, item.ItemName, item.CategoryId, item.Price);
                if (status)
                {
                    response = "Item was added successfully!";
                }
                else
                {
                    response = "Could not add this item to the database!";
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

        #region GetAllCategoryOrderDetails

        [HttpGet]
        public JsonResult GetAllCategoryOrderDetails(int categoryId)
        {
            List<CategoryItemDetail> categoryItemDetails = null;
            try
            {
                categoryItemDetails = repository.GetAllCategoryOrderDetails(categoryId);
                if(categoryItemDetails.Count == 0)
                {
                    return Json("No products with the given Category Id");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(ex.Message);
            }
            return Json(categoryItemDetails);
        }
        #endregion

        #region UpdatePrice

        [HttpPut]
        public JsonResult UpdatePrice(string itemId, decimal price)
        {
            string response;
            try
            {
                bool status = repository.UpdatePrice(itemId, price);
                if (status)
                {
                    response = "Price updated successfully!";
                }
                else
                {
                    response = "Could not update the price successfully!";
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
