using System.Collections.ObjectModel;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace tododdd
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new TodoListViewModel();
        }
    }

    public class TodoItem : INotifyPropertyChanged
    {
        private bool _isCompleted;

        public required string Text { get; set; }
        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                if (_isCompleted != value)
                {
                    _isCompleted = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


    public class TodoListViewModel : INotifyPropertyChanged
    {
        public TodoListViewModel()
        {
            Items = new ObservableCollection<TodoItem> { new TodoItem { Text = "Сделать мобилки" } };
            AddCommand = new Command<string>(AddItem, CanAddItem);
            RemoveCommand = new Command<TodoItem>(RemoveItem);
        }

        public ObservableCollection<TodoItem> Items { get; }

        private string _newItemText;
        public string NewItemText
        {
            get => _newItemText;
            set
            {
                if (SetField(ref _newItemText, value))
                {
                    ((Command)AddCommand).ChangeCanExecute();
                }
            }
        }

        public ICommand AddCommand { get; }
        private void AddItem(string itemText)
        {
            if (!string.IsNullOrWhiteSpace(itemText))
            {
                Items.Add(new TodoItem { Text = itemText });
                NewItemText = string.Empty;
            }
        }

        private bool CanAddItem(string itemText) => !string.IsNullOrWhiteSpace(itemText);

        public ICommand RemoveCommand { get; }
        private void RemoveItem(TodoItem item)
        {
            if (Items.Contains(item))
            {
                Items.Remove(item);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }

}