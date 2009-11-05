<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true"
   Inherits="System.Web.Mvc.ViewPage<ErrorMessageViewModel>" %>
<%@ Import Namespace="NDDDSample.Web.Controllers"%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        <%=ViewData.Model.Message%>
    </h2>
    <input type="button" title="Back" value="Back" onclick="javascript:history.back(1)" />
</asp:Content>
