<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage<CargoRoutingDTO>" %>
<%@ Import Namespace="NDDDSample.Interfaces.BookingRemoteService.Common.Dto"%>

<asp:Content ID="cargoAdminShow" ContentPlaceHolderID="MainContent" runat="server">
    <table>
        <caption>
            Details for cargo
            <%= ViewData.Model.TrackingId%></caption>
        <tbody>
            <tr>
                <td>
                    Origin
                </td>
                <td>
                    <%=ViewData.Model.Origin%>
                </td>
            </tr>
            <tr>
                <td>
                    Destination
                </td>
                <td>
                    <%= ViewData.Model.FinalDestination%>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <%=Html.ActionLink("Change destination", "PickNewDestination", "CargoAdmin", new { trackingId = ViewData.Model.TrackingId }, null)%>
                </td>
            </tr>
            <tr>
                <td>
                    Arrival deadline
                </td>
                <td>
                    <%= ViewData.Model.ArrivalDeadline.ToString("yyyy-MM-dd hh:mm")%>
                </td>
            </tr>
        </tbody>
    </table>
    <p>
    </p>       
    <%if (ViewData.Model.IsRouted)
      {%>
          <%if (ViewData.Model.IsMisrouted)
            {%>
                <p><em>Cargo is misrouted - 
                <%=Html.ActionLink("reroute this cargo", "SelectItinerary", "CargoAdmin", new { trackingId = ViewData.Model.TrackingId }, null)%>
                </em></p>    
        <%}%>
            
      <table border="1">
        <caption>Itinerary</caption>
        <thead>
          <tr>
            <td>Voyage number</td>
            <td colspan="2">Load</td>
            <td colspan="2">Unload</td>
          </tr>
        </thead>
        <tbody>
          <% foreach (var leg in ViewData.Model.Legs)
             { %>
            <tr>
              <td><%=leg.VoyageNumber%></td>
              <td><%=leg.FromLocation%></td>
              <td>
               (<%= leg.LoadTime.ToString("yyyy-MM-dd hh:mm")%>)
              </td>
              <td><%=leg.ToLocation%></td>
              <td>(<%= leg.UnloadTime.ToString("yyyy-MM-dd hh:mm")%>)
              </td>
            </tr>
          <% }%>
        </tbody>
      </table>
      <%
          }
      else
      {%>
   
      <p>      
        <strong>Not routed</strong> - 
         <%=Html.ActionLink("Route this cargo", "SelectItinerary", "CargoAdmin", new { trackingId = ViewData.Model.TrackingId }, null)%>        
      </p>   
    <%}%>    
</asp:Content>
