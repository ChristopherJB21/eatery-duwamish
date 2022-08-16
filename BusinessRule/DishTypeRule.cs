using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Data;
using BusinessRule.Helper;
using DataAccess;

namespace BusinessRule
{
    public class DishTypeRule
    {
        private static RedisHelper redisHelper = new RedisHelper();
        private static string key = "GetDishTypeList";
        public List<DishTypeData> GetDishTypeList()
        {
            try
            {
                List<DishTypeData> dishTypeList = new List<DishTypeData>();

                dishTypeList = redisHelper.GetClassStringKeyValue<List<DishTypeData>>(key);

                if (dishTypeList == null)
                {
                    dishTypeList = new DishTypeDB().GetDishTypeList();

                    TimeSpan expiredTime = new TimeSpan(0, 1, 0, 0);
                    redisHelper.SetClassKeyStringValue<List<DishTypeData>>(key, dishTypeList, expiredTime);
                }

                return dishTypeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DishTypeData GetDishTypeByID(int dishTypeID)
        {
            try
            {
                DishTypeData dishType = new DishTypeData();
                List<DishTypeData> dishTypeList = new List<DishTypeData>();

                dishTypeList = redisHelper.GetClassStringKeyValue<List<DishTypeData>>(key);

                if (dishTypeList == null)
                {
                    dishTypeList = new DishTypeDB().GetDishTypeList();

                    TimeSpan expiredTime = new TimeSpan(0, 1, 0, 0);
                    redisHelper.SetClassKeyStringValue<List<DishTypeData>>(key, dishTypeList, expiredTime);
                }

                dishType = dishTypeList.Where(x => dishTypeID == x.DishTypeID).FirstOrDefault();
                return dishType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
