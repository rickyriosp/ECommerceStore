namespace ShoppingCart.EventFeed
{
    public record Event
    (
        long SequenceNumber,
        DateTimeOffset OcurredAt,
        string Name,
        object Content
    );
}
