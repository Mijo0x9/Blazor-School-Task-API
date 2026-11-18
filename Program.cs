using MyApp.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Dev-only: Server-Sent Events endpoint for live browser reload.
// When `dotnet watch` restarts the server on a code change, this
// connection drops. The script in App.razor detects that and reloads
// the page, no matter where the user currently is.
if (app.Environment.IsDevelopment())
{
    app.MapGet("/dev-reload", async (HttpContext context) =>
    {
        context.Response.Headers.ContentType = "text/event-stream";
        context.Response.Headers.CacheControl = "no-cache";
        context.Response.Headers.Connection = "keep-alive";
        await context.Response.WriteAsync("retry: 1000\n\n");
        await context.Response.Body.FlushAsync();
        // Keep the stream open until the server shuts down or the client disconnects.
        await Task.Delay(Timeout.Infinite, context.RequestAborted);
    });
}

app.Run();
