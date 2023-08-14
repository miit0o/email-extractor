using System;
using System.IO;
using System.Text.RegularExpressions;

namespace email_extractor
{
    public class Grabber
    {
        internal static Boolean safeToFile;
        internal static String filePath;
        internal static void GrabCode(String url)
        {
            if (safeToFile == true)
            {
                CreateFile();
            }

            //fix URL if necessary
            if (url.StartsWith("http://") || url.StartsWith("https://"))
            {
                // all good :)
            }
            else
            {
                url = "http://" + url;
            }

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
                        if (safeToFile == true)
                        {
                            //append to file
                            using(StreamWriter sw = File.AppendText(filePath))
                            {
                                sw.WriteLine(match);
                            }


                            Console.WriteLine(match.Value);
                            prevMail = match.Value;
                        }
                        else
                        {
                            Console.WriteLine(match.Value);
                            prevMail = match.Value;
                        }
                    }
                }
                if (safeToFile == true)
                {
                    RemoveDuplicates(filePath);
                    Console.WriteLine("Results written to: " + filePath);
                }
            }
            else
            {
                Console.WriteLine("No email addresses found.");
            }
        }

        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmss");
        }
        internal static void CreateFile()
        {
            //get timestamp
            String timeStamp = GetTimestamp(DateTime.Now);

            //create file
            filePath = timeStamp + "-output.txt";
            using (StreamWriter sw = File.CreateText(filePath));
        }
        internal static void RemoveDuplicates(String filePath)
        {
            // Removes duplicates from text tile

            List<string> lines = new List<string>();
            List<string> uniqueLines = new List<string>();

            // Read the content of the text file
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"File '{filePath}' not found.");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading the file: {ex.Message}");
                return;
            }

            // Remove duplicates and store unique lines in a new list
            foreach (string line in lines)
            {
                if (!uniqueLines.Contains(line))
                {
                    uniqueLines.Add(line);
                }
            }

            // Write the unique lines back to the text file
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (string line in uniqueLines)
                    {
                        writer.WriteLine(line);
                    }
                }

                Console.WriteLine("Duplicates removed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to the file: {ex.Message}");
            }
        }

        internal static void ScanLinks(String url)
        {
            // Scan for other Links on the Page with the same base url
        }
    }
}
