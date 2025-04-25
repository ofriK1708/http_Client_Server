using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using client;

var jsonIgnoreNullOptions = new JsonSerializerOptions
{
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
};

using var client = new HttpClient();
client.BaseAddress = new Uri("http://localhost:9514");

var messageWithResponse = new MyMassage
{
    Id = 123456789,
    Year = 2025,
};

messageWithResponse.RequestId = await client.GetStringAsync(
    $"/test_get_method?id={messageWithResponse.Id}&year={messageWithResponse.Year}");
Console.WriteLine(messageWithResponse.RequestId);

var postResponse = await client.PostAsJsonAsync("/test_post_method", messageWithResponse);
var response = await postResponse.Content.ReadFromJsonAsync<Response>();
Console.WriteLine(response!.Message);

messageWithResponse.RequestId = null;
messageWithResponse.Id = (messageWithResponse.Id - 145987) % 34;
messageWithResponse.Year = (messageWithResponse.Year + 785) % 62;

var putResponse = await client.PutAsJsonAsync($"/test_put_method?id={response.Message}", messageWithResponse, jsonIgnoreNullOptions);
response = await putResponse.Content.ReadFromJsonAsync<Response>();
Console.WriteLine(response!.Message);

var deleteResponse = await client.DeleteAsync($"/test_delete_method?id={response.Message}");


