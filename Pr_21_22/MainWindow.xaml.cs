using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Pr_21_22
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        BaseStockEntities st = BaseStockEntities.GetContext();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            st.PriceTags.Load();
            st.ProductGroupDirectories.Load();
            st.InComes.Load();
            st.Shippers.Load();
            st.Sales.Load();
            productCatalog.ItemsSource = st.PriceTags.Local.ToBindingList();
            GroupProduct.ItemsSource = st.ProductGroupDirectories.Local.ToBindingList();
            Income.ItemsSource = st.InComes.Local.ToBindingList();
            Post.ItemsSource = st.Shippers.Local.ToBindingList();
            Sale.ItemsSource = st.Sales.Local.ToBindingList();
        }

        private void AddRecord_Click(object sender, RoutedEventArgs e)
        {
            Add add = new Add();
            add.ShowDialog();
            productCatalog.Focus();
        }

        private void ChangeRecord_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var row = (PriceTag)productCatalog.SelectedItem;
                Data.Id = row.IdProduct;
                Data.IdGroup = row.IdGroup;
                Change change = new Change();
                change.Owner = this;
                change.ShowDialog();
                productCatalog.Items.Refresh();
                productCatalog.Focus();
            }
            catch (Exception)
            {
                MessageBox.Show("Выберите запись для редактирования");
            }
        }

        private void DeleteRecord_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result;
            result = MessageBox.Show("Удалить запись?", "Удаление записи", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    PriceTag row = (PriceTag)productCatalog.SelectedItem;
            st.PriceTags.Remove(row);
            st.SaveChanges();
            productCatalog.Items.Refresh();
                }
                catch (ArgumentOutOfRangeException)
                {
                    MessageBox.Show("Выберите запись", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }

        PriceTag price;

        private void Price_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            price = (PriceTag)productCatalog.CurrentCell.Item;
        }

        private void Price_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (price == null)
            {
                price = new PriceTag();
                price.NameProduct = ((TextBox)(e.EditingElement)).Text;
                st.PriceTags.Add(price);
                st.SaveChanges();
            }
            else
            {
                product.NameGroup = ((TextBox)(e.EditingElement)).Text;
                st.SaveChanges();
            }
        }

        ProductGroupDirectory product;

        private void GroupProduct_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            product = (ProductGroupDirectory)GroupProduct.CurrentCell.Item;
        }

        private void GroupProduct_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (product == null)
            {
                product = new ProductGroupDirectory();
                product.NameGroup = ((TextBox)(e.EditingElement)).Text;
                st.ProductGroupDirectories.Add(product);
                st.SaveChanges();
            }
            else
            {
                product.NameGroup = ((TextBox)(e.EditingElement)).Text;
                st.SaveChanges();
            }
        }

        InCome come;
        private void InCome_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            come = (InCome)Income.CurrentCell.Item;
        }

        private void InCome_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (come == null)
            {
                come = new InCome();
                st.InComes.Add(come);
                st.SaveChanges();
            }
            else
            {
                st.SaveChanges();
            }
        }

        Sale sale;

        private void Sale_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            sale = (Sale)Sale.CurrentCell.Item;
        }

        private void Sale_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (sale == null)
            {
                sale = new Sale();
                st.Sales.Add(sale);
                st.SaveChanges();
            }
            else
            {
                st.SaveChanges();
            }
        }

        Shipper shipper;
        private void Post_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            shipper = (Shipper)Post.CurrentCell.Item;
        }

        private void Post_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (shipper == null)
            {
                shipper = new Shipper();
                st.Shippers.Add(shipper);
                st.SaveChanges();
            }
            else
            {
                st.SaveChanges();
            }
        }

        private void JanuaryProduct_Click(object sender, RoutedEventArgs e)
        {
            var jP = from p in st.InComes
                     where p.DateInComing.Month == 1
                     select new { p.PriceTag.NameProduct, p.QuantityInCome };
            january.ItemsSource = jP.ToList();
    
        }

        private void NullProduct(object sender, RoutedEventArgs e)
        {
            var nP = from p in st.InComes
                     where p.QuantityInCome == 0
                     select new { p.PriceTag.NameProduct };
            nullProduct.ItemsSource = nP.ToList();
        }


        private void Search_Click(object sender, RoutedEventArgs e)
        {
            Search search = new Search();
            search.ShowDialog();
        }

        private void End_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void IncreasePrice(object sender, RoutedEventArgs e)
        {
            int number;
            try
            {
                PriceTag catalog = (PriceTag)productCatalog.SelectedItem;
                number = st.Database.ExecuteSqlCommand($"UPDATE PriceTag SET Price = price * 1.15 WHERE IdProduct={catalog.IdProduct}");
                st = new BaseStockEntities();
                st.PriceTags.Load();
                productCatalog.ItemsSource = st.PriceTags.Local.ToBindingList();
                productCatalog.Items.Refresh();
                productCatalog.Focus();
            }
            catch
            {
                MessageBox.Show("Выберите запись для увеличения");
            }
        }


    }
}
