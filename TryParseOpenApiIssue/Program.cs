var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    _ = app.MapOpenApi();
    _ = app.UseSwaggerUI(options =>
    {
        options.EnableTryItOutByDefault();
        options.SwaggerEndpoint("/openapi/v1.json", $"{builder.Environment.ApplicationName} v1");
    });
}

app.UseHttpsRedirection();

// Will correcly report a student as a query string parameter
app.MapPost("/enroll", (Student student) => Results.NoContent());

// Will report a student as a query string parameter and as the request body
app.MapPost("/enroll-withaccepts", (Student student) => Results.NoContent())
   .Accepts<Student>("application/json");

// Will correctly report the parameter as a string
app.MapGet("/student/{student}", (Student student) => $"Hi {student.Name}");

// Will report the parameter as a string and as the request body
app.MapGet("/student-withaccepts/{student}", (Student student) => $"Hi {student.Name}")
    .Accepts<Student>("application/json");

app.Run();

public record Student(string Name)
{
    public static bool TryParse(string value, out Student? result)
    {
        if (value is null)
        {
            result = null;
            return false;
        }

        result = new Student(value);
        return true;
    }
}
