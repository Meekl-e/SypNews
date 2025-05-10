namespace WebApplication1.DB
{
  public class Person
    {
        public int id;
        public string name = String.Empty;
        public string birth = String.Empty;
        public List<Photo> photos = new List<Photo>();
        public List<Mast> orgs = new List<Mast>();
    }
  public class Mast
    {
        public int id;
        public string name = String.Empty;
        public string year = String.Empty;
        public List<Photo> photos = new List<Photo>();
        public List<Person> members = new List<Person>();
    }
  public class Photo
    {
        public int id;
        public string year = String.Empty;
        public string uri = String.Empty;
        public string name = String.Empty;
        public List<Person> persons = new List<Person>();
        public List<Mast> mastersks = new List<Mast>();
    }
  public class Reflect
    {
        public string string_id = String.Empty;
        public int id;
        public int toDoc;
        public int toPerson;
    }
  public class Part
    {
        public string string_id = String.Empty;
        public int id;
        public int inOrg;
        public int person;
    }
}
