using DefaultCorsPolicyNugetPackage;
using eMuhasebeServer.Application;
using eMuhasebeServer.Infrastructure;
using eMuhasebeServer.WebAPI.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//Herþeye izin veren Cors ayarý içeren kütüphane
builder.Services.AddDefaultCors();

//diðer katmanlar referans gösterildikten sonra DI lar yani oraya oluþturulan extensions metodlar buraya çaðrýldý.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

//eðer hata alýnýrsa veya validation hatasý fýrlatýrsa bunu resultpattern içerisinde gönderip ExceptionHandler metodu geriye response olarak basar
builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//kullancýý giriþi yapýlabilmesi için swagger a Authentication adýnda buton eklendi. O butonda buradaki ayarlarla beraber giriþ yapýlmasýna ve bu sayede istek atýldýðýnda otomatik olarak Authorization key i ile beraber Header da token gönderebilmeyi saðlar
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

//login iþlemine sahip ama register iþlemine sahip olunmadýðý için uygulama çalýþtýðýnda otomatik olarak default kullanýcý oluþturmasý için yazýlan extension metod
ExtensionsMiddleware.CreateFirstUser(app);

app.Run();
