using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data
{
    public class RecipeIngredientData
    {
        private int _recipeIngredientID;
        private int _recipeID;
        private string _ingredient;
        private int _quantity;
        private string _unit;

        public int RecipeIngredientID
        {
            get { return _recipeIngredientID; }
            set { _recipeIngredientID = value; }
        }

        public int RecipeID
        {
            get { return _recipeID; }
            set { _recipeID = value; }
        }

        public string Ingredient
        {
            get { return _ingredient; }
            set { _ingredient = value; }
        }

        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        public string Unit
        {
            get { return _unit; }
            set { _unit = value; }
        }
    }
}
