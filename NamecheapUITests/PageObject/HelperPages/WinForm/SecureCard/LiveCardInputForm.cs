using System;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
namespace NamecheapUITests.PageObject.HelperPages.WinForm.SecureCard
{
    public partial class LiveCardInputForm : Form
    {
        private LiveCardDataPreview _instanceOfForm2;
        public LiveCardInputForm()
        {
            InitializeComponent();
            _instanceOfForm2 = new LiveCardDataPreview();
            textBox1.Enabled = true;
        }
        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void bunifuFlatButton3_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
        private void textBox1_Enter(object sender, EventArgs e)
        {
            textBox1.MaxLength = 19;
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            string sVal = textBox1.Text;
            if (!string.IsNullOrEmpty(sVal) && e.KeyCode != Keys.Back)
            {
                sVal = sVal.Replace("-", string.Empty);
                string newst = Regex.Replace(sVal, ".{4}", "$0-");
                textBox1.Text = newst;
                textBox1.SelectionStart = textBox1.Text.Length;
                if (textBox1.SelectionStart.Equals(20))
                    textBox1.Text = textBox1.Text.Remove(textBox1.Text.LastIndexOf("-", StringComparison.Ordinal));
            }
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            label1.Visible = false;
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            // only allow one decimal point
            if ((e.KeyChar == '.') && (((TextBox)sender).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
        private void SecureCardPayment_Load(object sender, EventArgs e)
        {
            // comboBox1.ItemHeight = 90;
            comboBox1.FlatStyle = FlatStyle.Flat;
            comboBox2.ItemHeight = 90;
            comboBox2.DataSource =
                Enumerable.Range(DateTime.Now.Year, DateTime.Now.Year - DateTime.Now.Year + 30).ToList();
            comboBox2.SelectedItem = DateTime.Now.Year;
            textBox2.CharacterCasing = CharacterCasing.Upper;
            ((IList)comboBox1.Items).Clear();
            int remainingmonthinyear = 1;
            int currentYear = DateTime.Now.Year;
            int selectedYear = Convert.ToInt32(comboBox2.Text);
            if (currentYear == selectedYear)
                remainingmonthinyear = DateTime.Now.Month;
            for (var i = remainingmonthinyear; i <= 12; i++)
            {
                ((IList)comboBox1.Items).Add(i < 10 ? i.ToString().PadLeft(2, '0') : i.ToString());
            }
            comboBox1.SelectedIndex = 0;
            bunifuFlatButton5.Hide();
            SetButtonVisibility();
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            bunifuFlatButton5.Show();
            var exp = new Exception();
            var T = (TextBox)sender;
            T.Text = T.Text.Trim();
            try
            {
                var x = int.Parse(T.Text);
                //Customizing Condition (Only numbers larger than zero are permitted)
                if (x <= -1)
                    throw exp;
            }
            catch (Exception)
            {
                try
                {
                    int cursorIndex = T.SelectionStart - 1;
                    T.Text = T.Text.Remove(cursorIndex, 1);
                    //Align Cursor to same index
                    T.SelectionStart = cursorIndex;
                    T.SelectionLength = 0;
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            if (textBox3.Text.Length == textBox3.MaxLength)
                textBox4.Focus();
        }
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            bunifuFlatButton5.Show();
            var exp = new Exception();
            var T = (TextBox)sender;
            T.Text = T.Text.Trim();
            try
            {
                int x = int.Parse(T.Text);
                //Customizing Condition (Only numbers larger than zero are permitted)
                if (x <= -1)
                    throw exp;
                if (((TextBox)sender).TextLength > textBox4.MaxLength)
                    textBox5.Focus();
            }
            catch (Exception)
            {
                try
                {
                    int cursorIndex = T.SelectionStart - 1;
                    T.Text = T.Text.Remove(cursorIndex, 1);
                    //Align Cursor to same index
                    T.SelectionStart = cursorIndex;
                    T.SelectionLength = 0;
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            if (textBox4.Text.Length == textBox4.MaxLength)
                textBox5.Focus();
        }
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            bunifuFlatButton5.Show();
            var exp = new Exception();
            var T = (TextBox)sender;
            T.Text = T.Text.Trim();
            try
            {
                var x = int.Parse(T.Text);
                //Customizing Condition (Only numbers larger than zero are permitted)
                if (x <= -1)
                    throw exp;
            }
            catch (Exception)
            {
                try
                {
                    var cursorIndex = T.SelectionStart - 1;
                    T.Text = T.Text.Remove(cursorIndex, 1);
                    //Align Cursor to same index
                    T.SelectionStart = cursorIndex;
                    T.SelectionLength = 0;
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            if (textBox5.Text.Length == textBox5.MaxLength)
                textBox2.Focus();
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            bunifuFlatButton5.Show();
            var T = (TextBox)sender;
            try
            {
                //Not Allowing Numbers
                char[] unallowedCharacters =
                {
                    '0',
                    '1',
                    '2',
                    '3',
                    '4',
                    '5',
                    '6',
                    '7',
                    '8',
                    '9',
                    '!',
                    '"',
                    '#',
                    '$',
                    '%',
                    '&',
                    '+',
                    '(',
                    ')',
                    '*',
                    '+',
                    ',',
                    '-',
                    '.',
                    '/',
                    ':',
                    ';',
                    '=',
                    '>',
                    '?',
                    '<',
                    '@',
                    '[',
                    '\\',
                    ']',
                    '^',
                    '_',
                    '`',
                    '{',
                    '|',
                    '}',
                    '~'
                };
                if (TextContainsUnallowedCharacter(T.Text, unallowedCharacters))
                {
                    int cursorIndex = T.SelectionStart - 1;
                    T.Text = T.Text.Remove(cursorIndex, 1).ToUpper();
                    //Align Cursor to same index
                    T.SelectionStart = cursorIndex;
                    T.SelectionLength = 0;
                }
                SetButtonVisibility();
            }
            catch (Exception)
            {
                // ignored
            }
            if (textBox2.Text.Length == textBox2.MaxLength)
                bunifuFlatButton4.Focus();
        }
        private bool TextContainsUnallowedCharacter(string T, char[] unallowedCharacters)
        {
            return unallowedCharacters.Any(T.Contains);
        }
        private void bunifuFlatButton5_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
        }
        private void bunifuFlatButton4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length < textBox1.MaxLength)
            {
                label1.Visible = true;
                label1.Text = @"Invalid Card Number";
                label1.ForeColor = Color.Red;
                label1.Font = new Font("Helvetica", 7, FontStyle.Bold);
            }
            else
            {
                _instanceOfForm2 = new LiveCardDataPreview
                {
                    label9 = { Text = CardNumber },
                    label10 = { Text = CardexpMonth },
                    label12 = { Text = CardHolderName }
                };
                label1.Visible = false;
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                textBox3.Enabled = false;
                textBox4.Enabled = false;
                textBox5.Enabled = false;
                comboBox1.Enabled = false;
                comboBox2.Enabled = false;
                bunifuFlatButton4.Hide();
                bunifuFlatButton5.Hide();
                CvvNum = textBox3.Text + textBox4.Text + textBox5.Text;
                DateTime oDate = DateTime.ParseExact(CardexpYear, "yyyy", null);
                var yearLbl = oDate.ToString("yy");
                _instanceOfForm2.Cvvlbl.Text = CvvNum;
                _instanceOfForm2.label11.Text = yearLbl;
                var num1 = Regex.Replace(CardNumber, "-", string.Empty);
                var cardNumber1 = Regex.Replace(num1, ".{4}", "$0     ");
                _instanceOfForm2.label5.Text = cardNumber1.Substring(0, 5);
                Hide();
                _instanceOfForm2.ShowDialog();
            }
        }
        private void SetButtonVisibility()
        {
            if ((textBox1.Text != string.Empty) || (textBox2.Text != string.Empty))
            {
                bunifuFlatButton5.Cursor = Cursors.Hand;
                bunifuFlatButton5.Enabled = true;
            }
            else
            {
                bunifuFlatButton5.Enabled = false;
                bunifuFlatButton5.Cursor = Cursors.No;
            }
            if ((textBox1.Text != string.Empty) && (textBox2.Text != string.Empty) && (textBox3.Text != string.Empty) &&
                (textBox4.Text != string.Empty) && (textBox5.Text != string.Empty))
            {
                bunifuFlatButton4.Enabled = true;
                bunifuFlatButton4.Cursor = Cursors.Hand;
            }
            else
            {
                bunifuFlatButton4.Enabled = false;
                bunifuFlatButton4.Cursor = Cursors.No;
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            bunifuFlatButton5.Show();
            SetButtonVisibility();
            if (((TextBox)sender).TextLength == textBox1.MaxLength)
                comboBox1.Focus();
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((IList)comboBox1.Items).Clear();
            int remainingmonthinyear = 1;
            int currentYear = DateTime.Now.Year;
            int selectedYear = Convert.ToInt32(comboBox2.Text);
            if (currentYear == selectedYear)
                remainingmonthinyear = DateTime.Now.Month;
            for (var i = remainingmonthinyear; i <= 12; i++)
            {
                ((IList)comboBox1.Items).Add(i < 10 ? i.ToString().PadLeft(2, '0') : i.ToString());
            }
            comboBox1.SelectedIndex = 0;
        }
        public string CardNumber
        {
            get { return textBox1.Text; }
            set
            {
                textBox1.Text = value;
            }
        }
        public string CardexpMonth
        {
            get { return comboBox1.Text; }
            set
            {
                comboBox1.Text = value;
            }
        }
        public string CardHolderName
        {
            get { return textBox2.Text; }
            set
            {
                textBox2.Text = value;
            }
        }
        public string CardexpYear
        {
            get { return comboBox2.Text; }
            set
            {
                comboBox2.Text = value;
            }
        }
        /* public string CardCvv
         {
             get { return textBox3.Text + textBox4.Text + textBox5.Text; }
             set { CvvNum = value; }
         }*/
        public string CvvNum { get; private set; }
    }
}