// QuanLyPhuongTien.cs
namespace AutoSpeed;

public class QuanLyPhuongTien
{
    private readonly List<PhuongTien> _danhSach = new();

    // 1. Thêm phương tiện
    public void AddPhuongTien(PhuongTien pt)
    {
        ArgumentNullException.ThrowIfNull(pt);
        _danhSach.Add(pt);
    }

    // 2. In toàn bộ danh sách kèm giá lăn bánh (ĐA HÌNH)
    public void DisplayAll()
    {
        if (_danhSach.Count == 0)
        {
            Console.WriteLine("Danh sách trống.");
            return;
        }

        foreach (PhuongTien pt in _danhSach)
        {
            Console.WriteLine(pt.GetInfo());                       // gọi đúng bản override
            Console.WriteLine($"   => Giá lăn bánh: {pt.TinhGiaLanBanh():N0} VNĐ");
            Console.WriteLine();
        }
    }

    // 3. Tìm xe có giá lăn bánh cao nhất
    public PhuongTien? FindMaxGiaLanBanh()
    {
        if (_danhSach.Count == 0) return null;
        return _danhSach.MaxBy(pt => pt.TinhGiaLanBanh());
    }

    // 4. Tìm theo tên hãng (không phân biệt hoa thường)
    public List<PhuongTien> SearchByName(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return new List<PhuongTien>();

        return _danhSach
            .Where(pt => pt.TenHang.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}