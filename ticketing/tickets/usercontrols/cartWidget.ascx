<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cartWidget.ascx.cs" Inherits="tickets.usercontrols.cartWidget" %>

<style type="text/css">
        .submitLink {
        background-color: transparent !important;
        text-decoration: underline;
        border: none;
        font-variant: initial !important;
        text-transform: initial !important;
        color: blue !important;
        cursor: pointer;
        font-size: 12px !important;
        margin: 0px;
        padding: 0;
        vertical-align: inherit !important;
    }

    .orderSummaryInner {
        margin-top:17px;
        width: 275px;
        min-height: 300px;
        background-color: whitesmoke;
        border-radius: 10px;
        border: 2px solid #E02400;
        position: relative;
       /*margin-top: 15px;
       */ 
    }

    .orderSummaryHeader {
        border-top-left-radius: 7px;
        border-top-right-radius: 7px;
        background-color: #E02400;
        font-size: 18px;
        font-weight: bold;
        height: 30px;
        text-align: center;
        color: white;
    }

    .orderSummaryContent {
        background-color: whitesmoke;
        padding: 4px;
    }

    .summaryFooter {
        font-weight: bold;
        position: absolute;
        bottom: 2px;
        left: 4px;
    }
        .perfNameRow {
        text-align: center;
        font-weight: bold;
        font-style: italic;
        font-size:14px;
    }

    .subTotalDiv {
        position: absolute;
        bottom: 25px;
        left: 4px;
    }
    
    .updateProgress {
        position: absolute;
        right: 3px;
        bottom: 3px;
    }
</style>

<div class="orderSummaryInner" id="orderSummaryDiv" runat="server">
    <div class="orderSummaryHeader">
        <asp:Literal ID="titleLiteral" runat="server"></asp:Literal>
    </div>
    <div id="summaryDiv" class="orderSummaryContent" runat="server">
    </div>
    <div id="subTotalDiv" class="subTotalDiv" runat="server">
    </div>
    <span id="summaryFooter" runat="server" class="summaryFooter">Total: 
    </span>
    <div class="updateProgress">
        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
            <ProgressTemplate>
                
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
</div>