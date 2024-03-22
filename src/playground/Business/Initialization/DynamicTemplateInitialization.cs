using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Web;

namespace playground.Business.Initialization
{
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class DynamicTemplateInitialization : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            var events = ServiceLocator.Current.GetInstance<ITemplateResolverEvents>();
            events.TemplateResolved += OnTemplateResolved;
        }

        public void Uninitialize(InitializationEngine context)
        {
            ServiceLocator.Current.GetInstance<TemplateResolver>().TemplateResolved -= OnTemplateResolved;
        }

        private static void OnTemplateResolved(object sender, TemplateResolverEventArgs args)
        {
            var content = args.ItemToRender as IDynamicTemplateContent;

            if (content != null)
            {
                content.SetDynamicTemplate(args);
            }
        }
    }
}
