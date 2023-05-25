using BusinessObject;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NguyenThanhBinhWPF.Utils;
using Repository;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
using System.Xml.Linq;

namespace NguyenThanhBinhWPF.AdminContent
{
    /// <summary>
    /// Interaction logic for FlowerBouquetManagement.xaml
    /// </summary>
    public partial class FlowerBouquetManagementWindow : Window, INotifyPropertyChanged
    {
        private readonly IFlowerBouquetRepository _flowerBouquetRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISupplierRepository _supplierRepository;
        private ObservableCollection<FlowerBouquet>? _flowerBouquets;
        private FlowerBouquet? _selectedFlowerBouquet;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<FlowerBouquet>? FlowerBouquets
        {
            get => _flowerBouquets; set
            {
                _flowerBouquets = value;
                OnPropertyChanged(nameof(FlowerBouquets));
            }
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

       
       
        public FlowerBouquet? SelectedFlowerBouquet
        {
            get => _selectedFlowerBouquet; set
            {
                _selectedFlowerBouquet = value;
                OnPropertyChanged(nameof(SelectedFlowerBouquet));
            }
        }

        public FlowerBouquetAddOrUpdateWindow FlowerBouquetAddOrUpdateWindow { get; }

        public FlowerBouquetManagementWindow(IFlowerBouquetRepository flowerBouquetRepository
            , ICategoryRepository categoryRepository,
            ISupplierRepository supplierRepository, FlowerBouquetAddOrUpdateWindow flowerBouquetAddOrUpdateWindow)
        {
            _flowerBouquetRepository = flowerBouquetRepository;
            _categoryRepository = categoryRepository;
            _supplierRepository = supplierRepository;

            FlowerBouquetAddOrUpdateWindow = flowerBouquetAddOrUpdateWindow;
            DataContext = this;

            InitializeComponent();
            LoadFlowerBouquetList();
        }
        private void LoadFlowerBouquetList()
        {
            FlowerBouquets = null;
            FlowerBouquets = _flowerBouquetRepository.SearchFlowerBouquetInfomation(txtSearchCriteria.Text);
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e) => LoadFlowerBouquetList();

        private void Window_Closing(object sender, CancelEventArgs e) => this.CancelWindowClosing(sender, e);


        private void btnNew_Click(object sender, RoutedEventArgs e)
        {

            FlowerBouquetAddOrUpdateWindow.IsUpdate = false;
            _flowerBouquets = null;
            FlowerBouquetAddOrUpdateWindow.SelectedFlowerBouquet = GetFlowerBouquetObj();
            FlowerBouquetAddOrUpdateWindow.PrepareWindow().ShowDialog();
            LoadFlowerBouquetList();
            lvFlowerBouquetList.SelectedIndex = lvFlowerBouquetList.Items.Count - 1;
        }
        private void lvFlowerBouquetList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FlowerBouquetAddOrUpdateWindow.IsUpdate = true;
            if (lvFlowerBouquetList.SelectedIndex < 0 || SelectedFlowerBouquet == null) return;
            int index = lvFlowerBouquetList.SelectedIndex;// update item index
            _flowerBouquets = null;
            FlowerBouquetAddOrUpdateWindow.SelectedFlowerBouquet = GetFlowerBouquetObj(getID: true);// get customer Object
            FlowerBouquetAddOrUpdateWindow.PrepareWindow().ShowDialog();
            LoadFlowerBouquetList();
        }
        private FlowerBouquet? GetFlowerBouquetObj(bool getID = false)
        {
            FlowerBouquet? flowerBouquet;
            if (!getID)
                flowerBouquet = new FlowerBouquet { FlowerBouquetId = _flowerBouquetRepository.GetFlowerBouquets.Last().FlowerBouquetId + 1 };
            else
            {
                flowerBouquet = SelectedFlowerBouquet;
            }
            try
            {
                flowerBouquet.FlowerBouquetName = SelectedFlowerBouquet?.FlowerBouquetName ?? default;
                flowerBouquet.Description = SelectedFlowerBouquet?.Description ?? default;
                flowerBouquet.UnitPrice = SelectedFlowerBouquet?.UnitPrice ?? default;
                flowerBouquet.UnitsInStock = SelectedFlowerBouquet?.UnitsInStock ?? default;
                flowerBouquet.FlowerBouquetStatus = 1;
                flowerBouquet.SupplierId = SelectedFlowerBouquet?.SupplierId ?? default;
                flowerBouquet.CategoryId = SelectedFlowerBouquet?.CategoryId ?? throw new Exception("Please Select a Category");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}");
                flowerBouquet = null;
            }
            return flowerBouquet;
        }


           



        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            int index = lvFlowerBouquetList.SelectedIndex;// set item index
            bool notConfirm = MessageBox.Show("Are You Sure?", $"Deleting {SelectedFlowerBouquet?.FlowerBouquetName}", MessageBoxButton.YesNo) != MessageBoxResult.Yes;

            if (notConfirm) return; // return if not Confirm

            SelectedFlowerBouquet = GetFlowerBouquetObj(true);
            try
            {
                _flowerBouquetRepository.DeleteFlowerBouquet(SelectedFlowerBouquet);
                LoadFlowerBouquetList();
                SelectedFlowerBouquet = null;
                lvFlowerBouquetList.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Remove Flower Bouquet Unsuccessful \nex: {ex.Message}");
                lvFlowerBouquetList.SelectedIndex = index;

            }
        }

       
    }
}
