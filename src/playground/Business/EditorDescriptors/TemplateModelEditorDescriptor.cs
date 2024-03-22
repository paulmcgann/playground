using EPiServer.Framework.Web;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;
using EPiServer.Web.Routing;
using EPiServer.Cms.Shell.Extensions;
using EPiServer.ServiceLocation;

namespace playground.Business.EditorDescriptors
{
    [EditorDescriptorRegistration(TargetType = typeof(string), UIHint = "TemplateModel")]
    public class TemplateModelEditorDescriptor :EditorDescriptor
    {

        private readonly ITemplateRepository _templateRepository;

        public TemplateModelEditorDescriptor()
            : this(ServiceLocator.Current.GetInstance<ITemplateRepository>())
        {
        }

        public TemplateModelEditorDescriptor(ITemplateRepository templateRepository)
        {
            if (templateRepository == null)
                throw new ArgumentNullException("templateRepository");

            _templateRepository = templateRepository;
            ClientEditingClass = "epi-cms/contentediting/editors/SelectionEditor";
        }

        public override void ModifyMetadata(
            ExtendedMetadata metadata,
            IEnumerable<Attribute> attributes
        )
        {
            base.ModifyMetadata(metadata, attributes);

            metadata.CustomEditorSettings["uiType"] = metadata.ClientEditingClass;
            metadata.CustomEditorSettings["uiWrapperType"] = UiWrapperType.Floating;
            var contentType = metadata.FindOwnerContent()?.GetOriginalType();
            TemplateTypeCategories[] validTemplateTypeCategories;

            if (typeof(IRoutable).IsAssignableFrom(contentType))
            {
                validTemplateTypeCategories = new[] {
                TemplateTypeCategories.MvcController,
                TemplateTypeCategories.MvcView
            };
            }
            else
            {
                validTemplateTypeCategories = new[] {
                TemplateTypeCategories.MvcPartialComponent,
                TemplateTypeCategories.MvcPartialView
            };
            }

            var templateModels = _templateRepository
                .List(contentType)
                .Where(x => Array.IndexOf(validTemplateTypeCategories, x.TemplateTypeCategory) > -1);

            metadata.EditorConfiguration["selections"] = templateModels.Select(x => new SelectItem
            {
                Text = x.Name ?? x.TemplateType.Name,
                Value = x.TemplateType.FullName // Value stored in the database
            });
        }
    }
}
