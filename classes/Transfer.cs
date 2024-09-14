public record Transfer
{
  public required User User { get; init; }
  public required PlayerTransactionType PlayerTransactionType { get; init; }
  public required int Amount { get; init; }
}