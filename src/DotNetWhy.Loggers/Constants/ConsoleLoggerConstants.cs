namespace DotNetWhy.Loggers.Constants;

internal static class ConsoleLoggerConstants
{
    public static readonly Encoding Encoding = Encoding.UTF8;

    public static class Prefixes
    {
        public const string Solution = "\u1409";
        public const string Project = "\u1405";
        public const string Target = "\u1433";
    }

    public static class Separators
    {
        public const char Default = ' ';
        public const char Short = ':';
        public const string Long = " -> ";
    }

    public static class Widths
    {
        public static int Tab => 2;
        public static int DoubleTab => 2 * Tab;
        public static int TripleTab => 3 * Tab;
        public static int QuadrupleTab => 4 * Tab;
        public static int Max => (Console.WindowWidth >= 144 ? 144 : Console.WindowWidth >= 120 ? 120 : 80) - DoubleTab;
        public static int Label(Solution solution) =>
            new[]
            {
                solution.Name.Length,
                solution
                    .Projects
                    .Select(project => project.Name.Length).Max(),
                solution
                    .Projects
                    .SelectMany(project => project.Targets
                        .Select(target => target.Name.Length)).Max()
            }.Max() + DoubleTab;
    }
}