namespace ERP.Domain.Enums
{
    public enum PurchOrderStatus
    {
        Draft = 0,
        Approved = 1,
        PartiallyReceived = 2,
        FullyReceived = 3,
        Closed = 4,
        Cancelled = 5
    }
}
