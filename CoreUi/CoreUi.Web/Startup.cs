using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CoreUi.Razor;
using CoreUi.Razor.Data;
using CoreUi.Razor.Dialog;
using CoreUi.Razor.Event.Client;
using CoreUi.Razor.Event.Source;
using CoreUi.Razor.MultiClient;
using CoreUi.Razor.Proxy;
using CoreUi.Web.Controllers;
using idunno.Authentication.Basic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace CoreUi.Web
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // middleware
            services.AddSingleton<IEventSource, EventSource>();
            services.AddSingleton<IWebInteractionProvider, WebInteractionProvider>();
            services.AddSingleton<IInteractionProvider>(s => (IWebInteractionProvider)s.GetService(typeof(IWebInteractionProvider)));
            services.AddSingleton<IDialogManager, DialogManager>();
            
            // toolbox
            services.AddSingleton(s => new Toolbox.ToolBox(
                Path.GetDirectoryName(new Uri(typeof(Startup).Assembly.EscapedCodeBase).LocalPath + "\\plugins"),
                (IInteractionProvider)s.GetService(typeof(IInteractionProvider))));
            
            // redis
            string host = "127.0.0.1";
            int port = 6379;
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect($"{host}:{port}"));
            services.AddSingleton(s => ((IConnectionMultiplexer) s.GetService(typeof(IConnectionMultiplexer))).GetServer(host, port));
            
            // data
            services.AddSingleton<IDataProvider, DataProvider>();
            
            // cache
            services.AddSingleton<IEntityCache, EntityCache>();
            
            // user management
            services.AddSingleton<ICredentialService, CredentialService>();
            
            // auth
            services.AddSingleton<IAuthService, AuthService>();
            
            // web socket based event pipeline
            services.AddSingleton<WsMiddleware>();
            services.AddSingleton<IMultiClientManager, MultiClientManager>();
            services.AddSingleton<ISocketDriver, SocketDriver>();
            
            services 
                 // bind interface models with dynamic proxies
                .AddMvc(config => config.ModelBinderProviders.Insert(0, new ProxyBinderProvider()))
                 // render no empty fields in json responses
                .AddJsonOptions(options => options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
            services.AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme)
                .AddBasic(options =>
                {
                    options.Realm = "idunno";
                    options.Events = new BasicAuthenticationEvents
                    {
                        OnValidateCredentials = context =>
                        {
                            IAuthService validationService =
                                context.HttpContext.RequestServices.GetService<IAuthService>();
                            
                            if (validationService.ValidateCredentials(context.Username, context.Password))
                            {
                                Claim[] claims = new[]
                                {
                                    new Claim(ClaimTypes.NameIdentifier, context.Username, ClaimValueTypes.String, context.Options.ClaimsIssuer),
                                    new Claim(ClaimTypes.Name, context.Username, ClaimValueTypes.String, context.Options.ClaimsIssuer)
                                };

                                context.Principal = new ClaimsPrincipal(new ClaimsIdentity(claims, context.Scheme.Name));
                                context.Success();
                            }

                            return Task.CompletedTask;
                        }
                    };
                });
            
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Desktop/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            
                        
            WebSocketOptions webSocketOptions = new WebSocketOptions() 
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            };
            
            webSocketOptions.AllowedOrigins.Add("https://localhost:5001");
            webSocketOptions.AllowedOrigins.Add("wss://localhost:5001");
            
            app.UseWebSockets(webSocketOptions);
            
            app.UseMiddleware<WsMiddleware>();
            
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseSession();
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Desktop}/{action=Index}/{id?}");
            });
        }
    }
}