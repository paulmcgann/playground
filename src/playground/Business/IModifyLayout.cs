using playground.Models.ViewModels;

namespace playground.Business
{
    /// <summary>
    /// Defines a method which may be invoked by PageContextActionFilter allowing controllers
    /// to modify common layout properties of the view model.
    /// </summary>
    internal interface IModifyLayout
    {
        void ModifyLayout(LayoutModel layoutModel);
    }
}
