<%@ Page Title="User Information" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="UserInformation.aspx.vb" Inherits="HotelManagementSystem.UserInformation" %>

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
        <h1>Creación de Usuario
      </h1>
        <%--<ol class="breadcrumb">
            <li>
                <asp:HyperLink ID="brdhlDashboard" runat="server" NavigateUrl="~/Default.aspx"> <i class="fa fa-dashboard"></i>  <span>Home</span> </asp:HyperLink>
            </li>
            <li class="active">User Information</li>
        </ol>--%>
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

                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtFirstName">Nombres</label>  <i class="required_form_item "> * Requerido</i>
                                    <asp:TextBox ID="txtFirstName" CssClass="form-control validate[required]" placeholder="Introduzca nombres" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtLastName">Apellidos</label>  <i class="required_form_item "> * Requerido</i>
                                    <asp:TextBox ID="txtLastName" CssClass="form-control" placeholder="Introduzca apellidos" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="txtEmail">Email</label>  <i class="required_form_item "> * Requerido</i>
                            <asp:TextBox ID="txtEmail" CssClass="form-control validate[required,custom[email]]" TextMode="Email" placeholder="Introduzca Email" runat="server"></asp:TextBox>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtPassword">Contraseña</label>  <i class="required_form_item "> * Requerido</i>
                                    <asp:TextBox ID="txtPassword" CssClass="form-control validate[required, minSize[5]]" TextMode="Password" placeholder="Introduzca contraseña" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtRePassword">Confirma Contraseña</label>  <i class="required_form_item "> * Requerido</i>
                                    <asp:TextBox ID="txtRePassword" TextMode="Password" CssClass="form-control validate[required,equals[MainContent_txtPassword]]" placeholder="Introduzca contraseña nuevamente" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div> 
                        <div class="form-group">
                            <label for="DropDownListRoles">Rol del Usuario</label>  <i class="required_form_item "> * Requerido</i>
                             <asp:DropDownList Width="100%" CssClass="form-control select2 validate[required,custom[integer],minSize[1]]" ID="DropDownListRoles" runat="server" ClientIDMode="Static" /> 
                        </div> 
                        <div class="form-group">
                                    <label for="txtIpAddress">Dirección IP</label> 
                                    <asp:TextBox ID="txtIpAddress" CssClass="form-control" placeholder="Dirección IP" runat="server"></asp:TextBox>
                                </div>
                    </div>
                    <div class="box-footer">
                        <asp:Button ID="btnSubmit" CssClass="btn btn-info" runat="server" Text="Confirmar" />
                        <asp:HyperLink ID="HyperLink1" runat="server" CssClass="btn btn-info" NavigateUrl="~/UserInformation.aspx">Reiniciar</asp:HyperLink>
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
    <link rel="stylesheet" href="Assets/jQuery_Validation/css/validationEngine.jquery.css" type="text/css" />
    <script type="text/javascript" src="Assets/jQuery_Validation/js/languages/jquery.validationEngine-en.js" charset="utf-8"></script>
    <script type="text/javascript" src="Assets/jQuery_Validation/js/jquery.validationEngine.js" charset="utf-8"></script>
     <script src="Assets/loader-custom.js"></script>
    <script type="text/javascript">
        $(function () {
            $("#asp_form").validationEngine();
        });
    </script>
</asp:Content>
