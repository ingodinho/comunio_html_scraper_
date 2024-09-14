using HtmlAgilityPack;

public static class TransferEvaluator
{
  public static HtmlNodeCollection SelectTransferNodes(HtmlDocument htmlDocument)
  {
    return htmlDocument.DocumentNode.SelectNodes("//p[starts-with(@ng-repeat, 'transfer in entry.message')]");
  }

  public static List<Transfer> FormatTransferNodes(HtmlNodeCollection transferNodes)
  {

    var transfers = new List<Transfer>();

    foreach (HtmlNode node in transferNodes)
    {
      var userNode = node.Descendants("a").FirstOrDefault(a => a.GetAttributeValue("href", "").Contains("/users/"));

      if (userNode == null)
      {
        throw new Exception("No User in Transfer");
      }

      var userName = userNode.InnerHtml;
      var userId = userNode.GetAttributeValue("href", "").Split("/").Last();

      var fromComputer = node.OuterHtml.Contains("transfer in entry.message.FROM_COMPUTER");

      string amountString;

      var transferNode = node.Descendants("//span[@translate]").FirstOrDefault();
      var amountStrings = node.InnerText.Split(["wechselt für ", " von"], StringSplitOptions.None);

      amountString = amountStrings[1].Split("\r")[0].Trim().Replace(".", "");

      bool amountConverted = int.TryParse(amountString, out int amount);

      if (!amountConverted)
      {
        throw new Exception("Number could not be converted");
      }

      var transfer = new Transfer()
      {
        User = UserMapper.ToUser(userId),
        Amount = amount,
        PlayerTransactionType = fromComputer ? PlayerTransactionType.Bought : PlayerTransactionType.Sold
      };

      transfers.Add(transfer);
    }

    Console.WriteLine();
    return transfers;
  }

  public static Dictionary<User, Result> EvaluateAllTransfers(IEnumerable<IGrouping<User, Transfer>> groupedTransfers)
  {
    var resultDic = new Dictionary<User, Result> { };
    var sum = groupedTransfers;
    foreach (var group in groupedTransfers)
    {
      var balance = group.Sum(x => x.PlayerTransactionType == PlayerTransactionType.Bought ? -x.Amount : x.Amount);
      var tradesCount = group.Count();

      resultDic.Add(group.Key, new Result
      {
        balance = balance,
        user = group.Key,
        numberOfTrades = tradesCount
      });
    }

    return resultDic;
  }

  public static void WriteResults(Dictionary<User, Result> resultDic)
  {
    // Define the format for the table
    string headerFormat = "{0,-20} {1,15} {2,20}";
    string rowFormat = "{0,-20} {1,15:C2} {2,20}";

    // Print the header
    Console.WriteLine(headerFormat, "User", "Balance (€)", "Number of Trades");
    Console.WriteLine(new string('-', 55)); // Separator line

    // Print each row
    foreach (var kvp in resultDic)
    {
      User user = kvp.Key;
      Result result = kvp.Value;

      Console.WriteLine(rowFormat,
                        user.ToString(),
                        result.balance,
                        result.numberOfTrades);
    }
  }
}