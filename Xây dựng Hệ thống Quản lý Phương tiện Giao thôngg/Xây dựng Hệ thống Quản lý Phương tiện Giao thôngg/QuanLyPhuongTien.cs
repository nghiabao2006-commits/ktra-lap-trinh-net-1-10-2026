
namespace AutoSpeed;

public class QuanLyPhuongTien
{
    private readonly List<PhuongTien> _danhSach = new();

    public void AddPhuongTien(PhuongTien pt)
    {
        ArgumentNullException.ThrowIfNull(pt);
        _danhSach.Add(pt);
    }

    public void DisplayAll()
    {
        if (_danhSach.Count == 0)
        {
            Console.WriteLine("Danh sách trống.");
            return;
        }

        foreach (PhuongTien pt in _danhSach)
        {
            Console.WriteLine(pt.GetInfo());                       
            Console.WriteLine($"   => Giá lăn bánh: {pt.TinhGiaLanBanh():N0} VNĐ");
            Console.WriteLine();
        }
    }

    public PhuongTien? FindMaxGiaLanBanh()
    {
        if (_danhSach.Count == 0) return null;
        return _danhSach.MaxBy(pt => pt.TinhGiaLanBanh());
    }

    public List<PhuongTien> SearchByName(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return new List<PhuongTien>();

        return _danhSach
            .Where(pt => pt.TenHang.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}
