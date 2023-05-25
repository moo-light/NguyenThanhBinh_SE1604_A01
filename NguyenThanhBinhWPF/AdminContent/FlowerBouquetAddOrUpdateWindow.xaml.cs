using BusinessObject;
using NguyenThanhBinhWPF.Utils;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NguyenThanhBinhWPF.AdminContent
{
    /// <summary>
    /// Interaction logic for FlowerBouquetAddOrUpdateWindow.xaml
    /// </summary>
    public partial class FlowerBouquetAddOrUpdateWindow : Window, INotifyPropertyChanged
    {
        private IList<Category>? _categories;
        private IList<Supplier>? _suppliers;
        private static FlowerBouquet? _selectedFlowerBouquet;
        private readonly IFlowerBouquetRepository _flowerBouquetRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISupplierRepository _supplierRepository;

        public FlowerBouquetAddOrUpdateWindow(IFlowerBouquetRepository flowerBouquetRepository,
                                              ICategoryRepository categoryRepository,
                                              ISupplierRepository supplierRepository)
        {
            DataContext = this;
            _flowerBouquetRepository = flowerBouquetRepository;
            _categoryRepository = categoryRepository;
            _supplierRepository = supplierRepository;
            InitializeComponent();
            Categories = _categoryRepository.GetCategories;
            Suppliers = _supplierRepository.GetSuppliers;

        }

        public bool IsUpdate { get; internal set; }
        public FlowerBouquet? SelectedFlowerBouquet
        {
            get => _selectedFlowerBouquet; set
            {
                _selectedFlowerBouquet = value;
                this.OnPropertyChanged(PropertyChanged, nameof(SelectedFlowerBouquet));
            }
        }

        public IList<Category>? Categories
        {
            get => _categories; set
            {
                _categories = value;
                this.OnPropertyChanged(PropertyChanged, nameof(Categories));
            }
        }
        public IList<Supplier>? Suppliers
        {
            get => _suppliers; set
            {
                _suppliers = value;
                this.OnPropertyChanged(PropertyChanged, nameof(Suppliers));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        internal Window PrepareWindow()
        {
            //Categories = _categoryRepository.GetCategories;
            //Suppliers = _supplierRepository.GetSuppliers;
            //cboCategory.SelectedItem = SelectedFlowerBouquet.Category;
            //cboSupplier.SelectedItem = SelectedFlowerBouquet.Supplier;
            btnAction.Content = IsUpdate ? "Update" : "Add";
            return this;
        }

        private void btnAction_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (cboCategory.SelectedItem == null) throw new InvalidOperationException("Category is Empty Please Select Category");
                if (cboSupplier.SelectedItem == null) throw new InvalidOperationException("Supplier is Empty Please Select Supplier");

                if (IsUpdate)
                {
                    _flowerBouquetRepository.UpdateFlowerBouquet(SelectedFlowerBouquet);
                }
                else
                {
                    _flowerBouquetRepository.InsertFlowerBouquet(SelectedFlowerBouquet);
                }

                btnCancel_Click(sender, e);
                MessageBox.Show($"{btnAction.Content} Successful!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}");
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            this.CancelWindowClosing(sender, e);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
