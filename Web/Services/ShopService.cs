using Microsoft.JSInterop;
using MINT.EShop.API.Wrappers;
using MINT.EShop.Business.DTOs.OrderItems;
using MINT.EShop.Business.DTOs.Orders;
using Web.Models;

namespace Web.Services
{
    /// <summary>
    /// Дії покупця. Запис іде через fetch до Web (/api/shop/...), де токен оновлюється непомітно:
    /// circuit не може записати cookie, а його копія токена з часом застаріває.
    /// </summary>
    public class ShopService(IJSRuntime js)
    {
        private readonly IJSRuntime _js = js;

        /// <summary>
        /// Оформлює замовлення з кількох позицій (кошик чи «Купити зараз») з даними доставки та оплати.
        /// Повертає створене замовлення або текст помилки.
        /// </summary>
        public async Task<(OrderCreatedResponse? Order, string? Error)> PlaceOrderAsync(IEnumerable<CartItem> items, CheckoutDetails details)
        {
            var request = new CreateOrderRequest
            {
                OrderItems = items
                    .Select(i => new CreateOrderItemRequest { ProductId = i.ProductId, Quantity = i.Quantity })
                    .ToList(),
                Region = details.Region,
                City = details.City,
                DeliveryService = details.DeliveryService,
                DeliveryPoint = details.DeliveryPoint,
                PaymentMethod = details.PaymentMethod,
                RecipientPhone = details.RecipientPhone
            };

            try
            {
                var response = await _js.InvokeAsync<APIResponse<OrderCreatedResponse>>("callShopApi", "POST", "orders", request);
                if (response is { Success: true, Data: not null })
                {
                    return (response.Data, null);
                }

                var message = string.IsNullOrWhiteSpace(response?.Message)
                    ? "Не вдалося оформити замовлення. Спробуйте ще раз."
                    : response.Message;
                return (null, message);
            }
            catch (InvalidOperationException)
            {
                // Prerender: JS ще недоступний
                return (null, "Інтерфейс ще завантажується. Спробуйте ще раз.");
            }
            catch (Exception)
            {
                return (null, "Помилка з'єднання. Спробуйте ще раз.");
            }
        }

        /// <summary>Замовлення поточного користувача.</summary>
        public async Task<APIResponse<List<OrderResponse>>> GetMyOrdersAsync()
        {
            try
            {
                var response = await _js.InvokeAsync<APIResponse<List<OrderResponse>>>("callShopApi", "GET", "orders/my", null);
                return response ?? APIResponse<List<OrderResponse>>.FailureResponse("Помилка обробки відповіді сервера.");
            }
            catch (InvalidOperationException)
            {
                // Prerender: JS ще недоступний
                return APIResponse<List<OrderResponse>>.FailureResponse("Інтерфейс ще завантажується. Спробуйте ще раз.");
            }
            catch (Exception)
            {
                return APIResponse<List<OrderResponse>>.FailureResponse("Помилка з'єднання. Спробуйте ще раз.");
            }
        }

        /// <summary>Скасовує замовлення поточного користувача. Повертає текст помилки або null при успіху.</summary>
        public async Task<string?> CancelOrderAsync(Guid orderId)
        {
            try
            {
                var response = await _js.InvokeAsync<APIResponse<object>>("callShopApi", "PUT", $"orders/my/{orderId}/cancel", null);
                if (response is { Success: true }) return null;

                return string.IsNullOrWhiteSpace(response?.Message)
                    ? "Не вдалося скасувати замовлення. Спробуйте ще раз."
                    : response.Message;
            }
            catch (InvalidOperationException)
            {
                return "Інтерфейс ще завантажується. Спробуйте ще раз.";
            }
            catch (Exception)
            {
                return "Помилка з'єднання. Спробуйте ще раз.";
            }
        }
    }
}
