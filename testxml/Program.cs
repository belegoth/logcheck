using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogAnalayzer
{
    class Program
    {
        /// <summary>
        /// programm is analyse log file
        /// </summary>
        /// <param name="args"></param>
        /// -filename - Name of log file
        /// -datetime - DateTimeStart YYYYMMDDHHmmss
        /// -pattern - string Что будет искаться в строке
        /// 

        static void Main(string[] args)
        {
            string filename = "";
            DateTime? datetime = null;
            string pattern = "";
            bool bskipdatetime = false;
            {
                bool bwaitfilename = false;
                bool bwaitdatetime = false;
                bool bwaitpattern = false;
                foreach (string arg in args)
                {
                    if (bwaitfilename)
                    {
                        bwaitfilename = false;
                        // Читаем как файл
                        filename = arg;
                        // проверяем есть ли такой файл на диске
                        if (!System.IO.File.Exists(filename))
                        {
                            Console.WriteLine("Log file is not found:{0}", filename);
                            Program.ShowParameters();
                            Environment.Exit(-2);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    if (bwaitdatetime)
                    {
                        bwaitdatetime = false;
                        // Пытаемся распарсить строку со временем
                        DateTime dt;
                        if (DateTime.TryParseExact(arg, "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out dt))
                        {
                            datetime = dt;
                            continue;
                        }
                        else
                        {
                            Console.WriteLine("Time is not recognazid:{0}", arg);
                            Program.ShowParameters();
                            Environment.Exit(-1);
                        }
                    }
                    else
                    if (bwaitpattern)
                    {
                        // 
                        bwaitpattern = false;
                        pattern = arg;
                        continue;
                    }

                    bwaitfilename = false;
                    bwaitdatetime = false;
                    bwaitpattern = false;

                    if (arg.ToLower().Equals("-filename"))
                    {
                        bwaitfilename = true;
                    }
                    else
                    if (arg.ToLower().Equals("-datetime"))
                    {
                        bwaitdatetime = true;
                    }
                    else
                    if (arg.ToLower().Equals("-pattern"))
                    {
                        bwaitpattern = true;
                    }
                    if (arg.ToLower().Equals("-skipdatetime"))
                    {
                        bskipdatetime = true;
                    }
                }
            }
            while (true)
            {
                if (filename.Length < 1)
                {
                    Console.WriteLine("Name of log file is not set");
                    Program.ShowParameters();
                    break;
                }
                if (pattern == String.Empty)
                {
                    Console.WriteLine("pattern is not set");
                    Program.ShowParameters();
                    break;
                }
                if (datetime == null)
                {
                    datetime = DateTime.MinValue;
                }
                Program.ProcessFile(filename, (DateTime)datetime, pattern, bskipdatetime);
                break;
            }

            //Console.ReadKey();
        }

        static void ProcessFile(string filename, DateTime datetime, string pattern, bool bskipdatetime = false)
        {
            //
            //List<string> lsout = new List<string>();
            System.IO.FileStream file = System.IO.File.Open(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
            System.IO.StreamReader sr = new System.IO.StreamReader(file, Encoding.GetEncoding("windows-1251"));
            //Console.WriteLine("Start read:{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            //int icount = 0;
            while (!sr.EndOfStream)
            {
                //icount++;
                string line = sr.ReadLine();
                if (!bskipdatetime)
                {
                    // Эту проверку делаем только для тех записей, в которых есть информация о данных
                    if (line.Trim().Length < 20)
                    {
                        continue;
                    }
                }
                //Console.WriteLine(line);
                DateTime? dt = null;
                dt = Program.GetTime(line);
                if (dt != null || bskipdatetime)
                {
                    if ((dt > datetime) || bskipdatetime)
                    {
                        if (line.Contains(pattern))
                        {
                            //lsout.Add(line);
                            Console.WriteLine(line);
                        }
                    }
                }
            }
            //Console.WriteLine("Finish read:{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            //Console.WriteLine("icount:{0}, listcount:{1}", icount, lsout.Count);
            //Console.WriteLine(lsout.ToArray());
            //sr.Dispose();
        }

        static void ShowParameters()
        {
            Console.WriteLine("In case you look this meessage you call this program is not correct.");
            Console.Write(System.IO.Path.GetFileName(System.Reflection.Assembly.GetEntryAssembly().Location));
            Console.Write(" -filename \"name of file\"");
            Console.Write(" -datetime \"datetime in format YYYYMMDDHHmmss\" only this format is supported");
            Console.Write(" -pattern \"search pattern\"");
            Console.Write(" -skipdatetime ignore datetime");

        }

        static DateTime? GetTime(string line)
        {
            DateTime? retval = null;
            // Парсим время
            DateTime dt;
            string[] sacceptdtformat = new string[]
            {
                "yyyy-MM-dd HH:mm:ss"
                ,"yyyy.MM.dd HH:mm:ss"
                ,"dd.MM.yyyy HH:mm:ss"
                ,"dd.MM.yyyy H:mm:ss"
            };

            foreach (string vs in sacceptdtformat)
            {

                if (DateTime.TryParseExact(line.Substring(0, 19).Trim(), vs, null, System.Globalization.DateTimeStyles.None, out dt))
                {
                    //
                    retval = dt;
                    break;
                }
            }
            return retval;
        }
    }
}
