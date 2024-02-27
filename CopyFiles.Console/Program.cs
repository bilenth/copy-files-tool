using CopyFiles.Core;

internal class Program
{
    private static async Task Main(string[] args)
    {
        if (args.Length == 3)
        {
            string folderPath = args[0];
            string oldValue = args[1];
            string newValue = args[2];

            var message = await CopyFiles.Core.CopyFiles.RunAsync(folderPath, oldValue, newValue);

            await Console.Out.WriteLineAsync(message);
        }

        await Console.Out.WriteLineAsync("sss");
    }
}