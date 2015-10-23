using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Globalization;


namespace logchecker
{
    public class Logfile
    {

        [XmlAttribute("filename")]
        public string filename { get; set; }
        [XmlAttribute("active")]
        public bool active { get; set; }
        [XmlAttribute("checkupdatetime")]
        public bool checkupdatetime { get; set; }
        [XmlAttribute("countonly")]
        public bool countonly { get; set; }
        [XmlElement("updatetimelimit")]
        public int updatetimelimit { get; set; }
        [XmlArray("patterns"), XmlArrayItem("pattern")]
        public List<Pattern> listpatterns = new List<Pattern>();
        [XmlIgnore]
        public int lastreadposition { get; set; }
        [XmlIgnore]
        public int lastlogtime { get; set; }



    }
    [XmlRoot("logfiles")]
    public class Logfiles
    {
        [XmlArray("logfiles"), XmlArrayItem("logfile")]

        public List<Logfile> ListFiles { get; set; }
    }

    public enum PType
    {
        [XmlEnum("include")]
        include,

        [XmlEnum("exclude")]
        exclude
    }

    public class Pattern
    {
        [XmlAttribute("type")]
        public PType ptype;

        [XmlElement("pattern")]
        public string patterntext { get; set; }

    }


    public class Lastposition
    {

        [XmlElement("filename")]
        public string lastpositionfilename { get; set; }
        [XmlElement("lastposition")]
        public int lastposition { get; set; }
        [XmlElement("lastchecktime")]
        public DateTime lastchecktime { get; set; }

    }

    public class Lastpositions
    {
        [XmlArray("logfiles"), XmlArrayItem("logfile")]

        public List<Lastposition> ListPositions { get; set; }


    }

    public class Serializer
    {


        public static void serializeconfig()
        {

            try
            {

                Logfiles logfiles = new Logfiles();
                logfiles.ListFiles = new List<Logfile>();

                Logfile logfile = new Logfile();
                logfile.listpatterns.Add(new Pattern() { ptype = PType.exclude, patterntext = "text to exclude" });
                logfile.listpatterns.Add(new Pattern() { ptype = PType.exclude, patterntext = "text2 to exclude" });
                logfile.listpatterns.Add(new Pattern() { ptype = PType.include, patterntext = "text to include" });


                logfiles.ListFiles.Add(new Logfile { filename = "file1.txt", active = true, checkupdatetime = true, updatetimelimit = 20, listpatterns = logfile.listpatterns });
                logfiles.ListFiles.Add(new Logfile { filename = "file2.txt", active = true, checkupdatetime = false });


                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Logfiles));
                StreamWriter sw = new StreamWriter("logfileconfig.xml");
                xmlSerializer.Serialize(sw, logfiles);
                sw.Close();
                Console.WriteLine("Serialize complete");



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        public static List<Logfile> deserializeconfig()

        {

            //DeSerializer
            try
            {
                TextReader reader = new StreamReader("logfileconfig.xml");
                XmlSerializer x = new XmlSerializer(typeof(Logfiles));
                Logfiles listlogs = (Logfiles)x.Deserialize(reader);
                return listlogs.ListFiles;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }


        }


        public static void serializepozition(List<Lastposition> filelist)
        {

            try
            {


                Lastpositions lastpositions = new Lastpositions();
                //  lastpositions.ListPositions = new List<Lastposition>();
                lastpositions.ListPositions = filelist;
                //lastpositions.ListPositions.Add(new Lastposition { lastpositionfilename = "file1", lastposition = 123, lastchecktime = DateTime.Now });
                //lastpositions.ListPositions.Add(new Lastposition { lastpositionfilename = "file2", lastposition = 1343, lastchecktime = DateTime.Now });


                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Lastpositions));
                StreamWriter sw = new StreamWriter("lastposition.xml");
                xmlSerializer.Serialize(sw, lastpositions);
                sw.Close();
                Console.WriteLine("Lastposition serialize complete");



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        public static List<Lastposition> deserializeposition()

        {

            //DeSerializer
            try
            {
                TextReader reader = new StreamReader("lastposition.xml");
                XmlSerializer x = new XmlSerializer(typeof(Lastpositions));
                Lastpositions listpositions = (Lastpositions)x.Deserialize(reader);
                return listpositions.ListPositions;



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }


        }


    }

    public class Checklog
    {



        public static int Checkfile(Logfile log)  //return number of lines
        {
            
            if (log.active == true)

            {
                Console.WriteLine("Start check {0} from position {1}", log.filename, log.lastreadposition);
                #region check file exists
                if (!System.IO.File.Exists(log.filename))
                {
                    Console.WriteLine("Log file not found:{0}", log.filename);

                    return 0;
                }

                else
                #endregion
                {
                    try
                    {
                        int i = -1;
                        int numerror = 0;
                        FileStream file = File.Open(log.filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        StreamReader sr = new StreamReader(file, Encoding.GetEncoding("windows-1251"));

                       
                        
                         while (!sr.EndOfStream)
                           {
                            
                            string line = sr.ReadLine();

                            i += 1;
                            if (i > log.lastreadposition)
                                {

                                #region check patterns and output
                                //string line = sr.ReadLine();

                                List<string> incpatterns = new List<string>();
                                List<string> excpatterns = new List<string>();

                                foreach (Pattern p in log.listpatterns)
                                {
                                    if (p.ptype == PType.include)
                                    {
                                        incpatterns.Add(p.patterntext);
                                    }

                                    if (p.ptype == PType.exclude)
                                    {
                                        excpatterns.Add(p.patterntext);
                                    }
                                }
                                if (incpatterns.Any(line.Contains))
                                {

                                    if (!excpatterns.Any(line.Contains))
                                    {

                                        numerror += 1;
                                        Console.WriteLine(line);
                                        Console.WriteLine(GetTime(line));

                                    }

                                }
                                #endregion

                                }
                            //Console.WriteLine("Line time is {0}",GetTime(line));
                            }
                            Console.WriteLine("Total lines: {0}", i);
                            Console.WriteLine("Total number of errors: {0}", numerror);
                            
                        
                        return i;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return 0;

                    }

                }

             }

            else
            {
                Console.WriteLine("Skip logfile {0}", log.filename);
                return 0;

            }

        }

        public static DateTime? GetTime(string line)
        {
            
            string[] patterns =
                    {
                    "dd.MM.yyyy H:mm:ss","dd.MM.yyyy HH:mm:ss",  //Robosrvc
                    "yyyy-MM-dd HH:mm:ss,fff","yyMMdd HH:mm:ss.fff"                  // trient2,vfc
            //trident2:
            //2015-10-22 00:00:00,643 | INFO  | Сообщение ID-TRIDENT2-49363-1445398713905-0-35382692 передано в очередь mmpo2_sort_res_queue | org.niips.endpoints.oracleaq.OracleAqXmlTypeGuaranteedProducer | Camel (camelCtx) thread #131 - timer://exdTimer
            
            //r00roborpo
            //05.05.2015 0:00:17 Загрузка почты e - mail(4982): Проверка рег.почты

                    };

            DateTime parsedTime;
            DateTime? retval = null;
            foreach (string pattern in patterns)

            {
                if (DateTime.TryParseExact(line.Substring(0, 19).Trim(), pattern, null, DateTimeStyles.None, out parsedTime))
                {
                    retval = parsedTime;
                    break;
                }
            }

            return retval;
            
            }


        public static void Run()
        {
            List<Logfile> listconfig = Serializer.deserializeconfig();
            List<Lastposition> lplist = new List<Lastposition>();
            List<Lastposition> delplist = new List<Lastposition>();
            delplist = Serializer.deserializeposition();
            
            foreach (Logfile l in listconfig)
            {

                foreach(Lastposition lp in delplist)
                {
                    if (lp.lastpositionfilename == l.filename)
                    {

                        l.lastreadposition = lp.lastposition;

                    }

                }


                int lpos = Checkfile(l);
                lplist.Add(new Lastposition() { lastpositionfilename = l.filename, lastchecktime = DateTime.Now, lastposition = lpos });

                Console.ReadLine();

            }

            Serializer.serializepozition(lplist);

            foreach (Lastposition lp in lplist)

            {

                Console.WriteLine(lp.lastpositionfilename + " " + lp.lastchecktime + " " + lp.lastposition);

            }

            Console.ReadLine();
        }

    }

    class Program
    {
               
        static void Main(string[] args)
        {


            //Serializer.serializeconfig();

            Checklog.Run();
            Console.Read();


        }

    }

    }


   //todo
   // 1.Сделать парсер подстроки с датой в логе (DateTime? )
   

   






