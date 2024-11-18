using Cosmetify.Model;
using Cosmetify.Model.Enums;
using Cosmetify.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Cosmetify.ViewModel
{
    public class CommonViewModel : INotifyPropertyChanged
    {
        private CategoryRepository categoryRepository;

        private SubCategoryRepository subCategoryRepository = new();

        private SubSubCategoryRepository subSubCategoryRepository;

        private ProductRepository productRepository = new();

        private LeadsRepository leadsRepository = new();

        private ObservableCollection<CategoryModel> categoriesList;

        private ObservableCollection<ProductModel> productList;

        private ObservableCollection<CustomerModel> customerList;

        private ObservableCollection<SubCategoryModel> subCategoryList;

        private OrderRepository orderRepository = new OrderRepository();

        private TransactionRepository transactionRepository = new TransactionRepository();
        private double totalOrders;
        private double paidOrders;
        private double unpaidOrders;
        private double partialPaidOrders;
        private int cancelledOrders;
        private ActivesRepository activesRepository = new();
        private MasterFormulaRepository masterFormulaRepository = new();
        private BatchOrderRepository batchOrderRepository = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        public CommonViewModel() 
        {
            this.categoryRepository = new CategoryRepository(SubCategoryRepository);
            this.subSubCategoryRepository = new SubSubCategoryRepository(SubCategoryRepository);
            this.subCategoryRepository.CategoryRepository = this.CategoryRepository;
            this.subCategoryRepository.SubSubCategoryRepository = this.SubSubCategoryRepository;
            this.GetAllCategories();
            this.GetAllProducts();
            this.GetAllLeads();
            this.GetAllSubCategories();
        }

        public CategoryRepository CategoryRepository
        {
            get 
            {
                return this.categoryRepository; 
            }
            set
            {
                this.categoryRepository = value;
                this.NotifyPropertyChanged(nameof(CategoryRepository));
            }
        }

        public SubCategoryRepository SubCategoryRepository
        {
            get 
            { 
                return this.subCategoryRepository; 
            }
            set
            {
                this.subCategoryRepository = value;
                this.NotifyPropertyChanged(nameof(SubCategoryRepository));
            }
        }

        public SubSubCategoryRepository SubSubCategoryRepository
        {
            get 
            { 
                return this.subSubCategoryRepository; 
            }
            set
            {
                this.subSubCategoryRepository = value;
                this.NotifyPropertyChanged(nameof(SubSubCategoryRepository));
            }
        }

        public ProductRepository ProductRepository
        {
            get 
            { 
                return this.productRepository; 
            }
            set
            {
                this.productRepository = value;
                this.NotifyPropertyChanged(nameof(ProductRepository));
            }
        }

        public LeadsRepository LeadsRepository
        {
            get 
            { 
                return this.leadsRepository; 
            }
            set
            {
                this.leadsRepository = value;
                this.NotifyPropertyChanged(nameof(LeadsRepository));
            }
        }

        public ObservableCollection<CategoryModel> CategoriesList
        {
            get 
            { 
                return this.categoriesList;
            }
            set
            {
                this.categoriesList = value;
                this.NotifyPropertyChanged(nameof(CategoriesList));
            }
        }

        public ObservableCollection<SubCategoryModel> SubCategoryList
        {
            get
            {
                return this.subCategoryList;
            }

            set
            {
                this.subCategoryList = value;
                this.NotifyPropertyChanged(nameof(SubCategoryList));
            }
        }

        public OrderRepository OrderRepository
        {
            get 
            { 
                return this.orderRepository; 
            }
            set
            {
                this.orderRepository = value;
                this.NotifyPropertyChanged(nameof(OrderRepository));
            }
        }

        public TransactionRepository TransactionRepository
        {
            get
            {
                return this.transactionRepository;
            }

            set
            {
                this.transactionRepository = value;
                this.NotifyPropertyChanged(nameof(TransactionRepository));
            }
        }

        public ActivesRepository ActivesRepository
        {
            get
            {
                return this.activesRepository;
            }

            set
            {
                this.activesRepository = value;
                this.NotifyPropertyChanged(nameof(ActivesRepository));
            }
        }

        public MasterFormulaRepository MasterFormulaRepository
        {
            get => this.masterFormulaRepository; set
            {
                this.masterFormulaRepository = value;
                this.NotifyPropertyChanged(nameof(MasterFormulaRepository));
            }
        }

        public BatchOrderRepository BatchOrderRepository
        {
            get => this.batchOrderRepository; set
            {
                this.batchOrderRepository = value;
                this.NotifyPropertyChanged(nameof(BatchOrderRepository));
            }
        }

        public ObservableCollection<ProductModel> ProductList
        {
            get 
            { 
                return this.productList; 
            }
            set
            {
                this.productList = value;
                this.NotifyPropertyChanged(nameof(ProductList));
            }
        }

        public ObservableCollection<CustomerModel> CustomerList
        {
            get 
            { 
                return this.customerList; 
            }
            set
            {
                this.customerList = value;
                this.NotifyPropertyChanged(nameof(CustomerList));
            }
        }

        public double TotalOrders
        {
            get => this.totalOrders; set
            {
                this.totalOrders = value;
                this.NotifyPropertyChanged(nameof(TotalOrders));
            }
        }

        public double PaidOrders
        {
            get => this.paidOrders; set
            {
                this.paidOrders = value;
                this.NotifyPropertyChanged(nameof(PaidOrders));
            }
        }

        public double UnpaidOrders
        {
            get => this.unpaidOrders; set
            {
                this.unpaidOrders = value;
                this.NotifyPropertyChanged(nameof(UnpaidOrders));
            }
        }

        public double PartialPaidOrders
        {
            get => this.partialPaidOrders; set
            {
                this.partialPaidOrders = value;
                this.NotifyPropertyChanged(nameof(PartialPaidOrders));
            }
        }

        public int CancelledOrders
        {
            get => this.cancelledOrders; set
            {
                this.cancelledOrders = value;
                this.NotifyPropertyChanged(nameof(CancelledOrders));
            }
        }

        public void RefreshOrders()
        {
            this.GetCancelledOrders();
            this.GetPaidOrders();
            this.GetTotalOrders();
            this.GetUnpaidOrders();
            this.GetPPaidOrders();
        }

        private void GetAllCategories()
        {
            this.CategoriesList = this.categoryRepository.GetCategories();
        }

        private void GetAllSubCategories()
        {
            this.SubCategoryList = this.subCategoryRepository.GetSubCategories();
        }

        private void GetAllProducts()
        {
            this.ProductList = this.productRepository.GetAllProducts();
        }

        private void GetAllLeads()
        {
            this.CustomerList = this.leadsRepository.GetAllLeads();
        }

        private void GetTotalOrders()
        {
            this.TotalOrders = this.OrderRepository.GetAmountWithPaymentStatus();
        }

        private void GetPaidOrders()
        {
            this.PaidOrders = this.OrderRepository.GetAmountWithPaymentStatus(PaymentStatus.Paid.ToString());
        }

        private void GetUnpaidOrders()
        {
            this.UnpaidOrders = this.OrderRepository.GetAmountWithPaymentStatus(PaymentStatus.Unpaid.ToString());
        }

        private void GetCancelledOrders()
        {
            this.CancelledOrders = this.OrderRepository.GetCancelledOrders(OrderStatus.Cancelled.ToString());
        }

        private void GetPPaidOrders()
        {
            this.PartialPaidOrders = this.OrderRepository.GetAmountWithPaymentStatus(PaymentStatus.PartialPaid.ToString());
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
