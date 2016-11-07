<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UsersFreeze.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BankCheckSystem.WebForm1" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    	<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
    caption {
          text-align: left;
            background: #e4e5f7;
            padding: 4px;
               
}
		</style>
    <style>
        @import url( ../STYLES/style.css?);
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin-top:10px;margin-left:10px">
            <table class="tb_detail" width="90%" cellspacing="1" cellpadding="1">
                <tr>
                   <TD style=" background-color:#e4e5f7"   colspan="3"><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">银行查单账号冻结</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
                </tr>
                <tr>
                    <td style="text-align:right;">登陆账号：</td>
                    <td>
                       <asp:TextBox ID="txt_Fuser_login_account" runat="server"></asp:TextBox>
                    </td>
                   
                    <td>
                         <asp:Button ID="btnSerach" runat="server" Text="查 询" OnClick="btnSerach_Click"  />
                    </td>
                </tr>
                 <tr>
                    <td style="text-align:right;">用户状态：</td>
                    <td>
                        <asp:Label ID="lblStatus" runat="server"></asp:Label>
                    </td>
                   
                    <td>
                         
                    </td>
                </tr>
                <tr> 
                    <td style="text-align:right;width:120px">原因：</td>
                    
                    <td colspan="2" >
                     <asp:TextBox ID="txt_Reason" runat="server" Height="56px" Width="184px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                     <td colspan="3" style="text-align:center">
                         <asp:Button ID="btnFreeze" runat="server" Text="确认冻结" OnClick="btnFreeze_Click" Visible="false" OnClientClick="javascript: if(!confirm('确定冻结吗？')){return false;}" />
                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                           <asp:Button ID="btnUnFreeze" runat="server" Text="提交解冻" OnClick="btnUnFreeze_Click" Visible="false" OnClientClick="javascript: if(!confirm('确定解冻吗？')){return false;}"  />
                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                         <asp:Button ID="btnToInvalid" runat="server" Text="确认作废" OnClick="btnToInvalid_Click"  Visible="false" OnClientClick="javascript: if(!confirm('确定作废吗？')){return false;}"  />
                    </td>
                </tr>
            </table>
            <br />
             <asp:datagrid id="DataGrid1" runat="server" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" Caption="日志" CssClass="container"  
							BackColor="White" CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="False" Width="90%">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Fuser_bind_email" HeaderText="登录号"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fcreate_time" HeaderText="冻结时间"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Frec_admin_name" HeaderText="操作人"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Ftype_id_str" HeaderText="操作类别"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Frec_reason" HeaderText="原因"></asp:BoundColumn>
                               
							</Columns>
                            <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid>
     <webdiyer:aspnetpager id="Aspnetpager1" runat="server" AlwaysShow="True" ShowCustomInfoSection="left" NumericButtonTextFormatString="[{0}]"
							SubmitButtonText="转到" HorizontalAlign="right" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0" Width="90%"
							NumericButtonCount="10" OnPageChanged="ChangePage"></webdiyer:aspnetpager>
        </div>
        
        
        
    </form>
</body>
</html>
