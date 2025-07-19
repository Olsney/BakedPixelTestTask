using Code.UI.View;

namespace Code.UI.Services.Factory
{
    public interface IUIFactory
    {
        void CreateUIRoot();
        void CreatePresenters(HudView hudView);
    }
}