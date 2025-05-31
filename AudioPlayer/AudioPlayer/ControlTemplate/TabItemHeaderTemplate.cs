using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Media;

namespace AudioPlayer.ControlTemplate
{
    public class TabItemHeaderTemplate : IDataTemplate
    {
        // !!! BAD. The designer probably had nowhere to go to fix this implementation issue. How do you
        //          handle the template data / binding cycle?
        object? _theData = null;
        TabItem _control = null;
        TextBlock _controlTextBlock = null;

        string _header;

        // How to accomplish binding?
        public string Header
        {
            get { return _header; }
            set { _header = value; }
        }

        public TabItemHeaderTemplate()
        {
            this.Header = "Not Set";
        }

        public bool Match(object? data)
        {
            // Supposing there is one instance of the template per use (!!!) 
            return _theData == data;
        }

        public Control? Build(object? param)
        {
            _theData = param;

            if (_control == null)
            {
                _controlTextBlock = new TextBlock()
                {
                    Text = this.Header,
                    FontSize = 14,
                    FontWeight = FontWeight.Normal,
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
                };
                var panel = new StackPanel()
                {
                    Orientation = Avalonia.Layout.Orientation.Horizontal,
                    Children = { _controlTextBlock }
                };
                _control = new TabItem()
                {
                    Header = panel,
                    Cursor = new Cursor(StandardCursorType.Hand),
                    Padding = new Avalonia.Thickness(5, 0),
                    Background = Brushes.White
                };
            }
            else
            {
                _controlTextBlock.Text = this.Header;
            }

            return _control;
        }
    }
}
