using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyLinhKienDIenTu
{
    internal class CloseChildChildForm
    {
        // Đảm bảo rằng bạn khai báo các thuộc tính ở đây
        public static bool CloseFormSalary { get; set; }
        public static bool CloseFormMoney { get; set; }

        static CloseChildChildForm()
        {
            CloseFormSalary = false;
            CloseFormMoney = false;
        }
    }

}
