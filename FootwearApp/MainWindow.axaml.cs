using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Linq;
using FootwearApp.Tables;
using FootwearApp.Repositories;

namespace FootwearApp
{
    public partial class MainWindow : Window
    {
        private readonly CountryRep _countryRep = new CountryRep();
        private readonly BrandRep _brandRep = new BrandRep();
        private readonly CategoryRep _categoryRep = new CategoryRep();
        private readonly MaterialRep _materialRep = new MaterialRep();
        private readonly ShoeTypeRep _shoeTypeRep = new ShoeTypeRep();
        private readonly SoleRep _soleRep = new SoleRep();
        private readonly StyleRep _styleRep = new StyleRep();
        private readonly AudienceRep _audienceRep = new AudienceRep();
        private readonly FootwearCatalogRep _catalogRep = new FootwearCatalogRep();

        private string _currentTable = "country";

        public MainWindow()
        {
            InitializeComponent();

            var cbTables = this.FindControl<ComboBox>("CbTables");
            if (cbTables != null) cbTables.SelectionChanged += CbTables_SelectionChanged;

            var btnAdd = this.FindControl<Button>("BtnAdd");
            if (btnAdd != null) btnAdd.Click += BtnAdd_Click;

            var btnDelete = this.FindControl<Button>("BtnDelete");
            if (btnDelete != null) btnDelete.Click += BtnDelete_Click;

            var btnRefresh = this.FindControl<Button>("BtnRefresh");
            if (btnRefresh != null) btnRefresh.Click += BtnRefresh_Click;

            UpdateInputPanels();
            LoadData();
        }

        private void CbTables_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            var cb = this.FindControl<ComboBox>("CbTables");
            if (cb?.SelectedItem is ComboBoxItem item && item.Content != null)
            {
                _currentTable = item.Content.ToString()!;
                UpdateInputPanels();
                LoadData();
            }
        }

        private void UpdateInputPanels()
        {
            var panelCountryId = this.FindControl<StackPanel>("PanelCountryId");
            if (panelCountryId != null) panelCountryId.IsVisible = (_currentTable == "brand");

            bool isCatalog = (_currentTable == "footwear_catalog");
            for (int i = 1; i <= 20; i++)
            {
                var panel = this.FindControl<StackPanel>($"PanelCatalog{i}");
                if (panel != null) panel.IsVisible = isCatalog;
            }
        }

        private void LoadData()
        {
            var dataGrid = this.FindControl<DataGrid>("DataGridItems");
            if (dataGrid == null) return;

            dataGrid.ItemsSource = _currentTable switch
            {
                "country" => _countryRep.GetAll().ToList(),
                "brand" => _brandRep.GetAll().ToList(),
                "category" => _categoryRep.GetAll().ToList(),
                "material" => _materialRep.GetAll().ToList(),
                "shoe_type" => _shoeTypeRep.GetAll().ToList(),
                "sole" => _soleRep.GetAll().ToList(),
                "style" => _styleRep.GetAll().ToList(),
                "audience" => _audienceRep.GetAll().ToList(),
                "footwear_catalog" => _catalogRep.GetAll().ToList(),
                _ => null
            };
        }

        private void BtnRefresh_Click(object? sender, RoutedEventArgs e) => LoadData();

        private void BtnAdd_Click(object? sender, RoutedEventArgs e)
        {
            var txtName = this.FindControl<TextBox>("TxtBoxName");
            if (txtName == null || string.IsNullOrWhiteSpace(txtName.Text)) return;
            string nameVal = txtName.Text.Trim();

            bool success = false;

            switch (_currentTable)
            {
                case "country":
                    success = _countryRep.Add(new CountryTable(0, nameVal));
                    break;
                case "brand":
                    int cId = int.TryParse(this.FindControl<TextBox>("TxtBoxCountryId")?.Text, out var parsedCId) ? parsedCId : 1;
                    success = _brandRep.Add(new BrandTable(0, nameVal, cId));
                    break;
                case "category":
                    success = _categoryRep.Add(new CategoryTable(0, nameVal));
                    break;
                case "material":
                    success = _materialRep.Add(new MaterialTable(0, nameVal));
                    break;
                case "shoe_type":
                    success = _shoeTypeRep.Add(new ShoeTypeTable(0, nameVal));
                    break;
                case "sole":
                    success = _soleRep.Add(new SoleTable(0, nameVal));
                    break;
                case "style":
                    success = _styleRep.Add(new StyleTable(0, nameVal));
                    break;
                case "audience":
                    success = _audienceRep.Add(new AudienceTable(0, nameVal));
                    break;
                case "footwear_catalog":
                    success = _catalogRep.Add(new FootwearCatalogTable(
                        0,
                        nameVal,
                        int.TryParse(this.FindControl<TextBox>("TxtBoxShoeType")?.Text, out var v1) ? v1 : 1,
                        int.TryParse(this.FindControl<TextBox>("TxtBoxAudience")?.Text, out var v2) ? v2 : 1,
                        int.TryParse(this.FindControl<TextBox>("TxtBoxBrand")?.Text, out var v3) ? v3 : 1,
                        int.TryParse(this.FindControl<TextBox>("TxtBoxUpperMat")?.Text, out var v4) ? v4 : 1,
                        int.TryParse(this.FindControl<TextBox>("TxtBoxLiningMat")?.Text, out var v5) ? v5 : 1,
                        int.TryParse(this.FindControl<TextBox>("TxtBoxSoleMat")?.Text, out var v6) ? v6 : 1,
                        this.FindControl<TextBox>("TxtBoxColor")?.Text ?? "",
                        this.FindControl<TextBox>("TxtBoxSizes")?.Text ?? "",
                        this.FindControl<TextBox>("TxtBoxSeason")?.Text ?? "",
                        decimal.TryParse(this.FindControl<TextBox>("TxtBoxPrice")?.Text, out var v7) ? v7 : 0,
                        int.TryParse(this.FindControl<TextBox>("TxtBoxCategory")?.Text, out var v8) ? v8 : 1,
                        int.TryParse(this.FindControl<TextBox>("TxtBoxCatalogCountry")?.Text, out var v9) ? v9 : 1,
                        int.TryParse(this.FindControl<TextBox>("TxtBoxSoleType")?.Text, out var v10) ? v10 : 1,
                        this.FindControl<TextBox>("TxtBoxShaftHeight")?.Text ?? "",
                        this.FindControl<TextBox>("TxtBoxWidthFit")?.Text ?? "",
                        this.FindControl<TextBox>("TxtBoxFastener")?.Text ?? "",
                        int.TryParse(this.FindControl<TextBox>("TxtBoxMembrane")?.Text, out var v11) ? v11 : 0,
                        int.TryParse(this.FindControl<TextBox>("TxtBoxStyle")?.Text, out var v12) ? v12 : 1,
                        this.FindControl<TextBox>("TxtBoxCollection")?.Text ?? "",
                        int.TryParse(this.FindControl<TextBox>("TxtBoxWarranty")?.Text, out var v13) ? v13 : 12
                    ));
                    break;
            }

            if (success) 
            {
                txtName.Text = "";
                LoadData();
                Console.WriteLine("[DEBUG]: Успішно додано!");
            }
        }

        private void BtnDelete_Click(object? sender, RoutedEventArgs e)
        {
            var txtName = this.FindControl<TextBox>("TxtBoxName");
            if (txtName == null || string.IsNullOrWhiteSpace(txtName.Text))
            {
                Console.WriteLine("[DEBUG]: Введіть назву у поле вводу для видалення!");
                return;
            }

            string targetName = txtName.Text.Trim();
            bool success = false;
            Console.WriteLine($"[DEBUG]: Шукаємо та видаляємо запис за назвою '{targetName}' у таблиці {_currentTable}");

            // Шукаємо в базі об'єкт з такою назвою, щоб отримати його ID для видалення
            switch (_currentTable)
            {
                case "country":
                    var c = _countryRep.GetAll().FirstOrDefault(x => x.nazva.Equals(targetName, StringComparison.OrdinalIgnoreCase));
                    if (c != null) success = _countryRep.Delete(c);
                    break;
                case "brand":
                    var b = _brandRep.GetAll().FirstOrDefault(x => x.nazva.Equals(targetName, StringComparison.OrdinalIgnoreCase));
                    if (b != null) success = _brandRep.Delete(b);
                    break;
                case "category":
                    var cat = _categoryRep.GetAll().FirstOrDefault(x => x.nazva.Equals(targetName, StringComparison.OrdinalIgnoreCase));
                    if (cat != null) success = _categoryRep.Delete(cat);
                    break;
                case "material":
                    var mat = _materialRep.GetAll().FirstOrDefault(x => x.nazva.Equals(targetName, StringComparison.OrdinalIgnoreCase));
                    if (mat != null) success = _materialRep.Delete(mat);
                    break;
                case "shoe_type":
                    var st = _shoeTypeRep.GetAll().FirstOrDefault(x => x.nazva.Equals(targetName, StringComparison.OrdinalIgnoreCase));
                    if (st != null) success = _shoeTypeRep.Delete(st);
                    break;
                case "sole":
                    var sol = _soleRep.GetAll().FirstOrDefault(x => x.nazva.Equals(targetName, StringComparison.OrdinalIgnoreCase));
                    if (sol != null) success = _soleRep.Delete(sol);
                    break;
                case "style":
                    var sty = _styleRep.GetAll().FirstOrDefault(x => x.nazva.Equals(targetName, StringComparison.OrdinalIgnoreCase));
                    if (sty != null) success = _styleRep.Delete(sty);
                    break;
                case "audience":
                    var aud = _audienceRep.GetAll().FirstOrDefault(x => x.nazva.Equals(targetName, StringComparison.OrdinalIgnoreCase));
                    if (aud != null) success = _audienceRep.Delete(aud);
                    break;
                case "footwear_catalog":
                    var fc = _catalogRep.GetAll().FirstOrDefault(x => x.nazva.Equals(targetName, StringComparison.OrdinalIgnoreCase));
                    if (fc != null) success = _catalogRep.Delete(fc);
                    break;
            }

            if (success)
            {
                txtName.Text = "";
                LoadData();
                Console.WriteLine("[DEBUG]: Успішно видалено за назвою!");
            }
            else
            {
                Console.WriteLine("[DEBUG]: Запис з такою назвою не знайдено або сталася помилка БД!");
            }
        }
    }
}