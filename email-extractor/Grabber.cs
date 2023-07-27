using System;
using System.Text.RegularExpressions;

namespace email_extractor
{
    public class Grabber
    {
        internal static void GrabCode(String url)
        {
            //Get Page Source Code
            using (HttpClient client = new())
            {
                using (HttpResponseMessage response = client.GetAsync(url).Result)
                {
                    using (HttpContent content = response.Content)
                    {
                        string result = content.ReadAsStringAsync().Result;
                        GetMails(result);
                    }
                }
            }
        }

        internal static void GetMails(String sourceCode)
        {
            string emailPattern = @"\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}\b";

            // Use Regex.Matches to find all occurrences of email addresses in the input string
            MatchCollection matches = Regex.Matches(sourceCode, emailPattern, RegexOptions.IgnoreCase);

            if (matches.Count > 0)
            {
                Console.WriteLine("Good news! I found some email-addresses");
                string prevMail = "";
                foreach (Match match in matches)
                {
                    //Check if has been output already
                    if (match.Value.Equals(prevMail))
                    {
                        //skip to next
                    }
                    else
                    {
                        Console.WriteLine(match.Value);
                        prevMail = match.Value;
                    }
                }
            }
            else
            {
                Console.WriteLine("No email addresses found.");
            }
        }
    }
}
