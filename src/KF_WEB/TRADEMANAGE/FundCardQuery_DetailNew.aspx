<%@ Page language="c#" Codebehind="FundCardQuery_DetailNew.aspx.cs" AutoEventWireup="true" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.FundCardQuery_DetailNew" %>
<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FundCardQuery_Detail</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); .style2 { FONT-WEIGHT: bold; COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
	</HEAD>
	<BODY>
		<form id="Form1" method="post" encType="multipart/form-data" runat="server">
			<FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT>
			<br>
			<asp:panel id="Paneltitel" runat="server" Width="100%">
<TABLE cellSpacing=1 cellPadding=0 width="100%" align=center bgColor=#666666 
border=0>
  <TR bgColor=#eeeeee>
    <TD><FONT color=#ff0000><IMG height=20 src="../IMAGES/Page/post.gif" 
      width=20> <B>�ֻ���ֵ����¼��ѯ</B></FONT> 
      <DIV align=right><FONT face=Tahoma color=#ff0000></FONT></DIV></TD>
    <TD><FONT color=#ff0000><B>����Ա����: </B></FONT><SPAN class=style1><FONT 
      color=#ff0000><B>
<asp:label id=Label_uid runat="server">Label</asp:label></B></FONT></SPAN></TD></TR></TABLE>
<TABLE id=Table3 cellSpacing=1 cellPadding=1 width="100%" align=center 
  border=1>
  <TR>
    <TD style="HEIGHT: 6px" align=center colSpan=4></TD></TR>
  <TR>
    <TD align=right>
<asp:label id=Label4 runat="server">
								��ֵ���ţ�</asp:label></TD>
    <TD style="WIDTH: 306px">
<asp:textbox id=TextBox1_ListID runat="server" Width="192px" BorderColor="Gray" BorderWidth="1px"></asp:textbox></TD>
    <TD align=right>
<asp:label id=Label8 runat="server">
								����Ӧ�̶����ţ�</asp:label></TD>
    <TD>
<asp:textbox id=TextBox_Fsupply_list runat="server" Width="199px" BorderColor="Gray" BorderWidth="1px"></asp:textbox></TD></TR>
  <TR>
    <TD align=right>
<asp:label id=Label18 runat="server">
								��ֵ�����кţ�</asp:label></TD>
    <TD style="WIDTH: 306px">
<asp:textbox id=Textbox_Fcard_id runat="server" Width="192px" BorderColor="Gray" BorderWidth="1px"></asp:textbox></TD>
    <TD align=center colSpan=2>
<asp:button id=btsearch runat="server" Width="89px" Text="��ѯ"></asp:button></TD></TR>
  <TR>
    <TD align=center 
colSpan=4>&nbsp;&nbsp;</TD></TR></TABLE>
			</asp:panel>
			<asp:panel id="PanelList" runat="server" Width="100%">
<asp:DataGrid id=DGData runat="server" Width="100%" BorderColor="#E7E7FF" BorderWidth="1px" AutoGenerateColumns="False" PageSize="50" BorderStyle="None" BackColor="White" CellPadding="3" GridLines="Horizontal">
<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C">
</SelectedItemStyle>

<AlternatingItemStyle BackColor="#F7F7F7">
</AlternatingItemStyle>

<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF">
</ItemStyle>

<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C">
</HeaderStyle>

<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE">
</FooterStyle>

<Columns>
<asp:BoundColumn DataField="Flistid" HeaderText="��ֵ����">
<HeaderStyle Width="120px">
</HeaderStyle>
</asp:BoundColumn>
<asp:BoundColumn DataField="Fcard_id" HeaderText="�����к�">
<HeaderStyle Width="150px">
</HeaderStyle>
</asp:BoundColumn>
<asp:BoundColumn DataField="Fsupply_list" HeaderText="����Ӧ�̶�����">
<HeaderStyle Width="180px">
</HeaderStyle>
</asp:BoundColumn>
<asp:BoundColumn DataField="FStateName" HeaderText="����״̬">
<HeaderStyle Width="100px">
</HeaderStyle>
</asp:BoundColumn>
<asp:BoundColumn DataField="FSignName" HeaderText="���ױ��">
<HeaderStyle Width="100px">
</HeaderStyle>
</asp:BoundColumn>
<asp:BoundColumn DataField="FNumYuan" HeaderText="���">
<HeaderStyle Width="100px">
</HeaderStyle>
</asp:BoundColumn>
<asp:BoundColumn DataField="FCardtypeName" HeaderText="������">
<HeaderStyle Width="100px">
</HeaderStyle>
</asp:BoundColumn>
<asp:ButtonColumn Text="��ϸ" CommandName="Select">
<HeaderStyle Width="100px">
</HeaderStyle>
</asp:ButtonColumn>
</Columns>
</asp:DataGrid>
<webdiyer:aspnetpager id="pager" runat="server" NumericButtonTextFormatString="[{0}]" SubmitButtonText="ת��" 
OnPageChanged="ChangePage" HorizontalAlign="right" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0" 
ShowCustomInfoSection="left" NumericButtonCount="5" AlwaysShow="True"></webdiyer:aspnetpager>
			</asp:panel>
			<asp:panel id="PanelDetail" runat="server" Width="100%" Height="288px">
<TABLE id=Table2 style="WIDTH: 1105px; HEIGHT: 280px" cellSpacing=1 
cellPadding=1 width=1105 align=center border=1>
  <TR bgColor=#eeeeee height=24>
    <TD colSpan=2><FONT color=#ff0000><SPAN class=style1><IMG height=16 
      src="../IMAGES/Page/post.gif" width=15><STRONG>&nbsp; 
<asp:label id=lbTitle runat="server">�ֻ���ֵ����ϸ��Ϣ</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
      <SPAN class=style3></SPAN></STRONG></SPAN></FONT></TD></TR>
  <TR>
    <TD align=center colSpan=2>
      <TABLE id=Table1 style="WIDTH: 1096px; HEIGHT: 232px" cellSpacing=1 
      cellPadding=1 width=1096 border=0 runat="server">
        <TR style="HEIGHT: 1px" borderColor=#999999 bgColor=#999999>
          <TD colSpan=4></TD></TR>
        <TR>
          <TD style="HEIGHT: 1px" align=center width="20%">
            <P align=right>
<asp:label id=Label10 runat="server">��ֵ���ţ�</asp:label><FONT 
            face=����>:</FONT></P></TD>
          <TD style="WIDTH: 201px; HEIGHT: 1px" align=center width=201>
            <P align=left>
<asp:label id=labFListID runat="server" ForeColor="Blue"></asp:label></P></TD>
          <TD style="WIDTH: 160px; HEIGHT: 1px" align=center width=160>
            <P 
            align=right>
<asp:label id=Label1 runat="server">��ֵ��</asp:label><FONT 
            face=����>:</FONT></P></TD>
          <TD style="HEIGHT: 1px" align=center width="30%">
            <P align=left>
<asp:label id=labFNum runat="server" ForeColor="Blue"></asp:label></P></TD></TR>
        <TR style="HEIGHT: 1px" borderColor=#999999 bgColor=#999999>
          <TD colSpan=4></TD></TR>
        <TR>
          <TD style="HEIGHT: 12px" align=center>
            <P align=right>
<asp:label id=Label14 runat="server">����״̬��</asp:label><FONT 
            face=����>:</FONT></P></TD>
          <TD style="WIDTH: 201px; HEIGHT: 12px" align=center>
            <P align=left>
<asp:label id=labFState runat="server" ForeColor="Blue"></asp:label></P></TD>
          <TD style="WIDTH: 160px; HEIGHT: 12px" align=center>
            <P align=right>
<asp:label id=Label12 runat="server">���ױ�ǣ�</asp:label><FONT 
            face=����>:</FONT></P></TD>
          <TD style="HEIGHT: 12px" align=center>
            <P align=left>
<asp:label id=labFSign runat="server" ForeColor="Blue"></asp:label></P></TD></TR>
        <TR style="HEIGHT: 1px" borderColor=#999999 bgColor=#999999>
          <TD colSpan=4><FONT face=����></FONT></TD></TR>
        <TR>
          <TD style="HEIGHT: 12px" align=center>
            <P align=right>
<asp:label id=Label19 runat="server">����Ӧ�̶����ţ�</asp:label><FONT 
            face=����>:</FONT></P></TD>
          <TD style="WIDTH: 201px; HEIGHT: 12px" align=center>
            <P align=left>
<asp:label id=labFsupply_list runat="server" ForeColor="Blue"></asp:label></P></TD>
          <TD style="WIDTH: 160px; HEIGHT: 12px" align=center>
            <P align=right>
<asp:label id=Label21 runat="server">�����̷��سɹ����˵��ţ�</asp:label><FONT 
            face=����>:</FONT></P></TD>
          <TD style="HEIGHT: 12px" align=center>
            <P align=left>
<asp:label id=labFsp_back_prove runat="server" ForeColor="Blue"></asp:label></P></TD></TR>
        <TR style="HEIGHT: 1px" borderColor=#999999 bgColor=#999999>
          <TD colSpan=4><FONT face=����></FONT></TD></TR>
        <TR>
          <TD style="HEIGHT: 12px" align=center>
            <P align=right>
<asp:label id=Label3 runat="server">��ֵ�����кţ�</asp:label><FONT 
            face=����>:</FONT></P></TD>
          <TD style="WIDTH: 201px; HEIGHT: 12px" align=center>
            <P align=left>
<asp:label id=LabFcard_id runat="server" ForeColor="Blue"></asp:label></P></TD>
          <TD style="WIDTH: 160px; HEIGHT: 12px" align=center>
            <P align=right>
<asp:label id=Label6 runat="server">��ֵ�����ࣺ</asp:label><FONT 
            face=����>:</FONT></P></TD>
          <TD style="HEIGHT: 12px" align=center>
            <P align=left>
<asp:label id=LabFcard_type runat="server" ForeColor="Blue"></asp:label></P></TD></TR>
        <TR style="HEIGHT: 1px" borderColor=#999999 bgColor=#999999>
          <TD colSpan=4><FONT face=����></FONT></TD></TR>
        <TR>
          <TD style="HEIGHT: 12px" align=center>
            <P align=right>
<asp:label id=Label5 runat="server">��Ӧ��ID��</asp:label><FONT 
            face=����>:</FONT></P></TD>
          <TD style="WIDTH: 201px; HEIGHT: 12px" align=center>
            <P align=left>
<asp:label id=labFsupply_id runat="server" ForeColor="Blue"></asp:label></P></TD>
          <TD style="WIDTH: 160px; HEIGHT: 12px" align=center>
            <P align=right>
<asp:label id=Label15 runat="server">��ֵ��QQ��</asp:label><FONT 
            face=����>:</FONT></P></TD>
          <TD style="HEIGHT: 12px" align=center>
            <P align=left>
<asp:label id=labFuin runat="server" ForeColor="Blue"></asp:label></P></TD></TR>
        <TR style="HEIGHT: 1px" borderColor=#999999 bgColor=#999999>
          <TD colSpan=4><FONT face=����></FONT></TD></TR>
        <TR>
          <TD style="HEIGHT: 12px" align=center>
            <P align=right>
<asp:label id=Label7 runat="server">��ֵ��������</asp:label><FONT 
            face=����>:</FONT></P></TD>
          <TD style="WIDTH: 201px; HEIGHT: 12px" align=center>
            <P align=left>
<asp:label id=labFuser_name runat="server" ForeColor="Blue"></asp:label></P></TD>
          <TD style="WIDTH: 160px; HEIGHT: 12px" align=center>
            <P align=right><FONT face=����></FONT>
<asp:label id=Label16 runat="server">����ǰʱ�䣺</asp:label><FONT 
            face=����>:</FONT></P></TD>
          <TD style="HEIGHT: 12px" align=center>
            <P align=left>
<asp:label id=labFpay_front_time runat="server" ForeColor="Blue"></asp:label></P></TD></TR>
        <TR style="HEIGHT: 1px" borderColor=#999999 bgColor=#999999>
          <TD colSpan=4></TD></TR>
        <TR>
          <TD style="HEIGHT: 12px" align=center>
            <P align=right>
<asp:label id=Label23 runat="server">�̻�����ʱ�䣺</asp:label><FONT 
            face=����>:</FONT></P></TD>
          <TD style="WIDTH: 201px; HEIGHT: 12px" align=center>
            <P align=left>
<asp:label id=labFsp_time runat="server" ForeColor="Blue"></asp:label></P></TD>
          <TD style="WIDTH: 160px; HEIGHT: 12px" align=center>
            <P align=right>
<asp:label id=Label25 runat="server">����޸�ʱ�䣺</asp:label><FONT 
            face=����>:</FONT></P></TD>
          <TD style="HEIGHT: 12px" align=center>
            <P align=left>
<asp:label id=labFmodify_time runat="server" ForeColor="Blue"></asp:label></P></TD></TR>
        <TR style="HEIGHT: 1px" borderColor=#999999 bgColor=#999999>
          <TD colSpan=4></TD></TR></TABLE></FONT></TD></TR>
  <TR bgColor=#eeeeee height=24>
    <TD align=center colSpan=2>
<asp:hyperlink id=hlBack runat="server" Width="42px" ForeColor="Blue" NavigateUrl="javascript:history.go(-1)">����</asp:hyperlink></TD></TR></TABLE>
			</asp:panel>
		</form>
	</BODY>
</HTML>
