<%@ Page Title="Recipe Detail" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RecipeDetails.aspx.cs" Inherits="EateryDuwamish.RecipeDetails" %>
<%@ Register Src="~/UserControl/NotificationControl.ascx" TagName="NotificationControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <%--Datatable Configuration--%>
    <script type="text/javascript">
        function ConfigureDatatable() {
            var table = null;
            if ($.fn.dataTable.isDataTable('#htblIngredient')) {
                table = $('#htblIngredient').DataTable();
            }
            else {
                table = $('#htblIngredient').DataTable({
                    stateSave: false,
                    order: [[1, "asc"]],
                    columnDefs: [{ orderable: false, targets: [0] }]
                });
            }
            return table;
        }
    </script>
    <%--Checkbox Event Configuration--%>
    <script type="text/javascript">
        function ConfigureCheckboxEvent() {
            $('.checkDelete input').change(function () {
                var parent = $(this).parent();
                var value = $(parent).attr('data-value');
                var deletedList = [];

                if ($('#<%=hdfDeletedIngredients.ClientID%>').val())
                    deletedList = $('#<%=hdfDeletedIngredients.ClientID%>').val().split(',');

                if ($(this).is(':checked')) {
                    deletedList.push(value);
                    $('#<%=hdfDeletedIngredients.ClientID%>').val(deletedList.join(','));
                }
                else {
                    var index = deletedList.indexOf(value);
                    if (index >= 0)
                        deletedList.splice(index, 1);
                    $('#<%=hdfDeletedIngredients.ClientID%>').val(deletedList.join(','));
                }
            });
        }
    </script>
    <%--Main Configuration--%>
    <script type="text/javascript">
        function ConfigureElements() {
            ConfigureDatatable();
            ConfigureCheckboxEvent();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <script type="text/javascript">
                $(document).ready(function () {
                    ConfigureElements();
                });
                <%--On Partial Postback Callback Function--%>
                var prm = Sys.WebForms.PageRequestManager.getInstance();
                prm.add_endRequest(function () {
                    ConfigureElements();
                });
            </script>
            <uc1:NotificationControl ID="notifDetail" runat="server" />
            <div class="page-title">Details</div><hr style="margin:0"/>
            
            <%--FORM Ingredient--%>
            <asp:Panel runat="server" ID="pnlFormIngredient" Visible="false">
                <div class="form-slip">
                    <div class="form-slip-header">
                        <div class="form-slip-title">
                            FORM INGREDIENT - 
                            <asp:Literal runat="server" ID="litFormType"></asp:Literal>
                        </div>
                        <hr style="margin:0"/>
                    </div>
                    <div class="form-slip-main">
                        <asp:HiddenField ID="hdfRecipeIngredientId" runat="server" Value="0"/>
                        <div>
                            <%--Ingredient Field--%>
                            <div class="col-lg-6 form-group">
                                <div class="col-lg-4 control-label">
                                    Ingredient*
                                </div>
                                <div class="col-lg-6">
                                    <asp:TextBox ID="txtIngredient" CssClass="form-control" runat="server"></asp:TextBox>
                                    <%--Validator--%>
                                    <asp:RequiredFieldValidator ID="rfvIngredient" runat="server" ErrorMessage="Please fill this field"
                                        ControlToValidate="txtIngredient" ForeColor="Red" 
                                        ValidationGroup="InsertUpdateIngredient" Display="Dynamic">
                                    </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revIngredient" runat="server" ErrorMessage="This field has a maximum of 100 characters"
                                        ControlToValidate="txtIngredient" ValidationExpression="^[\s\S]{0,100}$" ForeColor="Red"
                                        ValidationGroup="InsertUpdateIngredient" Display="Dynamic">
                                    </asp:RegularExpressionValidator>
                                    <%--End of Validator--%>
                                </div>
                            </div>
                            <%--End of Ingredient Field--%>
                            <%--Quantity Field--%>
                            <div class="col-lg-6 form-group">
                                <div class="col-lg-4 control-label">
                                    Quantity*
                                </div>
                                <div class="col-lg-6">
                                    <asp:TextBox ID="txtQuantity" CssClass="form-control" runat="server" type="number"
                                        Min="0" Max="999999999"></asp:TextBox>
                                    <%--Validator--%>
                                    <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ErrorMessage="Please fill this field"
                                        ControlToValidate="txtQuantity" ForeColor="Red"
                                        ValidationGroup="InsertUpdateIngredient" Display="Dynamic">
                                    </asp:RequiredFieldValidator>
                                    <%--End of Validator--%>
                                </div>
                            </div>
                            <%--End of Quantity Field--%>
                            <%--Unit Field--%>
                            <div class="col-lg-6 form-group">
                                <div class="col-lg-4 control-label">
                                    Unit*
                                </div>
                                <div class="col-lg-6">
                                    <asp:TextBox ID="txtUnit" CssClass="form-control" runat="server"></asp:TextBox>
                                    <%--Validator--%>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please fill this field"
                                        ControlToValidate="txtUnit" ForeColor="Red" 
                                        ValidationGroup="InsertUpdateIngredient" Display="Dynamic">
                                    </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="This field has a maximum of 100 characters"
                                        ControlToValidate="txtUnit" ValidationExpression="^[\s\S]{0,100}$" ForeColor="Red"
                                        ValidationGroup="InsertUpdateIngredient" Display="Dynamic">
                                    </asp:RegularExpressionValidator>
                                    <%--End of Validator--%>
                                </div>
                            </div>
                            <%--End of Quantity Field--%>
                        </div>
                        <div class="col-lg-12">
                            <div class="col-lg-2">
                            </div>
                            <div class="col-lg-2">
                                <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Width="150px"
                                    Text="SAVE" ValidationGroup="InsertUpdateIngredient" OnClick="btnSave_Click">
                                </asp:Button>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <%--END OF FORM Ingredient--%>

            <div class="row">
                <div class="table-header">
                    <div class="table-header-title">
                        <asp:Literal runat="server" ID="litRecipeName"></asp:Literal>
                        INGREDIENT
                    </div>
                    <div class="table-header-button">
                        <asp:Button ID="btnAdd" runat="server" Text="ADD" CssClass="btn btn-primary" Width="100px" OnClick="btnAdd_Click" />
                        <asp:Button ID="btnDelete" runat="server" Text="DELETE" CssClass="btn btn-danger" Width="100px" OnClick="btnDelete_Click" />
                    </div>
                </div>
            </div>
            
            <div class="row">
                <div class="table-main col-sm-12">
                    <asp:HiddenField ID="hdfDeletedIngredients" runat="server" />
                    <asp:Repeater ID="rptIngredient" runat="server" Visible="true" OnItemDataBound="rptRecipe_ItemDataBound" OnItemCommand="rptRecipe_ItemCommand">
                        <HeaderTemplate>
                            <table id="htblIngredient" class="table">
                                <thead>
                                    <tr role="row">
                                        <th aria-sort="ascending" style="" colspan="1" rowspan="1"
                                            tabindex="0" class="sorting_asc center">
                                        </th>
                                        <th aria-sort="ascending" style="" colspan="1" rowspan="1" tabindex="0"
                                            class="sorting_asc text-center">
                                            Ingredient
                                        </th>
                                        <th aria-sort="ascending" style="" colspan="1" rowspan="1" tabindex="0"
                                            class="sorting_asc text-center">
                                            Quantity
                                        </th>
                                        <th aria-sort="ascending" style="" colspan="1" rowspan="1" tabindex="0"
                                            class="sorting_asc text-center">
                                            Unit
                                        </th>
                                    </tr>
                                </thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="odd" role="row" runat="server">
                                <td>
                                    <div style="text-align: center;">
                                        <asp:CheckBox ID="chkChoose" CssClass="checkDelete" runat="server">
                                        </asp:CheckBox>
                                    </div>
                                </td>
                                <td class="text-center align-middle">
                                    <asp:LinkButton ID="lbIngredient" runat="server" CommandName="EDIT"></asp:LinkButton>
                                </td>
                                <td class="text-center align-middle">
                                    <asp:Literal ID="lbQuantity" runat="server"></asp:Literal>
                                </td>
                                <td class="text-center align-middle">
                                    <asp:Literal ID="lbUnit" runat="server"></asp:Literal>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </tbody> 
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>

            <%--FORM Description--%>
            <asp:Panel runat="server" ID="pnlFormDescription" Visible="true">
                <div class="form-slip">
                    <div class="form-slip-header">
                        <div class="form-slip-title">
                            <asp:Literal runat="server" ID="litRecipeName2"></asp:Literal>
                            DESCRIPTION
                        </div>
                        <hr style="margin:0"/>
                    </div>
                    <div class="form-slip-main">
                        <asp:HiddenField ID="hdfRecipeDescriptionId" runat="server" Value="0"/>
                        <div class="col-lg-20">
                            <%--Ingredient Field--%>
                            <div class="col-lg-12 form-group">
                                <div class="col-lg-2 control-label">
                                    Description*
                                </div>
                                <div class="col-lg-10">
                                    <asp:TextBox ID="txtDescription" CssClass="form-control" runat="server" Height="250px" ReadOnly="true" TextMode="MultiLine"></asp:TextBox>
                                    <%--Validator--%>
                                    <asp:RegularExpressionValidator ID="revDescription" runat="server" ErrorMessage="This field has a maximum of 1000 characters"
                                        ControlToValidate="txtDescription" ValidationExpression="^[\s\S]{0,1000}$" ForeColor="Red"
                                        ValidationGroup="InsertUpdateDescription" Display="Dynamic">
                                    </asp:RegularExpressionValidator>
                                    <%--End of Validator--%>
                                </div>
                            </div>
                            <%--End of Ingredient Field--%>
                        </div>
                        <div class="col-lg-12">
                            <div class="col-lg-2">
                            </div>
                            <div class="col-lg-2">
                                <asp:Button runat="server" ID="btn_EditDescription" CssClass="btn btn-primary" Width="150px"
                                    Text="EDIT" ValidationGroup="InsertUpdateDescription" OnClick="btn_EditDescription_Click">
                                </asp:Button>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <%--END OF FORM Ingredient--%>

            <div><br /><br /></div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>