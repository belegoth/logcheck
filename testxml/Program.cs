using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;


namespace testxml


{
  //  [XmlRoot("Product")]

    public class Product
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("categoryname")]
        public string CategoryName { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("price")]
        public Price price { get; set; }

        [XmlElement("description")]
        public Description description { get; set; }

    }

    //[XmlRoot("price")]

    public class Price
    {

        [XmlAttribute("unit")]
        public string Unit { get; set; }
        [XmlText]
        public int Value { get; set; }

        

    }

   // [XmlRoot("description")]

    public class Description
    {
        [XmlElement("color")]
        public string Color { get; set; }
        [XmlElement("size")]
        public string Size { get; set; }
        [XmlElement("weight")]
        public string Weight { get; set; }


    }



    //<logfile>
    //  <incpattern>test pattern1</incpattern>
    //  <excpattern>test pattern3</excpattern>	
    //</logfile>



    class Program
    {
        static void Main(string[] args)
        {


            // Serializer
            //   try
            //{
            //    Product product = new Product
            //    {
            //        Id = "p01",
            //        Name = "Product Name 1",
            //        CategoryName = "Category",
            //        price = new Price { Unit = "USD", Value = 100 },
            //        description = new Description { Color = "red", Size = "Blue", Weight = "100gr" }
            //    };
            //    XmlSerializer xmlSerializer = new XmlSerializer(typeof(Product));
            //    StreamWriter sw = new StreamWriter("product.xml");
            //    xmlSerializer.Serialize(sw, product);
            //    sw.Close();
            //    Console.WriteLine("Serialize complete");



            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);

            //}

            try
            {
                List<Product> listproduct = new List<Product>();
                listproduct.Add(new Product
                {

                    Id = "p01",
                    Name = "Product Name 1",
                    CategoryName = "Category",
                    price = new Price { Unit = "USD", Value = 100 },
                    description = new Description { Color = "red", Size = "L", Weight = "100gr" }
                });



                listproduct.Add(new Product
                {

                    Id = "p02",
                    Name = "Product Name 2",
                    CategoryName = "Category",
                    price = new Price { Unit = "USD", Value = 1123 },
                    description = new Description { Color = "yellow", Size = "XXL", Weight = "1320gr" }
                });

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Product>));
                StreamWriter sw = new StreamWriter("product.xml");
                xmlSerializer.Serialize(sw, listproduct);
                sw.Close();
                Console.WriteLine("Serialize complete");
            }




            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

            //deSerializer
            try
                {



                StreamReader reader = new StreamReader("product.xml");
                XmlSerializer x = new XmlSerializer(typeof(List<Product>));
                List<Product> listproduct  = (List<Product>)x.Deserialize(reader);
                
                foreach (Product product in listproduct)
                {
                    Console.WriteLine(product.Name);
                    
                }
                
            }

            catch(Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();

        }

        
    }
}
