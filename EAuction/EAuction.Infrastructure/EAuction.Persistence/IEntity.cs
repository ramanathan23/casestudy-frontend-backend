namespace EAuction.Persistence.Entities
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}