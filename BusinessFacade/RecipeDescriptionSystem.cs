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
    public class RecipeDescriptionSystem
    {
        public RecipeDescriptionData GetRecipeDescription(int recipeID)
        {
            try
            {
                return new RecipeDescriptionDB().GetRecipeDescription(recipeID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int InsertUpdateRecipeDescription(RecipeDescriptionData recipeDescription)
        {
            try
            {
                return new RecipeDescriptionRule().InsertUpdateRecipeDescription(recipeDescription);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int DeleteRecipeDescriptions(IEnumerable<int> recipeDescriptionIDs)
        {
            try
            {
                return new RecipeDescriptionRule().DeleteRecipeDescriptions(recipeDescriptionIDs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
