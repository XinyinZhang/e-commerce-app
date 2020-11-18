using System.Runtime.Serialization;

namespace Core.Entities.OrderAggregate
{
    // track the status our order is in
    // we have the following three status below
    public enum OrderStatus
    {
      [EnumMember(Value = "Pending")]
      Pending,
      [EnumMember(Value = "PaymentReceived")]
      PaymentReceived,
      [EnumMember(Value = "PaymentFailed")]
      PaymentFailed
    }
}