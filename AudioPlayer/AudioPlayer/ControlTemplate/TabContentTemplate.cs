using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.Media;
using Avalonia.Media.Immutable;

namespace AudioPlayer.ControlTemplate
{
    public class TabContentTemplate : IDataTemplate
    {
        Border _border = null;
        ImmutableSolidColorBrush _background = new ImmutableSolidColorBrush(Colors.Transparent);

        public ImmutableSolidColorBrush ContentBackground
        {
            get { return _background; }
            set { _background = value; }
        }

        public Control Build(object view)
        {
            if (_border == null)
            {
                _border = new Border();
                _border.Child = new ContentPresenter()
                {
                    Content = view
                };
                _border.Background = _background;
                _border.Margin = new Thickness(-6,0,-6,0);
                _border.Padding = new Thickness(0);
            }
            else
            {
                (_border.Child as ContentPresenter).Content = view;
            }

            return _border;
        }

        public bool Match(object data)
        {
            return true;
        }
    }
}
