using EPiServer.DataAbstraction;

namespace Content_Cleaner.ViewModels
{
    public class ContentUsageBreadcrumb<TContent> : ContentUsage
    {
        public ContentUsageBreadcrumb(TContent content)
        {
            Content = content;
        }

        public TContent Content { get; }
        public string ContentUrl { get; set; }
        public string Breadcrumb { get; set; }
    }
}