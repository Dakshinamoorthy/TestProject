using System;
using System.Windows.Forms;

namespace NamecheapUITests.PageObject.HelperPages.WinForm.Paypal
{
    public class Paypal
    {
        [STAThread]
        public void RunPayPalLiveCard()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LivePaypalInputForm());

        }
    }
}