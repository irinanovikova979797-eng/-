// MainWindow.xaml.cs
using SearchAlgorithmsDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SearchAlgorithmsDemo
{
    public partial class MainWindow : Window
    {
        private List<Movie> movies = new List<Movie>();
        private List<Movie> sortedMovies = new List<Movie>();

        public MainWindow()
        {
            InitializeComponent();
            LoadMovies();
            MoviesListView.ItemsSource = movies;
        }

        private void LoadMovies()
        {
            movies = new List<Movie>
            {
                new Movie { Title = "Интерстеллар", Director = "Кристофер Нолан", ReleaseYear = 2014, Rating = 8.6 },
                new Movie { Title = "Начало", Director = "Кристофер Нолан", ReleaseYear = 2010, Rating = 8.7 },
                new Movie { Title = "Темный рыцарь", Director = "Кристофер Нолан", ReleaseYear = 2008, Rating = 9.0 },
                new Movie { Title = "Побег из Шоушенка", Director = "Фрэнк Дарабонт", ReleaseYear = 1994, Rating = 9.3 },
                new Movie { Title = "Крестный отец", Director = "Фрэнсис Форд Коппола", ReleaseYear = 1972, Rating = 9.2 },
                new Movie { Title = "Форрест Гамп", Director = "Роберт Земекис", ReleaseYear = 1994, Rating = 8.8 },
                new Movie { Title = "Криминальное чтиво", Director = "Квентин Тарантино", ReleaseYear = 1994, Rating = 8.9 },
                new Movie { Title = "Бойцовский клуб", Director = "Дэвид Финчер", ReleaseYear = 1999, Rating = 8.8 },
                new Movie { Title = "Матрица", Director = "Лана Вачовски", ReleaseYear = 1999, Rating = 8.7 },
                new Movie { Title = "Властелин колец", Director = "Питер Джексон", ReleaseYear = 2003, Rating = 8.9 }
            };

            sortedMovies = movies.OrderBy(m => m.Title).ToList();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            PerformSearch();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PerformSearch();
        }

        private void SearchTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PerformSearch();
        }

        private void PerformSearch()
        {
            string searchText = SearchTextBox.Text.ToLower();
            
            if (string.IsNullOrWhiteSpace(searchText))
            {
                MoviesListView.ItemsSource = movies;
                ResultsTextBlock.Text = $"Всего фильмов: {movies.Count}";
                return;
            }

            var searchType = (SearchTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            List<Movie> results;
            int iterations = 0;

            if (searchType == "Бинарный поиск")
            {
                results = BinarySearch(searchText, ref iterations);
            }
            else
            {
                results = LinearSearch(searchText, ref iterations);
            }

            MoviesListView.ItemsSource = results;
            ResultsTextBlock.Text = $"Найдено: {results.Count} | Итераций: {iterations} | Метод: {searchType}";
        }

        private List<Movie> LinearSearch(string searchText, ref int iterations)
        {
            var results = new List<Movie>();
            iterations = 0;

            foreach (var movie in movies)
            {
                iterations++;
                if (movie.Title.ToLower().Contains(searchText) ||
                    movie.Director.ToLower().Contains(searchText) ||
                    movie.ReleaseYear.ToString().Contains(searchText) ||
                    movie.Rating.ToString().Contains(searchText))
                {
                    results.Add(movie);
                }
            }

            return results;
        }

        private List<Movie> BinarySearch(string searchText, ref int iterations)
        {
            var results = new List<Movie>();
            iterations = 0;

            int left = 0;
            int right = sortedMovies.Count - 1;

            while (left <= right)
            {
                iterations++;
                int mid = (left + right) / 2;
                int comparison = string.Compare(sortedMovies[mid].Title, searchText, StringComparison.OrdinalIgnoreCase);

                if (sortedMovies[mid].Title.ToLower().Contains(searchText))
                {
                    // Нашли совпадение
                    results.Add(sortedMovies[mid]);
                    
                    // Ищем другие совпадения
                    for (int i = mid - 1; i >= 0 && sortedMovies[i].Title.ToLower().Contains(searchText); i--)
                        results.Add(sortedMovies[i]);
                    for (int i = mid + 1; i < sortedMovies.Count && sortedMovies[i].Title.ToLower().Contains(searchText); i++)
                        results.Add(sortedMovies[i]);
                    
                    break;
                }
                else if (comparison < 0)
                {
                    left = mid + 1;
                }
                else
                {
                    right = mid - 1;
                }
            }

            return results;
        }
    }
}
