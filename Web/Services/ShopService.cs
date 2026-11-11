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
        /// Оформлює замовлення з одного товару. Повертає створене замовлення або текст помилки.
        /// </summary>
        public async Task<(OrderCreatedResponse? Order, string? Error)> BuyNowAsync(Guid productId, int quantity)
        {
            var request = new CreateOrderRequest
            {
                OrderItems = [new CreateOrderItemRequest { ProductId = productId, Quantity = quantity }]
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
    }
}
