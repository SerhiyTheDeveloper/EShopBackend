namespace MINT.EShop.Core.Enums
{
    /// <summary>
    /// Спосіб оплати замовлення.
    /// </summary>
    public enum PaymentMethod
    {
        /// <summary>Оплата одразу (онлайн).</summary>
        Prepaid = 0,

        /// <summary>Оплата при отриманні товару.</summary>
        CashOnDelivery = 1
    }
}
