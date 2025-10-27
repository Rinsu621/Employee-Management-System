var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles(); // Enable serving files from wwwroot

app.MapGet("/", () => Results.Redirect("/salary-ui/index.html")); // Redirect root to your HTML

app.Run();
