using System;
using System.Drawing;
using System.Windows.Forms;
using ItemEditor.Configuration;
using ItemEditor.Data;
using ItemEditor.Structure;

namespace Toolset.Controls
{
    public class ctlItemList : ListBox
    {
        public ctlItemList()
        {
            DrawMode = DrawMode.OwnerDrawVariable;
            ItemHeight = 13;
        }

        protected async override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index >= Items.Count || e.Index <= -1)
                return;

            object item = Items[e.Index];
            if (item == null)
                return;

            int itemID = Convert.ToInt32(item.ToString().Split(' ')[0]);

            string text = item.ToString();
            SizeF stringSize = e.Graphics.MeasureString(text, Font);

            Brush brush = Brushes.Black;

            int index = cImport.ItemData.FindIndex(p => p.ID.Equals(itemID));

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                int stringPoint = cMySQL.configStructureTool[0].ShowItemIconInList == ShowItemIconInList.ONLY_CURRENT ? 13 : 0;

                e.Graphics.FillRectangle(new SolidBrush(SystemColors.Highlight), e.Bounds);
                e.Graphics.DrawString(text, Font, new SolidBrush(Color.White), new PointF(stringPoint, e.Bounds.Y + (e.Bounds.Height - stringSize.Height) / 2));

                if (cMySQL.configStructureTool[0].ShowItemIconInList == ShowItemIconInList.ONLY_CURRENT)
                {
                    try
                    {
                        Bitmap bmpIconMain = await cImport.GetIcon(IconType.ITEM, itemID);
                        Bitmap bmpIconResized = new Bitmap(bmpIconMain, new Size(bmpIconMain.Width - 17, bmpIconMain.Height - 17));

                        e.Graphics.DrawImage(bmpIconResized,
                             new PointF(0, (e.Bounds.Y + 1) + (e.Bounds.Height - stringSize.Height)));
                    }
                    catch { } //
                }

            }
            else
            {
                try
                {
                    if (cImport.ItemData[index].changesWasHere)
                        brush = Brushes.Orange;
                }
                catch { } //

                e.Graphics.FillRectangle(new SolidBrush(Color.White), e.Bounds);
                e.Graphics.DrawString(text, Font, brush, new PointF(0, e.Bounds.Y + (e.Bounds.Height - stringSize.Height) / 2));
            }

        }

        private void InitializeComponent()
        {
            SuspendLayout();
            ResumeLayout(false);

          //  frmMain frmMain = new frmMain();
          //  DrawItem += frmMain.lbItems_DrawItem;
        }

    }
}