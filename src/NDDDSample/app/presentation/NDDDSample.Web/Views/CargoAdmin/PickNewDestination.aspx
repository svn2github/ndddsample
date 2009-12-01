<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<NDDDSample.Web.Controllers.CargoAdmin.PickNewDestinationViewModel>" %>

<asp:Content ID="pickNewDestination" ContentPlaceHolderID="MainContent"
    runat="server">       
    <script src="<%= Url.Content("~/Scripts/calendar.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/YAHOO.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/event.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/dom.js") %>" type="text/javascript"></script>
       
     <style type="text/css">
        td {
          align: left;
        }
     </style>   
       
    <% using (Html.BeginForm("ChangeDestination", "CargoAdmin"))
       { %>
    
  <input type="hidden" name="trackingId" value="<%=ViewData.Model.Cargo.TrackingId%>"/>
  <table>
    <caption>Change destination for cargo 
        <%=ViewData.Model.Cargo.TrackingId%></caption>
    <tbody>
      <tr>
        <td width="250px">Current destination</td>
        <td align="left" >
            <%=ViewData.Model.Cargo.FinalDestination%>                
        </td>
      </tr>
      <tr>
        <td width="250px">New destination</td>
        <td align="left">
          <select name="unlocode">
           <% foreach(var location in ViewData.Model.Locations)
            {%>            
            <option value="<%=location.UnLocode%>">
                <%=location.UnLocode%>
            </option>
            <%
                }%>
          </select>
        </td>
      </tr>
    </tbody>
    <tfoot>
      <tr>
        <td> 
            <input type="submit" value="Change destination" /></td>
        <td>
          
        </td>
      </tr>
    </tfoot>
  </table>
    <%
     }%>

</asp:Content>
