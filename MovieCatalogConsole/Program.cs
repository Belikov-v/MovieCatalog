using System.Text;
using System.Text.Json;
using MovieCatalogConsole.service;
using MovieCatalogLibrary.model;
using MovieCatalogLibrary.service;

namespace MovieCatalogConsole;

public class Program
{
    private static readonly IFilmManager FilmManager = new InMemoryFilmManager();
    private static string _path = String.Empty;

    private static readonly Dictionary<int, (string Description, Action Action)> MenuItems = new()
    {
        { 1, ("Просмотра всех фильмов.", GetAllFilms) },
        { 2, ("Добавления нового фильма.", AddFilm) },
        { 3, ("Редактирования рейтинга фильма.", EditRatingFilm) },
        { 4, ("Удаления фильма.", DeleteFilm) },
        { 5, ("Выход.", Exit) }
    };

    public static void Main(string[] args)
    {
        Load();
        ShowMenu();
    }

    private static void Default()
    {
    }

    private static void GetAllFilms()
    {
        StringBuilder stringBuilder = new StringBuilder($"Каталог фильмов:");

        foreach (var film in FilmManager.GetAllFilms())
        {
            stringBuilder.Append($"\n {new string('-', 20)} \n");
            stringBuilder.Append(film);
            stringBuilder.Append($"\n {new string('-', 20)} \n");
        }

        Console.WriteLine(stringBuilder.ToString());
    }

    private static void EditRatingFilm()
    {
        int id = GetIdFilm();
        Film film = FilmManager.GetFilm(id);
        FilmBuilder.SetRating(film);
        FilmManager.EditFilm(film);
    }

    private static void DeleteFilm()
    {
        int id = GetIdFilm();
        FilmManager.DeleteFilm(id);
    }


    private static int GetIdFilm()
    {
        while (true)
        {
            Console.WriteLine("Введите id фильма.");
            try
            {
                int id = int.Parse(Console.ReadLine() ?? throw new ArgumentNullException());
                if (!FilmManager.IsIdExist(id)) throw new KeyNotFoundException();
                return id;
            }
            catch (FormatException e)
            {
                Console.WriteLine($"Ошибка: Некорректный формат ввода. (Детали: {e.Message})");
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine($"Ошибка: Входное значение не может быть null. (Детали: {e.Message})");
            }
            catch (OverflowException e)
            {
                Console.WriteLine($"Ошибка: Введенное значение выходит за допустимые пределы. (Детали: {e.Message})");
            }
            catch (KeyNotFoundException e)
            {
                Console.WriteLine($"Ошибка: Запрашиваемый ключ не найден. (Детали: {e.Message})");
            }
        }
    }

    private static void AddFilm()
    {
        Film film = FilmBuilder.BuildFilm();
        FilmManager.AddFilm(film);
    }

    private static void ShowMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Меню:");
            foreach (var item in MenuItems)
            {
                Console.WriteLine($"{item.Key}. {item.Value.Description}");
            }

            Console.WriteLine("Выберите пункт меню: ");
            if (int.TryParse(Console.ReadLine(), out int choice) && MenuItems.TryGetValue(choice, out var menuItem))
            {
                Console.Clear();
                menuItem.Action.Invoke();
            }
            else
            {
                Console.WriteLine("Неверный выбор.");
            }

            Console.WriteLine("\nНажмите любую клавишу, чтобы вернуться в меню...");
            Console.ReadKey();
        }
    }


    private static void Load()
    {
        while (true)
        {
            _path = FileManager.GetPathToFile();
            try
            {
                IEnumerable<Film> films = FileManager.ReadFile(_path);
                foreach (var film in films)
                {
                    FilmManager.AddFilm(film);
                }

                return;
            }

            catch (FileNotFoundException)
            {
                Console.WriteLine("Ошибка: Файл не найден.");
                _path = FileManager.GetPathToFile();
            }

            catch (IOException)
            {
                Console.WriteLine("Ошибка ввода-вывода.");
                _path = FileManager.GetPathToFile();
            }


            catch (JsonException)
            {
                Console.WriteLine("Ошибка при десериализации JSON.");
                _path = FileManager.GetPathToFile();
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message} Попробуйте загрузить в другой файл.");
                _path = FileManager.GetPathToFile();
            }
        }
    }

    private static void Exit()
    {
        List<Film> films = FilmManager.GetAllFilms().ToList();
        while (true)
        {
            try
            {
                string json = JsonParser.FilmsToJson(films);
                FileManager.WriteDateInFile(_path, json);
                Environment.Exit(0);
                return;
            }
            catch (NotSupportedException)
            {
                Console.WriteLine("Невозможно десериализовать JSON: тип не поддерживается.");
                _path = FileManager.GetPathToFile();
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Ошибка: Неверные символы.");
                _path = FileManager.GetPathToFile();
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Ошибка: Нет доступа к файлу.");
                _path = FileManager.GetPathToFile();
            }

            catch (FileNotFoundException)
            {
                Console.WriteLine("Ошибка: Файл не найден.");
                _path = FileManager.GetPathToFile();
            }

            catch (IOException)
            {
                Console.WriteLine("Ошибка ввода-вывода.");
                _path = FileManager.GetPathToFile();
            }


            catch (JsonException)
            {
                Console.WriteLine("Ошибка при десериализации JSON.");
                _path = FileManager.GetPathToFile();
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message} Попробуйте загрузить в другой файл.");
                _path = FileManager.GetPathToFile();
            }
        }
    }
}