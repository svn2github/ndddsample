<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage<CargoTrackingViewAdapter>" %>

<%@ Import Namespace="NDDDSample.Web.Controllers.Tracking" %>
<asp:Content ID="trackSearchContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Tracking your cargo</h2>
    <% using (Html.BeginForm("Search", "CargoTracking"))
       { %>
    <div>
        <table width="100px">
            <tr>
                <td>
                    <strong>Enter your tracking id:</strong>
                
                    <%=Html.TextBox("trackingId") %>
                
                    <input type="submit" value="Track!" />
                </td>
            </tr>
        </table>
    </div>
    <% if (ViewData.Model == null){%>
    <p class="notify">
        <%=TempData["Message"]%>
        [<em>Hint: try tracking "ABC123" or "JKL567".</em>]
    </p>
    <% }else { %>
    <div id="result">
        <h2>
            Cargo
            <%=ViewData.Model.TrackingId%>
            is now:
            <%=ViewData.Model.GetStatusText()%></h2>
        <p>
            Estimated time of arrival in
            <%=ViewData.Model.Destination%>:
            <%=ViewData.Model.Eta%>
        </p>
        <p>
            <%=ViewData.Model.GetNextExpectedActivity()%></p>
        <% 
        if (ViewData.Model.IsMisdirected)
           {
        %>
        <p class="notify">
            <img src="/Content/Images/error.png" alt="" />Cargo is misdirected</p>
        <% } %>
        <% if (ViewData.Model.Events.Count > 0)
           {%>
        <h3>
            Handling History</h3>
        <ul style="list-style-type: none;">
            <% foreach(var leg in ViewData.Model.Events)
                    {%>
            <li>
                <p>
                    <img style="vertical-align: top;" src="/Content/Images/<%=leg.IsExpected ? "tick" : "cross"%>.png"
                        alt="" />
                    &nbsp;<%=leg.Description%></p>
            </li>
            <%
                    }%>
        </ul>
        <% } %>

        <script type="text/javascript" charset="UTF-8">
            try {
                document.getElementById('trackingId').focus()
            } catch (e) { }
        </script>

        <% } %>
      <% } %>
</asp:Content>
