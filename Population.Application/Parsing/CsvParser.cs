

using System.Data;

public class CsvParser : IParser
{
    public DataTable Parse(string path)
    {
        var dataTable = new DataTable();
        dataTable.Columns.AddRange(new DataColumn[15]
        {
                    new DataColumn("Sex_ABS", typeof(string)),
                    new DataColumn("Sex", typeof(string)),
                    new DataColumn("AGE", typeof(string)),
                    new DataColumn("Age", typeof(string)),
                    new DataColumn("STATE", typeof(string)),
                    new DataColumn("State", typeof(string)),
                    new DataColumn("REGIONTYPE", typeof(string)),
                    new DataColumn("Geography Level", typeof(string)),
                    new DataColumn("ASGS_2016", typeof(string)),
                    new DataColumn("Region", typeof(string)),
                    new DataColumn("TIME", typeof(int)),
                    new DataColumn("Census year", typeof(int)),
                    new DataColumn("Value", typeof(int)),
                    new DataColumn("Flag Codes", typeof(string)),
                    new DataColumn("Flags", typeof(string)),
        });

        string csv = File.ReadAllText(path);

        var rows = csv.Split('\n');
        var rowsIndex = 0;
        foreach (string row in rows)
        {
            rowsIndex++;

            //skip header row
            if (rowsIndex == 1) continue;

            if (!string.IsNullOrEmpty(row))
            {
                dataTable.Rows.Add();
                int i = 0;
                foreach (string cell in row.Split(','))
                {
                    var rowNum = dataTable.Rows.Count - 1;
                    var cellNum = i;
                    dataTable.Rows[dataTable.Rows.Count - 1][i] = cell;
                    i++;
                }
            }
        }
        return dataTable;
    }
}