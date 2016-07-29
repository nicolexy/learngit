<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HandQTransQuery.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.HandQBusiness.HandQTransQuery" %>

<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head id="Head1" runat="server">
    <title>手Q转账查询</title>
    <style type="text/css">
        caption {
            text-align: left;
            background: #b0c3d1;
            padding: 4px;
        }

        tr {
            height: 25px;
        }
    </style>
    <link type="text/css" rel="Stylesheet" href="../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %>" />
    <link rel="Stylesheet" href="../Styles/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %>" />
    <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
</head>
<body>
    <form id="formMain" runat="server">
        <table cellspacing="0" rules="all" border="1" style="width: 95%; border-collapse: collapse;">
            <tr>
                <td style="width: 100%" bgcolor="#e4e5f7" colspan="3"><font color="red">
                    <img src="../images/page/post.gif" width="20" height="16">手Q转账查询</font>
                </td>
            </tr>
            <tr>
                <td style="width: 50%">财付通账号：
                    <asp:TextBox ID="txt_user" Width="200px" runat="server"></asp:TextBox>
                    <asp:RadioButton ID="TransOut" GroupName="TransType" runat="server" Text="转出" Checked="true" />
                    <asp:RadioButton ID="TransIn" GroupName="TransType" runat="server" Text="转入" />
                </td>
                <td>
                    开始日期:
                      <asp:TextBox ID="textBoxBeginDate" runat="server" Width="150px" onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})"  CssClass="Wdate"></asp:TextBox>                  
                    结束日期：
                        <asp:TextBox ID="textBoxEndDate" runat="server" Width="150px" onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})"  CssClass="Wdate"></asp:TextBox>                    
                </td>
                <td>
                    <asp:Button ID="btnQuery" runat="server" Width="80px" Text="查 询" OnClick="btnQuery_Click"></asp:Button>
                </td>
            </tr>
        </table>
        <br />
        <asp:DataGrid Width="95%" ID="DataGrid1" runat="server" AutoGenerateColumns="False">
            <HeaderStyle Font-Bold="True" Height="25px" />
            <Columns>
                <asp:BoundColumn HeaderText="付款人QQ" DataField="payer_uin" />
                <asp:BoundColumn HeaderText="付款人姓名" DataField="payer_name" />
                <asp:BoundColumn HeaderText="收款人QQ号码" DataField="seller_uin" />
                <asp:BoundColumn HeaderText="收款人姓名" DataField="seller_name" />
                <asp:BoundColumn HeaderText="转账总金额" DataField="total_fee" />
                <asp:BoundColumn HeaderText="实际付款金额" DataField="price" />
                <asp:BoundColumn HeaderText="手续费金额" DataField="charge_fee" />             
                  <asp:TemplateColumn HeaderText="转账单状态">
                    <ItemTemplate>
                      <span><%#Eval("state").ToString()=="1"?"下单":Eval("state").ToString()=="2"?"支付成功":Eval("state").ToString()=="12"?"充值成功":Eval("state").ToString()=="3"?"收款成功":Eval("state").ToString()=="4"?"退款申请中":Eval("state").ToString()=="5"?"退款成功":Eval("state").ToString()=="6"?"转入转账":Eval("state").ToString().ToString()=="7"?"转入退款":""%></span>                      
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn HeaderText="财付通交易单号" DataField="transaction_id" />
                <asp:BoundColumn HeaderText="支付商户订单号" DataField="listid" />
                <asp:BoundColumn HeaderText="转账商户订单号" DataField="sp_billno" />
                <asp:BoundColumn HeaderText="支付时间" DataField="create_time" />
                <asp:BoundColumn HeaderText="最后修改时间" DataField="modify_time" />
                <asp:BoundColumn HeaderText="备注" DataField="memo" />              
                <%--<asp:ButtonColumn Text="详情" CommandName="detail" />--%>
            </Columns>
        </asp:DataGrid>
        <webdiyer:AspNetPager ID="pager" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left" Width="90%"
            PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" OnPageChanged="ChangePage"
            SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]">
        </webdiyer:AspNetPager>
        <asp:Panel runat="server" ID="panelDetail" Width="1200" Visible="false">
            <table cellspacing="0" rules="all" border="1" style="width: 1200px; border-collapse: collapse;">
                <caption>
                    详情
                </caption>
                <tr>
                    <td style="text-align: right; width: 25%">保留字段：</td>
                    <td>
                        <asp:Label runat="server" ID="Label1"></asp:Label></td>
                    <td style="text-align: right; width: 25%">保留字段：</td>
                    <td>
                        <asp:Label runat="server" ID="Label2"></asp:Label></td>
                </tr>
            </table>
        </asp:Panel>

    </form>
</body>
</html>
