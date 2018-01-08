using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace NamecheapUITests.PageObject.HelperPages.WinForm.Paypal
{
    public partial class MyPreLoader : UserControl
    {
        List<Color> colors = new List<Color>();
        int cur_color = 0;
        public MyPreLoader()
        {
            InitializeComponent();
            // ParentForm.Opacity = 0.5;
            colors.Add(Color.FromArgb(0, 150, 136));
            colors.Add(Color.FromArgb(0, 188, 212));
            colors.Add(Color.FromArgb(63, 81, 181));
            colors.Add(Color.FromArgb(156, 39, 176));
            colors.Add(Color.FromArgb(128, 128, 0));
            colors.Add(Color.FromArgb(184, 134, 11));

            bunifuCircleProgressbar1.ProgressColor = colors[cur_color];
        }

        int dir = 1;
        private void stretch_Tick_1(object sender, EventArgs e)
        {
            if (bunifuCircleProgressbar1.Value == 90)
            {
                dir = -1;
                bunifuCircleProgressbar1.animationSpeed = 4;
                SwitchColor();

            }
            else if (bunifuCircleProgressbar1.Value == 10)
            {
                dir = +1;
                bunifuCircleProgressbar1.animationSpeed = 2;
                SwitchColor();
            }

            bunifuCircleProgressbar1.Value += dir;
        }

        void SwitchColor()
        {
            bunifuColorTransition1.Color1 = colors[cur_color];
            if (cur_color < colors.Count - 1)
            {
                cur_color++;
            }
            else
            {
                cur_color = 0;
            }
            bunifuColorTransition1.Color2 = colors[cur_color];
            color_transition.Start();
            //  bunifuColorTransition1.Color2 = colors[cur_color];
        }

        private void color_transition_Tick_1(object sender, EventArgs e)
        {
            if (bunifuColorTransition1.ProgessValue < 100)
            {
                bunifuColorTransition1.ProgessValue++;
                bunifuCircleProgressbar1.ProgressColor = bunifuColorTransition1.Value;
            }
            else
            {
                color_transition.Stop();
                bunifuColorTransition1.Color1 = bunifuColorTransition1.Color2;
                bunifuColorTransition1.ProgessValue = 0;
            }
        }

    }
}
