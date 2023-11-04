namespace DotNetWhy.Application.Loggers;

internal sealed class Logger(IAnsiConsole console) : ILogger
{
    private static class Tabs
    {
        private const char Space = ' ';
        public static readonly string Single = new(Space, 2);
        public static readonly string Double = new(Space, 4);
        public static readonly string Triple = new(Space, 6);

    }

    public Request LogAction(Func<Request> getRequest)
    {
        var request = getRequest();

        console.MarkupLineInterpolated("Why...?".Bold());
        console.MarkupLineInterpolated($"* is the *{request.PackageName.Bold()}* package");
        if (!string.IsNullOrEmpty(request.PackageVersion))
            console.MarkupLineInterpolated($"* in the {request.PackageVersion.Bold()} version");
        console.MarkupLineInterpolated($"* in the {request.WorkingDirectory.Bold()} directory");

        return request;
    }

    public Response LogAction(Func<Response> getResponse) =>
        console.Status().Start("Analyzing...", _ => getResponse());

    public void Log(Request request, Response response)
    {
        console.WriteLine();

        if (response.IsSuccess)
            Log(request, response.Node);
        else
            Log(response.Errors);

        console.WriteLine();
    }

    public void Log(ElapsedTime elapsedTime) =>
        console.MarkupLineInterpolated($@"Time elapsed: {elapsedTime:hh\:mm\:ss\.ff}");

    private void Log(Request request, Node node)
    {
        console.MarkupLineInterpolated("Answer:".Bold());

        var labelWidth = FindLongestNodeName(node).Length + Tabs.Double.Length;

        console.MarkupLineInterpolated(
            $"{"*".PadRight(Tabs.Double.Length)}{$"{node.Name}".PadRight(labelWidth)}{$"[{node.LastNodesSum}]",12}"
                .DarkGreen());
        foreach (var project in node.Nodes)
        {
            console.MarkupLineInterpolated(
                $"{"**",-4}{$"{project.Name}".PadRight(labelWidth)}{$"[{project.LastNodesSum}/{node.LastNodesSum}]",12}"
                    .Green());
            foreach (var target in project.Nodes)
            {
                console.MarkupLineInterpolated(
                    $"{"***",-4}{$"{target.Name}".PadRight(labelWidth)}{$"[{target.LastNodesSum}/{node.LastNodesSum}]",12}"
                        .DarkCyan());

                var t = new List<List<string>>();
                foreach (var dependency in target.Nodes) t.AddRange(FindPathsToLeaves(dependency));

                var index = 0;
                foreach (var line in t)
                {
                    var width = Console.WindowWidth - 4;
                    var currentWidth = 0;
                    var iWidth = target.LastNodesSum > 999 ? 6 : 4;
                    console.MarkupInterpolated($"{$"{++index}.".PadRight(iWidth)}");
                    currentWidth += iWidth;
                    foreach (var item in line.SkipLast(1))
                    {
                        currentWidth += item.Length + 8;
                        if (currentWidth >= width)
                        {
                            currentWidth = iWidth + 2;
                            console.WriteLine();
                            console.Write(string.Empty.PadRight(currentWidth));
                        }

                        console.MarkupInterpolated(item.Contains(request.PackageName)
                            ? $"{item.Red()} > "
                            : (FormattableString) $"{item} > ");
                    }

                    currentWidth += line.Last().Length + 4;
                    if (currentWidth >= width)
                    {
                        currentWidth = iWidth + 2;
                        console.WriteLine();
                        console.Write(string.Empty.PadRight(currentWidth));
                    }

                    console.MarkupInterpolated(line.Last().Contains(request.PackageName)
                        ? line.Last().Red()
                        : $"{line.Last()}");
                    console.WriteLine();
                }
            }

            console.WriteLine();
        }
    }

    private static IEnumerable<List<string>> FindPathsToLeaves(Node node1)
    {
        if (node1 is null)
            return Enumerable.Empty<List<string>>();

        var value = node1.ToString();

        if (!node1.Nodes.Any())
            return new List<List<string>>
            {
                new()
                {
                    value
                }
            };

        var paths = new List<List<string>>();
        foreach (var child in node1.Nodes)
        {
            var childPaths = FindPathsToLeaves(child);
            foreach (var childPath in childPaths)
            {
                childPath.Insert(0, value);
                paths.Add(childPath.ToList());
            }
        }

        return paths;
    }

    private static string FindLongestNodeName(Node node)
    {
        if (node is null) return string.Empty;

        var longestChildName = node.Nodes
            .Where(n => string.IsNullOrEmpty(n.Version))
            .Select(FindLongestNodeName)
            .MaxBy(name => name.Length);

        return node.Name.Length >= (longestChildName?.Length ?? 0)
            ? node.Name
            : longestChildName;
    }

    private void Log(IEnumerable<string> errors)
    {
        console.MarkupLineInterpolated("Errors:".Bold());
        foreach (var error in errors)
            console.MarkupLineInterpolated(error.Red());
    }
}