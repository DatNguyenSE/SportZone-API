namespace Adidas.Domain.Enums;
public enum OrderStatus
{
    Pending = 1,    // Đang chờ xử lý
    Placed = 2,     // Đã đặt hàng thành công (sau khi xác nhận/thanh toán)
    Paid = 3,       // Đã thanh toán
    Shipped = 4,    // Đang giao
    Completed = 5,  // Hoàn thành
    Cancelled = 6   // Đã hủy
}