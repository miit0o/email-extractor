using email_extractor;
string url = "";

Console.WriteLine("Please enter the URL you want to extract from");
url = Console.ReadLine();

Console.WriteLine("Scanning " + url + " for email-addresses...");
Grabber.GrabCode(url);

