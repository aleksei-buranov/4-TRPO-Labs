using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Part78.ViewModels
{
    internal class GenreEditorViewModel : ObservableValidator
    {
        private string _title;

        public GenreEditorViewModel(GenreViewModel existingGenre = null)
        {
            IsDataValid = false;
            WindowTitle = existingGenre is null ? "Окно создания нового жанра" : "Окно редактирования жанра";
            Title = existingGenre is null ? "Новый жанр" : existingGenre.Title;
            ErrorsChanged += OnErrorsChanged;
        }

        private void OnErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            OnPropertyChanged(nameof(RunSaveDataCommand));
        }

        public RelayCommand<object> RunSaveDataCommand => new RelayCommand<object>(SaveAndClose, o => !HasErrors);

        private void SaveAndClose(object o)
        {
            IsDataValid = true;
            if (o is Window window)
                window.Close();
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле не может быть пустым")]
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value, true);
        }

        public string WindowTitle { get; }

        public bool IsDataValid { get; set; }
    }
}
