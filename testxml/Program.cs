using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Globalization;

namespace test
{
    class Program
    {
   


        static void Main(string[] args)
        {
         DateTime? s;
            s = GetTime("2015-10-16 00:00:00,032");
            Console.WriteLine(s);
            Console.Read();
            
        }

        
        public static DateTime? GetTime(string line)
        {

            string[] patterns =
                    {


              "dd.MM.yyyy H:mm:ss","dd.MM.yyyy HH:mm:ss",  //Robosrvc
              "yyyy-MM-dd HH:mm:ss,fff","yyMMdd HH:mm:ss,fff"                  // trient2,vfc


                
            //trident2:
            //2015-10-22 00:00:00,643 | INFO  | Сообщение ID-TRIDENT2-49363-1445398713905-0-35382692 передано в очередь mmpo2_sort_res_queue | org.niips.endpoints.oracleaq.OracleAqXmlTypeGuaranteedProducer | Camel (camelCtx) thread #131 - timer://exdTimer
            
            //r00roborpo
            //05.05.2015 0:00:17 Загрузка почты e - mail(4982): Проверка рег.почты

                    };

            DateTime parsedTime;
            DateTime? retval = null;
            foreach (string pattern in patterns)

            {
                if (DateTime.TryParseExact(line, pattern, null, DateTimeStyles.None, out parsedTime))
                {
                    retval = parsedTime;
                    Console.WriteLine(parsedTime);
                    break;
                }
            }

            return retval;

        }
    }
}

