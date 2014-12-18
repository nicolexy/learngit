<%@ Page language="c#" Codebehind="RefundMain.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.RefundMain" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
		<title>RefundMain</title>
<meta content="Microsoft Visual Studio .NET 7.1" name=GENERATOR>
<meta content=C# name=CODE_LANGUAGE>
<meta content=JavaScript name=vs_defaultClientScript>
<meta content=http://schemas.microsoft.com/intellisense/ie5 name=vs_targetSchema>
<style type=text/css>@import url( ../STYLES/ossstyle.css );
.style2 {
	COLOR: #000000
}
.style3 {
	COLOR: #ff0000
}
BODY {
	BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif)
}
TD {
	FONT-SIZE: 9pt
}
.style4 {
	COLOR: #ff0000
}
</style>

<script language=javascript>
					function openModeBegin()
					{
					var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.TextBoxBeginDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');
					if(returnValue != null) Form1.TextBoxBeginDate.value=returnValue;
					}
		</script>
</HEAD>
<body background=../IMAGES/Page/bg01.gif 
MS_POSITIONING="GridLayout">
<form id=Form1 method=post runat="server"><FONT face=宋体>
<TABLE id=Table3 
style="Z-INDEX: 101; LEFT: 5%; WIDTH: 94%; POSITION: absolute; TOP: 5%; HEIGHT: 80%" 
borderColor=#666666 height=127 cellSpacing=1 cellPadding=1 width=383 
align=center border=1>
  <TR bgColor=#eeeeee>
    <TD style="HEIGHT: 4px" colSpan=2 height=4><FONT 
      color=#ff0000><SPAN class=style1 
      ><IMG height=16 src="../IMAGES/Page/post.gif" width=15 ><STRONG 
      >&nbsp; <asp:label id=lbTitle runat="server">汇总退单数据</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
<asp:label id=Label1 runat="server" ForeColor="ControlText">操作员代码：</asp:label><SPAN 
      class=style3><asp:label id=Label_uid runat="server">Label</asp:label></SPAN></STRONG></SPAN></FONT></TD></TR>
  <TR>
    <TD style="HEIGHT: 126px" vAlign=top align=center width="100%" colSpan=2 
    height=0><FONT face=宋体>
      <TABLE id=Table1 cellSpacing=1 cellPadding=1 width="99%" border=1 
      >
        <TR>
          <TD colSpan=3><asp:datagrid id=DataGrid1 runat="server" GridLines="Horizontal" AutoGenerateColumns="False" CellPadding="3" BackColor="White" BorderColor="#E7E7FF" BorderWidth="1px" BorderStyle="None" Width="100%" EnableViewState="False">
												<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
												<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
												<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
												<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
												<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
												<Columns>
												<asp:HyperLinkColumn DataNavigateUrlField="FUrl" DataNavigateUrlFormatString="RefundQuery.Aspx?{0}" DataTextField="Detail">
														<ItemStyle Font-Underline="True" ForeColor="Blue"></ItemStyle>
													</asp:HyperLinkColumn>
													<asp:BoundColumn DataField="FDate" HeaderText="日期"></asp:BoundColumn>
													<asp:BoundColumn DataField="FBankID" HeaderText="银行"></asp:BoundColumn>
													<asp:BoundColumn DataField="FStatusName" HeaderText="当前状态"></asp:BoundColumn>
													<asp:BoundColumn DataField="FMsg" HeaderText="操作提示"></asp:BoundColumn>
												</Columns>
												<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
											</asp:datagrid></TD></TR></TABLE></FONT></TD></TR>
  <TR>
    <TD align=center height=25 rowSpan=1><asp:label id=Label2 runat="server">选择日期</asp:label><asp:textbox id=TextBoxBeginDate runat="server" BorderColor="Gray" BorderWidth="1px"></asp:textbox><asp:imagebutton id=ButtonBeginDate runat="server" ImageUrl="../Images/Public/edit.gif"></asp:imagebutton></TD>
    <td><asp:button id=Button1 runat="server" Text="取得最新状态"></asp:button></TD></TR></TABLE></FONT></FORM>
	</body>
</HTML>
