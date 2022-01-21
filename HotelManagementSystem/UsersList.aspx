<%@ Page Title="Users List" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="UsersList.aspx.vb" Inherits="HotelManagementSystem.UsersList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.3/jquery.min.js"></script>
    <script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js"></script>
     <script src="Assets/loader-custom.js"></script>
    <script type="text/javascript">
        function showModal() {
            $("#myModal").modal('show');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
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

    <section class="content-header">
        <h1>List of User 
      </h1>
        <ol class="breadcrumb">
            <li>
                <asp:HyperLink ID="brdhlDashboard" runat="server" NavigateUrl="~/Default.aspx"> <i class="fa fa-dashboard"></i>  <span>Home</span> </asp:HyperLink>
            </li>
            <li class="active">List of User </li>
        </ol>
    </section>

    <!-- Main content -->
    <section class="content">
        <!-- /.row -->
        <div class="row">
            <!-- left column -->
            <div class="col-md-12">
                <div class="box box-info">
                    <div class="box-body">
                        <div class="table-responsive">
                            <asp:GridView ID="GridView1" CssClass="gvv table table-bordered table-striped" runat="server" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:BoundField DataField="USER_ID" HeaderText="ID" />
                                    <asp:BoundField DataField="FirstName" HeaderText="First Name" />
                                    <asp:BoundField DataField="LastName" HeaderText="Last Name" />
                                    <asp:BoundField DataField="Email" HeaderText="Email" />
                                    <asp:BoundField DataField="RegistrationDate" DataFormatString="{0:d-MMM-yyyy}" HeaderText="Registration Date" />
                                    <asp:BoundField DataField="LastLoginDate" DataFormatString="{0:d-MMM-yyyy}" HeaderText="LastLogin" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button Text="EDIT" CssClass="btn btn-info btn-sm" runat="server" OnClick="SendEdit" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button Text="DELETE" CssClass="btn btn-info btn-sm" runat="server" OnClick="SendDelete" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <!-- /.box-body -->
                </div>
            </div>
        </div>

    </section>
    <!-- /.content -->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ASPFooterContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".gvv").prepend($("<thead></thead>").append($(this).find("tr:first"))).dataTable({
                'paging': true,
                'lengthChange': true,
                'searching': true,
                'ordering': true,
                'info': true,
                'autoWidth': true
            });
        });
    </script>
</asp:Content>
