using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace DiagnosticMargin
{
    public partial class ViewPanel : StackPanel, INotifyPropertyChanged, IDiagnosticPanel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly IWpfTextView textView;
        private string contentTypeText;
        private string rolesText;
        private string viewportLeftText;
        private string viewportTopText;
        private string caretTopText;
        private string caretLineTopText;
        private string layoutText;
        private string layoutNewText;
        private string layoutTransText;

        private int layoutCount = 0;

        public ViewPanel(IWpfTextView textView)
        {
            InitializeComponent();
            DataContext = this;

            this.textView = textView;

            OnContentTypeChanged(null, null);

            StringBuilder builder = new StringBuilder();
            foreach (string role in textView.Roles)
            {
                builder.Append(role);
                builder.Append("  ");
            }
            this.rolesText = builder.ToString();

            OnLayoutChanged(null, null);
        }

        public string ContentTypeText
        {
            get { return this.contentTypeText; }
            set
            {
                this.contentTypeText = value;
                OnPropertyChanged("ContentTypeText");
            }
        }

        public string RolesText
        {
            get { return this.rolesText; }
        }

        public string ViewportLeftText
        {
            get { return this.viewportLeftText; }
            set
            {
                if (value != this.viewportLeftText)
                {
                    this.viewportLeftText = value;
                    OnPropertyChanged("ViewportLeftText");
                }
            }
        }

        public string ViewportTopText
        {
            get { return this.viewportTopText; }
            set
            {
                if (value != this.viewportTopText)
                {
                    this.viewportTopText = value;
                    OnPropertyChanged("ViewportTopText");
                }
            }
        }

        public string CaretTopText
        {
            get { return this.caretTopText; }
            set
            {
                if (value != this.caretTopText)
                {
                    this.caretTopText = value;
                    OnPropertyChanged("CaretTopText");
                }
            }
        }

        public string CaretLineTopText
        {
            get { return this.caretLineTopText; }
            set
            {
                if (value != this.caretLineTopText)
                {
                    this.caretLineTopText = value;
                    OnPropertyChanged("CaretLineTopText");
                }
            }
        }

        public string LayoutText
        {
            get { return this.layoutText; }
            set
            {
                this.layoutText = value;
                OnPropertyChanged("LayoutText");
            }
        }

        public string LayoutNewText
        {
            get { return this.layoutNewText; }
            set
            {
                this.layoutNewText = value;
                OnPropertyChanged("LayoutNewText");
            }
        }

        public string LayoutTransText
        {
            get { return this.layoutTransText; }
            set
            {
                if (value != this.layoutTransText)
                {
                    this.layoutTransText = value;
                    OnPropertyChanged("LayoutTransText");
                }
            }
        }

        void PropertiesButtonClick(object sender, RoutedEventArgs e)
        {
            Window win = new Window()
            {
                Content = PropertyDumper.PropertyDisplay(this.textView),
                Title = "View property bag",
                Height = 500,
                Width = 800
            };
            win.ShowDialog();
        }

        void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            LayoutText = (++this.layoutCount).ToString();
            if (e != null)
            {
                LayoutNewText = e.NewOrReformattedLines.Count.ToString();
                LayoutTransText = e.TranslatedLines.Count.ToString();
            }
            else
            {
                LayoutNewText = "0";
                LayoutTransText = "0";
            }

            ViewportLeftText = this.textView.ViewportLeft.ToString();
            ViewportTopText = this.textView.ViewportTop.ToString();
            CaretTopText = this.textView.Caret.Top.ToString();
            CaretLineTopText = this.textView.Caret.ContainingTextViewLine.Top.ToString();
        }

        private void OnCaretPositionChanged(object sender, CaretPositionChangedEventArgs e)
        {
            CaretTopText = this.textView.Caret.Top.ToString();
            CaretLineTopText = this.textView.Caret.ContainingTextViewLine.Top.ToString();
        }

        void OnContentTypeChanged(object sender, TextDataModelContentTypeChangedEventArgs args)
        {
            ContentTypeText = this.textView.TextDataModel.ContentType.DisplayName;
        }

        public UIElement UI
        {
            get { return this; }
        }

        public void Activate()
        {
            this.textView.TextDataModel.ContentTypeChanged += OnContentTypeChanged;
            this.textView.LayoutChanged += OnLayoutChanged;
            this.textView.Caret.PositionChanged += OnCaretPositionChanged;
        }

        public void Inactivate()
        {
            this.textView.TextDataModel.ContentTypeChanged -= OnContentTypeChanged;
            this.textView.LayoutChanged -= OnLayoutChanged;
            this.textView.Caret.PositionChanged -= OnCaretPositionChanged;
        }

        public void Close()
        {
            Inactivate();
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
