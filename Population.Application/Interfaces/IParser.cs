using System.Data;

public interface IParser
{
    DataTable Parse(string path);
}