using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Text;
using System.Xml.Serialization;


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
        [XmlElement("updatetimelimit")]
        public int updatetimelimit { get; set; }
        [XmlArray("patterns"), XmlArrayItem("pattern")]
        public List<Pattern> listpatterns = new List<Pattern>();



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


  

    class Program
    {
        static void Main(string[] args)
        {


            //Serializer
            try
            {

                Logfiles logfiles = new Logfiles();
                logfiles.ListFiles = new List<Logfile>();

                Logfile logfile = new Logfile();
                logfile.listpatterns.Add(new Pattern() { ptype = PType.exclude , patterntext="text to exclude"});
                logfile.listpatterns.Add(new Pattern() { ptype = PType.exclude , patterntext="text2 to exclude" });
                logfile.listpatterns.Add(new Pattern() { ptype = PType.include , patterntext="text to include" });


                logfiles.ListFiles.Add(new Logfile { filename = "file1", active = true, checkupdatetime = true, updatetimelimit = 20, listpatterns=logfile.listpatterns });
                logfiles.ListFiles.Add(new Logfile { filename = "file2", active = true, checkupdatetime = false });


                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Logfiles));
                StreamWriter sw = new StreamWriter("logfile.xml");
                xmlSerializer.Serialize(sw, logfiles);
                sw.Close();
                Console.WriteLine("Serialize complete");



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                  }


            //DeSerializer

            TextReader reader = new StreamReader("logfile.xml");
            XmlSerializer x = new XmlSerializer(typeof(Logfiles));
            Logfiles listlogs = (Logfiles)x.Deserialize(reader);
            
            foreach (Logfile l in listlogs.ListFiles)
            {
                Console.WriteLine(l.filename);
                foreach  (Pattern p in l.listpatterns)
                {
                    Console.WriteLine(p.ptype + p.patterntext);
                }
            }
                          
            
                
                Console.ReadLine();
            }
        }


    }
    







