using EPiServer.Web;

namespace playground.Business
{
    public interface IDynamicTemplateContent
    {
        void SetDynamicTemplate(TemplateResolverEventArgs args);
    }
}
