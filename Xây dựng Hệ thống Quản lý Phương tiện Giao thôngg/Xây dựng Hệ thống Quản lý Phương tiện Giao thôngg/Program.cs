
using AutoSpeed;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.InputEncoding = System.Text.Encoding.UTF8;   

var ql = new QuanLyPhuongTien();

while (true)
{
    Console.WriteLine("\n===== QUẢN LÝ PHƯƠNG TIỆN AUTOSPEED =====");
    Console.WriteLine("1. Thêm Ô tô");
    Console.WriteLine("2. Thêm Xe máy");
    Console.WriteLine("3. Hiển thị tất cả (kèm giá lăn bánh)");
    Console.WriteLine("4. Tìm phương tiện có giá lăn bánh cao nhất");
    Console.WriteLine("5. Tìm theo tên hãng");
    Console.WriteLine("0. Thoát");
    Console.Write("Chọn: ");

    string? chon = Console.ReadLine();

    try
    {
        switch (chon)
        {
            case "1": NhapOTo(ql); break;
            case "2": NhapXeMay(ql); break;
            case "3": ql.DisplayAll(); break;
            case "4":
                var max = ql.FindMaxGiaLanBanh();
                if (max == null) Console.WriteLine("Danh sách trống.");
                else
                {
                    Console.WriteLine("Phương tiện đắt nhất:");
                    Console.WriteLine(max.GetInfo());
                    Console.WriteLine($"=> Giá lăn bánh: {max.TinhGiaLanBanh():N0} VNĐ");
                }
                break;
            case "5":
                string kw = DocChuoi("Nhập từ khóa tên hãng: ");
                var kq = ql.SearchByName(kw);
                if (kq.Count == 0) Console.WriteLine("Không tìm thấy.");
                foreach (var pt in kq) Console.WriteLine(pt.GetInfo());
                break;
            case "0": return;
            default: Console.WriteLine("Lựa chọn không hợp lệ!"); break;
        }
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"[LỖI] {ex.Message}");
        Console.WriteLine("Không tạo được đối tượng. Vui lòng thử lại.");
    }
}

static void NhapOTo(QuanLyPhuongTien ql)
{
    string ma = DocChuoi("Mã PT: ");
    string hang = DocChuoi("Tên hãng: ");
    int nam = DocSoNguyen("Năm sản xuất: ");
    decimal gia = DocDecimal("Giá gốc (VNĐ): ");
    int soCho = DocSoNguyen("Số chỗ ngồi: ");
    double dungTich = DocDouble("Dung tích động cơ (lít): ");

    ql.AddPhuongTien(new OTo(ma, hang, nam, gia, soCho, dungTich));
    Console.WriteLine("Đã thêm Ô tô thành công!");
}

static void NhapXeMay(QuanLyPhuongTien ql)
{
    string ma = DocChuoi("Mã PT: ");
    string hang = DocChuoi("Tên hãng: ");
    int nam = DocSoNguyen("Năm sản xuất: ");
    decimal gia = DocDecimal("Giá gốc (VNĐ): ");
    int cc = DocSoNguyen("Dung tích xy lanh (cc): ");

    ql.AddPhuongTien(new XeMay(ma, hang, nam, gia, cc));
    Console.WriteLine("Đã thêm Xe máy thành công!");
}

static string DocChuoi(string thongBao)
{
    Console.Write(thongBao);
    return Console.ReadLine() ?? string.Empty;
}

static int DocSoNguyen(string thongBao)
{
    while (true)
    {
        Console.Write(thongBao);
        if (int.TryParse(Console.ReadLine(), out int kq)) return kq;
        Console.WriteLine("Vui lòng nhập một số nguyên!");
    }
}

static decimal DocDecimal(string thongBao)
{
    while (true)
    {
        Console.Write(thongBao);
        if (decimal.TryParse(Console.ReadLine(), out decimal kq)) return kq;
        Console.WriteLine("Vui lòng nhập một số hợp lệ!");
    }
}

static double DocDouble(string thongBao)
{
    while (true)
    {
        Console.Write(thongBao);
        if (double.TryParse(Console.ReadLine(), out double kq)) return kq;
        Console.WriteLine("Vui lòng nhập một số hợp lệ!");
    }
}
