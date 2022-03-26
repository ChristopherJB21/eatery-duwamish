using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data
{
    public class RecipeDescriptionData
    {
        private int _recipeDescriptionID;
        private int _recipeID;
        private string _description;

        public int RecipeDescription
        {
            get { return _recipeDescriptionID; }
            set { _recipeDescriptionID = value; }
        }

        public int RecipeID
        {
            get { return _recipeID; }
            set { _recipeID = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
    }
}
