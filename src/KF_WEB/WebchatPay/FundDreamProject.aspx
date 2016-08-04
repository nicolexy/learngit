<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FundDreamProject.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.WebchatPay.FundDreamProject" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        caption {
            text-align: left;
            background: #b0c3d1;
            padding: 4px;
        }
        tr {
            height:25px
        }
        .bluequestionmark {
            background-repeat: no-repeat;
            display: inline-block;
            position: relative; /*the out div must be position:relative*/
        }

        .tipinfo {
            display: none;
            position: absolute;
            background-color:white;
            z-index:9999;
        }

        .bluequestionmark:hover .tipinfo {
            white-space: nowrap; /*the pop up infomation will show in one line*/
            display: block;
            border: 1px solid #0094ff;
            position: absolute;
            top: 18px;
            left: 25px;
            padding: 6px 10px;
            background-color: white;
            z-index:9999;
        }
    </style>
      <link type="text/css" rel="Stylesheet" href="../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %>" />
    <link rel="Stylesheet" href="../Styles/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %>" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
      <asp:DataGrid Width="1200" ID="dg1" runat="server" AutoGenerateColumns="False" CssClass="tab_dg" OnItemCommand="dg1_ItemCommand1" >
            <HeaderStyle Font-Bold="True" Height="25px" />
              <Columns>
                  <asp:BoundColumn HeaderText="计划账户" DataField="Fplan_id" />
                   <asp:BoundColumn HeaderText="计划名称" DataField="Fplan_name" />
                   <asp:BoundColumn HeaderText="计划总金额" DataField="Ftotal_plan_fee" />
                   <asp:BoundColumn HeaderText="总申购金额" DataField="Ftotal_buy_fee" />
                   <asp:BoundColumn HeaderText="计划状态" DataField="Fstate" />
                   <asp:BoundColumn HeaderText="创建时间" DataField="Fcreate_time" />
              <asp:ButtonColumn Text="交易单列表" CommandName="Command_trans" />
                <asp:ButtonColumn Text="资产详情" CommandName="Command_2" />
             </Columns>
        </asp:DataGrid>
         <webdiyer:AspNetPager ID="AspNetPager1" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left" Width="1200px"
            PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" OnPageChanged="OnChangePage1"
            SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]">
        </webdiyer:AspNetPager>

         <asp:DataGrid Width="1200" ID="DataGrid2" runat="server" AutoGenerateColumns="False" CssClass="tab_dg">
            <HeaderStyle Font-Bold="True" Height="25px" />
             <Columns>
                 <asp:BoundColumn DataField="Flistid" HeaderText="交易单号"></asp:BoundColumn>
                 <asp:BoundColumn DataField="Fbuyid" HeaderText="申购单号"></asp:BoundColumn>
                 <asp:BoundColumn DataField="Ftype" HeaderText="申购状态"></asp:BoundColumn>
                 <asp:BoundColumn DataField="Fplan_id" HeaderText="计划账户"></asp:BoundColumn>
                 <asp:BoundColumn DataField="Fspid" HeaderText="资金商户号"></asp:BoundColumn>
                 <asp:BoundColumn DataField="FundName" HeaderText="基金名称"></asp:BoundColumn>
                 <asp:BoundColumn DataField="Ffund_code" HeaderText="基金编码"></asp:BoundColumn>
                 <asp:BoundColumn DataField="Ftotal_fee" HeaderText="金额"></asp:BoundColumn>
                 <asp:BoundColumn DataField="Facc_time" HeaderText="对账时间"></asp:BoundColumn>
                 <asp:BoundColumn DataField="Fstate" HeaderText="买入状态"></asp:BoundColumn>
                 <asp:BoundColumn DataField="Ffail_reason" HeaderText="失败原因"></asp:BoundColumn>
             </Columns>
        </asp:DataGrid>
         <webdiyer:AspNetPager ID="AspNetPager2" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left" Width="1200px"
            PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" OnPageChanged="OnChangePage2"
            SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]">
        </webdiyer:AspNetPager>

         <asp:DataGrid Width="1200" ID="DataGrid3" runat="server" AutoGenerateColumns="False" CssClass="tab_dg" >
            <HeaderStyle Font-Bold="True" Height="25px" />
              <Columns>
                 <asp:BoundColumn DataField="Fplan_id" HeaderText="计划账户"></asp:BoundColumn>
                   <asp:BoundColumn DataField="Fspid" HeaderText="资金商户号"></asp:BoundColumn>
                   <asp:BoundColumn DataField="FundName" HeaderText="基金名称"></asp:BoundColumn>
                   <asp:BoundColumn DataField="Ffund_code" HeaderText="基金编码"></asp:BoundColumn>
                   <asp:BoundColumn DataField="Ftotal_buy_fee" HeaderText="总申购金额"></asp:BoundColumn>
                   <asp:BoundColumn DataField="Ftotal_profit" HeaderText="累计总收益"></asp:BoundColumn>
                   <asp:BoundColumn DataField="Fprofit" HeaderText="昨日收益"></asp:BoundColumn>
                   <asp:BoundColumn DataField="Fstate" HeaderText="计划状态"></asp:BoundColumn>
                   <asp:BoundColumn DataField="Ftotal_control_unit" HeaderText="总受控份额"></asp:BoundColumn>
                   <asp:BoundColumn DataField="Fbusiness_type" HeaderText="业务类型"></asp:BoundColumn>
                  </Columns>
        </asp:DataGrid>
         <webdiyer:AspNetPager ID="AspNetPager3" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left" Width="1200px"
            PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" OnPageChanged="OnChangePage3"
            SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]">
        </webdiyer:AspNetPager>
    </div>
    </form>
</body>
</html>
