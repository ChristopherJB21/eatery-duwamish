using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data
{
    public class RecipeData
    {
        private int _recipeID;
        private int _dishID;
        private string _recipeName;

        public int RecipeID
        {
            get { return _recipeID; }
            set { _recipeID = value; }
        }

        public int DishID
        {
            get { return _dishID; }
            set { _dishID = value; }
        }

        public string RecipeName
        {
            get { return _recipeName; }
            set { _recipeName = value; }
        }
    }
}
