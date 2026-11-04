using Avalonia.Controls;
using Avalonia.Interactivity;
using FootwearApp.Repositories;
using FootwearApp.Tables;

namespace FootwearApp
{
    public partial class FootwearCatalogWindow : Window
    {
        private FootwearCatalogRep repo = new FootwearCatalogRep();

        public FootwearCatalogWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            MainDataGrid.ItemsSource = null;
            MainDataGrid.ItemsSource = repo.GetAll();
        }

        private void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (MainDataGrid.SelectedItem is FootwearCatalogTable item)
            {
                NameBox.Text = item.nazva;
                PriceBox.Text = item.price.ToString();
                ColorBox.Text = item.color;
                UpdateIdBox.Text = item.id.ToString();
            }
        }

        private int GetNextId()
        {
            int maxId = 0;
            foreach (var item in repo.GetAll())
            {
                if (item.id > maxId) maxId = item.id;
            }
            return maxId + 1;
        }

        private void OnAddClick(object? sender, RoutedEventArgs e)
        {
            string name = NameBox.Text?.Trim() ?? "";
            if (string.IsNullOrEmpty(name) || !decimal.TryParse(PriceBox.Text, out decimal price)) return;

            int nextId = GetNextId();
            // Передаємо значення конструктора (з урахуванням дефолth-параметрів для додаткових полів)
            var newCatalog = new FootwearCatalogTable(nextId, name, 0, 0, 0, 0, 0, 0, ColorBox.Text ?? "", "", "", price, 0, 0, 0, "", "", "", 0, 0, "", 0);

            if (repo.Add(newCatalog))
            {
                LoadData();
                NameBox.Text = "";
                PriceBox.Text = "";
                ColorBox.Text = "";
                UpdateIdBox.Text = "";
            }
        }

        private void OnUpdateClick(object? sender, RoutedEventArgs e)
        {
            if (!int.TryParse(UpdateIdBox.Text, out int id) || id <= 0) return;
            string name = NameBox.Text?.Trim() ?? "";
            if (string.IsNullOrEmpty(name) || !decimal.TryParse(PriceBox.Text, out decimal price)) return;

            var updatedCatalog = new FootwearCatalogTable(id, name, 0, 0, 0, 0, 0, 0, ColorBox.Text ?? "", "", "", price, 0, 0, 0, "", "", "", 0, 0, "", 0);

            if (repo.Update(updatedCatalog)) LoadData();
        }

        private void OnDeleteClick(object? sender, RoutedEventArgs e)
        {
            if (!int.TryParse(UpdateIdBox.Text, out int id) || id <= 0) return;

            var emptyCatalog = new FootwearCatalogTable(id, "", 0, 0, 0, 0, 0, 0, "", "", "", 0, 0, 0, 0, "", "", "", 0, 0, "", 0);
            if (repo.Delete(emptyCatalog))
            {
                LoadData();
                NameBox.Text = "";
                PriceBox.Text = "";
                ColorBox.Text = "";
                UpdateIdBox.Text = "";
            }
        }
    }
}