var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "Specimens API",
        Description = "A look at specimens that are thirsty at the Cincinnati Zoo and Botanical Garden",
        TermsOfService = new Uri("https://plantplaces.com/privacy.shtml"),
        Contact = new Microsoft.OpenApi.Models.OpenApiContact { 
         Name = "Contact PlantPlaces",
            Url = new Uri("https://plantplaces.com/privacy.shtml")
        },
        License = new Microsoft.OpenApi.Models.OpenApiLicense
        {
            Name = "Plant Places License",
            Url = new Uri("https://plantplaces.com/privacy.shtml")
        }

    });
   

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
