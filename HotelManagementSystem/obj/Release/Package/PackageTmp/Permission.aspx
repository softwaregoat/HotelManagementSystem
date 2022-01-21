<%@ Page Title="Permission" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Permission.aspx.vb" Inherits="HotelManagementSystem.Permission" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="Assets/plugins/iCheck/square/blue.css">
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
        Permission
      </h1>
      <ol class="breadcrumb">
        <li> <asp:HyperLink ID="brdhlDashboard" runat="server" NavigateUrl="~/Default.aspx"> <i class="fa fa-dashboard"></i>  <span>Home</span> </asp:HyperLink>  </li>
        <li class="active">Permission</li>
      </ol>
    </section>

        <div class="modal modal-info fade" id="myModal" role="dialog">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">Message</h4>
                </div>
                <div class="modal-body">
                    <asp:Label ID="lblMessege" runat="server" Text="Label"></asp:Label>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default pull-right" data-dismiss="modal">OK</button>
                </div>
            </div>
        </div>
    </div>

    <!-- Main content -->
    <section class="content">
      <div class="row">
        <!-- left column -->
        <div class="col-md-6">
          <!-- Form Element sizes -->
          <div class="box box-info">
            <div class="box-body">

               <%-- <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <label>User Name</label>
                             <asp:DropDownList CssClass="form-control select2" ID="DropDownListUsers" runat="server" Width="100%" DataSourceID="UserDataSource" DataTextField="FirstName" DataValueField="USER_ID" AutoPostBack="True" />
                            <asp:SqlDataSource ID="UserDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConStringHotelMngSys %>" SelectCommand="SELECT [USER_ID], [FirstName], [LastName] FROM [UserInformation]"></asp:SqlDataSource>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <label>Check into checkbox to grant permission</label>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConStringHotelMngSys %>" SelectCommand="SELECT [WebForm], [AllowUser] FROM [Permissions] WHERE ([USER_ID] = @USER_ID)">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="DropDownListUsers" DefaultValue="0" Name="USER_ID" PropertyName="SelectedValue" Type="Int64" />
                                </SelectParameters>
                            </asp:SqlDataSource>--%>
                 <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <label>Role Name</label>
                             <asp:DropDownList CssClass="form-control select2" ID="DropDownListRoles" runat="server" Width="100%" DataSourceID="UserDataSource" DataTextField="ROLE_Name" DataValueField="ROLE_ID" AutoPostBack="True" />
                            <asp:SqlDataSource ID="UserDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConStringHotelMngSys %>" SelectCommand="SELECT [ROLE_ID], [ROLE_Name] FROM [UserRoles]"></asp:SqlDataSource>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <label>Check into checkbox to grant permission</label>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConStringHotelMngSys %>" SelectCommand="SELECT [WebForm], [AllowUser] FROM [PermissionsRoleBased] WHERE ([ROLE_ID] = @ROLE_ID)">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="DropDownListRoles" DefaultValue="0" Name="ROLE_ID" PropertyName="SelectedValue" Type="Int64" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <div class="table-responsive">
                            <asp:GridView ID="GridView1" CssClass="table table-responsive table-condensed table-striped table-bordered" runat="server" OnRowDataBound="OnRowDataBound" AutoGenerateColumns="False" DataSourceID="SqlDataSource1">
                                <Columns>
                                    <asp:BoundField DataField="WebForm" HeaderText="WebForm" SortExpression="WebForm" />
                                    <asp:BoundField DataField="AllowUser" HeaderText="ALLOW" SortExpression="AllowUser" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                             <asp:CheckBox ID="chkRow" runat="server" />
                                        </ItemTemplate>
                                        </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
              <div class="box-footer">
                <asp:Button ID="btnSubmit" OnClick="GetSelectedRecords"  CssClass="btn btn-info" runat="server" Text="Submit" />
                <asp:HyperLink ID="HyperLink1" runat="server" CssClass="btn btn-info" NavigateUrl="~/Permission.aspx">Reset</asp:HyperLink>
              </div>
            <!-- /.box-body -->
          </div>
          <!-- /.box -->
        </div>
        <!--/.col (left) -->
      </div>
      <!-- /.row -->
    </section>
    <!-- /.content -->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ASPFooterContent" runat="server">
    <!-- iCheck 1.0.1 -->
<script src="Assets/plugins/iCheck/icheck.min.js"></script>
<!-- FastClick -->
<script src="Assets/bower_components/fastclick/lib/fastclick.js"></script>

    <!-- Select2 -->
    <script src="Assets/bower_components/select2/dist/js/select2.full.min.js"></script>
     <script src="Assets/loader-custom.js"></script>
        <script type="text/javascript">
        $(document).ready(function () {
            $('.select2').select2();
        });
    </script>
</asp:Content>
