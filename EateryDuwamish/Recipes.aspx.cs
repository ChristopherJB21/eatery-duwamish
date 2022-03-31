using BusinessFacade;
using Common.Data;
using Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EateryDuwamish
{
    public partial class Recipes : System.Web.UI.Page
    {
        protected int dishID;
        protected const string DEFAULT_DDL_VALUE = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Session["DishID"] as string))
            {
                dishID = Convert.ToInt32(Session["DishID"].ToString());
                litDishID.Text = Session["DishName"].ToString();
                /*Session.Remove("DishID");*/
                /*Session.Remove("DishName");*/
                
                if (!IsPostBack)
                {
                    ShowNotificationIfExists();
                    LoadRecipeTable();
                }

            } else
            {
                Response.Redirect("Dish.aspx");
            }
        }

        #region FORM MANAGEMENT
        private void FillForm(RecipeData recipe)
        {
            hdfRecipeId.Value = recipe.RecipeID.ToString();
            txtRecipeName.Text = recipe.RecipeName;
        }
        private void ResetForm()
        {
            hdfRecipeId.Value = String.Empty;
            txtRecipeName.Text = String.Empty;
        }
        private RecipeData GetFormData()
        {
            RecipeData recipe = new RecipeData();
            recipe.RecipeID = String.IsNullOrEmpty(hdfRecipeId.Value) ? 0 : Convert.ToInt32(hdfRecipeId.Value);
            recipe.RecipeName = txtRecipeName.Text.ToString();
            recipe.DishID = dishID;
            return recipe;
            
        }
        #endregion
        
        #region DATA TABLE MANAGEMENT
        private void LoadRecipeTable()
        {
            try
            {
                List<RecipeData> ListRecipe = new RecipeSystem().GetRecipeList(dishID);
                rptRecipe.DataSource = ListRecipe;
                rptRecipe.DataBind();
            }
            catch (Exception ex)
            {
                notifRecipe.Show($"ERROR LOAD TABLE: {ex.Message}", NotificationType.Danger);
            }
        }
        protected void rptRecipe_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                RecipeData recipe = (RecipeData)e.Item.DataItem;
                LinkButton lbRecipeName = (LinkButton)e.Item.FindControl("lbRecipeName");

                lbRecipeName.Text = recipe.RecipeName;
                lbRecipeName.CommandArgument = recipe.RecipeID.ToString();
                
                CheckBox chkChoose = (CheckBox)e.Item.FindControl("chkChoose");
                chkChoose.Attributes.Add("data-value", recipe.RecipeID.ToString());
            }
        }
        protected void rptRecipe_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "EDIT")
            {
                LinkButton lbRecipeName = (LinkButton)e.Item.FindControl("lbRecipeName");

                int recipeID = Convert.ToInt32(e.CommandArgument.ToString());
                FillForm(new RecipeData
                {
                    RecipeID = recipeID,
                    RecipeName = lbRecipeName.Text.ToString(),
                    DishID = dishID
                });
                litFormType.Text = $"UBAH: {lbRecipeName.Text}";
                pnlFormRecipe.Visible = true;
                btnSave.Text = "SAVE CHANGE";
                txtRecipeName.Focus();
            } else if (e.CommandName == "DETAIL")
            {
                /*
                int recipeID = Convert.ToInt32(e.CommandArgument.ToString());
                LinkButton lbRecipeName = (LinkButton)e.Item.FindControl("lbRecipeName");
                
                Session["RecipeID"] = recipeID.ToString();
                Session["RecipeName"] = lbRecipeName.Text;
                

                Response.Redirect("Recipes.aspx");
                */
            }
        }
        #endregion


        #region BUTTON EVENT MANAGEMENT
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                RecipeData recipe = GetFormData();
                int rowAffected = new RecipeSystem().InsertUpdateRecipe(recipe);
                if (rowAffected <= 0)
                    throw new Exception("No Data Recorded");
                Session["save-success"] = 1;
                Response.Redirect("Recipes.aspx");
            }
            catch (Exception ex)
            {
                notifRecipe.Show($"ERROR SAVE DATA: {ex.Message}", NotificationType.Danger);
            }
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ResetForm();
            litFormType.Text = $"TAMBAH";
            pnlFormRecipe.Visible = true;
            btnSave.Text = "SAVE";
            txtRecipeName.Focus();
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string strDeletedIDs = hdfDeletedRecipes.Value;
                IEnumerable<int> deletedIDs = strDeletedIDs.Split(',').Select(Int32.Parse);
                int rowAffected = new RecipeSystem().DeleteRecipes(deletedIDs);
                if (rowAffected <= 0)
                    throw new Exception("No Data Deleted");
                Session["delete-success"] = 1;
                Response.Redirect("Recipes.aspx");
            }
            catch (Exception ex)
            {
                notifRecipe.Show($"ERROR DELETE DATA: {ex.Message}", NotificationType.Danger);
            }
        }
        #endregion


        #region NOTIFICATION MANAGEMENT
        private void ShowNotificationIfExists()
        {
            if (Session["save-success"] != null)
            {
                notifRecipe.Show("Data sukses disimpan", NotificationType.Success);
                Session.Remove("save-success");
            }
            if (Session["delete-success"] != null)
            {
                notifRecipe.Show("Data sukses dihapus", NotificationType.Success);
                Session.Remove("delete-success");
            }
        }
        #endregion

    }
}