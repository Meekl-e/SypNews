using System.Net.Mime;
using System.Text;
using System.Xml.Linq;
public class HtmlResult : IResult
{
    private readonly string _html;

    public HtmlResult(string html, List<String> placeholders)
    {
        int idx = 0;    
        foreach (string item in placeholders) {
            html = html.Replace("{"+idx.ToString()+"}", item);
            idx++;
        }
        _html = html;
    }

    public Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.ContentType = MediaTypeNames.Text.Html;
        httpContext.Response.ContentLength = Encoding.UTF8.GetByteCount(_html);
        return httpContext.Response.WriteAsync(_html);
    }

}