### Trading Post Web Scraper


The web scraper uses Selenium and a cron job is set up to run every 24 hours. 
The program writes into an Excel file with two sheets:

- Categories (category, number of items)
- Items (name, price, and url) from the items scraped in the starting page


The query selectors are found using inspect element, and copying it as CSS Selectors or using the class name as shown below.

<p align="center">
    <img src="inspecting_selectors.gif">
</p>


The result of the scrape can be found in the [excel file](excel/data.xslx).