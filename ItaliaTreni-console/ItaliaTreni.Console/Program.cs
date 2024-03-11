// See https://aka.ms/new-console-template for more information
using CsvHelper;
using CsvHelper.Configuration;
using ItaliaTreni.Console;
using RestSharp;
using System.Configuration;
using System.Globalization;

Console.WriteLine("Hello. I proceed with importing the file");

string pathInput = ConfigurationManager.AppSettings["FileInputPath"];
string pathArchive = ConfigurationManager.AppSettings["FileArchivePath"];
string urlApi = ConfigurationManager.AppSettings["UrlApi"];
string fileName = "Import " + DateTime.Now.ToString("dd-MM-yyyy") + ".csv";
string fullPathInput = Path.Combine(pathInput, fileName);
string fullPathArchive = Path.Combine(pathArchive, fileName);

var csvParserCulture = CultureInfo.GetCultureInfo(ConfigurationManager.AppSettings["CultureInfo"]);

int numberOfElementsToImport = int.Parse(ConfigurationManager.AppSettings["NumberOfElementsToImport"]);

var csvConfiguration = new CsvConfiguration(csvParserCulture)
{
    Delimiter = ","
};

int counter = 0;
try
{
    bool isApiSuccess = false;
    List<DataFile> datas = new List<DataFile>();

    var client = new RestClient(urlApi);
    var request = new RestRequest(urlApi);
    request.RequestFormat = RestSharp.DataFormat.Json;
    var createFileRequest = new CreateFileRequest();
    createFileRequest.Name = fileName;

    using (var fileStream = File.OpenRead(fullPathInput))
    using (var streamReader = new StreamReader(fileStream))
    using (var csvReader = new CsvReader(streamReader, csvConfiguration))
    {
        csvReader.Context.RegisterClassMap<InputFileMap>();

        while (csvReader.Read())
        {
            counter++;
            var record = csvReader.GetRecord<InputFileCsvRow>();
            datas.Add(new DataFile
            {
                MM = int.Parse(record.MM),
                P1 = double.Parse(record.P1, CultureInfo.InvariantCulture),
                P2 = double.Parse(record.P2, CultureInfo.InvariantCulture),
                P3 = double.Parse(record.P3, CultureInfo.InvariantCulture),
                P4 = double.Parse(record.P4, CultureInfo.InvariantCulture)
            });

            if (counter == numberOfElementsToImport)
            {
                createFileRequest.Datas = datas;
                request.AddBody(createFileRequest);
                var response = client.Post(request);
                isApiSuccess = response.IsSuccessful;

                datas = new List<DataFile>();
                counter = 0;
            }
        }
    }

    if (datas.Any() && isApiSuccess)
    {
        createFileRequest.Datas = datas;
        request.AddBody(createFileRequest);
        var response = client.Post(request);
        isApiSuccess = response.IsSuccessful;
    }

    if (isApiSuccess)
    {
        //Move file to archive file
        Console.WriteLine("Move file to in folder archive");
        File.Move(fullPathInput, fullPathArchive);
    }
}
catch (Exception e)
{
    Console.WriteLine("Exception: " + e.Message);
}
finally
{
    Console.WriteLine("Executing finally block.");
}