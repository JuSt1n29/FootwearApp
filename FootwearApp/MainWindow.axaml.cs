using Avalonia.Controls;
using Avalonia.Interactivity;

namespace FootwearApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenCountryWin(object? sender, RoutedEventArgs e) => new CountryWindow().Show();
        private void OpenCategoryWin(object? sender, RoutedEventArgs e) => new CategoryWindow().Show();
        private void OpenMaterialWin(object? sender, RoutedEventArgs e) => new MaterialWindow().Show();
        private void OpenShoeTypeWin(object? sender, RoutedEventArgs e) => new ShoeTypeWindow().Show();
        private void OpenSoleWin(object? sender, RoutedEventArgs e) => new SoleWindow().Show();
        private void OpenStyleWin(object? sender, RoutedEventArgs e) => new StyleWindow().Show();
        private void OpenAudienceWin(object? sender, RoutedEventArgs e) => new AudienceWindow().Show();
        private void OpenBrandWin(object? sender, RoutedEventArgs e) => new BrandWindow().Show();
        private void OpenCatalogWin(object? sender, RoutedEventArgs e) => new FootwearCatalogWindow().Show();
    }
}