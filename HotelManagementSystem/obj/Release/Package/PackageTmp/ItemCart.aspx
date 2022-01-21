<%@ Page Title="Item Cart" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ItemCart.aspx.vb" Inherits="HotelManagementSystem.ItemCart" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.3/jquery.min.js"></script>
    <script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js"></script>
    <script type="text/javascript">
        function showModal() {
            $("#myModal").modal('show');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <!-- Content Header (Page header) -->
    <section class="content-header">
      <h1>
        Item Cart
           <small>Food Services</small>
      </h1>
      <ol class="breadcrumb">
        <li> <asp:HyperLink ID="brdhlDashboard" runat="server" NavigateUrl="~/Default.aspx"> <i class="fa fa-dashboard"></i>  <span>Home</span> </asp:HyperLink>  </li>
        <li class="active">Item Cart</li>
      </ol>
    </section>



   <!-- Main content -->
    <section class="content">
      <div class="row">
        <!-- left column -->
        <div class="col-md-6">

          <!-- Form Element sizes -->
          <div class="box box-info">
            <div class="box-header with-border">
              <h3 class="box-title">Billing Information</h3>
            </div>
           <div class="box-body">
               <div class="row">
                   <div class="col-md-4">
                              <div class="form-group">
                                  <label for="txtBillNo">BILL NO</label>
                                    <asp:TextBox ID="txtBillNo" ReadOnly="true" CssClass="form-control" runat="server" Font-Bold="True"></asp:TextBox>
                              </div>
                   </div>
                   <div class="col-md-4">
                              <div class="form-group">
                                  <label for="txtBillDate">BILL DATE</label>
                                    <asp:TextBox ID="txtBillDate" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
                              </div>
                   </div>
                   <div class="col-md-4">
                                <label for="txtGuestID">GUEST ID</label> <img id="loader" style="display: none;" src="Assets/img/ajax-loader.gif" width="43" height="11" /> 
                             <div class="input-group">
                                    <asp:TextBox ID="txtGuestID" TextMode="Number" CssClass="form-control" runat="server"></asp:TextBox>
                                    <span class="input-group-btn">
                                        <asp:Button ID="btnFindGuest" CssClass="btn btn-info btn-flat" runat="server" Text="FIND" />
                                    </span>
                              </div>
                   </div>
               </div>

               <div class="form-group">
                   <label for="txtGuestName">Guest Name</label>
                   <asp:TextBox ID="txtGuestName" CssClass="form-control" placeholder="Guest Name" ReadOnly="true" runat="server"></asp:TextBox>
               </div>

                    <div class="row">
                        <div class="col-md-4">
                            <label for="txtTax1">TAX 1 : <asp:Label ID="lblTAX1" runat="server" Text=""></asp:Label> </label>
                            <asp:TextBox ID="txtTax1" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-md-4">
                            <label for="txtTax2">TAX 2 : <asp:Label ID="lblTAX2" runat="server" Text=""></asp:Label></label>
                            <asp:TextBox ID="txtTax2" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-md-4">
                            <label for="txtTax3">TAX 3 : <asp:Label ID="lblTAX3" runat="server" Text=""></asp:Label></label>
                            <asp:TextBox ID="txtTax3" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>

               <div class="row">
                   <div class="col-md-4">
                       <label for="txtItemCost">ITEM COST
                       </label>
                       <asp:TextBox ID="txtItemCost" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                   </div>
                   <div class="col-md-4">
                       <label for="txtTotalTax">TOTAL TAX</label>
                       <asp:TextBox ID="txtTotalTax" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                   </div>
                   <div class="col-md-4">
                       <label for="txtGTotal">G. TOTAL</label>
                       <asp:TextBox ID="txtGTotal" CssClass="form-control" ReadOnly="true" runat="server" ></asp:TextBox>
                   </div>
               </div>


               <div class="row">
                   <div class="col-md-4">
                       <label for="txtDiscount">DISCOUNT</label>
                       <asp:TextBox ID="txtDiscount" CssClass="form-control" runat="server" ></asp:TextBox>
                   </div>
                   <div class="col-md-4">
                       <label for="txtPayable">PAYABLE</label>
                       <asp:TextBox ID="txtPayable" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                   </div>
                   <div class="col-md-4">
                       <label for="txtPayment">PAYMENT</label>
                       <asp:TextBox ID="txtPayment" CssClass="form-control" runat="server"></asp:TextBox>
                   </div>
               </div>


           </div>
           <!-- /.box-body -->
       </div>
          <!-- /.box -->
        </div>
        <!--/.col (left) -->
        <div class="col-md-6">
            <div class="box box-info">
                <div class="box-body">

                   <div class="row">
                        <div class="col-md-8">
                            <label for="ddlFoodItem">Food Item Name</label>
                             <asp:DropDownList ID="ddlFoodItem" CssClass="form-control select2" Width="100%" runat="server"></asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                             <label for="txtQuantity">Quantity</label>
                             <div class="input-group">
                                    <asp:TextBox ID="txtQuantity"  CssClass="form-control" runat="server"></asp:TextBox>
                                    <span class="input-group-btn">
                                        <asp:Button ID="btnSoldItem" CssClass="btn btn-info btn-flat" runat="server" Text="ADD" />
                                    </span>
                              </div>
                        </div>
                    </div>
                    <br />
                    <asp:GridView ID="gvItemCart" CssClass="table table-bordered table-condensed table-responsive" CellSpacing="0" runat="server" AutoGenerateColumns="false">
                        <Columns>
                            <asp:TemplateField HeaderText="ID"  ItemStyle-CssClass="ROW_ID">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("ROW_ID") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ITEM NAME"  ItemStyle-CssClass="FOOD_ITEM_NAME">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("FOOD_ITEM_NAME") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Quantity"  ItemStyle-CssClass="Quantity">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("Quantity") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Price"  ItemStyle-CssClass="UnitPrice">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("UnitPrice") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total"  ItemStyle-CssClass="TotalPrice">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("TotalPrice") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton Text="Delete" runat="server" CssClass="Delete btn btn-sm btn-default" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>


                </div>
            </div>
        </div>
      </div>
      <!-- /.row -->

        <div class="row">
            <div class="col-md-12">
                <div class="box box-info">
                      <div class="box-footer">
                        <asp:Button ID="btnCartInSubmit" CssClass="btn btn-info" runat="server" Text="Submit" > </asp:button>
                        <asp:Button ID="btnPrintItemCart" CssClass="btn btn-info" runat="server" Text="Print View" > </asp:button>
                      </div>
                </div>
            </div>
        </div>
    </section>
    <!-- /.content -->



</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ASPFooterContent" runat="server">
    <script src="Assets/bower_components/select2/dist/js/select2.full.min.js"></script>
    <link rel="stylesheet" href="Assets/jQuery_Validation/css/validationEngine.jquery.css" type="text/css"/>
    <script type="text/javascript" src="Assets/jQuery_Validation/js/languages/jquery.validationEngine-en.js" charset="utf-8"></script>
    <script type="text/javascript" src="Assets/jQuery_Validation/js/jquery.validationEngine.js" charset="utf-8"></script>
    <script src="Assets/HotelManagSys.js"></script>
    <script src="Assets/FoodServicesCart.js"></script>
        <script type="text/javascript">
        $(function () {
            $("#asp_form").validationEngine();
        });
    </script>
</asp:Content>
