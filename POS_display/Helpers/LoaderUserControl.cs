using System;
using System.Collections.Generic;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace POS_display.Helpers
{
    public partial class LoaderUserControl : UserControl
    {
        #region Fields
        private readonly int _increment = 1;
        private readonly int _radius = 3;
        private readonly int _n = 8;
        private int _next;
        private Timer _timer = null;
        #endregion

        #region Constructor
        public LoaderUserControl()
        {
            _timer = new Timer();
            _timer.Tick += (s, e) => Invalidate();

            if (!DesignMode)
                _timer.Enabled = true;

            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.UserPaint |
                     ControlStyles.SupportsTransparentBackColor, true);

            BackColor = Color.Transparent;
        }
        #endregion

        #region Properties
        public bool IsLoading { get; set; } = false;
        public int SpinnerLength { get; set; } = 80;
        #endregion

        #region Override
        protected override void OnPaint(PaintEventArgs e)
        {            
            if (Parent != null && (BackColor.A != 255 || BackColor == Color.Transparent))
            {
                using (var bmp = new Bitmap(Parent.Width, Parent.Height))
                {
                    Parent.Controls.Cast<Control>()
                          .Where(c => Parent.Controls.GetChildIndex(c) > Parent.Controls.GetChildIndex(this) && c.Bounds.IntersectsWith(Bounds))
                          .OrderByDescending(c => Parent.Controls.GetChildIndex(c))
                          .ToList()
                          .ForEach(c => c.DrawToBitmap(bmp, c.Bounds));

                    e.Graphics.DrawImage(bmp, -Left, -Top);

                    if (this.BackColor != Color.Transparent)
                        e.Graphics.FillRectangle(new SolidBrush(this.BackColor), new Rectangle(0, 0, Width, Height));
                }
            }

            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            if (!IsLoading) return;

            var length = SpinnerLength;
            var center = new PointF(Width / 2, Height / 2);
            var bigRadius = length / 2 - _radius - (_n - 1) * _increment;
            float unitAngle = 360 / _n;

            if (!DesignMode)
                _next++;

            _next = _next >= _n ? 0 : _next;
            var a = 0;
            for (var i = _next; i < _next + _n; i++)
            {
                var factor = i % _n;
                var c1X = center.X + (float)(bigRadius * Math.Cos(unitAngle * factor * Math.PI / 180));
                var c1Y = center.Y + (float)(bigRadius * Math.Sin(unitAngle * factor * Math.PI / 180));
                var currRad = _radius + a * _increment;
                var c1 = new PointF(c1X - currRad, c1Y - currRad);

                e.Graphics.FillEllipse(Brushes.Black, c1.X, c1.Y, 2 * currRad, 2 * currRad);

                using (Pen pen = new Pen(Color.Black, 2))
                    e.Graphics.DrawEllipse(pen, c1.X, c1.Y, 2 * currRad, 2 * currRad);

                a++;
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            _timer.Enabled = Visible;
            base.OnVisibleChanged(e);
        }
        #endregion
    }
}
