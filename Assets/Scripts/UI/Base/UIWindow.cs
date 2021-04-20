namespace UI.Base
{
    public class UIWindow : UIView
    {
        public bool CloseOthers = true;
        
        
        public virtual void SetConfig(WindowConfig config) {}
    }
}