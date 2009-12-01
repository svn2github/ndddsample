<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<NDDDSample.Web.Controllers.CargoAdmin.RegistrationFormViewModel>" %>

<asp:Content ID="registragionForm" ContentPlaceHolderID="MainContent" runat="server">

    <script src="<%= Url.Content("~/Scripts/calendar.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/YAHOO.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/event.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/dom.js") %>" type="text/javascript"></script>

    <style type="text/css">
        tr {
          align: left;          
        }
     </style>
    <% using (Html.BeginForm("Register", "CargoAdmin"))
 {%>
       
       <table>
    <caption>Book new cargo</caption>
    <tbody>
      <tr>
        <td align="right">Origin:</td>
        <td align="left">          
            <select name="originUnlocode">
                <% foreach (var unLoc in ViewData.Model.UnLoccodes)
            {%>
                <option value="<%=unLoc%>">
                    <%=unLoc%>
                </option>
                <%
                }%>
            </select>            
        </td>
      </tr>
      <tr >
        <td align="right">Destination:</td>
        <td align="left">        
            <select name="destinationUnlocode">
                <% foreach (var unLoc in ViewData.Model.UnLoccodes)
            {%>
                <option value="<%=unLoc%>">
                    <%=unLoc%>
                </option>
                <%
                }%>
            </select>
        </td>
      </tr>
        <tr>
          <td align="right">Arrival deadline:</td>
          <td align="left">
            <input readonly="readonly" name="arrivalDeadline" onclick="calendar.toggle( event, this, 'cal1')"
                type="text" size="10" id="cal1" value="<%=DateTime.Now.ToString("M/dd/yyyy")%>" />&nbsp;
            <img alt="" src="/Content/Images/calendarTrigger.gif" class="calendarTrigger" onclick="calendar.toggle( event, this, 'cal1')"/>
          </td>
        </tr>
    </tbody>
    <tfoot>
      <tr>
        <td> </td>
        <td align="left">
          <input type="submit" value="Book"/>
        </td>
      </tr>
    </tfoot>
  </table>       
   <%
 }%>

</asp:Content>
