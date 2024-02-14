using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Part78.ViewModels
{
    public class FilmEditorViewModel : ObservableValidator
    {
        private string _selectedGenre;
        private string _title;
        private string _yearString;
        private string _durationString;

        public FilmEditorViewModel(IEnumerable<string> genres, FilmViewModel existingFilm = null)
        {
            Genres = new List<string>(genres);
            _selectedGenre = existingFilm is null
                ? Genres.FirstOrDefault()
                : Genres.FirstOrDefault(g => g == existingFilm.FilmGenreLabel) ?? Genres.FirstOrDefault();
            IsDataValid = false;
            WindowTitle = existingFilm is null ? "Окно создания нового фильма" : "Окно редактирования фильма";
            Title = existingFilm is null ? "Новый фильм" : existingFilm.FilmName;
            YearString = existingFilm is null ? DateTime.Today.Year.ToString() : existingFilm.FilmYear.ToString();
            DurationString = existingFilm is null ? ((int)default).ToString() : existingFilm.FilmDuration.ToString();
            ErrorsChanged += OnErrorsChanged;
        }

        private void OnErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            OnPropertyChanged(nameof(RunSaveDataCommand));
        }

        public bool IsDataValid { get; private set; }

        public RelayCommand<object> RunSaveDataCommand => new RelayCommand<object>(SaveAndClose, o => !HasErrors);

        private void SaveAndClose(object o)
        {
            IsDataValid = true;
            if (o is Window window)
                window.Close();
        }

        public List<string> Genres { get; }

        public string WindowTitle { get; }

        public string SelectedGenre
        {
            get => _selectedGenre;
            set => SetProperty(ref _selectedGenre, value);
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле не может быть пустым")]
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value, true);
        }

        [CustomValidation(typeof(FilmEditorViewModel), nameof(ValidateYear))]
        public string YearString
        {
            get => _yearString;
            set => SetProperty(ref _yearString, value, true);
        }

        [CustomValidation(typeof(FilmEditorViewModel), nameof(ValidateDuration))]
        public string DurationString
        {
            get => _durationString;
            set => SetProperty(ref _durationString, value, true);
        }

        public int Year => int.TryParse(YearString, out var parseResult) ? parseResult : default;
        public int Duration => int.TryParse(DurationString, out var parseResult) ? parseResult : default;

        public static ValidationResult ValidateYear(string stringYear)
        {
            var error = string.Empty;
            var parseResult = int.TryParse(stringYear, out var year);
            if (!parseResult)
                error = "Введите число";
            else if (year > DateTime.Today.Year)
                error = "Год не может быть больше текущего";
            else if (year < 0)
                error = "Год не может быть отрицательным";

            return string.IsNullOrEmpty(error) ? ValidationResult.Success : new ValidationResult(error);
        }

        public static ValidationResult ValidateDuration(string stringDuration)
        {
            var error = string.Empty;
            var parseResult = int.TryParse(stringDuration, out var duration);
            if (!parseResult)
                error = "Введите число";
            else if (duration < 0)
                error = "Продолжительность фильма не может быть отрицательной";

            return string.IsNullOrEmpty(error) ? ValidationResult.Success : new ValidationResult(error);
        }
    }
}
