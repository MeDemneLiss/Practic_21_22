using System;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Windows;

namespace Pr_21_22
{
    public partial class Change : Window
    {
        public Change()
        {
            InitializeComponent();
        }
        BaseStockEntities st = BaseStockEntities.GetContext();
        PriceTag product;
        ProductGroupDirectory group;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            st.ProductGroupDirectories.Load();
            idGroupT.ItemsSource = st.ProductGroupDirectories.ToList();
            st.PriceTags.Load();
            product = st.PriceTags.Find(Data.Id);
            group = st.ProductGroupDirectories.Find(Data.IdGroup);       
            idGroupT.Text = group.NameGroup;
            idProductT.Text = product.IdProduct.ToString();
            nameProductT.Text = product.NameProduct;

            unitsOfMeterageT.Text = product.UnitsOfMeterage;
            priceT.Text = (Convert.ToInt32(product.Price)).ToString();

        }
        private void Change_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (!int.TryParse(idProductT.Text, out int idProduct) || idProduct <= 0)
            {
                errors.AppendLine("Код продукта введен неверно");
            }
            if (nameProductT.Text == string.Empty)
            {
                errors.AppendLine("Наименование продукта введено неверно");
            }
            if (string.IsNullOrEmpty(idGroupT.Text))
            {
                errors.AppendLine("Выберите группу");
            }
            if (unitsOfMeterageT.Text == string.Empty)
            {
                errors.AppendLine("Единица измерения введена неверно");
            }
            if (!int.TryParse(priceT.Text, out int price) || price <= 0)
            {
                errors.AppendLine("Цена введена неверно");
            }
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            product.IdProduct = idProduct;
            product.NameProduct = nameProductT.Text;
            var idGroup = st.ProductGroupDirectories.Where(p => idGroupT.Text == p.NameGroup).FirstOrDefault();
            product.ProductGroupDirectory = idGroup;
            product.UnitsOfMeterage = unitsOfMeterageT.Text;
            product.Price = price;
            try
            {
                st.SaveChanges();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    

        private void End_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
