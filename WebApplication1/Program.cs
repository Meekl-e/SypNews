using WebApplication1;


Console.OutputEncoding = System.Text.Encoding.Unicode;
var builder = WebApplication.CreateBuilder(args);

var controller = new DB_controller();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseStaticFiles();
app.UseHttpsRedirection();



//app.MapGet("/{user}/{message}", (string user, string message) => $"Hello, {user}! Your message: {message}");

app.MapGet("/{day}/{month}/{year}", (string day, string month, string year) => News.CreateNews(day,month, year, controller));

app.Run();


