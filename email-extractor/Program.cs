using email_extractor;
string url = "";

Console.WriteLine("Please enter the URL you want to extract from");
url = Console.ReadLine();

Console.WriteLine("Would you like to save the extracted emails to a file? (Y/N)");
string decision = Console.ReadLine();
if (decision.Equals("Y") || decision.Equals("y"))
{
    Grabber.safeToFile = true;
    Console.WriteLine("Will do!");
}
else
{
    Console.WriteLine("Alright, wont safe the output.");
}

Console.WriteLine("Scanning " + url + " for email-addresses...");
Grabber.GrabCode(url);

