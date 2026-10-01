Họ tên: Hoàng Nghĩa Bảo 
MSV: 24810320282
Lớp: D19QTANM1
Trường: Đại học Điện Lực

## Câu 1: Value Types vs Reference Types (Stack vs Heap)

### Khái niệm

| Tiêu chí | Value Types (Kiểu giá trị) | Reference Types (Kiểu tham chiếu) |
|---|---|---|
| Ví dụ | `int`, `double`, `bool`, `char`, `struct`, `enum`, `DateTime` | `class`, `string`, `object`, `array`, `delegate`, `interface` |
| Kế thừa từ | `System.ValueType` | `System.Object` |
| Dữ liệu lưu ở đâu | Lưu **trực tiếp giá trị** tại nơi biến được khai báo | Biến chỉ lưu **địa chỉ (tham chiếu)**; đối tượng thật nằm trên **Heap** |
| Khi gán `a = b` | **Sao chép giá trị** (hai bản độc lập) | **Sao chép tham chiếu** (hai biến trỏ cùng một đối tượng) |
| Giá trị `null` | Không (trừ khi dùng `Nullable<T>` / `int?`) | Có thể là `null` |
| Giải phóng bộ nhớ | Tự động khi ra khỏi phạm vi (scope) | Do **Garbage Collector (GC)** thu dọn |
| Hiệu năng | Nhanh, ít áp lực lên GC | Tốn chi phí cấp phát và GC |

### Cơ chế lưu trữ

- **Stack**: vùng nhớ theo cơ chế LIFO, dùng cho biến cục bộ, tham số hàm, địa chỉ trả về. Cấp phát và giải phóng rất nhanh.
- **Heap**: vùng nhớ cho các đối tượng có vòng đời linh hoạt, do GC quản lý.


## Câu 2: Init-only Properties (`init`) khác gì `set` thông thường?

> `init` được giới thiệu từ **C# 9**.

### Sự khác nhau

| Tiêu chí | `set` | `init` |
|---|---|---|
| Thời điểm gán giá trị | **Bất kỳ lúc nào** (trước và sau khi tạo đối tượng) | **Chỉ trong lúc khởi tạo** đối tượng |
| Nơi được phép gán | Mọi nơi có quyền truy cập | Object initializer, constructor, `with` expression, hoặc trong chính các accessor `init` khác |
| Sau khi khởi tạo | Vẫn thay đổi được (mutable) | **Bất biến** (immutable), gán lại sẽ báo lỗi biên dịch |


### Trường hợp sử dụng thực tế

1. **DTO / Request / Response model**: dữ liệu nhận về từ API không nên bị sửa lung tung trong quá trình xử lý.
2. **Cấu hình ứng dụng (Options/Settings)**: nạp một lần khi khởi động, sau đó chỉ đọc.
3. **Value Object / Entity bất biến** trong Domain-Driven Design: ví dụ `Money`, `Address`.
4. **Kết hợp với `record`** và biểu thức `with` để tạo bản sao đã chỉnh sửa mà không làm thay đổi bản gốc.
5. **Thread-safe hơn**: đối tượng bất biến có thể chia sẻ giữa nhiều luồng mà không cần khóa.

**Ưu điểm so với constructor**: cú pháp object initializer rõ ràng (gán theo tên), tránh constructor có quá nhiều tham số mà vẫn giữ được tính bất biến. Từ C# 11 có thể kết hợp thêm từ khóa `required` để bắt buộc phải gán khi khởi tạo.


## Câu 3: `virtual` (lớp cha) vs `override` (lớp con) trong Đa hình

### Sự khác nhau

| Tiêu chí | `virtual` (lớp cha) | `override` (lớp con) |
|---|---|---|
| Vai trò | **Cho phép** lớp con ghi đè phương thức | **Ghi đè** (thay thế) cài đặt của lớp cha |
| Nằm ở | Lớp cơ sở (base class) | Lớp dẫn xuất (derived class) |
| Có thân hàm không | Có, là cài đặt **mặc định** | Có, là cài đặt **riêng** của lớp con |
| Bắt buộc không | Không bắt buộc lớp con phải override | Chỉ dùng khi lớp cha đã khai báo `virtual`, `abstract` hoặc `override` |
| Chữ ký (signature) | Định nghĩa chữ ký gốc | Phải **trùng khớp** tên, tham số, kiểu trả về với phương thức gốc |

### Cơ chế Đa hình

Khi gọi phương thức `virtual` qua biến kiểu lớp cha, CLR dựa vào **kiểu thực tế của đối tượng lúc chạy (runtime type)** (thông qua bảng phương thức ảo - *vtable*) để quyết định gọi phiên bản `override` nào. Đây là **liên kết muộn (late binding / dynamic dispatch)**.



## Câu 4: Vì sao thành phần `static` không truy xuất được qua thể hiện (Object Instance) tạo bằng `new`?

### Lý do

1. **`static` thuộc về kiểu (type), không thuộc về đối tượng.** Thành viên static chỉ có **một bản duy nhất** cho cả lớp, được cấp phát khi kiểu được nạp vào (type loading) và dùng chung cho mọi nơi.
2. **Thể hiện (instance) chỉ chứa dữ liệu của thành viên không static.** Khi dùng `new`, vùng nhớ của đối tượng trên Heap chỉ gồm các trường instance; các trường static được lưu ở vùng riêng gắn với kiểu, không nằm trong đối tượng.
3. **Tránh gây hiểu nhầm về ngữ nghĩa.** Nếu cho phép `obj.StaticMember`, người đọc sẽ tưởng đây là dữ liệu riêng của `obj`, trong khi thực chất sửa nó là sửa dữ liệu dùng chung của toàn bộ lớp. C# chọn cấm điều này (khác với Java vốn cho phép nhưng chỉ cảnh báo).
4. **Truy xuất static không cần đối tượng tồn tại**, nên không phụ thuộc vào việc có `new` hay không (có thể gọi ngay cả khi chưa tạo bất kỳ đối tượng nào).

Trình biên dịch báo lỗi **CS0176**: *Member 'X' cannot be accessed with an instance reference; qualify it with a type name instead.*

### Chiều ngược lại

Phương thức `static` cũng **không thể trực tiếp dùng thành viên instance**, vì khi gọi nó không có đối tượng cụ thể nào (không có `this`). Muốn dùng, phải truyền đối tượng vào làm tham số.
