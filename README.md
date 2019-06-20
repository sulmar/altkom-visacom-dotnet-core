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


## Router

Instalacja
~~~ bash
dotnet add package Microsoft.AspNetCore.Routing;
~~~

Startup.cs

~~~ csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddRouting();
}

public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
   var routeBuilder = new RouteBuilder(app);

   routeBuilder.Routes.Add(new Route(new MyRouter(), "sensors/{number:int}",
       app.ApplicationServices.GetService<IInlineConstraintResolver>()));
       
   app.UseRouter(routeBuilder.Build());
}
~~~

MyRouter.cs

~~~ csharp

public class MyRouter : IRouter
    {
        public VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            return null;
        }

        public Task RouteAsync(RouteContext context)
        {
            var number = context.RouteData.Values["number"] as string;
            
            if (string.IsNullOrEmpty(number)) {
                return Task.FromResult(0);
            }

            if (!Int32.TryParse(number, out int value)) {
                return Task.FromResult(0);
            }

            var requestPath = context.HttpContext.Request.Path;

            context.Handler = async c => await c.Response.WriteAsync($"Number = ({number})");

            return Task.FromResult(0);      
        }
    }
~~~

## Mapowanie tras

Startup.cs

~~~ csharp
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{

  app.Map("/sensors", HandleMapTest1);

  app.Map("/devices", node =>
  {
      node.Map("/active", HandleMapTest2);
      node.Map("/nonactive", HandleMapTest3);
  } );
}


private static void HandleMapTest1(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Map Test 1");
            });
        }

        private static void HandleMapTest2(IApplicationBuilder app)
        { 
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Active devices");
            });
        }

     private static void HandleMapTest3(IApplicationBuilder app)
        { 
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Non active devices");
            });
        }

~~~

## Mapowanie metod

Instalacja
~~~ bash
dotnet add package Microsoft.AspNetCore.Routing;
~~~

Startup.cs

~~~ csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddRouting();
}

public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
   var routeBuilder = new RouteBuilder(app);

 routeBuilder.MapGet("", request => request.Response.WriteAsync("Hello World"));

 routeBuilder.MapGet("sensors", request => request.Response.WriteAsync("Sensors"));

 routeBuilder.MapGet("sensors/{id:int}", request => request.Response.WriteAsync($"Sensor id {request.GetRouteValue("id")}"));

  routeBuilder.MapPost("post", request => request.Response.WriteAsync("Created"));
       
   app.UseRouter(routeBuilder.Build());
}
~~~

## Asynchroniczność

### Asynchroniczna metoda _Main()_ w C# 7.0

Program.cs

~~~ csharp

static void Main(string[] args) => MainAsync(args).GetAwaiter().GetResult();

        static async Task MainAsync(string[] args)
        {
                await DoWorkAsync();
         }
~~~

### Asynchroniczna metoda _Main()_ od C# 7.2

Project.csproj

~~~ xml

<PropertyGroup>
  <LangVersion>latest</LangVersion>
</PropertyGroup>
 
~~~

Program.cs

~~~ csharp
static async Task Main(string[] args)
 {
     await DoWorkAsync();
 }

~~~

## Kondycja

### Instalacja

~~~ bash
 dotnet add package Microsoft.AspNetCore.Diagnostics.HealthChecks
~~~


### Konfiguracja

Startup.cs

~~~ csharp 
public void ConfigureServices(IServiceCollection services)
{
   services.AddHealthChecks();
}

public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
 app.UseHealthChecks("/health");
}

~~~

### Dodanie własnej obsługi

RandomHealthCheck.cs

~~~ csharp

public class RandomHealthCheck  : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            if (DateTime.UtcNow.Minute % 2 == 0)
            {
                return Task.FromResult(HealthCheckResult.Healthy());
            }

            return Task.FromResult(HealthCheckResult.Unhealthy(description: "failed"));
        }
    }
~~~

Startup.cs

~~~ csharp

public void ConfigureServices(IServiceCollection services)
{
 services.AddHealthChecks()
             .AddCheck<RandomHealthCheck>("random");
}

~~~

### Dashboard

Instalacja

~~~ bash
dotnet add package AspNetCore.HealthChecks.UI
~~~
          

Startup.cs

~~~ csharp

public void ConfigureServices(IServiceCollection services)
{
   services.AddHealthChecksUI();
}

public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    app.UseHealthChecks("/health",  new HealthCheckOptions()
      {
          Predicate = _ => true,
          ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
      });
}
~~~

appsettings.json

~~~ json

 "HealthChecks-UI": {
    "HealthChecks": [
      {
        "Name": "Http and UI on single project",
        "Uri": "http://localhost:5000/health"
      }
    ],
    "Webhooks": [],
    "EvaluationTimeOnSeconds": 10,
    "MinimumSecondsBetweenFailureNotifications": 60
  }
  
~~~

Wskazówka: Przejdź na http://localhost:5000/healthchecks-ui aby zobaczyc panel

### Kondycja SQL Server

~~~ bash
dotnet add package AspNetCore.HealthChecks.SqlServer 
~~~

Startup.cs

~~~ csharp

public void ConfigureServices(IServiceCollection services)
{
 services.AddHealthChecksUI()
    .AddSqlServer(Configuration.GetConnectionStrings("MyConnection");
}

~~~


### Kondycja DbContext

~~~ bash
dotnet add package Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore
~~~

Startup.cs

~~~ csharp

public void ConfigureServices(IServiceCollection services)
{
  services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("MyConnection"));
            
    services.AddHealthChecks()
            .AddDbContextCheck<AppDbContext>();

}

~~~

## Signal-R

### Utworzenie huba

CustomersHub.cs

~~~ csharp

public class CustomersHub : Hub
{
     public override Task OnConnectedAsync()
     {
         return base.OnConnectedAsync();
     }

     public Task CustomerAdded(Customer customer)
     {
         return this.Clients.Others.SendAsync("Added", customer);
     } 
}
~~~

### Utworzenie odbiorcy

~~~ bash
dotnet add package Microsoft.AspNetCore.SignalR.Client
~~~

Program.cs

~~~ csharp
static async Task Main(string[] args)
{
     const string url = "http://localhost:5000/hubs/customers";

    HubConnection connection = new HubConnectionBuilder()
        .WithUrl(url)
        .Build();

    Console.WriteLine("Connecting...");

    await connection.StartAsync();

    Console.WriteLine("Connected.");

    connection.On<Customer>("Added",
        customer => Console.WriteLine($"Received customer {customer.FirstName} {customer.LastName}"));

    }
}
~~~

### Utworzenie nadawcy

~~~ bash
dotnet add package Microsoft.AspNetCore.SignalR.Client
~~~

Program.cs

~~~ csharp
static async Task Main(string[] args)
{
    const string url = "http://localhost:5000/hubs/customers";

    HubConnection connection = new HubConnectionBuilder()
        .WithUrl(url)
        .Build();

    Console.WriteLine("Connecting...");
    await connection.StartAsync();
    Console.WriteLine("Connected.");          
    await connection.SendAsync("CustomerAdded", customer);
    Console.WriteLine($"Sent {customer.FirstName} {customer.LastName}");
}

~~~


### Wstrzykiwanie huba

CustomersController.cs

~~~ csharp

 public class CustomersController : ControllerBase
 {
    private readonly IHubContext<CustomersHub> hubContext;
   
    public CustomersController(IHubContext<CustomersHub> hubContext)
     {
         this.hubContext = hubContext;
     }
     
    [HttpPost]
     public async Task<IActionResult> Post( Customer customer)
     {
         customersService.Add(customer);

         await hubContext.Clients.All.SendAsync("Added", customer);

         return CreatedAtRoute(new { Id = customer.Id }, customer);
     }
 }

~~~

### Autentykacja

Program.cs

~~~ csharp
static async Task Main(string[] args)
{
    const string url = "http://localhost:5000/hubs/customers";

    var username = "marcin";
    var password = "12345";

    var credentialBytes = Encoding.UTF8.GetBytes($"{username}:{password}");
    var credentials = Convert.ToBase64String(credentialBytes);

    string parameter = $"Basic {credentials}";

    HubConnection connection = new HubConnectionBuilder()
        .WithUrl(url, options => options.Headers.Add("Authorization", parameter))
        .Build();

    Console.WriteLine("Connecting...");
    await connection.StartAsync();
    Console.WriteLine("Connected.");          
    await connection.SendAsync("CustomerAdded", customer);
    Console.WriteLine($"Sent {customer.FirstName} {customer.LastName}");
}

~~~


### Utworzenie silnie typowanego huba

CustomersHub.cs

~~~ csharp

public interface ICustomersHub
{
    Task Added(Customer customer);
}

public class CustomersHub : Hub<ICustomersHub>
{
     public Task CustomerAdded(Customer customer)
     {
         return this.Clients.Others.Added(customer);
     } 
}
~~~

CustomersController.cs

~~~ csharp

 public class CustomersController : ControllerBase
 {
    private readonly IHubContext<CustomersHub, ICustomersHub> hubContext;
   
    public CustomersController(IHubContext<CustomersHub, ICustomersHub> hubContext)
     {
         this.hubContext = hubContext;
     }
     
    [HttpPost]
     public async Task<IActionResult> Post( Customer customer)
     {
         customersService.Add(customer);

         await hubContext.Clients.All.Added(customer);

         return CreatedAtRoute(new { Id = customer.Id }, customer);
     }
 }

~~~


### Grupy


~~~ csharp

public async Task AddToGroup(string groupName)
{
    await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

    await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has joined the group {groupName}.");
}

public async Task RemoveFromGroup(string groupName)
{
    await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

    await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}.");
}

~~~



## Docker

- Uruchomienie pierwszego kontenera
~~~ bash
docker run ubuntu /bin/echo 'Hello world'
~~~

- Uruchomienie w trybie interaktywnym
~~~ bash
docker run -i -t --rm ubuntu /bin/bash
~~~

### Przydatne komendy
- ``` docker images ``` - lista wszystkich obrazów na twojej maszynie
- ``` docker pull <image> ``` - pobranie obrazu
- ``` docker run <image> ``` - uruchomienie obrazu (pobiera jeśli nie ma)
- ``` docker ps ``` - lista wszystkich uruchomionych kontenerów na twojej maszynie
- ``` docker ps -a``` - lista wszystkich przyłączonych ale nie uruchomionych kontenerów
- ``` docker start <containter_name> ``` - uruchomienie kontenera wg nazwy
- ``` docker stop <containter_name> ``` - zatrzymanie kontenera wg nazwy

### Konteneryzacja aplikacji .NET Core

* Utwórz plik Dockerfile

~~~
FROM microsoft/dotnet:2.0-sdk
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# copy and build everything else
COPY . ./
RUN dotnet publish -c Release -o out
ENTRYPOINT ["dotnet", "out/Hello.dll"]
~~~

