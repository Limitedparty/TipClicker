using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TipClicker
{
    class Manager
    {
        public static Keys key1 = Keys.Shift;
        public static Keys key2 = Keys.Control;


        public static void Start ()
        {
            PrintLogo();
            while (true)
            {
                Console.Write("\nWhat do you want? (P)lay or (R)ecord click map? (P/R): ");
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.P:
                        Console.WriteLine("");
                        string[] fileEntries = Directory.GetFiles(Application.StartupPath);
                        int n = 0;
                        foreach (string fileName in fileEntries)
                        {
                            Console.WriteLine(n + ") " + Path.GetFileName(fileName));
                            n++;
                        }
                        string input = "null";
                        while (!int.TryParse(input, out n))
                        {
                            Console.Write("Select file:");
                            input = Console.ReadLine();
                        }
                        string filepath = Application.StartupPath + "\\" + Path.GetFileName(fileEntries[n]);
                        while (true)
                            PlayerTest(filepath);
                        break;
                    case ConsoleKey.R:
                        Console.WriteLine("");
                        while (true)
                            WriterLoop();
                    default:
                        break;
                }
            }
        }

        public static void PrintLogo()
        {
            string logo = "\n" +
                          " ████████╗██╗██████╗  ██████╗██╗     ██╗ ██████╗██╗  ██╗███████╗██████╗ \n" +
                          " ╚══██╔══╝██║██╔══██╗██╔════╝██║     ██║██╔════╝██║ ██╔╝██╔════╝██╔══██╗\n" +
                          "    ██║   ██║██████╔╝██║     ██║     ██║██║     █████╔╝ █████╗  ██████╔╝\n" +
                          "    ██║   ██║██╔═══╝ ██║     ██║     ██║██║     ██╔═██╗ ██╔══╝  ██╔══██╗\n" +
                          "    ██║   ██║██║     ╚██████╗███████╗██║╚██████╗██║  ██╗███████╗██║  ██║\n" +
                          "    ╚═╝   ╚═╝╚═╝      ╚═════╝╚══════╝╚═╝ ╚═════╝╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝\n" +
                          "  by LP 2017 (c)               Version: " + Program.version + "";
            Console.WriteLine(logo + "\n\n");
        }

        public static void WriterLoop()
        {
            Console.WriteLine("Press Shift key for start recording...");
            while (Control.ModifierKeys != key1) { Thread.Sleep(1); }
            Console.WriteLine("Recording started! Press Ctrl for stop.");

            long milliseconds = DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;

            bool clickState = false;

            String clickmap = "CLICKMAP/TIPCLICKER";

            while (true)
            {
                long now = (DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond) - milliseconds;

                // Active
                if (Emulator.ifMouseActive() && !clickState)
                {
                    Console.WriteLine("1=" + now);
                    clickmap += "\r\n" + "1=" + now;
                    clickState = true;
                }
                // Deacive
                if (!Emulator.ifMouseActive() && clickState)
                {
                    Console.WriteLine("0=" + now);
                    clickmap += "\r\n" + "0=" + now;
                    clickState = false;
                }
                // Exit
                if (Control.ModifierKeys == key2)
                {
                    break;
                }
                //Thread.Sleep(1);
            }
            Console.WriteLine("Recording stoped. Saving...");
            System.IO.StreamWriter file = new System.IO.StreamWriter(Application.StartupPath + "\\record.txt");
            file.WriteLine(clickmap);

            file.Close();
        }

        public static void PlayerTest(string filepath)
        {

            Console.WriteLine("Press Shift key for start...");
            while (Control.ModifierKeys != key1) { Thread.Sleep(1); }

            int counter = 0;
            string line;

            System.IO.StreamReader file =
               new System.IO.StreamReader(filepath);

            long milliseconds = DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;

            bool exit = false;

            while ((line = file.ReadLine()) != null && !exit)
            {
                if (counter != 0)
                {
                    long l1 = long.Parse(line.Split(new Char[] { Char.Parse("=") })[1]);
                    String l2 = line.Split(new Char[] { Char.Parse("=") })[0];
                    while (true)
                    {
                        long now = (DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond) - milliseconds;
                        if (now >= l1)
                        {
                            if (l2 == "1")
                            {
                                Console.WriteLine(line + " / " + now);
                                Emulator.ClickDown();
                            } else
                            {
                                Console.WriteLine(line + " / " + now);
                                Emulator.ClickUp();
                            }
                            break;
                        }
                        if (Control.ModifierKeys == key2)
                        {
                            exit = true;
                            break;
                        }
                        //Thread.Sleep(1);
                    }
                }
                counter++;
            }

            Emulator.ClickUp();
            file.Close();
        }
    }
}
