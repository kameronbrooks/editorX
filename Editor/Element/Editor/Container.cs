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

        public override string tag
        {
            get
            {
                return "container";
            }
        }

        protected override void PreGUI()
        {
        }

        protected override void OnGUI()
        {
            DrawChildren();
        }
    }
}