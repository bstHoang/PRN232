using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;
using System.Collections;
using System.Reflection;

public class CsvOutputFormatter : TextOutputFormatter
{
    public CsvOutputFormatter()
    {
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
        SupportedEncodings.Add(Encoding.UTF8);
    }

    protected override bool CanWriteType(Type type)
    {
        return typeof(IEnumerable).IsAssignableFrom(type) || base.CanWriteType(type);
    }

    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
    {
        var response = context.HttpContext.Response;
        var buffer = new StringBuilder();

        var enumerable = context.Object as IEnumerable<object>;
        if (enumerable == null)
        {
            enumerable = new List<object> { context.Object };
        }

        var enumerator = enumerable.GetEnumerator();
        if (enumerator.MoveNext())
        {
            var props = enumerator.Current.GetType().GetProperties();
            buffer.AppendLine(string.Join(",", props.Select(p => p.Name)));

            do
            {
                var line = string.Join(",", props.Select(p => p.GetValue(enumerator.Current)?.ToString()));
                buffer.AppendLine(line);
            } while (enumerator.MoveNext());
        }

        await response.WriteAsync(buffer.ToString());
    }
}
