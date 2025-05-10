using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using System.Reflection;
using WebApplication1.DB;


namespace WebApplication1
{
    public class DB_controller
    {

        List<Person> peoples = new List<Person>();
        List<Mast> mastersks = new List<Mast>();
        List<Photo> photos = new List<Photo>();
        List<Reflect> reflects = new List<Reflect>();
        List<Part> participations = new List<Part>();

        public Photo? GetImageOfYear(string year)
        {
            IEnumerable<Photo> photos_x = photos;
            return photos_x.Where(x => x.year == year).FirstOrDefault();

        }
        public Photo GetImageRandom()
        {
            var rand = new Random();
            return photos[rand.Next(photos.Count() - 1)];

        }

        public IEnumerable<Person> GetPersonsBirthdaysFromDate(string day, string month)
        {
            
            IEnumerable<Person> peoples_x = peoples;
            return peoples_x.Where(x => x.birth.Split(".")[0] == day && x.birth.Split(".")[1] == month);
        }


        public IEnumerable<Mast> GetMastOfYear(string year)
        {
            IEnumerable<Mast> mast_x = mastersks;
            return mast_x.Where(x => x.year == year);

        }
        public DB_controller()
        {
            var xDB = XElement.Load("DB/SypCassete_current.fog");
            // Загрузка элементов
            foreach (XElement xElement in xDB.Elements())
            {
                if (xElement.Name.LocalName == "person")
                {
                    string idFromBD = xElement.Attribute("{http://www.w3.org/1999/02/22-rdf-syntax-ns#}about").Value;
                    string name = xElement.Element("name").Value;
                    string birth = xElement.Element("birth")?.Value;
                    if (birth == null)
                    {
                        birth = string.Empty;
                    }
                    peoples.Add(new Person() { id = idFromBD.GetHashCode(), name=name, birth = birth } );

                }
                if (xElement.Name.LocalName == "org-sys")
                {
                    string idFromBD = xElement.Attribute("{http://www.w3.org/1999/02/22-rdf-syntax-ns#}about").Value;
                    string name = xElement.Element("name").Value;
                    DB.Mast mast = new DB.Mast { id=idFromBD.GetHashCode(), name=name };

                    string fromDate = xElement.Element("from-date")?.Value;
                    if (fromDate != null)
                    {
                        mast.year = fromDate;
                    }
                    mastersks.Add(mast);
                }
                if (xElement.Name.LocalName == "photo-doc")
                {
                    string idFromBD = xElement.Attribute("{http://www.w3.org/1999/02/22-rdf-syntax-ns#}about").Value;
                    XElement xStore = xElement.Element("iisstore");
                    Photo doc = new Photo() { id=idFromBD.GetHashCode() };
                    if (xStore != null)
                    {
                        string uri = xStore.Attribute("uri").Value;
                        doc.uri = "/database/images/"+String.Join("/",uri.Split("/").Skip(4).ToArray()) + ".jpg";
                        
                    }
                    string fromDate = xElement.Element("from-date")?.Value;
                    if (fromDate != null)
                    {
                        doc.year = fromDate;
                        Console.WriteLine(doc.year);
                    }
                    string name = xElement.Element("name")?.Value;
                    if (name != null)
                    {
                        doc.name = name;

                    }
                    photos.Add(doc);
                }
                if (xElement.Name.LocalName == "reflection")
                {
                    string idFromBD = xElement.Attribute("{http://www.w3.org/1999/02/22-rdf-syntax-ns#}about").Value;
                    int toDoc = xElement.Element("in-doc").Attribute("{http://www.w3.org/1999/02/22-rdf-syntax-ns#}resource").Value.GetHashCode();
                    int toPerson = xElement.Element("reflected").Attribute("{http://www.w3.org/1999/02/22-rdf-syntax-ns#}resource").Value.GetHashCode();

                    reflects.Add(new Reflect { string_id= idFromBD, id =idFromBD.GetHashCode(), toDoc=toDoc, toPerson=toPerson });

                }
                if (xElement.Name.LocalName == "participation")
                {
                    string idFromBD = xElement.Attribute("{http://www.w3.org/1999/02/22-rdf-syntax-ns#}about").Value;
                    int member = xElement.Element("participant").Attribute("{http://www.w3.org/1999/02/22-rdf-syntax-ns#}resource").Value.GetHashCode();
                    int inOrg = xElement.Element("in-org").Attribute("{http://www.w3.org/1999/02/22-rdf-syntax-ns#}resource").Value.GetHashCode();

                    participations.Add(new Part() { string_id = idFromBD, id = idFromBD.GetHashCode(), person=member, inOrg=inOrg });
                }
            }
            // Обработка Reflection and participation
            IEnumerable<Person> persons_x = peoples;
            IEnumerable<Photo> photos_x = photos;
            IEnumerable<Mast> mastersks_x = mastersks;

            foreach (Reflect refl in reflects) { 
                Person person = persons_x.Where(x => x.id == refl.toPerson).FirstOrDefault();
                Photo photo = photos_x.Where(x => x.id == refl.toDoc).FirstOrDefault();
                Mast org = mastersks_x.Where(x => x.id == refl.toPerson).FirstOrDefault();

                if (photo != null) {

                    if (person != null)
                    {
                        person.photos.Add(photo);
                        photo.persons.Add(person);
                        //Console.WriteLine("{0}, {1}, {2}, FOUND", person, photo, refl.id);
                    }
                    else if (org != null)
                    {
                        photo.mastersks.Add(org);
                        org.photos.Add(photo);
                        //Console.WriteLine("{0}, {1}, {2}, FOUND MAST", org, photo, refl.id);
                    }
                    
                
                }
            }
            foreach (Part part in participations) { 
                Person person = persons_x.Where(x => x.id == part.person).FirstOrDefault();
                Mast org = mastersks_x.Where(x => x.id == part.inOrg).FirstOrDefault();

                if (person != null && org != null)
                {
                    person.orgs.Add(org);
                    org.members.Add(person);
                }
            }
            
        }
        
    }

}
