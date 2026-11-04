using Avalonia.Controls;
using Avalonia.Interactivity;
using FootwearApp.Repositories;
using FootwearApp.Tables;

namespace FootwearApp
{
    public partial class BrandWindow : Window
    {
        private BrandRep repo = new BrandRep();

        public BrandWindow()
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
            if (MainDataGrid.SelectedItem is BrandTable item)
            {
                NameBox.Text = item.nazva;
                CountryIdBox.Text = item.id_country.ToString();
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
            if (string.IsNullOrEmpty(name) || !int.TryParse(CountryIdBox.Text, out int countryId)) return;

            int nextId = GetNextId();
            if (repo.Add(new BrandTable(nextId, name, countryId)))
            {
                LoadData();
                NameBox.Text = "";
                CountryIdBox.Text = "";
                UpdateIdBox.Text = "";
            }
        }

        private void OnUpdateClick(object? sender, RoutedEventArgs e)
        {
            if (!int.TryParse(UpdateIdBox.Text, out int id) || id <= 0) return;
            string name = NameBox.Text?.Trim() ?? "";
            if (string.IsNullOrEmpty(name) || !int.TryParse(CountryIdBox.Text, out int countryId)) return;

            if (repo.Update(new BrandTable(id, name, countryId))) LoadData();
        }

        private void OnDeleteClick(object? sender, RoutedEventArgs e)
        {
            if (!int.TryParse(UpdateIdBox.Text, out int id) || id <= 0) return;

            if (repo.Delete(new BrandTable(id, "", 0)))
            {
                LoadData();
                NameBox.Text = "";
                CountryIdBox.Text = "";
                UpdateIdBox.Text = "";
            }
        }
    }
}