namespace EditorX
{
    public class Container : Element
    {
        private Window _window;

        public Window window
        {
            get
            {
                return _window;
            }
            set
            {
                _window = value;
            }
        }

        protected override void PreGUI()
        {
        }

        protected override void OnGUI()
        {
            DrawChildren();
        }

        public override void RequestRepaint()
        {
            window.Repaint();
        }
    }
}