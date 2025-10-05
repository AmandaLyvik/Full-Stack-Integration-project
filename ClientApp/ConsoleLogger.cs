using Microsoft.JSInterop;
using System.Threading.Tasks;

public class ConsoleLogger
{
    private readonly IJSRuntime _js;

    public ConsoleLogger(IJSRuntime js)
    {
        _js = js;
    }

    public ValueTask LogAsync(string message)
    {
        return _js.InvokeVoidAsync("console.log", message);
    }

    public ValueTask ErrorAsync(string message)
    {
        return _js.InvokeVoidAsync("console.error", message);
    }
}
