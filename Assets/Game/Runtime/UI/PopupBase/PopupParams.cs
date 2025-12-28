namespace Game.Runtime.UI.PopupBase
{
    internal abstract class PopupParams
    {
        public string ContentName { get; private set; }
        public PopupParams(string contentName)
        {
            ContentName = contentName;
        }
    }
}
