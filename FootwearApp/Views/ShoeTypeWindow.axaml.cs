using Avalonia.Controls;
using Avalonia.Interactivity;
using FootwearApp.Repositories;
using FootwearApp.Tables;

namespace FootwearApp
{
    public partial class ShoeTypeWindow : Window
    {
        private ShoeTypeRep repo = new ShoeTypeRep();

        public ShoeTypeWindow()
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
            if (MainDataGrid.SelectedItem is ShoeTypeTable item)
            {
                NameBox.Text = item.nazva;
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
            if (string.IsNullOrEmpty(name)) return;
            int nextId = GetNextId();
            if (repo.Add(new ShoeTypeTable(nextId, name)))
            {
                LoadData();
                NameBox.Text = "";
                UpdateIdBox.Text = "";
            }
        }

        private void OnUpdateClick(object? sender, RoutedEventArgs e)
        {
            if (!int.TryParse(UpdateIdBox.Text, out int id) || id <= 0) return;
            string name = NameBox.Text?.Trim() ?? "";
            if (string.IsNullOrEmpty(name)) return;
            if (repo.Update(new ShoeTypeTable(id, name))) LoadData();
        }

        private void OnDeleteClick(object? sender, RoutedEventArgs e)
        {
            if (!int.TryParse(UpdateIdBox.Text, out int id) || id <= 0) return;
            if (repo.Delete(new ShoeTypeTable(id, "")))
            {
                LoadData();
                NameBox.Text = "";
                UpdateIdBox.Text = "";
            }
        }
    }
}