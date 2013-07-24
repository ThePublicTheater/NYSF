<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CartCountdown.ascx.cs"
Inherits="Nysf.Apps.OldStyleTickets.CartCountdown"
%><script src="/js/Countdown.js"></script>
<asp:Literal ID="PreCountdownBlurb" runat="server">Your cart will expire in:
</asp:Literal><span id="CountdownPanel"></span><script>
    window.onload = WindowLoad;
    function WindowLoad(event) {
        ActivateCountDown("CountdownPanel", <asp:Literal ID="CountdownSeconds"
runat="server" />);
    }
</script>