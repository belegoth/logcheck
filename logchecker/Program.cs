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
    
   
    class Program
    {

        
        static void Main(string[] args)
        {

            Serializer.serializepozition();

    
            List<Lastposition> listposition = Serializer.deserializeposition();
            foreach (Lastposition lp in listposition)
            {
                Console.WriteLine(lp.lastpositionfilename);
                Console.WriteLine(lp.lastposition);
            }

            Console.Read();

            List<Logfile> listconfig = Serializer.deserializeconfig();

            foreach (Logfile l in listconfig)
            {
                Console.WriteLine(l.filename);
                foreach (Pattern p in l.listpatterns)
                {
                    Console.WriteLine(p.ptype + p.patterntext);
                    Console.ReadLine();

                }
            }




        }


    }


    }
    







