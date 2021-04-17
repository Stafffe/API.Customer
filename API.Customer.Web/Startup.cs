using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace API.Customer.Web
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      //services.AddAuthentication(AzureADDefaults.BearerAuthenticationScheme)    //May add authorization later like this and with an 'AzureAd' config section
      //    .AddAzureADBearer(options => Configuration.Bind("AzureAd", options));

      services.AddControllers();

      services.AddSwaggerGen(options =>
      {
        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme         //Does nothing atm but can be usefull later in case of jwt authorization
        {
          Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
          In = ParameterLocation.Header,
          Name = "Authorization",
          Type = SecuritySchemeType.ApiKey
        });
                                    
        options.OperationFilter<SecurityRequirementsOperationFilter>();
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      IdentityModelEventSource.ShowPII = true;

      app.UseSwagger(); 
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API.Customer");
      });

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
