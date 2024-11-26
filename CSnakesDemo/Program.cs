using CSnakes.Runtime;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

IPythonEnvironment GetEnvironment(string[] strings, string pythonsrc)
{
    var builder = Host.CreateDefaultBuilder(strings)
        .ConfigureServices(services =>
        {
            var home = Path.Join(Environment.CurrentDirectory, pythonsrc);
            var venv = Path.Join(home, $".venv-{pythonsrc}");
            services
                .AddLogging(loggingBuilder => loggingBuilder.SetMinimumLevel(LogLevel.Information))
                .WithPython()
                .WithHome(home)
                .WithVirtualEnvironment(venv)
                .FromNuGet("3.12.4")
                .WithPipInstaller();
        });

    var app = builder.Build();
    var env = app.Services.GetRequiredService<IPythonEnvironment>();
    return env;
}

void RunModule1(IMain module)
{
    Console.WriteLine($"Invocando a Python desde C# ({DateTime.Now.TimeOfDay})");
    module.Start();

    var tensor = module.PytorchDemo().AsTensorSpan<float>();
    Console.WriteLine("Accediendo al tensor compartido por Python desde C#:");
    for (var i=0; i < 5; i++)
    {
        Console.Write("        ");
        for (var j=0; j < 5; j++)
        {
            Console.Write($"{tensor[i, j]:F4}  ");
        }

        Console.WriteLine();
    }
    Console.WriteLine();

    module.Stop();
}

void RunModule2(IMain2 module)
{
    Console.WriteLine($"Invocando a Python desde C# ({DateTime.Now.TimeOfDay})");
    module.Start();

    var tensor = module.PytorchDemo().AsTensorSpan<float>();
    Console.WriteLine("Accediendo al tensor compartido por Python desde C#:");
    for (var i = 0; i < 5; i++)
    {
        Console.Write("        ");
        for (var j = 0; j < 5; j++)
        {
            Console.Write($"{tensor[i, j]:F4}  ");
        }

        Console.WriteLine();
    }
    Console.WriteLine();

    module.Stop();
}

Console.WriteLine($"Arrancando C# ({DateTime.Now.TimeOfDay})");

var module1 = GetEnvironment(args, "pythonsrc").Main();
RunModule1(module1);

//var module2 = GetEnvironment(args, "pythonsrc2").Main2();
//RunModule2(module2);

Console.WriteLine($"Fin de C# ({DateTime.Now.TimeOfDay})");
