using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

public class HorizontalLine : Label
{
    private Color border_color = SystemColors.ControlText;
    private int border_width = 1;

    [Category("Appearance"), Description("To set the border color."), DefaultValue(typeof(Color), "ControlText")]
    public Color BorderColor
    {
        get { return border_color; }
        set { border_color = value; }
    }

    [Category("Appearance"), Description("To set the border width."), DefaultValue(typeof(int), "1")]
    public int BorderWidth
    {
        get { return border_width; }
        set 
        {
            border_width = value;
            this.Height = border_width;
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        ControlPaint.DrawBorder(e.Graphics, ClientRectangle,
                                     border_color, border_width, ButtonBorderStyle.Solid,
                                     border_color, border_width, ButtonBorderStyle.Solid,
                                     border_color, border_width, ButtonBorderStyle.Solid,
                                     border_color, border_width, ButtonBorderStyle.Solid);
        this.Height = border_width;
    }

    public override string Text 
    {
        get { return ""; }
        set { value = ""; }
    }

    public override bool AutoSize
    {
        get { return false; }
        set { value = false; }
    }
}