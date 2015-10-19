using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


        public void serializeconfig()
        {

            try
            {

                Logfiles logfiles = new Logfiles();
                logfiles.ListFiles = new List<Logfile>();

                Logfile logfile = new Logfile();
                logfile.listpatterns.Add(new Pattern() { ptype = PType.exclude, patterntext = "text to exclude" });
                logfile.listpatterns.Add(new Pattern() { ptype = PType.exclude, patterntext = "text2 to exclude" });
                logfile.listpatterns.Add(new Pattern() { ptype = PType.include, patterntext = "text to include" });


                logfiles.ListFiles.Add(new Logfile { filename = "file1", active = true, checkupdatetime = true, updatetimelimit = 20, listpatterns = logfile.listpatterns });
                logfiles.ListFiles.Add(new Logfile { filename = "file2", active = true, checkupdatetime = false });


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
                TextReader reader = new StreamReader("logfile.xml");
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


        public static void serializepozition()
        {

            try
            {


                Lastpositions lastpositions = new Lastpositions();
                lastpositions.ListPositions = new List<Lastposition>();

                lastpositions.ListPositions.Add(new Lastposition { lastpositionfilename = "file1", lastposition = 123, lastchecktime = DateTime.Now });
                lastpositions.ListPositions.Add(new Lastposition { lastpositionfilename = "file2", lastposition = 1343, lastchecktime = DateTime.Now });


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





}
