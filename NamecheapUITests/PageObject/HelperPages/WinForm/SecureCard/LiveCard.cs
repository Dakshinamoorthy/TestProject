using System;
using System.Windows.Forms;

namespace NamecheapUITests.PageObject.HelperPages.WinForm.SecureCard
{
    public class LiveCard
    {
        [STAThread]
        public void RunSecureCardPayment()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LiveCardInputForm());

        }
    }
}