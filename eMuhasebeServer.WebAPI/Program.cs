using DefaultCorsPolicyNugetPackage;
using eMuhasebeServer.Application;
using eMuhasebeServer.Infrastructure;
using eMuhasebeServer.WebAPI.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//Her�eye izin veren Cors ayar� i�eren k�t�phane
builder.Services.AddDefaultCors();

//di�er katmanlar referans g�sterildikten sonra DI lar yani oraya olu�turulan extensions metodlar buraya �a�r�ld�.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

//e�er hata al�n�rsa veya validation hatas� f�rlat�rsa bunu resultpattern i�erisinde g�nderip ExceptionHandler metodu geriye response olarak basar
builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//kullanc�� giri�i yap�labilmesi i�in swagger a Authentication ad�nda buton eklendi. O butonda buradaki ayarlarla beraber giri� yap�lmas�na ve bu sayede istek at�ld���nda otomatik olarak Authorization key i ile beraber Header da token g�nderebilmeyi sa�lar
builder.Services.AddSwaggerGen(setup =>
{
    var jwtSecuritySheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** yourt JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(jwtSecuritySheme.Reference.Id, jwtSecuritySheme);

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecuritySheme, Array.Empty<string>() }
                });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//default ayarlar
app.UseHttpsRedirection();

app.UseCors();

app.UseExceptionHandler();

app.MapControllers();

//login i�lemine sahip ama register i�lemine sahip olunmad��� i�in uygulama �al��t���nda otomatik olarak default kullan�c� olu�turmas� i�in yaz�lan extension metod
ExtensionsMiddleware.CreateFirstUser(app);

app.Run();
