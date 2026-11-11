namespace Web.Models
{
    /// <summary>
    /// Спільний вигляд категорії та виробника для компонента керування довідниками.
    /// </summary>
    public record CatalogItem(Guid Id, string Name, string Slug);
}
