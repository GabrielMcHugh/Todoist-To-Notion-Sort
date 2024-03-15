var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

HttpClient client = new HttpClient();

// Call asynchronous network methods in a try/catch block to handle exceptions.
try
{
    using HttpResponseMessage response = await client.GetAsync("http://www.contoso.com/");
    response.EnsureSuccessStatusCode();
    string responseBody = await response.Content.ReadAsStringAsync();
    // Above three lines can be replaced with new helper method below
    // string responseBody = await client.GetStringAsync(uri);

    Console.WriteLine(responseBody);
}
catch (HttpRequestException e)
{
    Console.WriteLine("\nException Caught!");
    Console.WriteLine("Message :{0} ", e.Message);
}

app.Run();
