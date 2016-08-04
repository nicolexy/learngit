<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomsApplicationQuery.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.WebchatPay.CustomsApplicationQuery" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head id="Head1" runat="server">
    <title>商户海关申请查询</title>
    <style type="text/css">
        @import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> );

        UNKNOWN {
            COLOR: #000000;
        }

        .style3 {
            COLOR: #ff0000;
        }

        BODY {
            BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif);
        }

        .auto-style1 {
            width: 100%;
            height: 19px;
        }
    </style>
</head>
<body>
    <form id="formMain" runat="server">
        <table border="1" cellspacing="1" cellpadding="1" width="1100">
            <tr>
                <td style="width: 100%" bgcolor="#e4e5f7" colspan="5"><font color="red">
                    <img src="../images/page/post.gif" width="20" height="16">商户海关申请查询</font>
                </td>
            </tr>
            <tr>
                <td>商户号：<asp:TextBox ID="txtpartner" Width="200px" runat="server"></asp:TextBox></td>

                <td align="center">
                    <asp:Button ID="btnQuery" runat="server" Width="80px" Text="查 询" OnClick="btnQuery_Click"></asp:Button></td>
            </tr>
        </table>
        <br />
        <table border="1" cellspacing="0" cellpadding="0" width="1100">
            <tr>
                <td bgcolor="#e4e5f7" colspan="5" class="auto-style1"><font color="red">
                    <img src="../images/page/post.gif" width="20" height="16">详情</font>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <div>
                        商户号：
                    </div>
                </td>
                <td>
                    <asp:TextBox ID="txt_partner" runat="server"></asp:TextBox>
                </td>
                <td style="text-align: right">商户名：</td>
                <td>
                    <asp:TextBox ID="txt_sp_name" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="text-align: right">商户类型：</td>
                <td>
                    <asp:TextBox ID="txt_merchant_type" runat="server"></asp:TextBox></td>
                <td style="text-align: right">邮箱：</td>
                <td>
                    <asp:TextBox ID="txt_contact_email" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="text-align: right">联系人姓名：</td>
                <td>
                    <asp:TextBox ID="txt_contact_name" runat="server"></asp:TextBox></td>
                <td style="text-align: right">电话：</td>
                <td>
                    <asp:TextBox ID="txt_contact_phone" runat="server"></asp:TextBox></td>
            </tr>
        </table>
        <br />

        <asp:GridView ID="DataGrid1" runat="server" Width="1100px" itemstyle-horizontalalign="center"
            HeaderStyle-HorizontalAlign="center" PageSize="5" AutoGenerateColumns="false" 
            GridLines="horizontal" CellPadding="1" BackColor="white" BorderWidth="1px" BorderStyle="none" BorderColor="#e7e7ff">
            <FooterStyle ForeColor="#4a3c8c" BackColor="#b5c7de"></FooterStyle>
            <HeaderStyle Font-Bold="true" HorizontalAlign="center" ForeColor="#f7f7f7" BackColor="#4a3c8c"></HeaderStyle>
            <RowStyle HorizontalAlign="center" />
            <Columns>
              <%--  <asp:BoundField DataField="number" HeaderText="序号">
                    <HeaderStyle Width="150px" HorizontalAlign="center"></HeaderStyle>
                </asp:BoundField>--%>
               <asp:BoundField DataField="custom_id_str" HeaderText="发送海关">
                    <HeaderStyle Width="150px" HorizontalAlign="center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="customs_company_code" HeaderText="备案号">
                    <HeaderStyle Width="150px" HorizontalAlign="center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="customs_company_name" HeaderText="备案名">
                    <HeaderStyle Width="150px" HorizontalAlign="center"></HeaderStyle>
                </asp:BoundField>
            </Columns>
        </asp:GridView>
    </form>
</body>
</html>
