using OrderFlow.Console.Services;
using System.Text.Json;
using OrderFlow.Console.Models;

namespace OrderFlow.Console.Watchers;

public class InboxWatcher : IDisposable
{
    private SemaphoreSlim semaphoreSlim = new SemaphoreSlim(2, 2);
    private FileSystemWatcher watcher;
    
    public InboxWatcher(string path, OrderPipeline pipeline)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        Directory.CreateDirectory(Path.Combine(path, "processed"));
        Directory.CreateDirectory(Path.Combine(path, "failed"));
        
        watcher = new FileSystemWatcher
        {
            Path = path,
            Filter = "*.json",
            NotifyFilter = NotifyFilters.FileName
        };

        watcher.Created += async (sender, args) =>
        {
            System.Console.WriteLine($"New file discovered at: {args.FullPath}");
            var directory = Path.GetDirectoryName(args.FullPath);
            var filename = Path.GetFileName(args.FullPath);

            var processed = Path.Combine(directory!, "processed", filename);
            var failedFiles = Path.Combine(directory!, "failed", filename);
            var errors = Path.Combine(directory!, "failed", filename + "errors.json");

            await semaphoreSlim.WaitAsync();
            
            try
            {
                var content = await SafeRead(args.FullPath); // Na przyszłość - "args.FULLPATH" to po w tym przypadku "path"
                if (content == null)
                {
                    System.Console.WriteLine($"[Wacther] No file {filename} found after 5 different tries.");
                    return;
                }

                System.Console.WriteLine("File located and loaded successfully.");
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var orders = JsonSerializer.Deserialize<List<Order>>(content, options);
                var simulator = new ExternalServiceSimulator();

                foreach (var order in orders)
                {
                    pipeline.ProcessOrder(order);
                    await simulator.ProcessOrderAsync(order);
                }

                File.Move(args.FullPath, processed);
                System.Console.WriteLine($"[Watcher] File {filename} moved to processed.");
            }
            catch (Exception e)
            {
                System.Console.WriteLine($"[Watcher] Error processing {filename}: {e.Message}");
                File.Move(args.FullPath, failedFiles);
                File.WriteAllText(errors, e.Message);
            }
            finally
            {
                semaphoreSlim.Release();
            }
        };
        
        watcher.EnableRaisingEvents = true;
        System.Console.WriteLine($"Starting inbox watcher: {path}");
    }
    
    public void Dispose() // Zamykanie watcher'a i semaphore'a
    {
        watcher.Dispose();
        semaphoreSlim.Dispose();
        GC.SuppressFinalize(this);
    }
    
    private static async Task<string?> SafeRead(string path, int tries = 5) // Bezpieczne otwieranie pliku 
    {
        for (var i = 0; i < tries; i++)
        {
            try
            {
                return await File.ReadAllTextAsync(path);
            }
            catch (IOException)
            {
                await Task.Delay(200); 
            }
        }
        return null;
    }
}