namespace DotNetWhy.Application.Loggers;

internal interface ILogger
{
    Request LogAction(Func<Request> getRequest);
    Response LogAction(Func<Response> getResponse);
    void Log(Request request, Response response);
    void Log(ElapsedTime elapsedTime);
}