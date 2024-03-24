using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
// system
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using OfficeOpenXml;
class Program
{

    /*
    * Write the categories into an excel file
    * @param url: the url of the website
    /* Clicks on the Show More button, then retrieve the categories and the number of items
    /* in each category
    /* Add into a List tuple and write into an excel file
    */
    public static void writeCategories(string url)
    {
        var categoryData = new List<Tuple<string, int>>();
        IWebDriver driver = new ChromeDriver();
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        // navigate to website
        driver.Navigate().GoToUrl(url);

        var show_more_button = driver.FindElement(By.CssSelector("#cssmenu > ul > li.search-colum__category.has-sub.active > ul.shows.search-colum__group.list-column.facet-category > li > span > a"));
        show_more_button.Click();

        // wait for the new contents to load by adding a delay
        wait.Until(d => d.FindElements(By.CssSelector("span.category")));
        // get the categories, example retrieved: Automative (64559)
        var categories = driver.FindElements(By.CssSelector("span.category"));
        var number = driver.FindElements(By.CssSelector("small"));

        Console.WriteLine($"Categories: {categories.Count}, Number Element: {number.Count}");

        for (int i = 0; i < number.Count; i++) {
            string categoryName = categories[i].Text;
            // skip if the category name is empty or show less
            if (categoryName == "" || categoryName == "- Show Less...") {
                continue;
            }
            string numberText = number[i].Text;
            // regex to match and remove the brackets
            Match match = Regex.Match(numberText, @"\((\d+)\)");
            if (match.Success) {
                string numberValue = match.Groups[1].Value;
                categoryData.Add(new Tuple<string, int>(categoryName, int.Parse(numberValue)));
            }
        }


        // // write into excel file 
        using (var package = new ExcelPackage(new FileInfo("excel/data.xlsx")))
        {
            var worksheet = package.Workbook.Worksheets.Add("Categories");
            // initialize columns
            worksheet.Cells[1, 1].Value = "Category";
            worksheet.Cells[1, 2].Value = "Number of Items";
            // write into individual columns
            for (int i = 1; i < categoryData.Count; i++)
            {
                worksheet.Cells[i + 1, 1].Value = categoryData[i - 1].Item1;
                worksheet.Cells[i + 1, 2].Value = categoryData[i - 1].Item2;
            }

            package.Save();

        }
        driver.Quit();
    }
    /*
    * Write the individual items in the landing page into an excel file
    * @param url: the url of the website
    */
    public static void writeItems(string url)
    {
        IWebDriver driver = new ChromeDriver();
        // navigate to website
        driver.Navigate().GoToUrl(url);
        // get the names first
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(120));
        // wait until the div elements are properly loaded
        // elements are obtained using manual inspection by inspect elements
        wait.Until(d => d.FindElements(By.CssSelector("h3.ad__title.text-left")).Count > 0);
        wait.Until(d => d.FindElements(By.CssSelector(".thumbnail__link.InStock")).Count > 0);
        wait.Until(d => d.FindElements(By.CssSelector("span.ad__price")).Count > 0);
        // retrieve the elements based on class or css selector
        ReadOnlyCollection<IWebElement> names = driver.FindElements(By.CssSelector("h3.ad__title.text-left"));
        ReadOnlyCollection<IWebElement> urls = driver.FindElements(By.CssSelector(".thumbnail__link.InStock"));
        ReadOnlyCollection<IWebElement> prices = driver.FindElements(By.CssSelector("span.ad__price"));

        Console.WriteLine($"Names: {names.Count}, Prices: {prices.Count}, Urls: {urls.Count}");

        using (var package = new ExcelPackage(new FileInfo("excel/data.xlsx")))
        {
            var worksheet = package.Workbook.Worksheets.Add("Items");
            // initialize columns
            worksheet.Cells[1, 1].Value = "Name";
            worksheet.Cells[1, 2].Value = "Price";
            worksheet.Cells[1, 3].Value = "Url";
            // write into individual columns
            for (int i = 1; i < urls.Count; i++)
            {
                worksheet.Cells[i + 1, 1].Value = names[i - 1].Text;
                worksheet.Cells[i + 1, 2].Value = prices[i - 1].Text;
                worksheet.Cells[i + 1, 3].Value = urls[i - 1].GetAttribute("href");
            }

            package.Save();

        }

        driver.Quit();
    }

    static void Main()
    {
        Program program = new Program();
        string url = "https://www.tradingpost.com.au/search-results/?q=&filter-location-text=&filter-location-dist=25&cat=";
        Program.writeCategories(url);

        Program.writeItems(url);

    }
}