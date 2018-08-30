using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using ItemEditor.Structure.Item;

namespace ItemEditor.Controls
{

    public class ctlLanguage : ComboBox
    {

        public ctlLanguage()
        {
            this.DrawItem += new DrawItemEventHandler(ctlLanguage_DrawItem);
            this.MeasureItem += new MeasureItemEventHandler(ctlLanguage_MeasureItem);
        }

        private void ctlLanguage_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Make sure we're not trying to draw something that isn't there.
            if (e.Index >= this.Items.Count || e.Index <= -1)
                return;


            // Get the item object.
            object item = this.Items[e.Index];
            if (item == null)
                return;

            e.DrawBackground();
            string text = item.ToString();
            SizeF stringSize = e.Graphics.MeasureString(text, Font);

            // Draw item inside comboBox
            if ((e.State & DrawItemState.ComboBoxEdit) != DrawItemState.ComboBoxEdit && e.Index > -1)
            {
                if (!string.IsNullOrEmpty(text) && !string.IsNullOrWhiteSpace(text))
                {
                    Language lang = (Language)Enum.Parse(typeof(Language), text, true);
                    e.Graphics.DrawImage(Base64ToImage(getLanguageImage(lang)), new PointF(5, (e.Bounds.Y + 3) + (e.Bounds.Height - 24)));
                }

                e.Graphics.DrawString(text, Font, new SolidBrush(Color.Black), new PointF(40, e.Bounds.Y + (e.Bounds.Height - stringSize.Height) / 2));
            }
            // Draw visible text
            else if (e.Index > -1)
            {
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    e.Graphics.FillRectangle(new SolidBrush(SystemColors.Control), e.Bounds);
                }

                Language lang = (Language)Enum.Parse(typeof(Language), text, true);
                e.Graphics.DrawImage(Base64ToImage(getLanguageImage(lang)), new PointF(5, (e.Bounds.Y - 2) + (e.Bounds.Height - 24)));

                e.Graphics.DrawString(text, Font, new SolidBrush(Color.Black), new PointF(40, e.Bounds.Y + (e.Bounds.Height - stringSize.Height) / 2));
            }

            e.DrawFocusRectangle();
        }

        private void ctlLanguage_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 22;
        }

        public static String getLanguageImage(Enum value)
        {
            String valueText = value.ToString();
            Type type = value.GetType();

            FieldInfo fi = type.GetField(valueText);
            Object[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
            {
                DescriptionAttribute attribute = (DescriptionAttribute)attributes[0];
                return attribute.Description;
            }

            return valueText;
        }

        public Image Base64ToImage(string commands)
        {
            byte[] photoarray = Convert.FromBase64String(commands);
            MemoryStream ms = new MemoryStream(photoarray, 0, photoarray.Length);
            ms.Write(photoarray, 0, photoarray.Length);
            Image image = System.Drawing.Image.FromStream(ms);
            return image;
        }
    }
}
