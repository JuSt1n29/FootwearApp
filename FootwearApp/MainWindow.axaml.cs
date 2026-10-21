using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using FootwearApp.Repositories;
using FootwearApp.Tables;

namespace FootwearApp
{
    public partial class MainWindow : Window
    {
        private string currentTable = "Country";
        private int selectedId = -1;

        public MainWindow()
        {
            InitializeComponent();
            Console.WriteLine("[DEBUG] MainWindow ініціалізовано.");
            BuildInputFields("Country");
            LoadTableData("Country");
        }

        private void OnTableSelected(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                selectedId = -1;
                if (btn == BtnCountry) currentTable = "Country";
                else if (btn == BtnBrand) currentTable = "Brand";
                else if (btn == BtnCategory) currentTable = "Category";
                else if (btn == BtnMaterial) currentTable = "Material";
                else if (btn == BtnShoeType) currentTable = "ShoeType";
                else if (btn == BtnSole) currentTable = "Sole";
                else if (btn == BtnStyle) currentTable = "Style";
                else if (btn == BtnAudience) currentTable = "Audience";
                else if (btn == BtnCatalog) currentTable = "FootwearCatalog";

                Console.WriteLine($"[DEBUG] Змінено активну таблицю на: {currentTable}");

                BuildInputFields(currentTable);
                LoadTableData(currentTable);
            }
        }

        private void BuildInputFields(string tableName)
        {
            InputsPanel.Children.Clear();

            // Поле ID
            AddInputField("ID", "IdBox", isReadOnly: false);

            if (tableName == "FootwearCatalog")
            {
                AddInputField("Name", "NameBox");
                AddInputField("Price", "PriceBox");
                AddInputField("Color", "ColorBox");
            }
            else if (tableName == "Brand")
            {
                AddInputField("Name", "NameBox");
                AddInputField("Country ID", "CountryIdBox");
            }
            else
            {
                AddInputField("Name", "NameBox");
            }
            Console.WriteLine($"[DEBUG] Поля введення побудовано для таблиці: {tableName}");
        }

        private void AddInputField(string placeholder, string name, bool isReadOnly = false)
        {
            var panel = new StackPanel { Spacing = 2 };
            panel.Children.Add(new TextBlock { Text = placeholder, FontSize = 10, Foreground = Avalonia.Media.Brushes.Gray });
            
            var textBox = new TextBox { Name = name, Width = 100, Height = 28, IsReadOnly = isReadOnly };
            panel.Children.Add(textBox);
            
            InputsPanel.Children.Add(panel);
        }

        // Надійний пошук TextBox всередині динамічної панелі
        private TextBox? GetTextBoxByName(string name)
        {
            foreach (var child in InputsPanel.Children)
            {
                if (child is StackPanel sp)
                {
                    foreach (var subChild in sp.Children)
                    {
                        if (subChild is TextBox tb && tb.Name == name)
                        {
                            return tb;
                        }
                    }
                }
            }
            return null;
        }

        private void LoadTableData(string tableName)
        {
            try
            {
                MainDataGrid.ItemsSource = null;

                System.Collections.IEnumerable data = tableName switch
                {
                    "Country" => new CountryRep().GetAll(),
                    "Brand" => new BrandRep().GetAll(),
                    "Category" => new CategoryRep().GetAll(),
                    "Material" => new MaterialRep().GetAll(),
                    "ShoeType" => new ShoeTypeRep().GetAll(),
                    "Sole" => new SoleRep().GetAll(),
                    "Style" => new StyleRep().GetAll(),
                    "Audience" => new AudienceRep().GetAll(),
                    "FootwearCatalog" => new FootwearCatalogRep().GetAll(),
                    _ => null
                };

                MainDataGrid.ItemsSource = data;
                Console.WriteLine($"[DEBUG] Дані для таблиці {tableName} успішно завантажено в DataGrid.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Помилка завантаження даних для {tableName}: {ex.Message}");
            }
        }

        private void OnDataGridSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (MainDataGrid.SelectedItem == null) return;

            var item = MainDataGrid.SelectedItem;
            
            // Зчитуємо ID
            var idProp = item.GetType().GetProperty("id");
            if (idProp != null)
            {
                selectedId = (int)(idProp.GetValue(item) ?? -1);
                var idBox = GetTextBoxByName("IdBox");
                if (idBox != null) idBox.Text = selectedId.ToString();
            }

            // Зчитуємо Назву
            var nameProp = item.GetType().GetProperty("nazva") ?? item.GetType().GetProperty("name");
            if (nameProp != null)
            {
                string val = nameProp.GetValue(item)?.ToString() ?? "";
                var nameBox = GetTextBoxByName("NameBox");
                if (nameBox != null) nameBox.Text = val;
            }

            Console.WriteLine($"[DEBUG] Вибрано рядок: ID = {selectedId}");
        }

        private void OnAddClick(object? sender, RoutedEventArgs e)
        {
            var nameBox = GetTextBoxByName("NameBox");
            string nameVal = nameBox?.Text ?? "";

            Console.WriteLine($"[DEBUG] Спроба додавання запису в '{currentTable}' з ім'ям: '{nameVal}'");

            if (string.IsNullOrWhiteSpace(nameVal))
            {
                Console.WriteLine("[WARNING] Помилка: Поле 'Name' порожнє!");
                return;
            }

            bool success = false;
            try
            {
                success = currentTable switch
                {
                    "Country" => new CountryRep().Add(new CountryTable(0, nameVal)),
                    "Category" => new CategoryRep().Add(new CategoryTable(0, nameVal)),
                    "Material" => new MaterialRep().Add(new MaterialTable(0, nameVal)),
                    "ShoeType" => new ShoeTypeRep().Add(new ShoeTypeTable(0, nameVal)),
                    "Sole" => new SoleRep().Add(new SoleTable(0, nameVal)),
                    "Style" => new StyleRep().Add(new StyleTable(0, nameVal)),
                    "Audience" => new AudienceRep().Add(new AudienceTable(0, nameVal)),
                    "Brand" => AddBrandHelper(),
                    _ => false
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Виняток у репозиторії під час Add: {ex.Message}");
            }

            if (success)
            {
                Console.WriteLine("[DEBUG] Запис успішно додано!");
                LoadTableData(currentTable);
                ClearInputs();
            }
            else
            {
                Console.WriteLine("[ERROR] Не вдалося додати запис у базу даних.");
            }
        }

        private bool AddBrandHelper()
        {
            var nameBox = GetTextBoxByName("NameBox");
            var countryIdBox = GetTextBoxByName("CountryIdBox");
            int.TryParse(countryIdBox?.Text, out int cId);
            return new BrandRep().Add(new BrandTable(0, nameBox?.Text ?? "", cId));
        }

        private void OnUpdateClick(object? sender, RoutedEventArgs e)
        {
            var idBox = GetTextBoxByName("IdBox");
            if (int.TryParse(idBox?.Text, out int parsedId))
            {
                selectedId = parsedId;
            }

            Console.WriteLine($"[DEBUG] Спроба оновлення запису з ID = {selectedId} в таблиці '{currentTable}'");

            if (selectedId == -1)
            {
                Console.WriteLine("[WARNING] Помилка: ID не вказано або рядок не вибрано!");
                return;
            }

            var nameBox = GetTextBoxByName("NameBox");
            string nameVal = nameBox?.Text ?? "";

            bool success = false;
            try
            {
                success = currentTable switch
                {
                    "Country" => new CountryRep().Update(new CountryTable(selectedId, nameVal)),
                    "Category" => new CategoryRep().Update(new CategoryTable(selectedId, nameVal)),
                    "Material" => new MaterialRep().Update(new MaterialTable(selectedId, nameVal)),
                    "ShoeType" => new ShoeTypeRep().Update(new ShoeTypeTable(selectedId, nameVal)),
                    "Sole" => new SoleRep().Update(new SoleTable(selectedId, nameVal)),
                    "Style" => new StyleRep().Update(new StyleTable(selectedId, nameVal)),
                    "Audience" => new AudienceRep().Update(new AudienceTable(selectedId, nameVal)),
                    "Brand" => UpdateBrandHelper(selectedId),
                    _ => false
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Виняток у репозиторії під час Update: {ex.Message}");
            }

            if (success)
            {
                Console.WriteLine("[DEBUG] Запис успішно оновлено!");
                LoadTableData(currentTable);
                ClearInputs();
            }
            else
            {
                Console.WriteLine("[ERROR] Не вдалося оновити запис у базі даних.");
            }
        }

        private bool UpdateBrandHelper(int id)
        {
            var nameBox = GetTextBoxByName("NameBox");
            var countryIdBox = GetTextBoxByName("CountryIdBox");
            int.TryParse(countryIdBox?.Text, out int cId);
            return new BrandRep().Update(new BrandTable(id, nameBox?.Text ?? "", cId));
        }

        private void OnDeleteClick(object? sender, RoutedEventArgs e)
        {
            var idBox = GetTextBoxByName("IdBox");
            if (int.TryParse(idBox?.Text, out int parsedId))
            {
                selectedId = parsedId;
            }

            Console.WriteLine($"[DEBUG] Спроба видалення запису з ID = {selectedId} з таблиці '{currentTable}'");

            if (selectedId == -1)
            {
                Console.WriteLine("[WARNING] Помилка: ID для видалення не вказано!");
                return;
            }

            bool success = false;
            try
            {
                success = currentTable switch
                {
                    "Country" => new CountryRep().Delete(new CountryTable(selectedId, "")),
                    "Category" => new CategoryRep().Delete(new CategoryTable(selectedId, "")),
                    "Material" => new MaterialRep().Delete(new MaterialTable(selectedId, "")),
                    "ShoeType" => new ShoeTypeRep().Delete(new ShoeTypeTable(selectedId, "")),
                    "Sole" => new SoleRep().Delete(new SoleTable(selectedId, "")),
                    "Style" => new StyleRep().Delete(new StyleTable(selectedId, "")),
                    "Audience" => new AudienceRep().Delete(new AudienceTable(selectedId, "")),
                    "Brand" => new BrandRep().Delete(new BrandTable(selectedId, "", 0)),
                    _ => false
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Виняток у репозиторії під час Delete: {ex.Message}");
            }

            if (success)
            {
                Console.WriteLine("[DEBUG] Запис успішно видалено!");
                LoadTableData(currentTable);
                ClearInputs();
            }
            else
            {
                Console.WriteLine("[ERROR] Не вдалося видалити запис.");
            }
        }

        private void ClearInputs()
        {
            selectedId = -1;
            foreach (var child in InputsPanel.Children)
            {
                if (child is StackPanel sp)
                {
                    foreach (var subChild in sp.Children)
                    {
                        if (subChild is TextBox tb) tb.Text = string.Empty;
                    }
                }
            }
        }
    }
}