using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OnlineFoodOrderDALCrossPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineFoodOrderDALCrossPlatform
{       
    public class AdminRepository
    {
        #region Do not modify the signature

        readonly OnlineFoodOrderDBContext context;
        public AdminRepository(OnlineFoodOrderDBContext onlineFoodOrderDBContext)
        {
            // To-do: Implement necessary code here
            context = onlineFoodOrderDBContext;
        }

        #endregion

        #region AddItem
        public bool AddItem(string itemId, string itemName, int categoryId, decimal price)
        {
            bool isAddedSuccessfully = false;
            try
            {
                Item itemToBeAdded = new Item { 
                                    ItemId = itemId, 
                                    ItemName = itemName, 
                                    CategoryId = categoryId, 
                                    Price = price 
                                  };

                context.Items.Add(itemToBeAdded);
                context.SaveChanges();
                isAddedSuccessfully = true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                isAddedSuccessfully = false;
            }
            return isAddedSuccessfully;
        }

        #endregion   

        #region GetAllCategoryOrderDetails
        public List<CategoryItemDetail> GetAllCategoryOrderDetails(int categoryId)
        {
            List<CategoryItemDetail> categoryItemDetails = null;
            try
            {
                SqlParameter prmCategoryId = new SqlParameter("@CategoryId", categoryId);
                categoryItemDetails = context.CategoryItemDetails.FromSqlRaw("SELECT * FROM ufn_GetAllOrderDetails(@CategoryId)", prmCategoryId).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return categoryItemDetails;
        }

        #endregion

        #region UpdatePrice
        public bool UpdatePrice(string itemId, decimal itemPrice)
        {
            bool isUpdated = false;
            try
            {
                Item item = context.Items.Find(itemId);
                if(item != null)
                {
                    item.Price = itemPrice;
                    context.SaveChanges();
                    isUpdated = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return isUpdated;
        }

        #endregion
    }
}
