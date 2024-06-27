using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;

namespace AmazonGrpcService.Parsers
{
    public class AmazonParser
    {

        public async Task<AmazonProperty> GetProductDetail(string url)
        {
            var driver = new ChromeDriver();
            driver.Navigate().GoToUrl(url);
            var amazon = new AmazonProperty();
            try
            {

                var price = driver.FindElement(By.XPath("//*[@id='corePrice_feature_div']/div/span/span[2]")).Text;
                if (price == "")
                {
                    price = driver.FindElement(By.XPath("//*[@id='corePrice_feature_div']/div/span/span[1]")).Text;
                }
                double dprice = 0.00;
                double.TryParse(price.Replace(',', '.'), out dprice);

                amazon.Price = dprice;
            }
            catch (Exception)
            {


            }



            try
            {
                var previousPrice = driver.FindElement(By.XPath("//*[@id='corePrice_desktop']/div/table/tbody/tr[1]/td[2]/span[1]/span[1]")).Text;
                if (previousPrice == "")
                {
                    previousPrice = driver.FindElement(By.XPath("//*[@id='corePrice_desktop']/div/table/tbody/tr[1]/td[2]/span[1]/span[2]")).Text;
                }
                double dpreviousPrice = 0.00;
                double.TryParse(previousPrice.Replace(',', '.'), out dpreviousPrice);
                amazon.PreviousPrice = dpreviousPrice;
            }
            catch (Exception)
            {


            }

            var categories = driver.FindElements(By.XPath("//*[@id='wayfinding-breadcrumbs_feature_div']/ul/li"));
            List<string> categoryList = new List<string>();
            foreach (var category in categories)
            {
                try
                {
                    categoryList.Add(category.FindElement(By.TagName("a")).Text);
                }
                catch (Exception)
                {


                }

            }
            amazon.Categories = categoryList;
            //var priceaftercoma = driver.FindElement(By.XPath("//*[@id='corePrice_feature_div']/div/span/span[2]/span[2]")).Text;
            var productInfo = driver.FindElements(By.XPath("//*[@id='feature-bullets']/ul/li"));
            List<string> productInfoList = new List<string>();
            foreach (var product in productInfo)
            {
                productInfoList.Add(product.Text);
            }
            amazon.Infos = productInfoList;
            var amazonLinks = driver.FindElements(By.XPath("//*[@id='altImages']/ul/li"));
            foreach (var item in amazonLinks)
            {
                try
                {
                    var deneme = item.FindElement(By.TagName("span"));

                    if (deneme != null)
                    {
                        Actions actionProvider = new Actions(driver);
                        actionProvider.MoveToElement(deneme).Click().Perform();
                    }
                }
                catch (Exception)
                {

                }
            }
            List<string> result = new List<string>();
            var jpgLinks = driver.FindElements(By.XPath("//*[@id='main-image-container']/ul/li"));

            for (int i = 0; i < jpgLinks.Count; i++)
            {
                try
                {
                    var jpgDiv = jpgLinks[i].FindElement(By.XPath("//*[@id='imgTagWrapperId']"));
                    var jpgimg = jpgLinks[i].FindElement(By.TagName("img"));

                    var stringjpg = jpgimg.GetAttribute("src");
                    if (stringjpg != null)
                    {
                        result.Add(stringjpg);
                    }

                }
                catch (Exception)
                {


                }
            }
            driver.Close();
            amazon.Jpgs = result;
            return await Task.FromResult(amazon);
        }

    }
    public class AmazonProperty
    {
        public List<string> Jpgs { get; set; }
        public double Price { get; set; }
        public string AfterComa { get; set; }
        public List<string> Infos { get; set; }
        public List<string> Categories { get; set; }
        public double PreviousPrice { get; set; }

    }

}
