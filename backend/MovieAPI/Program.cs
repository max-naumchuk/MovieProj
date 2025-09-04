var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSingleton<DataLoader>();
var app = builder.Build();
const string Path = "backend/MovieAPI/Data/";
DataLoader.LoadData(Path);

app.Run();
