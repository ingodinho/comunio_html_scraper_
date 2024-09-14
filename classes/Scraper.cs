using HtmlAgilityPack;

public class Scraper
{

  public HtmlDocument? HtmlDocument { get; set; }

  public bool ParseHtmlFile(string filePath)
  {
    var doc = new HtmlDocument();
    doc.Load(filePath);

    HtmlDocument = doc;

    return true;
  }

  public HtmlDocument? GetHtmlDocument()
  {
    return this.HtmlDocument;
  }
}
