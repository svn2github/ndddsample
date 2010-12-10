<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="NDDDSample.Web.Views.Home.Index" %>
<%@ Import Namespace="NDDDSample.Web.Controllers" %>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%= Html.Encode(ViewData["Message"]) %></h2>            
       <p> Welcome to the <strong>C# Domain-Driven Design sample</strong> application.</p>
    <p>
        There are two web interfaces available:</p>
    <ul>
        <li><strong>Public</strong> <%= Html.ActionLink("cargo tracking", "Index", "CargoTracking")%></li>
        <li><strong>Administration</strong> of <%= Html.ActionLink("booking and routing", "List", "CargoAdmin")%>.<br /> (Before access it, please run NDDDSample.Interfaces.BookingRemoteService.Host and NDDDSample.Interfaces.PathfinderRemoteService.Host, it is WCF services host used by <strong>Administration</strong> to 'access' domain model and external routing. 
        <br /><strong>Public</strong> interface use the domain model 'directly' i.e. in same process) </li>        
    </ul>
    <p>
        The Incident Logging application, that is used to register handling events, is a
        stand-alone application (under development now!).</p>
    <p>
        Please visit the <a href="http://dddsample.sf.net">project website</a> for more
        information and a screencast demonstration of how the application works.</p>
    <p>
        <i>This project is C# version of the <a href="http://dddsample.sourceforge.net/">Domain-Driven
            Design Sample</a>, which was developed by DDD enthusiasts :). 
            Java version of the application is joint effort by Eric Evans' company <a href="http://www.domainlanguage.com">
        Domain Language</a> and the Swedish software consulting company<a href="http://www.citerus.se">Citerus</a>.
        <strong>For more info about C# version visit <a href="http://code.google.com/p/ndddsample/">
           
        
            the link.</a>  </strong> </i>         
        </p>        
</asp:Content>
