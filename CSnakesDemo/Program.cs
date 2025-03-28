using CSnakes.Runtime;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Numerics.Tensors;

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
                .FromRedistributable("3.12")
                .WithUvInstaller();
        });

    var app = builder.Build();
    var env = app.Services.GetRequiredService<IPythonEnvironment>();
    return env;
}

void RunModule(IMainModule module)
{
    Console.WriteLine($"Invocando a Python desde C# ({DateTime.Now.TimeOfDay})");
    module.Start();

    var tensor = module.Demo(size: 2000).AsTensorSpan<double>();
    Console.WriteLine("Accediendo al tensor compartido por Python desde C#:");
    PrintTensor(tensor);

    module.Stop();
}

void PrintTensor(TensorSpan<double> tensorSpan)
{
    for (var i=0; i < 5; i++)
    {
        Console.Write("        ");
        for (var j=0; j < 5; j++)
        {
            Console.Write($"{tensorSpan[i, j]:F4}  ");
        }

        Console.WriteLine();
    }

    Console.WriteLine();
}


Console.WriteLine($"Arrancando C# ({DateTime.Now.TimeOfDay})");

var module = GetEnvironment(args, "pythonsrc").MainModule();
RunModule(module);

Console.WriteLine($"Fin de C# ({DateTime.Now.TimeOfDay})");
