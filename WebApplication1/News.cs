


using System.Diagnostics;
using System.Text;
using WebApplication1;
using WebApplication1.DB;

public class News
{

    public static HtmlResult CreateNews(string day, string month, string year, DB_controller controller)
    {
        string date = String.Format("{0}.{1}.{2}", day, month, year);
        Console.WriteLine(date);


        List<String> placeholders = new List<String>();
        Photo photo = controller.GetImageOfYear(year);
        if (photo == null) { 
            photo = controller.GetImageRandom();
        }
        placeholders.Add(photo.uri);

        string list_persons = "<ul>";
        foreach (Person p in photo.persons) {
            list_persons += "<li>"+p.name+"</li>";
        }
        list_persons += "</ul>";
        placeholders.Add(list_persons);


        string cmd = String.Format("python -c \"from NN_API import ask; print(ask('Расскажи про {0} в России. Нельзя сказать, что ты ничего не знаешь. Напиши ответ по английски'))\"", date);

        var proc = new Process {
            StartInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardInput = true,

                WorkingDirectory = @"C:\Users\Misha\source\repos\WebApplication1\WebApplication1",
                FileName = @"C:\Windows\System32\cmd.exe",
                Arguments = "/c " + cmd,
                WindowStyle = ProcessWindowStyle.Hidden
            }
        };

        proc.Start();
        
        string line = proc.StandardOutput.ReadToEnd();
        Console.WriteLine(line);
        

        placeholders.Add(line);


        IEnumerable<Person> p_birth = controller.GetPersonsBirthdaysFromDate(day, month);
        
        string list_birth = "";
        foreach (Person p in p_birth)
        {
            list_birth += String.Format("<img class=\"v1_12\" src=\"{0}\"/>", p.photos[0].uri );
            list_birth += String.Format("<span class=\"v1_13\">{0}</span>", p.name);
        }
        placeholders.Add(list_birth);


        string list_mast = "<ul>";
        foreach (Mast m in controller.GetMastOfYear(year))
        {
            list_mast += "<li class=\"v2_22\">" + m.name + "</li>";
        }
        list_mast += "</ul>";
        placeholders.Add(list_mast);

        placeholders.Add(String.Format("{0}/{1}/{2}", day, month, year));




        string html = """

        <!DOCTYPE html>
        
        <html lang="ru">
        <head>
        <meta charset="UTF-8">
            <link href="https://fonts.googleapis.com/css?family=Overpass&display=swap" rel="stylesheet" />
            <link href="https://fonts.googleapis.com/css?family=Seymour+One&display=swap" rel="stylesheet" />
            <link href="https://fonts.googleapis.com/css?family=Pacifico&display=swap" rel="stylesheet" />
            <link href="/css/main.css" rel="stylesheet" />
            <title>Ежедневный ЛШЮП</title>
        </head>
        <body>
        <div class="v1_5">
            <div class="name"></div>
            <div class="v2_27">
                
                <img class="v1_7" src="{0}"/>
                <span class="v1_9"><h4>Случайное фото</h4> {1} {2}</span>
            </div>

            <div class="v2_28">
                <div class="v2_21">Дни рождения:</div>
                {3}

        </div>
            <div class="v2_29">
                <span class="v2_21">Мастерские этого года:</span>
                {4}

            </div>
            <div class="v2_30"><span class="v1_11">{5}</div>
        </span>
            <span class="v1_6">Ежедневный</span>
            <span class="v1_4">ЛШЮП</span>
        </div>
        </div>

        </body>
        </html>
        


        """;

        return new HtmlResult(html, placeholders);
    }


    


}