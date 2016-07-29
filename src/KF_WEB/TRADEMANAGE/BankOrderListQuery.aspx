<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Page language="c#" Codebehind="BankOrderListQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.BankOrderListQuery" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>BankOrderListQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
        <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
		<script src="../SCRIPTS/Local.js"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT>
			<br>
			<TABLE id="Table4" style="Z-INDEX: 101; LEFT: 16px; WIDTH: 1040px" cellSpacing="1" cellPadding="1"
				width="1040" align="center" border="1">
				<TR bgColor="#eeeeee">
					<TD colspan="2"><IMG height="16" src="../IMAGES/Page/post.gif" width="15">&nbsp;<asp:label id="lbTitle" runat="server" ForeColor="Red">���ж�����ѯ</asp:label>
						&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
						</FONT>����Ա����: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN>
					</TD>
				</TR>
				<TR>
                    <TD colspan="2">&nbsp;&nbsp;<asp:label id="Label2" runat="server">�Ƹ�ͨ����</asp:label>
					<asp:textbox id="txtUinListId" runat="server"></asp:textbox>
                   </TD>
                </TR>
                <TR>
                    <TD>&nbsp;&nbsp;<asp:label id="Label4" runat="server">���ж�����</asp:label>
					<asp:textbox id="txtBankListId" runat="server"></asp:textbox>
                  ������ <asp:label id="Label3" runat="server">��ʼ����</asp:label>
						<asp:textbox id="TextBoxBeginDate" runat="server" Width="152px" BorderStyle="Groove"  onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>
						<FONT face="����">&nbsp;
						</FONT>
						<asp:label id="Label5" runat="server">��������</asp:label>
						<asp:textbox id="TextBoxEndDate" runat="server" Width="152px" BorderStyle="Groove"  onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>
						
              
                   </TD>
                   <TD Width="30%">
                    &nbsp;&nbsp;<FONT face="����"><asp:button id="Button1" runat="server" Width="80px" Text="�� ѯ" onclick="btnSearch_Click"></asp:button></FONT></TD>
				</TR>
               <TR>
                    <TD><asp:label id="Label10" runat="server">ѡ��������ѯ�ļ���</asp:label>&nbsp;<asp:FileUpload id="File1" runat="server" /></TD>
                    <TD Width="30%"><asp:button id="Button2" runat="server" Visible="true" Width="80px" Text="������ѯ" onclick="btnSearchMore_Click"></asp:button> 
                    <asp:Button ID="btn_outExcel" Width="80px" runat="server" Text="�������" OnClick="btn_outExcel_Click" Visible="true"></asp:Button>
                    </td>
              </TR>
                </TABLE>
			<TABLE id="Table1" style="Z-INDEX: 102; LEFT: 16px; WIDTH: 98%" cellSpacing="1" cellPadding="1"
				width="1040" align="center" border="1" runat="server">
				<TR>
					<TD><asp:datagrid id="DataGrid1" runat="server" Width="100%" EnableViewState="False" PageSize="200"
							AutoGenerateColumns="False" CellPadding="3" BackColor="White" BorderWidth="1px" BorderStyle="None"
							BorderColor="#E7E7FF" GridLines="Horizontal">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle   ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
                              <asp:TemplateColumn HeaderText="���ж���">
									<ItemTemplate>

                                       <%# "<a href ='FundQuery.aspx?from=bankquery&checkdate="+ ConvertDateToString(Eval("Fpay_time"))+"&czID="+Eval("Fbank_listid")+"'>"+Eval("Fbank_listid")+"</a>" %>

									</ItemTemplate>
								</asp:TemplateColumn>
                                
                                <asp:TemplateColumn HeaderText="�Ƹ�ͨ����">
									<ItemTemplate>
                                        <%# (Eval("Page_OrderTypeID")!=null&&Eval("Page_OrderTypeID").ToString()=="1")?"<a href ='TradeLogQuery.aspx?from=bankquery&id="+Eval("Flistid")+"'>"+Eval("Flistid")+"</a>":Eval("Flistid") %>
									</ItemTemplate>
								</asp:TemplateColumn>
                                <asp:BoundColumn DataField="Fpaynum_str" HeaderText="���׽��"><HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
							        <ItemStyle Wrap="False" HorizontalAlign=Center></ItemStyle></asp:BoundColumn>
								<asp:BoundColumn DataField="TradeState_str" HeaderText="����״̬"><HeaderStyle Wrap="False" HorizontalAlign="Center" ></HeaderStyle>
							        <ItemStyle Wrap="False" HorizontalAlign=Center></ItemStyle></asp:BoundColumn>
                                <asp:BoundColumn DataField="CompanyName" HeaderText="�̻�����"><HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
							        <ItemStyle Wrap="False" HorizontalAlign=Center></ItemStyle></asp:BoundColumn>
                                <asp:BoundColumn DataField="WWWAdress" HeaderText="�̻���ַ"><HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
							        <ItemStyle Wrap="False" HorizontalAlign=Center></ItemStyle></asp:BoundColumn>
                               <asp:BoundColumn DataField="Fbuy_bank_type_str" HeaderText="��������"><HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
							        <ItemStyle Wrap="False" HorizontalAlign=Center></ItemStyle></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fmemo" HeaderText="����˵��">
                                    <HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
							        <ItemStyle Wrap="False" HorizontalAlign=Center></ItemStyle>
                                 </asp:BoundColumn>
								<%--<asp:TemplateColumn HeaderText="�༭">
									<ItemTemplate>
										<asp:LinkButton id="lbChange" runat="server" CommandName="CHANGE">�༭</asp:LinkButton>&nbsp;
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="ɾ��">
									<ItemTemplate>
										<asp:LinkButton id="lbDel" runat="server" CommandName="DEL">ɾ��</asp:LinkButton>
									</ItemTemplate>
								</asp:TemplateColumn>--%>
							</Columns>
							<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>

