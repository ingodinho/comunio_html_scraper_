const string filePath = "./data/comunio.html";
// const string userIdMatze = "/users/12322012";

var scraper = new Scraper();
scraper.ParseHtmlFile(filePath);

var document = scraper.GetHtmlDocument();

if (document == null)
{
  throw new Exception("html document is null");
}

var transferNodes = TransferEvaluator.SelectTransferNodes(document);

var transfers = TransferEvaluator.FormatTransferNodes(transferNodes);

var groupedTransfers = transfers.GroupBy(x => x.User);
var resultDic = TransferEvaluator.EvaluateAllTransfers(groupedTransfers);
TransferEvaluator.WriteResults(resultDic);