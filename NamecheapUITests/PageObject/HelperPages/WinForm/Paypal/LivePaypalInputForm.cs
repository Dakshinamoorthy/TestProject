using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
namespace NamecheapUITests.PageObject.HelperPages.WinForm.Paypal
{
    public partial class LivePaypalInputForm : Form
    {
        private static readonly string PasswordPlaceholder = UiConstantHelper.PasswordPlaceholder;
        private static readonly string EmailPlaceholder = UiConstantHelper.EmailPlaceholder;
        private readonly PrivateFontCollection _privateFontCollection = new PrivateFontCollection();
        public LivePaypalInputForm()
        {
            InitializeComponent();
            Error_Pic.Enabled = false;
            Error_Pic.Cursor = Cursors.IBeam;
            Paywithpaypal_Lbl.Location = new Point(460, 75);
            Show_HideChk.Checked = false;
        }
        private void Emailaddress_Txt_Enter(object sender, EventArgs e)
        {
            if (Emailaddress_Txt.Text != EmailPlaceholder) return;
            Emailaddress_Txt.Focus();
            Emailaddress_Txt.Text = string.Empty;
        }
        private void Emailaddress_Txt_Leave(object sender, EventArgs e)
        {
            if (Emailaddress_Txt.Text != string.Empty) return;
            Emailaddress_Txt.Text = EmailPlaceholder;
            Emailaddress_Txt.Font = new Font("Arial Narrow", 11);
            Emailaddress_Txt.ForeColor = Color.FromArgb(108, 115, 120);
            Clear_Btn.Visible = false;
            Seperator.Visible = false;
        }
        private void Emailaddress_Txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            Emailaddress_Txt.ForeColor = Color.Black;
            FontChange();
            Emailaddress_Txt.Font = new Font(_privateFontCollection.Families[0], 12);
            Seperator.Visible = true;
            Clear_Btn.Visible = true;
            ErrorPwd_icon.Visible = false;
        }
        protected void FontChange()
        {
            int fontLength1 = Properties.Resources.helvetica.Length;
            byte[] fontdata1 = Properties.Resources.helvetica;
            IntPtr data1 = Marshal.AllocCoTaskMem(fontLength1);
            Marshal.Copy(fontdata1, 0, data1, fontLength1);
            _privateFontCollection.AddMemoryFont(data1, fontLength1);
        }
        private void Password_Txt_Enter(object sender, EventArgs e)
        {
            if (Password_Txt.Text != PasswordPlaceholder) return;
            Password_Txt.Focus();
            Password_Txt.Text = string.Empty;
            Password_Txt.isPassword = true;
        }
        private void Password_Txt_Leave(object sender, EventArgs e)
        {
            if (Password_Txt.Text != string.Empty) return;
            Password_Txt.Text = PasswordPlaceholder;
            Password_Txt.Font = new Font("Arial Narrow", 11);
            Password_Txt.ForeColor = Color.FromArgb(108, 115, 120);
            Password_Txt.isPassword = false;
            Clear_Btn.Visible = false;
            Seperator.Visible = false;
        }
        private void Password_Txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            Password_Txt.ForeColor = Color.Black;
            FontChange();
            Password_Txt.Font = new Font(_privateFontCollection.Families[0], 12);
            Clear_Btn.Visible = true;
            Seperator.Visible = true;
            ErrorPwd_icon.Visible = false;
        }
        private void Show_HideChk_OnChange(object sender, EventArgs e)
        {
            if (Show_HideChk.Checked)
            {
                Password_Txt.isPassword = false;
                Show_HideLbl.Text = @"Hide Password";
            }
            else if (Show_HideChk.Checked == false)
            {
                Password_Txt.isPassword = true;
                Show_HideLbl.Text = @"Show Password";
            }
        }
        private void bunifuFlatButton3_Click(object sender, EventArgs e)
        {
            if ((Emailaddress_Txt.Text.Equals(EmailPlaceholder) || string.IsNullOrEmpty(Emailaddress_Txt.Text)) &&
                (Password_Txt.Text.Equals(PasswordPlaceholder) || string.IsNullOrEmpty(Password_Txt.Text)))
            {
                Emailaddress_Txt.Text = EmailPlaceholder;
                Password_Txt.Text = PasswordPlaceholder;
                ErrorEmail_icon.Visible = true;
                ErrorPwd_icon.Visible = true;
                Emailaddress_Txt.LineIdleColor = Color.FromArgb(199, 47, 56);
                Emailaddress_Txt.ForeColor = Color.FromArgb(199, 47, 56);
                Password_Txt.LineIdleColor = Color.FromArgb(199, 47, 56);
                Password_Txt.ForeColor = Color.FromArgb(199, 47, 56);
            }
            else if (Emailaddress_Txt.Text.Equals(EmailPlaceholder) || string.IsNullOrEmpty(Emailaddress_Txt.Text))
            {
                Emailaddress_Txt.Text = EmailPlaceholder;
                ErrorEmail_icon.Visible = true;
                Emailaddress_Txt.LineIdleColor = Color.FromArgb(199, 47, 56);
                Emailaddress_Txt.ForeColor = Color.FromArgb(199, 47, 56);
            }
            else if (Password_Txt.Text.Equals(PasswordPlaceholder) || string.IsNullOrEmpty(Password_Txt.Text))
            {
                Password_Txt.Text = PasswordPlaceholder;
                ErrorPwd_icon.Visible = true;
                Password_Txt.LineIdleColor = Color.FromArgb(199, 47, 56);
                Password_Txt.ForeColor = Color.FromArgb(199, 47, 56);
            }
            else
            {
                ErrorPwd_icon.Visible = false;
                Error_Pic.Visible = false;
                ErrorEmail_icon.Visible = false;
                Password_Txt.LineIdleColor = Color.FromArgb(157, 163, 166);
                Emailaddress_Txt.LineIdleColor = Color.FromArgb(157, 163, 166);
                var isValidEmail = IsValidEmail(Emailaddress_Txt.Text);
                if (!isValidEmail)
                {
                    Paywithpaypal_Lbl.Location = new Point(460, 46);
                    Error_Pic.Visible = true;
                }
                else
                {
                    var papalDict = AddDetailsToDict(Emailaddress_Txt.Text, Password_Txt.Text);
                    UtilityHelper.LivePaypalDetails = papalDict;
                }
            }
        }
        protected bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        protected SortedDictionary<Enum, string> AddDetailsToDict(string email, string pwd)
        {
            var realPayPalDetailsDict = new SortedDictionary<Enum, string>();
            var emailAddress = email;
            var password = pwd;
            realPayPalDetailsDict.Add(EnumHelper.PayPalDetails.EmailAddress, emailAddress);
            realPayPalDetailsDict.Add(EnumHelper.PayPalDetails.Password, password);
            DisplayCompletedPage();
            return realPayPalDetailsDict;
        }
        private void Close_Btn_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void Min_Btn_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
        private void Clear_Btn_Click(object sender, EventArgs e)
        {
            Emailaddress_Txt.Text = string.Empty;
            Password_Txt.Text = string.Empty;
        }
        protected void DisplayCompletedPage()
        {
            LoadingWheel.Focus();
            LoadingTxt_Lbl.Focus();
            Wait_Txt.Focus();
            panel2.Visible = true;
            LoadingTxt_Lbl.Visible = true;
            LoadingWheel.Visible = true;
            Wait_Txt.Visible = true;
            Lock_icon.Visible = true;
            bunifuFlatButton1.Enabled = false;
            bunifuFlatButton2.Enabled = false;
            bunifuFlatButton1.Cursor = Cursors.No;
            bunifuFlatButton2.Cursor = Cursors.No;
            Loading_Timer.Start();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            // panel2.Visible = false;
            LoadingTxt_Lbl.Visible = false;
            LoadingWheel.Visible = false;
            Wait_Txt.Visible = false;
            Lock_icon.Visible = false;
            Loading_Timer.Stop();
            done_Pic.Visible = true;
            Done_label.Visible = true;
            data1_Lbl.Visible = true;
            data2_Lbl.Visible = true;
            done_timer.Start();
        }
        private void done_timer_Tick(object sender, EventArgs e)
        {
            done_Pic.Visible = false;
            Done_label.Visible = false;
            data1_Lbl.Visible = false;
            data2_Lbl.Visible = false;
            done_timer.Stop();
            Application.Exit();
        }
    }
}