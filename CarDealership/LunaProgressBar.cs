using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CarDealership
{
    public class LunaProgressBar : ProgressBar
    {
        private Color lunaTeal = Color.FromArgb(38, 101, 140);
        private Color lunaCyan = Color.FromArgb(84, 172, 191);

        public LunaProgressBar()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rect = this.ClientRectangle;
            Graphics g = e.Graphics;

            using (SolidBrush brush = new SolidBrush(this.BackColor))
            {
                g.FillRectangle(brush, rect);
            }

            int progressWidth = (int)(rect.Width * ((double)this.Value / this.Maximum));

            if (progressWidth > 0)
            {
                Rectangle progressRect = new Rectangle(rect.X, rect.Y, progressWidth, rect.Height);

                using (LinearGradientBrush gradient = new LinearGradientBrush(
                    progressRect, lunaCyan, lunaTeal, LinearGradientMode.Horizontal))
                {
                    g.FillRectangle(gradient, progressRect);
                }
            }

            using (Pen pen = new Pen(lunaTeal, 1))
            {
                g.DrawRectangle(pen, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
            }
        }
    }
}