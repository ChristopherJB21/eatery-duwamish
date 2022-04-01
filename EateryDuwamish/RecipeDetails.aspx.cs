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
    public partial class RecipeDetails : System.Web.UI.Page
    {
        protected int recipeID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Session["RecipeID"] as string))
            {
                recipeID = Convert.ToInt32(Session["RecipeID"].ToString());
                litRecipeName.Text = Session["RecipeName"].ToString().ToUpper();
                litRecipeName2.Text = Session["RecipeName"].ToString().ToUpper();

                if (!IsPostBack)
                {
                    ShowNotificationIfExists();
                    LoadIngredientTable();
                    LoadDescription();
                }

            }
            else
            {
                Response.Redirect("Recipes.aspx");
            }

            if (!IsPostBack)
            {
                ShowNotificationIfExists();
                LoadIngredientTable();
            }
        }

        #region FORM MANAGEMENT
        private void FillForm(RecipeIngredientData ingredient)
        {
            hdfRecipeIngredientId.Value = ingredient.RecipeIngredientID.ToString();
            txtIngredient.Text = ingredient.Ingredient.ToString();
            txtQuantity.Text = ingredient.Quantity.ToString();
            txtUnit.Text = ingredient.Unit;
        }
        private void ResetForm()
        {
            hdfRecipeIngredientId.Value = String.Empty;
            txtIngredient.Text = String.Empty;
            txtQuantity.Text = String.Empty;
            txtUnit.Text = String.Empty;
        }
        private RecipeIngredientData GetFormIngredientData()
        {
            RecipeIngredientData ingredient = new RecipeIngredientData();
            ingredient.RecipeIngredientID = String.IsNullOrEmpty(hdfRecipeIngredientId.Value) ? 0 : Convert.ToInt32(hdfRecipeIngredientId.Value);
            ingredient.Ingredient = txtIngredient.Text;
            ingredient.Quantity = Convert.ToInt32(txtQuantity.Text);
            ingredient.Unit = txtUnit.Text;
            ingredient.RecipeID = recipeID;
            
            return ingredient;

        }
        private RecipeDescriptionData GetFormDescriptionData()
        {
            RecipeDescriptionData description = new RecipeDescriptionData();
            description.RecipeDescriptionID = String.IsNullOrEmpty(hdfRecipeDescriptionId.Value) ? 0 : Convert.ToInt32(hdfRecipeDescriptionId.Value);
            description.RecipeID = recipeID;
            description.Description = String.IsNullOrEmpty(txtDescription.Text) ? "" : txtDescription.Text.ToString();

            return description;
        }
        #endregion
        
        #region DATA TABLE MANAGEMENT
        private void LoadIngredientTable()
        {
            try
            {
                List<RecipeIngredientData> ListIngredient = new RecipeIngredientSystem().GetRecipeIngredientList(recipeID);
                rptIngredient.DataSource = ListIngredient;
                rptIngredient.DataBind();
            }
            catch (Exception ex)
            {
                notifDetail.Show($"ERROR LOAD TABLE: {ex.Message}", NotificationType.Danger);
            }
        }
        private void LoadDescription()
        {
            try
            {
                RecipeDescriptionData description = new RecipeDescriptionSystem().GetRecipeDescription(recipeID);
                txtDescription.Text = description.Description;
                hdfRecipeDescriptionId.Value = description.RecipeDescriptionID.ToString();
            }
            catch (Exception ex)
            {
                notifDetail.Show($"ERROR LOAD DESCRIPTION: {ex.Message}", NotificationType.Danger);
            }
        }
        protected void rptRecipe_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                RecipeIngredientData ingredient = (RecipeIngredientData)e.Item.DataItem;
                LinkButton lbIngredient = (LinkButton)e.Item.FindControl("lbIngredient");
                Literal lbQuantity = (Literal)e.Item.FindControl("lbQuantity");
                Literal lbUnit = (Literal)e.Item.FindControl("lbUnit");

                lbIngredient.Text = ingredient.Ingredient;
                lbIngredient.CommandArgument = ingredient.RecipeIngredientID.ToString();

                lbQuantity.Text = ingredient.Quantity.ToString();
                lbUnit.Text = ingredient.Unit.ToString();

                CheckBox chkChoose = (CheckBox)e.Item.FindControl("chkChoose");
                chkChoose.Attributes.Add("data-value", ingredient.RecipeIngredientID.ToString());
            }
        }
        protected void rptRecipe_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "EDIT")
            {
                LinkButton lbIngredient = (LinkButton)e.Item.FindControl("lbIngredient");
                Literal lbQuantity = (Literal)e.Item.FindControl("lbQuantity");
                Literal lbUnit = (Literal)e.Item.FindControl("lbUnit");

                int recipeIngredientId = Convert.ToInt32(e.CommandArgument.ToString());
                FillForm(new RecipeIngredientData
                {
                    RecipeIngredientID = recipeIngredientId,
                    RecipeID = recipeID,
                    Ingredient = lbIngredient.Text.ToString(),
                    Quantity = Convert.ToInt32(lbQuantity.Text),
                    Unit = lbUnit.Text.ToString()
                });

                litFormType.Text = $"UBAH: {lbIngredient.Text}";
                pnlFormIngredient.Visible = true;
                btnSave.Text = "Save Change";
                txtIngredient.Focus();
            }
        }
        #endregion
        
        #region BUTTON EVENT MANAGEMENT
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                RecipeIngredientData ingredient = GetFormIngredientData();
                int rowAffected = new RecipeIngredientSystem().InsertUpdateRecipeIngredient(ingredient);
                if (rowAffected <= 0)
                    throw new Exception("No Data Recorded");
                Session["save-success"] = 1;
                Response.Redirect("RecipeDetails.aspx");
            }
            catch (Exception ex)
            {
                notifDetail.Show($"ERROR SAVE DATA: {ex.Message}", NotificationType.Danger);
            }
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ResetForm();
            litFormType.Text = $"TAMBAH";
            pnlFormIngredient.Visible = true;
            btnSave.Text = "SAVE";
            txtIngredient.Focus();
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string strDeletedIDs = hdfDeletedIngredients.Value;
                IEnumerable<int> deletedIDs = strDeletedIDs.Split(',').Select(Int32.Parse);
                int rowAffected = new RecipeIngredientSystem().DeleteRecipeIngredients(deletedIDs);
                if (rowAffected <= 0)
                    throw new Exception("No Data Deleted");
                Session["delete-success"] = 1;
                Response.Redirect("RecipeDetails.aspx");
            }
            catch (Exception ex)
            {
                notifDetail.Show($"ERROR DELETE DATA: {ex.Message}", NotificationType.Danger);
            }
        }
        protected void btn_EditDescription_Click(object sender, EventArgs e)
        {
            if (btn_EditDescription.Text == "EDIT")
            {
                btn_EditDescription.Text = "SAVE";
                txtDescription.ReadOnly = false;
            } else if (btn_EditDescription.Text == "SAVE")
            {
                try
                {
                    RecipeDescriptionData description = GetFormDescriptionData();
                    int rowAffected = new RecipeDescriptionSystem().InsertUpdateRecipeDescription(description);
                    if (rowAffected <= 0)
                        throw new Exception("No Data Recorded");
                    btn_EditDescription.Text = "EDIT";
                    txtDescription.ReadOnly = true;
                    Session["save-success"] = 1;
                    Response.Redirect("RecipeDetails.aspx");
                }
                catch (Exception ex)
                {
                    notifDetail.Show($"ERROR SAVE DATA: {ex.Message}", NotificationType.Danger);
                }
            }
        }
        #endregion

        #region NOTIFICATION MANAGEMENT
        private void ShowNotificationIfExists()
        {
            if (Session["save-success"] != null)
            {
                notifDetail.Show("Data sukses disimpan", NotificationType.Success);
                Session.Remove("save-success");
            }
            if (Session["delete-success"] != null)
            {
                notifDetail.Show("Data sukses dihapus", NotificationType.Success);
                Session.Remove("delete-success");
            }
        }
        #endregion
    }

}