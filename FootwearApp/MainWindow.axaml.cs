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

            // 1. Поля для введення даних при додаванні
            AddInputField("Name", "NameBox");

            if (tableName == "FootwearCatalog")
            {
                AddInputField("Price", "PriceBox");
                AddInputField("Color", "ColorBox");
            }
            else if (tableName == "Brand")
            {
                AddInputField("Country ID", "CountryIdBox");
            }

            // 2. Поле для оновлення та видалення за ID
            AddInputField("Record ID (Update/Delete)", "UpdateIdBox");

            Console.WriteLine($"[DEBUG] Поля побудовано для: {tableName}");
        }

        private void AddInputField(string placeholder, string name)
        {
            var panel = new StackPanel { Spacing = 2 };
            panel.Children.Add(new TextBlock { Text = placeholder, FontSize = 10, Foreground = Avalonia.Media.Brushes.Gray });
            
            var textBox = new TextBox { Name = name, Width = 120, Height = 28 };
            panel.Children.Add(textBox);
            
            InputsPanel.Children.Add(panel);
        }

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
                Console.WriteLine($"[DEBUG] Дані для таблиці {tableName} успішно завантажено.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Помилка завантаження: {ex.Message}");
            }
        }

        private void OnDataGridSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (MainDataGrid.SelectedItem == null) return;

            var item = MainDataGrid.SelectedItem;
            var type = item.GetType();
            
            // Автоматично підтягуємо назву в NameBox (враховуємо і властивості, і поля)
            var nameVal = GetValueFromItem(item, "nazva") ?? GetValueFromItem(item, "name") ?? GetValueFromItem(item, "Name");
            if (nameVal != null)
            {
                var nameBox = GetTextBoxByName("NameBox");
                if (nameBox != null) nameBox.Text = nameVal.ToString();
            }

            // Автоматично підтягуємо ID в UpdateIdBox (враховуємо і id, і Id)
            var idVal = GetValueFromItem(item, "id") ?? GetValueFromItem(item, "Id");
            if (idVal != null)
            {
                var updateIdBox = GetTextBoxByName("UpdateIdBox");
                if (updateIdBox != null) updateIdBox.Text = idVal.ToString();
            }
        }

        private object? GetValueFromItem(object item, string memberName)
        {
            var type = item.GetType();
            var prop = type.GetProperty(memberName);
            if (prop != null) return prop.GetValue(item);

            var field = type.GetField(memberName);
            if (field != null) return field.GetValue(item);

            return null;
        }

        // ДОДАВАННЯ (З виправленим надійним визначенням наступного ID через Property або Field)
        private void OnAddClick(object? sender, RoutedEventArgs e)
        {
            var nameBox = GetTextBoxByName("NameBox");
            string nameVal = nameBox?.Text?.Trim() ?? "";

            Console.WriteLine($"[DEBUG UI] Спроба додавання в '{currentTable}' з ім'ям: '{nameVal}'");

            if (string.IsNullOrWhiteSpace(nameVal))
            {
                Console.WriteLine("[WARNING] Поле 'Name' порожнє!");
                return;
            }

            bool success = false;
            try
            {
                int nextId = GetNextIdForTable(currentTable);

                success = currentTable switch
                {
                    "Country" => new CountryRep().Add(new CountryTable(nextId, nameVal)),
                    "Category" => new CategoryRep().Add(new CategoryTable(nextId, nameVal)),
                    "Material" => new MaterialRep().Add(new MaterialTable(nextId, nameVal)),
                    "ShoeType" => new ShoeTypeRep().Add(new ShoeTypeTable(nextId, nameVal)),
                    "Sole" => new SoleRep().Add(new SoleTable(nextId, nameVal)),
                    "Style" => new StyleRep().Add(new StyleTable(nextId, nameVal)),
                    "Audience" => new AudienceRep().Add(new AudienceTable(nextId, nameVal)),
                    "Brand" => AddBrandHelper(nextId, nameVal),
                    "FootwearCatalog" => AddCatalogHelper(nextId, nameVal),
                    _ => false
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Помилка Add: {ex.Message}");
            }

            if (success)
            {
                Console.WriteLine("[DEBUG UI] Успішно додано!");
                LoadTableData(currentTable);
                ClearInputs();
            }
            else
            {
                Console.WriteLine("[ERROR] Не вдалося додати запис.");
            }
        }

        private int GetNextIdForTable(string tableName)
        {
            int maxId = 0;

            try
            {
                System.Collections.IEnumerable? data = tableName switch
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

                if (data != null)
                {
                    foreach (var item in data)
                    {
                        if (item == null) continue;

                        int currentId = 0;
                        var idObj = GetValueFromItem(item, "id") ?? GetValueFromItem(item, "Id");

                        if (idObj != null && int.TryParse(idObj.ToString(), out int parsedId))
                        {
                            currentId = parsedId;
                        }

                        if (currentId > maxId)
                        {
                            maxId = currentId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Помилка при обчисленні NextId: {ex.Message}");
            }

            int nextId = maxId + 1;
            Console.WriteLine($"[DEBUG] Знайдений максимальний ID для {tableName}: {maxId}, наступний буде: {nextId}");
            return nextId;
        }

        private bool AddBrandHelper(int idVal, string nameVal)
        {
            var countryIdBox = GetTextBoxByName("CountryIdBox");
            if (!int.TryParse(countryIdBox?.Text, out int cId)) return false;
            return new BrandRep().Add(new BrandTable(idVal, nameVal, cId));
        }

        private bool AddCatalogHelper(int idVal, string nameVal)
        {
            var priceBox = GetTextBoxByName("PriceBox");
            var colorBox = GetTextBoxByName("ColorBox");
            if (!decimal.TryParse(priceBox?.Text, out decimal price)) return false;
            
            return new FootwearCatalogRep().Add(new FootwearCatalogTable(idVal, nameVal, 0, 0, 0, 0, 0, 0, "", "", "", price, 0, 0, 0, "", "", "", 0, 0, colorBox?.Text ?? "", 0));
        }

        // ОНОВЛЕННЯ ЗА ID
        private void OnUpdateClick(object? sender, RoutedEventArgs e)
        {
            var updateIdBox = GetTextBoxByName("UpdateIdBox");
            var nameBox = GetTextBoxByName("NameBox");

            if (updateIdBox == null || !int.TryParse(updateIdBox.Text, out int idVal) || idVal <= 0)
            {
                Console.WriteLine("[WARNING] Введіть коректний ID у полі 'Record ID' для оновлення!");
                return;
            }

            string nameVal = nameBox?.Text?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(nameVal))
            {
                Console.WriteLine("[WARNING] Поле 'Name' порожнє!");
                return;
            }

            bool success = false;
            try
            {
                success = currentTable switch
                {
                    "Country" => new CountryRep().Update(new CountryTable(idVal, nameVal)),
                    "Category" => new CategoryRep().Update(new CategoryTable(idVal, nameVal)),
                    "Material" => new MaterialRep().Update(new MaterialTable(idVal, nameVal)),
                    "ShoeType" => new ShoeTypeRep().Update(new ShoeTypeTable(idVal, nameVal)),
                    "Sole" => new SoleRep().Update(new SoleTable(idVal, nameVal)),
                    "Style" => new StyleRep().Update(new StyleTable(idVal, nameVal)),
                    "Audience" => new AudienceRep().Update(new AudienceTable(idVal, nameVal)),
                    "Brand" => new BrandRep().Update(new BrandTable(idVal, nameVal, 0)),
                    "FootwearCatalog" => new FootwearCatalogRep().Update(new FootwearCatalogTable(idVal, nameVal, 0, 0, 0, 0, 0, 0, "", "", "", 0, 0, 0, 0, "", "", "", 0, 0, "", 0)),
                    _ => false
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Помилка Update: {ex.Message}");
            }

            if (success)
            {
                Console.WriteLine("[DEBUG UI] Запис успішно оновлено за ID!");
                LoadTableData(currentTable);
                ClearInputs();
            }
            else
            {
                Console.WriteLine("[ERROR] Не вдалося оновити запис.");
            }
        }

        // ВИДАЛЕННЯ ЗА ID
        private void OnDeleteClick(object? sender, RoutedEventArgs e)
        {
            var updateIdBox = GetTextBoxByName("UpdateIdBox");

            if (updateIdBox == null || !int.TryParse(updateIdBox.Text, out int idVal) || idVal <= 0)
            {
                Console.WriteLine("[WARNING] Введіть коректний ID у полі 'Record ID' для видалення!");
                return;
            }

            bool success = false;
            try
            {
                success = currentTable switch
                {
                    "Country" => new CountryRep().Delete(new CountryTable(idVal, "")),
                    "Category" => new CategoryRep().Delete(new CategoryTable(idVal, "")),
                    "Material" => new MaterialRep().Delete(new MaterialTable(idVal, "")),
                    "ShoeType" => new ShoeTypeRep().Delete(new ShoeTypeTable(idVal, "")),
                    "Sole" => new SoleRep().Delete(new SoleTable(idVal, "")),
                    "Style" => new StyleRep().Delete(new StyleTable(idVal, "")),
                    "Audience" => new AudienceRep().Delete(new AudienceTable(idVal, "")),
                    "Brand" => new BrandRep().Delete(new BrandTable(idVal, "", 0)),
                    "FootwearCatalog" => new FootwearCatalogRep().Delete(new FootwearCatalogTable(idVal, "", 0, 0, 0, 0, 0, 0, "", "", "", 0, 0, 0, 0, "", "", "", 0, 0, "", 0)),
                    _ => false
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Помилка Delete: {ex.Message}");
            }

            if (success)
            {
                Console.WriteLine("[DEBUG UI] Запис успішно видалено за ID!");
                LoadTableData(currentTable);
                ClearInputs();
            }
            else
            {
                Console.WriteLine("[ERROR] Не вдалося видалити. Перевір, чи існує такий ID.");
            }
        }

        private void ClearInputs()
        {
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