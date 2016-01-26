<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HandQTransQuery.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.HandQBusiness.HandQTransQuery" %>

<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>手Q转账查询</title>
    <style type="text/css">
        caption {
            text-align: left;
            background: #b0c3d1;
            padding: 4px;
        }
        tr {
            height:25px
        }
       
    </style>
    <link type="text/css" rel="Stylesheet" href="../STYLES/ossstyle.css" />
    <link rel="Stylesheet" href="../Styles/ossstyle.css" />
    <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
</head>
<body>
    <form id="formMain" runat="server">
        <table  cellspacing="0" rules="all" border="1"  style="width:1200px;border-collapse:collapse;">
            <tr>
                <td style="width: 100%" bgcolor="#e4e5f7" colspan="2"><font color="red">
                    <img src="../images/page/post.gif" width="20" height="16">手Q转账查询</font>
                </td>
            </tr>
            <tr>
                <td style="width:50%">
                    财付通账号：
                    <asp:TextBox ID="txt_user" Width="200px" runat="server"></asp:TextBox>
                    <asp:RadioButton ID="TransOut" GroupName="TransType" runat="server" Text="转出" Checked="true" />
                     <asp:RadioButton ID="TransIn" GroupName="TransType" runat="server" Text="转入" />
                    
                </td>
                <td>
                    <asp:Button ID="btnQuery" runat="server" Width="80px" Text="查 询" OnClick="btnQuery_Click"></asp:Button>
                </td>
            </tr>
        </table>
        <br />
        <asp:DataGrid Width="1200" ID="DataGrid1" runat="server" AutoGenerateColumns="False">
            <HeaderStyle Font-Bold="True" Height="25px" />
            <Columns>
                <asp:BoundColumn HeaderText="付款方账号" DataField="Fpayer_uin" />
                <asp:BoundColumn HeaderText="收款方账号" DataField="Fseller_uin" />
                <asp:BoundColumn HeaderText="金额" DataField="Ftotal_fee" />
                <asp:BoundColumn HeaderText="付款状态" DataField="Fstate" />
                <asp:BoundColumn HeaderText="支付单号" DataField="Fsave_listid" />
                <asp:BoundColumn HeaderText="支付商户订单号" DataField="Flistid" />
                <asp:BoundColumn HeaderText="转账商户订单号" DataField="Ftransfer_listid" />
                <asp:BoundColumn HeaderText="创建时间" DataField="Fcreate_time" />
                <asp:BoundColumn HeaderText="最后修改时间" DataField="Fmodify_time" />
                <asp:BoundColumn HeaderText="备注" DataField="Fmemo" />
                <%--<asp:ButtonColumn Text="详情" CommandName="detail" />--%>
            </Columns>
        </asp:DataGrid>
        <webdiyer:AspNetPager ID="pager" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left" Width="1200px"
            PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" OnPageChanged="ChangePage"
            SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]">
        </webdiyer:AspNetPager>
        <asp:Panel runat="server" ID="panelDetail" Width="1200"  Visible="false">
            <table cellspacing="0" rules="all" border="1"  style="width:1200px;border-collapse:collapse;">
                 <caption>
                        详情
                    </caption>
                <tr>
                    <td style="text-align: right; width:25%">保留字段：</td>
                    <td>
                        <asp:Label runat="server" ID="Label1"></asp:Label></td>
                    <td style="text-align: right; width:25%">保留字段：</td>
                    <td>
                        <asp:Label runat="server" ID="Label2"></asp:Label></td>
                </tr>
            </table>
        </asp:Panel>

    </form>
</body>
</html>
