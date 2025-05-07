using HtmlAgilityPack;


    public class HtmlParser
    {
        public (string redeemPrice, string adjustCondition) ParseHtml(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            // 使用 CSS 選擇器查詢元素          
            var redeemNode = doc.GetElementbyId("redeem_price");//doc.DocumentNode.SelectSingleNode("//*[@id='redeem_price']");
            var adjustNode = doc.GetElementbyId("adjust_condition");// doc.DocumentNode.SelectSingleNode("//*[@id='adjust_condition']");

            // 防呆處理 + 格式化
            string GetSafeText(HtmlNode node) =>
                node?.InnerText.Trim() ?? "N/A";

            return (
                redeemPrice: FormatText(GetSafeText(redeemNode)),
                adjustCondition: FormatText(GetSafeText(adjustNode))
            );
        }

        private string FormatText(string text)
        {
            // 進階文字清理 (依需求擴充)
            return text
                .Replace("\n", "")
                .Replace("\t", "")
                .Trim();
        }
    }

