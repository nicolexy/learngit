<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="BeforeCancelTradeQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.BeforeCancelTradeQuery" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>BeforeCancelTradeQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE style="LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1" cellPadding="1" width="820"
				border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colspan="2"><FONT face="����"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;ע��ǰ���ײ�ѯ</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>����Ա����: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<TD align="right"><asp:label id="Label4" runat="server">�ڲ�ID��</asp:label></TD>
                    <TD><asp:textbox id="txtuid" style="WIDTH: 180px;" runat="server"></asp:textbox></TD>
				</TR>
				<TR>
                    <TD align="center" colspan="2"><asp:button id="btnQuery" runat="server" Width="80px" Text="�� ѯ" onclick="btnQuery_Click"></asp:button>
				</TR>
			</TABLE>
            <br/>
             <br/>
            <div style="Z-INDEX: 102; LEFT: 5.02%;POSITION: absolute; TOP: 184px; HEIGHT: 35%;" >
                <asp:DataGrid ID="dg_info" runat="server"  AutoGenerateColumns="False">
                    <ItemStyle Wrap="False"></ItemStyle>
				    <HeaderStyle Wrap="False" BackColor="#EEEEEE"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="Flistid" HeaderText="�Ƹ�ͨ������" ItemStyle-Width="300px" />
                        <asp:BoundColumn DataField="Fpaynum_str" HeaderText="���" ItemStyle-Width="110px"/>
                        <asp:BoundColumn DataField="Fmodify_time" HeaderText="ʱ��" ItemStyle-Width="200px"/>
                        <asp:BoundColumn DataField="Fmemo" HeaderText="��ע"  ItemStyle-Width="200px"/>
                    </Columns>
                </asp:DataGrid>
             </div>
		</form>
	</body>
</HTML>
