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

            // 1. Поля для основних даних (Name, Price, Color тощо)
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

            // 2. Поле виключно для ОНОВЛЕННЯ за ID
            AddInputField("Update ID", "UpdateIdBox");

            Console.WriteLine($"[DEBUG] Поля побудовано для: {tableName}");
        }

        private void AddInputField(string placeholder, string name)
        {
            var panel = new StackPanel { Spacing = 2 };
            panel.Children.Add(new TextBlock { Text = placeholder, FontSize = 10, Foreground = Avalonia.Media.Brushes.Gray });
            
            var textBox = new TextBox { Name = name, Width = 100, Height = 28 };
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
            
            // При кліку на рядок підтягуємо назву в NameBox
            var nameProp = item.GetType().GetProperty("nazva") ?? item.GetType().GetProperty("name");
            if (nameProp != null)
            {
                var valObj = nameProp.GetValue(item);
                if (valObj != null)
                {
                    string val = valObj.ToString() ?? "";
                    var nameBox = GetTextBoxByName("NameBox");
                    if (nameBox != null) nameBox.Text = val;
                }
            }

            // Підтягуємо ID в UpdateIdBox для зручності оновлення
            var idProp = item.GetType().GetProperty("id");
            if (idProp != null)
            {
                var idObj = idProp.GetValue(item);
                if (idObj != null)
                {
                    var updateIdBox = GetTextBoxByName("UpdateIdBox");
                    if (updateIdBox != null) updateIdBox.Text = idObj.ToString();
                }
            }
        }

        // ДОДАВАННЯ
        private void OnAddClick(object? sender, RoutedEventArgs e)
        {
            var nameBox = GetTextBoxByName("NameBox");
            string nameVal = nameBox?.Text?.Trim().ToLower() ?? "";

            Console.WriteLine($"[DEBUG UI] Спроба додавання в '{currentTable}' з ім'ям: '{nameVal}'");

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
                    "Country" => new CountryRep().Add(new CountryTable(0, nameVal)),
                    "Category" => new CategoryRep().Add(new CategoryTable(0, nameVal)),
                    "Material" => new MaterialRep().Add(new MaterialTable(0, nameVal)),
                    "ShoeType" => new ShoeTypeRep().Add(new ShoeTypeTable(0, nameVal)),
                    "Sole" => new SoleRep().Add(new SoleTable(0, nameVal)),
                    "Style" => new StyleRep().Add(new StyleTable(0, nameVal)),
                    "Audience" => new AudienceRep().Add(new AudienceTable(0, nameVal)),
                    "Brand" => AddBrandHelper(nameVal),
                    "FootwearCatalog" => AddCatalogHelper(nameVal),
                    _ => false
                };
            }
            catch (MySql.Data.MySqlClient.MySqlException mysqlEx) when (mysqlEx.Number == 1062)
            {
                Console.WriteLine("[ERROR] Дублікат! Запис із такою назвою вже існує.");
                return;
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

        private bool AddBrandHelper(string nameVal)
        {
            var countryIdBox = GetTextBoxByName("CountryIdBox");
            if (!int.TryParse(countryIdBox?.Text, out int cId)) return false;
            return new BrandRep().Add(new BrandTable(0, nameVal, cId));
        }

        private bool AddCatalogHelper(string nameVal)
        {
            var priceBox = GetTextBoxByName("PriceBox");
            var colorBox = GetTextBoxByName("ColorBox");
            if (!decimal.TryParse(priceBox?.Text, out decimal price)) return false;
            
            return new FootwearCatalogRep().Add(new FootwearCatalogTable(0, nameVal, 0, 0, 0, 0, 0, 0, "", "", "", price, 0, 0, 0, "", "", "", 0, 0, colorBox?.Text ?? "", 0));
        }

        // ОНОВЛЕННЯ (Використовує UpdateIdBox для вибору запису та NameBox для нових даних)
        private void OnUpdateClick(object? sender, RoutedEventArgs e)
        {
            var updateIdBox = GetTextBoxByName("UpdateIdBox");
            var nameBox = GetTextBoxByName("NameBox");

            if (updateIdBox == null || !int.TryParse(updateIdBox.Text, out int idVal) || idVal <= 0)
            {
                Console.WriteLine("[WARNING] Введіть коректний ID для оновлення у полі 'Update ID'!");
                return;
            }

            string nameVal = nameBox?.Text?.Trim().ToLower() ?? "";
            Console.WriteLine($"[DEBUG UI] Спроба оновлення в '{currentTable}' за ID: {idVal} на нове значення: '{nameVal}'");

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
                Console.WriteLine("[DEBUG UI] Запис успішно оновлено!");
                LoadTableData(currentTable);
                ClearInputs();
            }
            else
            {
                Console.WriteLine("[ERROR] Не вдалося оновити запис.");
            }
        }

        // ВИДАЛЕННЯ (Використовує NameBox за назвою без ID)
        private void OnDeleteClick(object? sender, RoutedEventArgs e)
        {
            var nameBox = GetTextBoxByName("NameBox");
            string nameVal = nameBox?.Text?.Trim().ToLower() ?? "";

            Console.WriteLine($"[DEBUG UI] Спроба видалення з '{currentTable}' за ім'ям: '{nameVal}'");

            if (string.IsNullOrWhiteSpace(nameVal))
            {
                Console.WriteLine("[WARNING] Поле 'Name' порожнє, неможливо видалити!");
                return;
            }

            bool success = false;
            try
            {
                success = currentTable switch
                {
                    "Country" => new CountryRep().Delete(new CountryTable(0, nameVal)),
                    "Category" => new CategoryRep().Delete(new CategoryTable(0, nameVal)),
                    "Material" => new MaterialRep().Delete(new MaterialTable(0, nameVal)),
                    "ShoeType" => new ShoeTypeRep().Delete(new ShoeTypeTable(0, nameVal)),
                    "Sole" => new SoleRep().Delete(new SoleTable(0, nameVal)),
                    "Style" => new StyleRep().Delete(new StyleTable(0, nameVal)),
                    "Audience" => new AudienceRep().Delete(new AudienceTable(0, nameVal)),
                    "Brand" => new BrandRep().Delete(new BrandTable(0, nameVal, 0)),
                    "FootwearCatalog" => new FootwearCatalogRep().Delete(new FootwearCatalogTable(0, nameVal, 0, 0, 0, 0, 0, 0, "", "", "", 0, 0, 0, 0, "", "", "", 0, 0, "", 0)),
                    _ => false
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Помилка Delete: {ex.Message}");
            }

            if (success)
            {
                Console.WriteLine("[DEBUG UI] Запис успішно видалено за назвою!");
                LoadTableData(currentTable);
                ClearInputs();
            }
            else
            {
                Console.WriteLine("[ERROR] Не вдалося видалити. Перевір, чи існує таке ім'я.");
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