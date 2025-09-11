using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

public class TransparentDataGridView : DataGridView
{
    // VB Converted To C# And Code Edit : Murat Çakmak
    private Image _img;
    public enum Choices
    {
        No,
        Yes
    }

    [Category("Appearance"), Description("To set the background transparent.")]
    public Choices Background_Transparent
    {
        get;
        set;
    }

    /*
    private void SetTransparentProperties(bool SetAsTransparent)
    {
        if (SetAsTransparent)
        {
            base.DoubleBuffered = true;
            base.EnableHeadersVisualStyles = false;
            base.ColumnHeadersDefaultCellStyle.BackColor = Color.Transparent;
            base.RowHeadersDefaultCellStyle.BackColor = Color.Transparent;
            SetCellStyle(Color.Transparent);
        }
        else
        {
            base.DoubleBuffered = false;
            base.EnableHeadersVisualStyles = true;
            base.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Control;
            base.RowHeadersDefaultCellStyle.BackColor = SystemColors.Control;
            SetCellStyle(Color.White);
        }
    }
    */
    protected override void PaintBackground(Graphics graphics, Rectangle clipBounds, Rectangle gridBounds)
    {
        if (Background_Transparent == Choices.Yes)
        {
            base.PaintBackground(graphics, clipBounds, gridBounds);
            if (this.Parent.BackgroundImage != null)
            {
                Rectangle rectSource = new Rectangle(this.Location.X, this.Location.Y, this.Width, this.Height);
                Rectangle rectDest = new Rectangle(0, 0, rectSource.Width, rectSource.Height);

                Bitmap b = new Bitmap(Parent.ClientRectangle.Width, Parent.ClientRectangle.Height);
                Graphics.FromImage(b).DrawImage(this.Parent.BackgroundImage, Parent.ClientRectangle);


                graphics.DrawImage(b, rectDest, rectSource, GraphicsUnit.Pixel);
                SetCellsTransparent();
            }
        }
    }

    public void SetCellsTransparent()
    {
        this.EnableHeadersVisualStyles = false;
        this.ColumnHeadersDefaultCellStyle.BackColor = Color.Transparent;
        this.RowHeadersDefaultCellStyle.BackColor = Color.Transparent;


        foreach (DataGridViewColumn col in this.Columns)
        {
            col.DefaultCellStyle.BackColor = Color.Transparent;
            col.DefaultCellStyle.SelectionBackColor = Color.Transparent;
        }
    }

    //to set background image
    /*
    protected override void PaintBackground(System.Drawing.Graphics graphics, System.Drawing.Rectangle clipBounds, System.Drawing.Rectangle gridBounds)
    {
        base.PaintBackground(graphics, clipBounds, gridBounds);

        if (_img != null)
        {
            // Create image.
            Image newImage = _img;

            // Draw image to screen.
            switch (Background_Image_Layout)
            {
                case Choices.None:
                    graphics.DrawImage(newImage, 0, 0);
                    break;
                case Choices.Tile:
                    int count_x = Size.Width / newImage.Width;
                    int count_y = Size.Height / newImage.Height;
                    for (int i = 0; i < count_x; i++ )
                    {
                        for (int j = 0; j < count_y; j++)
                        {
                            graphics.DrawImage(newImage, i * newImage.Width, j * newImage.Height);
                        }
                    }
                    break;
                case Choices.Center:
                    int x = (Size.Width - newImage.Width) / 2;
                    int y = (Size.Height - newImage.Height) / 2;
                    graphics.DrawImage(newImage, x, y);
                    break;
                case Choices.Stretch:
                    graphics.DrawImage(newImage, 0, -100, Size.Width, Size.Height+100);
                    break;
                case Choices.Zoom:
                    graphics.DrawImage(newImage, 0, 0, Size.Width, Size.Height);
                    break;
            }
        }
    }
    */
    /*
    protected override void OnColumnAdded(System.Windows.Forms.DataGridViewColumnEventArgs e)
    {
        base.OnColumnAdded(e);
        if (_img != null)
        {
            SetCellStyle(Color.Transparent);
        }
    }

    private void SetCellStyle(Color cellColour)
    {
        foreach (DataGridViewColumn col in base.Columns)
        {
            col.DefaultCellStyle.BackColor = cellColour;
            col.DefaultCellStyle.SelectionBackColor = cellColour;
        }
    }
    */
}