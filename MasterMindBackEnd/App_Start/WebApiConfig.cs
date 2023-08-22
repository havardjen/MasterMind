using MasterMindDataAccess;
using MasterMindResources.Interfaces;
using MasterMindService;
using System.Web.Http;
using Unity;

namespace MasterMindBackEnd
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new UnityContainer();
            container.RegisterType<ICharactersService, CharactersService>();
            container.RegisterType<ICharactersRepository, CharactersRepository>();
            container.RegisterInstance<string>(@"Data Source=D:\OneDrive\Utvikling\MasterMind\MasterMindDB.db");
            config.DependencyResolver = new UnityResolver(container);

            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(name: "DefaultApi", routeTemplate: "api/{controller}/{id}", defaults: new { id = RouteParameter.Optional });
        }
    }
}
