using CommunityToolkit.Mvvm.ComponentModel;

namespace Part78.ViewModels
{
    public class GenreViewModel : ObservableObject
    {
        private string _title;

        public GenreViewModel(int id, string title)
        {
            Id = id;
            Title = title;
        }

        public int Id { get; }

        public string Title
        {
            get => _title;
            set
            {
                if (value == _title) return;
                _title = value;
                OnPropertyChanged();
            }
        }
    }
}
