using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Net;
using XDevkit;

namespace XDRPC_Downloader
{
    class Utilities
    {
        IXboxConsole client; //you know what this fucker is
        public string Connect(string IP)
        {
            if (!IPAddress.TryParse(IP, out IPAddress junk)) //checks if they gave an ip or not
                return "notIP";
            try
            {
                client = new XboxManagerClass().OpenConsole(IP); //connects
                return "connected";
            }
            catch
            {
                return "failed"; //self explanatory
            }
        }
        public bool CheckForXDRPC()
        {
            List<string> sa = new List<string>();
            IXboxFiles files = client.DirectoryFiles(@"Hdd:\"); //retrieves all files on hdd
            foreach (IXboxFile fi in files)
            {
                if (fi.Name == @"Hdd:\XDRPC.xex") //checks if file name is xdrpc
                {
                    sa.Add(fi.Name); //adds filename (useless but I was using it for testing)
                    sa.Add(fi.Size.ToString()); //adds filesize
                    if (sa[1] != "73728") //checks if filesize is correct
                        return false;
                    else
                        return true;
                }
            }
            
            return false;
        }
        public bool CheckForXDRPCInINI()
        {
            string launchPath = Path.Combine(Path.GetTempPath(), "launch.ini"); //%temp%/launch.ini
            try
            {
                client.ReceiveFile(launchPath, @"Hdd:\launch.ini"); //retrieves console's launch ini
            }
            catch
            {
                //cba to do this
            }
            string text = File.ReadAllText(launchPath); //stores all the shit
            if (!text.ToUpper().Contains("XDRPC")) //probably the least efficient way but eh
                return false;
            else
                return true;
        }
        public string DownloadXDRPC()
        {
            string path = Path.Combine(Path.GetTempPath(), "XDRPC.xex"); //%temp%/XDRPC.xex
            WebClient wc = new WebClient();
            wc.DownloadFile("http://download847.mediafire.com/132x71fpt5hg/hotottbgmvtntn7/XDRPC.xex", path); //yeah
            client.SendFile(path, @"Hdd:\XDRPC.xex"); //sends file
            return "";
        }
        public void AddXDRPCInINI()
        {
            string launchPath = "data"; //why not?
            client.ReceiveFile(launchPath, @"Hdd:\launch.ini");//gets current ini
            string[] text = File.ReadAllLines(launchPath); //stores all lines seperately onto an array
            List<int> plugins = new List<int>(); //create's a string list (should be int but I was testing) (lists are nice imo than arrays)
            foreach (string line in text)
                if (!line.Contains("Hdd") && (line.Contains("plugin2") || line.Contains("plugin3") || line.Contains("plugin4") || line.Contains("plugin5"))) //if line is either plugin2,3,4 or 5 (plugin 1 is usually xbdm and we wouldn't be connected rn if xbdm wasn't added)
                    plugins.Add(Array.IndexOf(text, line)); //gets index of line
            string lineToUse = text[plugins[0]]; //use first good plugin line
            lineToUse = lineToUse.Substring(0, 7); //get first 7 characters of plugin line (plugin2), that way if they do or do not have spaces it doesn't matter.
            lineToUse += @" = Hdd:\XDRPC.xex"; //Concatenates the plugin location and =
            text[plugins[0]] = lineToUse; //makes the original line the new line
            File.WriteAllLines(launchPath, text); //writes it to original file
            client.SendFile(launchPath, @"Hdd:\launch.ini"); //sends it to console
            File.Delete(launchPath); //deletes file
        }

        public void snowy()
        {
            if (!CheckForXDRPC())
                DownloadXDRPC();
            if (!CheckForXDRPCInINI())
                AddXDRPCInINI();
        }
    }
}