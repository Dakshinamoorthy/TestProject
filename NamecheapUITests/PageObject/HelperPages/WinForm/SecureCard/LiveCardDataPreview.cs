using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
namespace NamecheapUITests.PageObject.HelperPages.WinForm.SecureCard
{
    public partial class LiveCardDataPreview : Form
    {
        private LiveCardInputForm _inputcard;
        public LiveCardDataPreview()
        {
            InitializeComponent();
            EditAddPanel.Hide();
            //_instanceOfForm2 = new LiveCardInputForm();
            Adddetails.Hide();
            EditButton.Hide();
            /* bunifuImageButton1.Hide();*/
        }
        private void PreviewCard_Load(object sender, EventArgs e)
        {
            Size = new Size(459, 288);
            label1.Text = @"dkreative";
            label1.Font = new Font("Serif", 10, FontStyle.Bold);
            label2.Text = @"in OUR ctrl";
            label2.Font = new Font("Serif", 7, FontStyle.Bold);
            label3.Text = @"moneyback
         card ";
            label3.Font = new Font("Serif", 11, FontStyle.Bold);
            label4.Text = @"Doorstep of the web";
            label4.Font = new Font("Serif", 8, FontStyle.Regular);
            label5.Font = new Font("Serif", 8, FontStyle.Bold);
            label6.Text = @"MONTH/YEAR";
            label6.Font = new Font("Serif", 7, FontStyle.Regular);
            label7.Text = @"VALID
THRU ";
            label7.Font = new Font("Serif", 5, FontStyle.Bold);
            label8.Text = @"/";
            PrivateFontCollection pfc = new PrivateFontCollection();
            int fontLength = Properties.Resources.OCRAEXT.Length;
            byte[] fontdata = Properties.Resources.OCRAEXT;
            IntPtr data = Marshal.AllocCoTaskMem(fontLength);
            Marshal.Copy(fontdata, 0, data, fontLength);
            pfc.AddMemoryFont(data, fontLength);
            label9.Font = new Font(pfc.Families[0], 22, FontStyle.Bold);
            label10.Font = new Font(pfc.Families[0], 14, FontStyle.Bold);
            label11.Font = new Font(pfc.Families[0], 14, FontStyle.Bold);
            label12.Font = new Font(pfc.Families[0], 12, FontStyle.Bold);
            label8.Font = new Font(pfc.Families[0], 12, FontStyle.Bold);
        }
        private void PreviewCard_MouseHover(object sender, EventArgs e)
        {
            Adddetails.Show();
            EditButton.Show();
            Size = new Size(459, 316);
            EditAddPanel.Show();
            /* bunifuImageButton1.Show();
             panel1.BackColor = Color.FromArgb(25, Color.Transparent);*/
        }
        private void pictureBox2_MouseHover(object sender, EventArgs e)
        {
            Size = new Size(459, 316);
            EditAddPanel.Show();
            Adddetails.Show();
            EditButton.Show();
            /*  panel1.Show();
              bunifuImageButton1.Show();
              panel1.BackColor = Color.FromArgb(25, Color.Transparent);*/
        }
        public void Adddetails_Click(object sender, EventArgs e)
        {
            // LiveCardInputForm Myform = new LiveCardInputForm();
            var realCardDetailsDict = new SortedDictionary<Enum,
          string>();
            var cardNumber = Regex.Replace(Regex.Replace(label9.Text, @"\s+", ""), @"[^0-9a-zA-Z]+", "");
            var cardHolder = label12.Text;
            var expmonth = label10.Text;
            var expYear = label11.Text;
            var cvvNumber = Cvvlbl.Text;
            realCardDetailsDict.Add(EnumHelper.CardDetails.CardNumber, cardNumber);
            realCardDetailsDict.Add(EnumHelper.CardDetails.NameonCard, cardHolder);
            realCardDetailsDict.Add(EnumHelper.CardDetails.ExpMonth, expmonth);
            realCardDetailsDict.Add(EnumHelper.CardDetails.ExpYear, expYear);
            realCardDetailsDict.Add(EnumHelper.CardDetails.CvvNumber, cvvNumber);
            UtilityHelper.LivecardDetails = realCardDetailsDict;
            _inputcard = new LiveCardInputForm();
            _inputcard.Dispose();
            _inputcard.Close();
            Close();
        }
        private void EditAddPanel_MouseHover(object sender, EventArgs e)
        {
            Size = new Size(459, 316);
            EditAddPanel.Show();
            Adddetails.Show();
            EditButton.Show();
        }
        private void Adddetails_MouseMove(object sender, MouseEventArgs e)
        {
            Size = new Size(459, 316);
            EditAddPanel.Show();
            Adddetails.Show();
            EditButton.Show();
        }
        public void EditButton_Click(object sender, EventArgs e)
        {
            Hide();
            _inputcard = new LiveCardInputForm();
            _inputcard.ShowDialog();
        }
    }
}