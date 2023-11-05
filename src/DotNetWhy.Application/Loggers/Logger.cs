namespace DotNetWhy.Application.Loggers;

internal sealed class Logger(IAnsiConsole console) : ILogger
{
    private readonly IDictionary<int, Func<string, FormattableString>> _headerLabelsColors =
        new Dictionary<int, Func<string, FormattableString>>
        {
            {1, Extensions.DarkGreen},
            {2, Extensions.Green},
            {3, Extensions.DarkCyan}
        };

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
        console
            .Status()
            .SpinnerStyle(new Style(Color.Green))
            .Start("Analyzing...", _ => getResponse());

    public void Log(Request request, Response response)
    {
        console.WriteLine();

        if (response.IsSuccess)
            Log(request, response.Node);
        else
            Log(response.Errors);
    }

    public void Log(ElapsedTime elapsedTime) =>
        console.MarkupLineInterpolated($@"Time elapsed: {elapsedTime:hh\:mm\:ss\.ff}");

    private void Log(Request request, Node node)
    {
        var maxWidth = Console.WindowWidth - Tabs.Double;
        var nameWidth = GetLongestNodeNameLength(node);
        var rootLastNodesSum = node.LastNodesSum;

        console.MarkupLineInterpolated("Answer:".Bold());

        console.MarkupLineInterpolated(GetHeaderLabel(1, node, nameWidth));
        foreach (var project in node.Nodes)
        {
            console.MarkupLineInterpolated(GetHeaderLabel(2, project, nameWidth, rootLastNodesSum));
            foreach (var target in project.Nodes)
            {
                console.MarkupLineInterpolated(GetHeaderLabel(3, target, nameWidth, rootLastNodesSum));

                var index = 0;

                var paths = target.Nodes.SelectMany(GetPaths);
                foreach (var path in paths)
                {
                    var indexWidth = target.LastNodesSum.ToString().Length >= Tabs.Double
                        ? Tabs.Triple
                        : Tabs.Double;

                    var width = indexWidth;

                    console.MarkupInterpolated($"{$"{++index}.".PadRight(indexWidth)}");

                    for (var iterator = 0; iterator < path.Count; iterator++)
                    {
                        var item = path.ElementAt(iterator);
                        var isLastItem = iterator == path.Count - 1;
                        width +=
                            item.Length +
                            (isLastItem
                                ? Tabs.Double
                                : Tabs.Triple);

                        if (width >= maxWidth)
                        {
                            width = indexWidth + Tabs.Single;
                            console.WriteLine();
                            console.Write(string.Empty.PadRight(width));
                        }

                        console.MarkupInterpolated(
                            item.Contains(request.PackageName)
                                ? item.Red()
                                : $"{item}");
                        if (!isLastItem)
                            console.MarkupInterpolated($" {Characters.Separator} ");
                    }

                    console.WriteLine();
                }
            }

            console.WriteLine();
        }
    }

    private static int GetLongestNodeNameLength(Node node) =>
        GetLongestNodeName(node).Length + Tabs.Double;

    private static string GetLongestNodeName(Node node)
    {
        if (node is null)
            return string.Empty;

        var longestNodeName = node
            .Nodes
            .Where(n => string.IsNullOrEmpty(n.Version))
            .Select(GetLongestNodeName)
            .MaxBy(name => name.Length);

        return node.Name.Length >= (longestNodeName?.Length ?? default)
            ? node.Name
            : longestNodeName;
    }

    private FormattableString GetHeaderLabel(
        int level,
        Node node,
        int nameWidth,
        int parentLastNodesSum = default)
    {
        var prefix = new string(Characters.Level, level).PadRight(Tabs.Double);
        var name = node.Name.PadRight(nameWidth);
        var suffixText = parentLastNodesSum is 0
            ? $"[{node.LastNodesSum}]"
            : $"[{node.LastNodesSum}/{parentLastNodesSum}]";
        var suffix = suffixText.PadLeft(Tabs.Triple * Tabs.Single);

        return _headerLabelsColors[level]($"{prefix}{name}{suffix}");
    }

    private static IEnumerable<List<string>> GetPaths(Node node)
    {
        if (node is null)
            return Enumerable.Empty<List<string>>();

        var nodeValue = node.ToString();

        if (!node.Nodes.Any())
            return new List<List<string>>
            {
                new()
                {
                    nodeValue
                }
            };

        var paths = new List<List<string>>();
        foreach (var childNode in node.Nodes)
        {
            var childPaths = GetPaths(childNode);
            foreach (var childPath in childPaths)
            {
                childPath.Insert(0, nodeValue);
                paths.Add(childPath.ToList());
            }
        }

        return paths;
    }

    private void Log(IEnumerable<string> errors)
    {
        console.MarkupLineInterpolated("Errors:".Bold());
        foreach (var error in errors)
            console.MarkupLineInterpolated(error.Red());
        console.WriteLine();
    }

    private static class Characters
    {
        public const char Level = '*';
        public const char Separator = '>';
    }

    private static class Tabs
    {
        public const int Single = 2;
        public const int Double = 4;
        public const int Triple = 6;
    }
}