using Common.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemFramework;

namespace DataAccess
{
    public class RecipeIngredientDB
    {
        public List<RecipeIngredientData> GetRecipeIngredientList(int recipeID)
        {
            try
            {
                string SpName = "dbo.RecipeIngredient_Get";
                List<RecipeIngredientData> ListRecipeIngredient = new List<RecipeIngredientData>();
                using (SqlConnection SqlConn = new SqlConnection())
                {
                    SqlConn.ConnectionString = SystemConfigurations.EateryConnectionString;
                    SqlConn.Open();
                    SqlCommand SqlCmd = new SqlCommand(SpName, SqlConn);
                    SqlCmd.CommandType = CommandType.StoredProcedure;
                    SqlCmd.Parameters.Add(new SqlParameter("@RecipeId", recipeID));
                    using (SqlDataReader Reader = SqlCmd.ExecuteReader())
                    {
                        if (Reader.HasRows)
                        {
                            while (Reader.Read())
                            {
                                RecipeIngredientData recipeIngredient = new RecipeIngredientData();
                                recipeIngredient.RecipeIngredientID = Convert.ToInt32(Reader["RecipeIngredientID"]);
                                recipeIngredient.RecipeID = Convert.ToInt32(Reader["RecipeID"]);
                                recipeIngredient.Ingredient = Convert.ToString(Reader["Ingredient"]);
                                recipeIngredient.Quantity = Convert.ToInt32(Reader["Quantity"]);
                                recipeIngredient.Unit = Convert.ToString(Reader["Unit"]);
                                ListRecipeIngredient.Add(recipeIngredient);
                            }
                        }
                    }
                    SqlConn.Close();
                }
                return ListRecipeIngredient;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertUpdateRecipeIngredient(RecipeIngredientData recipeIngredient, SqlTransaction SqlTran)
        {
            try
            {
                string SpName = "dbo.RecipeIngredient_InsertUpdate";
                SqlCommand SqlCmd = new SqlCommand(SpName, SqlTran.Connection, SqlTran);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlParameter RecipeIngredientId = new SqlParameter("@RecipeIngredientID", recipeIngredient.RecipeIngredientID);
                RecipeIngredientId.Direction = ParameterDirection.InputOutput;
                SqlCmd.Parameters.Add(RecipeIngredientId);

                SqlCmd.Parameters.Add(new SqlParameter("@RecipeID", recipeIngredient.RecipeID));
                SqlCmd.Parameters.Add(new SqlParameter("@Ingredient", recipeIngredient.Ingredient));
                SqlCmd.Parameters.Add(new SqlParameter("@Quantity", recipeIngredient.Quantity));
                SqlCmd.Parameters.Add(new SqlParameter("@Unit", recipeIngredient.Unit));
                return SqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteRecipeIngredients(string recipeIngredientIDs, SqlTransaction SqlTran)
        {
            try
            {
                string SpName = "dbo.RecipeIngredient_Delete";
                SqlCommand SqlCmd = new SqlCommand(SpName, SqlTran.Connection, SqlTran);
                SqlCmd.CommandType = CommandType.StoredProcedure;
                SqlCmd.Parameters.Add(new SqlParameter("@RecipeIngredientIDs", recipeIngredientIDs));
                return SqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
