namespace DotNet.Processor.Helpers;

internal static class TextHelpers
{
    internal static async Task RewriteTextAsyncTo(this TextReader textReader, StringBuilder stringBuilder)
    {
        await Task.Yield();

        string line;
        while ((line = await textReader.ReadLineAsync()) is not null)
        {
            stringBuilder.AppendLine(line);
        }
    }
}