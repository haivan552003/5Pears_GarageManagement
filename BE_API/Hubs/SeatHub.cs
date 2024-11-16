using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace BE_API.Hubs
{
    public class SeatHub:Hub
    {
        private static ConcurrentDictionary<(int seatId, int tripDetailID), string> selectedSeats = new();
        private static ConcurrentDictionary<string, bool> userCheckoutStatus = new ConcurrentDictionary<string, bool>();
        private int tripDetailID;

        // Hàm chọn ghế
        public async Task SelectSeat(int seatId, int tripDetailID)
        {
            // Kiểm tra nếu ghế đã được chọn cho tripDetailID này
            if (!selectedSeats.ContainsKey((seatId, tripDetailID)))
            {
                selectedSeats[(seatId, tripDetailID)] = Context.ConnectionId;  // Lưu trữ ghế đã chọn với kết nối
                await Clients.Others.SendAsync("ReceiveSeatSelection", seatId, tripDetailID);  // Gửi thông báo cho các client khác
            }
        }

        // Hàm bỏ chọn ghế
        public async Task UnselectSeat(int seatId, int tripDetailID)
        {
            if (selectedSeats.TryGetValue((seatId, tripDetailID), out string connectionId) && connectionId == Context.ConnectionId)
            {
                selectedSeats.TryRemove((seatId, tripDetailID), out _);  // Xóa ghế khỏi danh sách đã chọn
                await Clients.Others.SendAsync("ReceiveSeatDeselection", seatId, tripDetailID);  // Gửi thông báo cho các client khác
            }
        }


        // Gửi danh sách ghế đã chọn tới client mới kết nối
        public override async Task OnConnectedAsync()
        {
            var takenSeatIds = selectedSeats.Keys.ToList();
            await Clients.Caller.SendAsync("ReceiveAllSelectedSeats", takenSeatIds);

            await base.OnConnectedAsync();
        }

        // Khi client ngắt kết nối, release các ghế đã chọn
        public override async Task OnDisconnectedAsync(System.Exception? exception)
        {
            if (userCheckoutStatus.ContainsKey(Context.ConnectionId) && userCheckoutStatus[Context.ConnectionId])
            {
                return;
            }

            var seatsToRelease = selectedSeats.Where(s => s.Value == Context.ConnectionId).Select(s => s.Key).ToList();

            foreach (var seatId in seatsToRelease)
            {
                selectedSeats.TryRemove(seatId, out _);
                await Clients.Others.SendAsync("ReceiveSeatDeselection", seatId);
            }

            await base.OnDisconnectedAsync(exception);
        }

        // Khi checkout
        public async Task OnCheckout()
        {
            userCheckoutStatus[Context.ConnectionId] = true;

            await Clients.Others.SendAsync("ReceiveCheckoutNotification");
        }

    }
}
