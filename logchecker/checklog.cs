using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace logchecker
{
   public class Checklog 
    {

        
        public static void Checkfile(string filename, List<Pattern> patterns)
        {
            Console.WriteLine(filename);

            if (!System.IO.File.Exists(filename))
            {
                Console.WriteLine("Log file not found:{0}", filename);
                
            }
            else
            {
                try {
                    FileStream file = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    StreamReader sr = new StreamReader(file, Encoding.GetEncoding("windows-1251"));
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();

                        List<string> incpatterns = new List<string>();
                        List<string> excpatterns = new List<string>();

                        foreach (Pattern p in patterns)
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


                        if (incpatterns.Any(line.Contains)) {

                            if (!excpatterns.Any(line.Contains))
                            {

                                Console.WriteLine(line);

                            }

                        }
                

                    }


                    


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }

            }


        }



        public static void Printinfo()
        {
            List<Logfile> listconfig = Serializer.deserializeconfig();

            foreach (Logfile l in listconfig)
            {

              
              Checkfile(l.filename, l.listpatterns);
                               
                
              Console.ReadLine();

            }


        }





    }
}
