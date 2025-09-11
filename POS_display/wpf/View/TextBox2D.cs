using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace POS_display.wpf.View
{
    public class TextBox2D : System.Windows.Controls.TextBox
    {
        public TextBox2D()
        {
            this.PreviewTextInput += new TextCompositionEventHandler(TextBox2D_PreviewTextInput);
        }

        private void TextBox2D_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == ((char)29).ToString())
            {
                this.Text += (char)29;
                this.CaretIndex = this.Text.Length + 1;
            }
        }
    }
}
