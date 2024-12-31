using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyLinhKienDIenTu
{
    public static class Round
    {
        //làm tròn viền button
        public static void SetRoundedButton(Button button)
        {
            // Tạo một GraphicsPath để định hình của Button
            GraphicsPath path = new GraphicsPath();

            int cornerRadius = 20; // Độ cong của góc, bạn có thể điều chỉnh theo ý muốn

            // Thêm các góc vào path, làm cho góc trên cùng bên trái và góc dưới cùng bên trái trở nên tròn
            path.AddArc(new Rectangle(0, 0, cornerRadius * 2, cornerRadius * 2), 180, 90);
            path.AddLine(cornerRadius, 0, button.Width - cornerRadius, 0);
            path.AddArc(new Rectangle(button.Width - cornerRadius * 2, 0, cornerRadius * 2, cornerRadius * 2), 270, 90);
            path.AddLine(button.Width, cornerRadius, button.Width, button.Height - cornerRadius);
            path.AddArc(new Rectangle(button.Width - cornerRadius * 2, button.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2), 0, 90);
            path.AddLine(button.Width - cornerRadius, button.Height, cornerRadius, button.Height);
            path.AddArc(new Rectangle(0, button.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2), 90, 90);
            path.AddLine(0, button.Height - cornerRadius, 0, cornerRadius);
            path.CloseFigure();

            // Đặt hình dạng của Button thành hình dạng vừa tạo
            button.Region = new Region(path);
        }

        //làm tròn viền panel
        public static void SetSharpCornerPanel(Panel panel)
        {
            // Tạo một GraphicsPath để định hình của góc
            GraphicsPath path = new GraphicsPath();

            int cornerRadius = 20; // Độ cong của góc, bạn có thể điều chỉnh theo ý muốn

            // Thêm các góc vào path, trừ góc dưới cùng bên trái
            path.AddArc(new Rectangle(0, 0, cornerRadius * 2, cornerRadius * 2), 180, 90);
            path.AddArc(new Rectangle(panel.Width - cornerRadius * 2, 0, cornerRadius * 2, cornerRadius * 2), 270, 90);
            path.AddArc(new Rectangle(panel.Width - cornerRadius * 2, panel.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2), 0, 90);
            path.AddArc(new Rectangle(0, panel.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2), 90, 90);
            path.CloseFigure();

            // Đặt hình dạng của Panel thành hình dạng vừa tạo
            panel.Region = new Region(path);
        }

    }
}
