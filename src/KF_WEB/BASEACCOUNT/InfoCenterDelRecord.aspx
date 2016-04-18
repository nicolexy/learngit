<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InfoCenterDelRecord.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.InfoCenterDelRecord" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table cellspacing="0" cellpadding="0" width="95%" align="center" border="0">
        <tr>
            <td>删除记录</td>
        </tr>
        <tr>
            <td valign="top" align="center">
                <asp:DataGrid ID="dgList" runat="server" Width="1150px" ItemStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center" HorizontalAlign="Center" PageSize="5" AutoGenerateColumns="False"
                    GridLines="Horizontal" CellPadding="1" BackColor="White" BorderWidth="1px" BorderStyle="None"
                    BorderColor="#E7E7FF" AllowPaging="True">
                    <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                    <SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
                    <AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
                    <ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C">
                    </HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="Fqqid" HeaderText="帐号"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Fmemo" HeaderText="备注"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Fauthen_operator" HeaderText="操作人员"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Fmodify_time" HeaderText="最后修改时间"></asp:BoundColumn>
                    </Columns>
                    <PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
        <TR>
		    <TD align="right">
			    <input type="button" name="btn_back" value="返回" style="WIDTH: 80px;" onclick="history.go(-1)" />
		    </TD>
	    </TR>
    </table>
    </div>
    </form>
</body>
</html>
