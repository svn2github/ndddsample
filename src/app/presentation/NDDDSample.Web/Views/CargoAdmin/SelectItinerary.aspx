<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<NDDDSample.Web.Controllers.CargoAdmin.SelectItineraryViewModel>" %>

<asp:Content ID="selectItinerary" ContentPlaceHolderID="MainContent" runat="server">

    <script src="<%= Url.Content("~/Scripts/calendar.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/YAHOO.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/event.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/dom.js") %>" type="text/javascript"></script>

    <table>
        <caption>
            Select route</caption>
        <tr>
            <td>
                Cargo
                <%=ViewData.Model.Cargo.TrackingId%>
                is going from
                <%=ViewData.Model.Cargo.Origin%>
                to
                <%=ViewData.Model.Cargo.FinalDestination%>
            </td>
        </tr>
    </table>
    <%
        int itStatus = 0;
        foreach (var it in ViewData.Model.RouteCandidates)
        {
            itStatus++;
            using (Html.BeginForm("AssignItinerary", "CargoAdmin"))
            {
    %>
    <%=Html.Hidden("trackingId", ViewData.Model.Cargo.TrackingId)%>
    <table>
        <caption>
            Route candidate
            <%=itStatus%></caption>
        <thead>
            <tr>
                <td>
                    Voyage
                </td>
                <td>
                    From
                </td>
                <td>
                </td>
                <td>
                    To
                </td>
                <td>
                </td>
            </tr>
        </thead>
        <tbody>
            <%
                int legStatus = 0;
                foreach (var leg in it.Legs)
                {           
            %>           
            <input type="hidden" name="legCommands.Index" value="<%=legStatus%>" />            
            <input type="hidden" name="legCommands[<%=legStatus%>].VoyageNumber" value="<%=leg.VoyageNumber%>" />
            <input type="hidden" name="legCommands[<%=legStatus%>].FromUnLocode" value="<%=leg.FromLocation%>" />
            <input type="hidden" name="legCommands[<%=legStatus%>].ToUnLocode" value="<%=leg.ToLocation%>" />
            <input type="hidden" name="legCommands[<%=legStatus%>].FromDate" value="<%=leg.LoadTime.ToString("yyyy-MM-dd hh:mm")%>" />
            <input type="hidden" name="legCommands[<%=legStatus%>].ToDate" value="<%=leg.UnloadTime.ToString("yyyy-MM-dd hh:mm")%>" />
            <tr>
                <td>
                    <%=leg.VoyageNumber%>
                </td>
                <td>
                    <%=leg.FromLocation%>
                </td>
                <td>
                    <%=leg.LoadTime.ToString("yyyy-MM-dd hh:mm")%>
                </td>
                <td>
                    <%=leg.ToLocation%>
                </td>
                <td>
                    <%=leg.UnloadTime.ToString("yyyy-MM-dd hh:mm")%>
                </td>
            </tr>
            <%
                legStatus++;
        }%>
        </tbody>
        <tfoot>
            <tr>
                <td colspan="3">
                    <p>
                        <input type="submit" value="Assign cargo to this route" />
                    </p>
                </td>
            </tr>
        </tfoot>
    </table>
    <%
        }
        }
    %>
</asp:Content>
