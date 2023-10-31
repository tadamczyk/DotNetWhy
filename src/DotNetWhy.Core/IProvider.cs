namespace DotNetWhy.Core;

public interface IProvider
{
    Response Get(Request request);
}