using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
// system
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using OfficeOpenXml;
class Program
{

    public static void writeCategories(string url, string cssSelector)
    {
        var categoryData = new List<Tuple<string, int>>();
        IWebDriver driver = new ChromeDriver();
        // navigate to website
        driver.Navigate().GoToUrl(url);
        // get the categories
        var categories = driver.FindElements(By.CssSelector(cssSelector));
        foreach (var category in categories)
        {
            MatchCollection match = Regex.Matches(category.Text, @"([A-Za-z\s]+)\s\((\d+)\)");
            foreach (Match m in match)
            {
                // trim to remove newline
                string categoryName = m.Groups[1].Value.Trim();
                int number = int.Parse(m.Groups[2].Value);
                categoryData.Add(new Tuple<string, int>(categoryName, number));
            }
        }

        // write into excel file 
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Categories");
            // initialize columns
            worksheet.Cells[1, 1].Value = "Category";
            worksheet.Cells[1, 2].Value = "Number of Items";

            for (int i = 1; i < categoryData.Count; i++)
            {
                worksheet.Cells[i + 1, 1].Value = categoryData[i - 1].Item1;
                worksheet.Cells[i + 1, 2].Value = categoryData[i - 1].Item2;
            }

            package.SaveAs(new System.IO.FileInfo("excel/data.xlsx"));

        }
        driver.Quit();
    }

    public static void writeItems(string url, string cssSelector)
    {
        IWebDriver driver = new ChromeDriver();
        // navigate to website
        driver.Navigate().GoToUrl(url);
        // get the names first
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(40));
        wait.Until(d => d.FindElements(By.CssSelector("h3.ad__title.text-left")).Count > 0);
        wait.Until(d => d.FindElements(By.CssSelector(".thumbnail__link.clickout.InStock")).Count > 0);
        wait.Until(d => d.FindElements(By.CssSelector("span.ad__price")).Count > 0);

        ReadOnlyCollection<IWebElement> names = driver.FindElements(By.CssSelector("h3.ad__title.text-left"));
        ReadOnlyCollection<IWebElement> urls = driver.FindElements(By.CssSelector(".thumbnail__link.clickout.InStock"));
        ReadOnlyCollection<IWebElement> prices = driver.FindElements(By.CssSelector("span.ad__price"));

        System.Console.WriteLine($"Names: {names.Count}, Prices: {prices.Count}, Urls: {urls.Count}");
        // var names = driver.FindElements(By.CssSelector("h3.ad__title.text-left"));
        // var urls = driver.FindElements(By.CssSelector(".thumbnail__link.clickout.InStock"));
        // var prices = driver.FindElements(By.CssSelector("span.ad__price"));
        System.Console.WriteLine($"Names: {names.Count}, Prices: {prices.Count}, Urls: {urls.Count}");

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Items");
            // initialize columns
            worksheet.Cells[1, 1].Value = "Name";
            worksheet.Cells[1, 2].Value = "Price";
            worksheet.Cells[1, 3].Value = "Url";

            for (int i = 1; i < urls.Count; i++)
            {
                worksheet.Cells[i + 1, 1].Value = names[i - 1].Text;
                worksheet.Cells[i + 1, 2].Value = prices[i - 1].Text;
                worksheet.Cells[i + 1, 3].Value = urls[i - 1].GetAttribute("href");
            }

            package.SaveAs(new System.IO.FileInfo("excel/data.xlsx"));

        }

        // System.Console.WriteLine($"Names: {names.Count}, Prices: {prices.Count}, Urls: {urls.Count}");

        // for (var i = 0; i < prices.Count; i++)
        // {
        //     System.Console.WriteLine($"Name: {names[i].Text}, Price: {prices[i].Text}");
        // }
        driver.Quit();
    }

    static void Main()
    {
        Program program = new Program();
        string categoryUrl = "https://www.tradingpost.com.au/search-results/?q=&filter-location-text=&filter-location-dist=25&cat=";
        string categoryCssSelector = "#cssmenu > ul > li.search-colum__category.has-sub.active > ul:nth-child(2)";
        Program.writeCategories(categoryUrl, categoryCssSelector);


        string itemUrl = "https://www.tradingpost.com.au/search-results/?q=&filter-location-text=&filter-location-dist=25&cat=";
        string itemCssSelector = "#uid_119500 > div:nth-child(2) > div > div > div > div";
        Program.writeItems(itemUrl, itemCssSelector);

        // driver.Navigate().GoToUrl("https://www.tradingpost.com.au/search-results/?q=&filter-location-text=&filter-location-dist=25&cat=");
        // // wait for 10 seconds to retrieve from database
        // WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(45));
        // IWebElement titles = wait.Until(d => d.FindElement(By.CssSelector("#uid_125637 > div:nth-child(22) > div.thumbnail.clearfix.seller-link.clickout-link > div > div > div")));
        // System.Console.WriteLine(titles.Text);
    }
}