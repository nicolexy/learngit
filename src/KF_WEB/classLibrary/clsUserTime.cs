using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace TENCENT.OSS.CFT.KF.KF_Web.classLibrary
{
	/// <summary>
	/// dateControl ��ժҪ˵����
	/// </summary>
	/// <summary>
	/// �û��Զ���ʱ��ؼ���ȡ���ꡢ�¡��ա�ʱ���֡���
	/// </summary>
	[DefaultProperty("Text"),
	ToolboxData("<{0}:clsUserTime runat=server></{0}:clsUserTime>")]
	//INamingContainerΪ�ӿؼ��ṩ��һ���µ�������Χ,��֤�ӿؼ���IDΨһ��
	public class clsUserTime : System.Web.UI.WebControls.WebControl, INamingContainer
	{
		private System.Web.UI.WebControls.DropDownList ddlYear;
		private DropDownList ddlMonth;
		private DropDownList ddlDay;
		private DropDownList ddlHour;
		private DropDownList ddlMinite;
		private DropDownList ddlSecond;

		private static int beginYear = 0; //��ʼ��ţ��������ʹ��static�Է����ε��ú����������
		private static bool IsInit = false; //�Ƿ��Ѿ���ʼ�������������ʹ��static�Է����ε��ú����������

		public int Year //ȡ�ؼ�ʱ�䣺��
		{
			get
			{
				return ddlYear.SelectedIndex + beginYear;
			}
		}

		public int Month //ȡ�ؼ�ʱ�䣺��
		{
			get
			{
				return ddlMonth.SelectedIndex + 1;
			}
		}

		public int Day //ȡ�ؼ�ʱ�䣺��
		{
			get
			{
				return ddlDay.SelectedIndex + 1;
			}
		}

		public int Hour //ȡ�ؼ�ʱ�䣺ʱ
		{
			get
			{
				return ddlHour.SelectedIndex;
			}
		}

		public int Minite //ȡ�ؼ�ʱ�䣺��
		{
			get
			{
				return ddlMinite.SelectedIndex;
			}
		}

		public int Second //ȡ�ؼ�ʱ�䣺��
		{
			get
			{
				return ddlSecond.SelectedIndex;
			}
		}

		//����ʼ��ų�ʼ���ؼ�������Ѿ���ʼ�����ؼ���ֱ�ӷ���
		public void InitControls(int startYear, int endYear)
		{
			if (IsInit)
			{
				return;
			}
			EnsureChildControls();
			if (startYear > endYear)
			{
				int tmp = startYear;
				startYear = endYear;
				endYear = startYear;
			}
			beginYear = startYear;
			for (int i = startYear; i <= endYear; ++i)
			{
				ddlYear.Items.Add(i.ToString());
			}
			for (int i = 1; i <=12; ++i)
			{
				ddlMonth.Items.Add(i.ToString());
			}
			for (int i = 1; i <= 31; ++i)
			{
				ddlDay.Items.Add(i.ToString());
			}
			for (int i = 0; i < 24; ++i)
			{
				ddlHour.Items.Add(i.ToString());
			}
			for (int i = 0; i < 60; ++i)
			{
				ddlMinite.Items.Add(i.ToString());
				ddlSecond.Items.Add(i.ToString());
			}
			IsInit = true;
		}
		//����Controls���ԣ�ȡ��ControlCollection���󣬸ö����ʾUI��νṹ�еķ������ؼ����ӿؼ�
		public override ControlCollection Controls
		{
			get
			{
				//ȷ���������Ƿ�����ӿؼ�
				EnsureChildControls();
				return base.Controls;
			}
		}

		//��̬�����ӿؼ�
		protected override void CreateChildControls()
		{
			//�����ꡢ�¡��ա�ʱ���֡��������б��
			this.Controls.Clear();
			ddlYear = new DropDownList();
			ddlYear.ID = "ddlYear";
			ddlYear.Width = 54;

			ddlMonth = new DropDownList();
			ddlMonth.ID = "ddlMonth";
			ddlMonth.Width = 40;

			ddlDay = new DropDownList();
			ddlDay.ID = "ddlDay";
			ddlDay.Width = 40;

			ddlHour = new DropDownList();
			ddlHour.ID = "ddlHour";
			ddlHour.Width = 40;

			ddlMinite = new DropDownList();
			ddlMinite.ID = "ddlMinite";
			ddlMinite.Width = 40;

			ddlSecond = new DropDownList();
			ddlSecond.ID = "ddlSecond";
			ddlSecond.Width = 40;

			this.Controls.Add(ddlYear);
			this.Controls.Add(ddlMonth);
			this.Controls.Add(ddlDay);
			this.Controls.Add(ddlHour);
			this.Controls.Add(ddlMinite);
			this.Controls.Add(ddlSecond);

			base.CreateChildControls ();
		}

		/// <summary>
		/// ���˿ؼ����ָ�ָ�������������
		/// </summary>
		/// <param name="output"> Ҫд������ HTML ��д�� </param>
		protected override void Render(HtmlTextWriter output)
		{
			//��Ҫ���ֵ�HTML������ӵ�ָ����System.Web.HtmlTextWriter��
			AddAttributesToRender(output);
			output.AddAttribute(System.Web.UI.HtmlTextWriterAttribute.Cellpadding, "0", false);

			//�ƶ�һ�������ӿؼ�����һ����
			output.RenderBeginTag(HtmlTextWriterTag.Table); //��
			output.RenderBeginTag(HtmlTextWriterTag.Tr); //��

			//�������б��
			output.RenderBeginTag(HtmlTextWriterTag.Td); //��
			ddlYear.RenderControl(output);
			output.RenderEndTag();

			output.RenderBeginTag(HtmlTextWriterTag.Td);
			output.Write("��");
			output.RenderEndTag();

			//�������б��
			output.RenderBeginTag(HtmlTextWriterTag.Td);
			ddlMonth.RenderControl(output);
			output.RenderEndTag();

			output.RenderBeginTag(HtmlTextWriterTag.Td);
			output.Write("��");
			output.RenderEndTag();

			//�������б��
			output.RenderBeginTag(HtmlTextWriterTag.Td);
			ddlDay.RenderControl(output);
			output.RenderEndTag();

			output.RenderBeginTag(HtmlTextWriterTag.Td);
			output.Write("��");
			output.RenderEndTag();

			//ʱ�����б��
			output.RenderBeginTag(HtmlTextWriterTag.Td);
			ddlHour.RenderControl(output);
			output.RenderEndTag();

			output.RenderBeginTag(HtmlTextWriterTag.Td);
			output.Write("ʱ");
			output.RenderEndTag();

			//�������б��
			output.RenderBeginTag(HtmlTextWriterTag.Td);
			ddlMinite.RenderControl(output);
			output.RenderEndTag();

			output.RenderBeginTag(HtmlTextWriterTag.Td);
			output.Write("��");
			output.RenderEndTag();

			//�������б��
			output.RenderBeginTag(HtmlTextWriterTag.Td);
			ddlSecond.RenderControl(output);
			output.RenderEndTag();

			output.RenderBeginTag(HtmlTextWriterTag.Td);
			output.Write("��");
			output.RenderEndTag();

			output.RenderEndTag();
			output.RenderEndTag();
		}

		public DateTime GetTime() //ȡ��ѡ����ʱ��
		{
			DateTime time = new DateTime(this.Year, this.Month, this.Day, this.Hour, this.Minite, this.Second, 0);
			return time;
		}

		//����ؼ������������б���ֵ
		public void ClearControls()
		{
			ddlYear.Items.Clear();
			ddlMonth.Items.Clear();
			ddlDay.Items.Clear();
			ddlHour.Items.Clear();
			ddlMinite.Items.Clear();
			ddlSecond.Items.Clear();
			IsInit = false;
		}

		//���������������б���ѡ��
		public void ClearSelection()
		{
			ddlYear.ClearSelection();
			ddlMonth.ClearSelection();
			ddlDay.ClearSelection();
			ddlHour.ClearSelection();
			ddlMinite.ClearSelection();
			ddlSecond.ClearSelection();
		}
	}
}