using BusinessRule;
using Common.Data;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessFacade
{
    public class RecipeIngredientSystem
    {
        public List<RecipeIngredientData> GetRecipeIngredientList(int recipeID)
        {
            try
            {
                return new RecipeIngredientDB().GetRecipeIngredientList(recipeID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int InsertUpdateRecipeIngredient(RecipeIngredientData recipeIngredient)
        {
            try
            {
                return new RecipeIngredientRule().InsertUpdateRecipeIngredient(recipeIngredient);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int DeleteRecipeIngredients(IEnumerable<int> recipeIngredientIDs)
        {
            try
            {
                return new RecipeIngredientRule().DeleteRecipeIngredients(recipeIngredientIDs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
