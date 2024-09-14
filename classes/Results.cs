public record Result
{
  public required User user { get; set; }
  public required int balance { get; set; }
  public required int numberOfTrades { get; set; }
}