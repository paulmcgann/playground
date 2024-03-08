using EPiServer.Shell.ObjectEditing.EditorDescriptors;
using EPiServer.Shell.ObjectEditing;
using static playground.Globals;

namespace playground.Business.EditorDescriptors
{
    [EditorDescriptorRegistration(TargetType = typeof(string), UIHint = CmsUiHints.IconPicker)]
    public class IconPickerEditorDescriptor : EditorDescriptor
    {
        public override void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
        {
            ClientEditingClass = "foundation/editors/IconPicker";

            base.ModifyMetadata(metadata, attributes);
        }
    }
}
