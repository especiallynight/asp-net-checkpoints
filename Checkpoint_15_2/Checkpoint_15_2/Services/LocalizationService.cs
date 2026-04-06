namespace Checkpoint_15_2.Services
{

    public class LocalizationService
    {
        public string CurrentLanguage { get; private set; } = "RU";

        public event Action? OnLanguageChanged;
        public void SetLanguage(string lang)
        {
            if (CurrentLanguage != lang)
            {
                CurrentLanguage = lang;
                OnLanguageChanged?.Invoke();
            }
        }

        public string GetText(string key)
        {
            return (CurrentLanguage, key) switch
            {
                ("RU", "Welcome") => "Добро пожаловать",
                ("EN", "Welcome") => "Welcome",
                ("RU", "Logout") => "Выйти",
                ("EN", "Logout") => "Logout",
                ("RU", "NotFoundTitle") => "404 - Страница не найдена",
                ("EN", "NotFoundTitle") => "404 - Page Not Found",
                ("RU", "GoToHome") => "Домой",
                ("EN", "GoToHome") => "Go to home",
                ("RU", "Details") => "Детали",
                ("EN", "Details") => "Details",
                ("RU", "BackToProducts") => "Назад к продуктам",
                ("EN", "BackToProducts") => "Back to products",
                ("RU", "ProductList") => "Список продуктов",
                ("EN", "ProductList") => "Product List",
                ("RU", "ServiceList") => "Список услуг",
                ("EN", "ServiceList") => "Service List",
                ("RU", "UnknownType") => "Неизвестный тип",
                ("EN", "UnknownType") => "Unknown Type",
                ("RU", "LogRouteChanged") => "Пользователь перешел на",
                ("EN", "LogRouteChanged") => "User navigated to",
                ("RU", "NavigatedTo") => "Переход на",
                ("EN", "NavigatedTo") => "Navigated to",
                ("RU", "Products") => "Продукты",
                ("EN", "Products") => "Products",
                ("RU", "All") => "Все",
                ("EN", "All") => "All",
                ("RU", "GoToDetails") => "Перейти к деталям",
                ("EN", "GoToDetails") => "Go to details",
                _ => key 
            };
        }
    }
}
