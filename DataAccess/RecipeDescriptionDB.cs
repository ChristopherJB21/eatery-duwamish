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
    public class RecipeDescriptionDB
    {
        public RecipeDescriptionData GetRecipeDescription(int recipeID)
        {
            try
            {
                string SpName = "dbo.RecipeDescription_Get";
                RecipeDescriptionData RecipeDescription = new RecipeDescriptionData();
                using (SqlConnection SqlConn = new SqlConnection())
                {
                    SqlConn.ConnectionString = SystemConfigurations.EateryConnectionString;
                    SqlConn.Open();
                    SqlCommand SqlCmd = new SqlCommand(SpName, SqlConn);
                    SqlCmd.CommandType = CommandType.StoredProcedure;
                    SqlCmd.Parameters.Add(new SqlParameter("@RecipeId", recipeID));
                    using (SqlDataReader Reader = SqlCmd.ExecuteReader())
                    {
                        if (Reader.HasRows && Reader.Read())
                        {
                            RecipeDescription.RecipeDescriptionID = Convert.ToInt32(Reader["RecipeDescriptionID"]);
                            RecipeDescription.RecipeID = Convert.ToInt32(Reader["RecipeID"]);
                            RecipeDescription.Description = Convert.ToString(Reader["Description"]);
                        }
                    }
                    SqlConn.Close();
                }
                return RecipeDescription;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertUpdateRecipeDescription(RecipeDescriptionData recipeDescription, SqlTransaction SqlTran)
        {
            try
            {
                string SpName = "dbo.RecipeDescription_InsertUpdate";
                SqlCommand SqlCmd = new SqlCommand(SpName, SqlTran.Connection, SqlTran);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlParameter RecipeDescriptionId = new SqlParameter("@RecipeDescriptionID", recipeDescription.RecipeDescriptionID);
                RecipeDescriptionId.Direction = ParameterDirection.InputOutput;
                SqlCmd.Parameters.Add(RecipeDescriptionId);

                SqlCmd.Parameters.Add(new SqlParameter("@RecipeID", recipeDescription.RecipeID));
                SqlCmd.Parameters.Add(new SqlParameter("@Description", recipeDescription.Description));
                return SqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteRecipeDescriptions(string recipeDescriptionIDs, SqlTransaction SqlTran)
        {
            try
            {
                string SpName = "dbo.RecipeDescription_Delete";
                SqlCommand SqlCmd = new SqlCommand(SpName, SqlTran.Connection, SqlTran);
                SqlCmd.CommandType = CommandType.StoredProcedure;
                SqlCmd.Parameters.Add(new SqlParameter("@recipeDescriptionIDs", recipeDescriptionIDs));
                return SqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
