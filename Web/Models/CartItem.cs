namespace Web.Models
{
    /// <summary>
    /// Позиція кошика, що зберігається в браузері (localStorage): лише ідентифікатор і кількість.
    /// Назва, ціна й залишок завжди беруться з API, тому вони не застарівають.
    /// </summary>
    public record CartItem(Guid ProductId, int Quantity);
}
