# altkom-visacom-dotnet-core
Przykłady ze szkolenia .NET Core 2.2 dla zaawansowanych

# NET Core

## Przydatne komendy CLI
- ``` dotnet new {template} ``` - utworzenie nowego projektu na podstawie wybranego szablonu
- ``` dotnet new {template} -o {output} ``` - utworzenie nowego projektu w podanym katalogu
- ``` dotnet restore ``` - pobranie bibliotek nuget na podstawie pliku projektu
- ``` dotnet build ``` - kompilacja projektu
- ``` dotnet run ``` - uruchomienie projektu
- ``` dotnet run {app.dll}``` - uruchomienie aplikacji
- ``` dotnet test ``` - uruchomienie testów jednostkowych
- ``` dotnet run watch``` - uruchomienie projektu w trybie śledzenia zmian
- ``` dotnet test ``` - uruchomienie testów jednostkowych w trybie śledzenia zmian
- ``` dotnet add {project.csproj} referencje {library.csproj} ``` - dodanie odwołania do biblioteki
- ``` dotnet remove {project.csproj} referencje {library.csproj} ``` - usunięcie odwołania do biblioteki
- ``` dotnet new sln ``` - utworzenie nowego rozwiązania
- ``` dotnet sln {solution.sln} add {project.csproj}``` - dodanie projektu do rozwiązania
- ``` dotnet sln {solution.sln} remove {project.csproj}``` - usunięcie projektu z rozwiązania
- ``` dotnet publish -c Release -r {platform}``` - publikacja aplikacji
- ``` dotnet publish -c Release -r win10-x64``` - publikacja aplikacji dla Windows
- ``` dotnet publish -c Release -r linux-x64``` - publikacja aplikacji dla Linux
- ``` dotnet publish -c Release -r osx-x64``` - publikacja aplikacji dla MacOS

## OWIN

Startup.cs

~~~ csharp
 public void Configure(IApplicationBuilder app, IHostingEnvironment env)
  {
      app.UseOwin(pipeline => pipeline(next => OwinHandler));
  }
            

 public Task OwinHandler(IDictionary<string, object> environment)
{
     string responseText = "Hello World via OWIN";
     byte[] responseBytes = Encoding.UTF8.GetBytes(responseText);

    // OWIN Environment Keys: http://owin.org/spec/spec/owin-1.0.0.html

    var requestMethod = (string) environment["owin.RequestMethod"];
    var requestScheme = (string) environment["owin.RequestScheme"];
    var requestHeaders = (IDictionary<string, string[]>)environment["owin.RequestHeaders"];
    var requestQueryString = (string) environment["owin.RequestQueryString"];         

    var responseStream = (Stream)environment["owin.ResponseBody"];

    var responseHeaders = (IDictionary<string, string[]>)environment["owin.ResponseHeaders"];

    responseHeaders["Content-Length"] = new string[] { responseBytes.Length.ToString(CultureInfo.InvariantCulture) };
    responseHeaders["Content-Type"] = new string[] { "text/plain" };

    return responseStream.WriteAsync(responseBytes, 0, responseBytes.Length);
}

~~~
