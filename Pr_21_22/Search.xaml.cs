using System;
using System.Collections.Generic;
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

namespace Pr_21_22
{
    /// <summary>
    /// Логика взаимодействия для Search.xaml
    /// </summary>
    public partial class Search : Window
    {
        public Search()
        {
            InitializeComponent();
        }
        BaseStockEntities db = BaseStockEntities.GetContext();
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if (nameText.Text != string.Empty)
            {
                var request = db.PriceTags.Where(p => nameText.Text.ToLower() == p.NameProduct.ToLower()).FirstOrDefault();

                if (request == null)
                {
                    MessageBox.Show("Данный продукт не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                groupText.Text = request.ProductGroupDirectory.NameGroup;
            }
            else MessageBox.Show("Введите название продукта", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void SearchPrice_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(shipperText.Text, out int shipper) || shipper <= 0)
            {
                MessageBox.Show("Цена продукта введен неверно","Ошибка",MessageBoxButton.OK, MessageBoxImage.Error) ;
                return;
            }       
            
                var request = db.Shippers.Where(p => shipper == p.IdShipper).FirstOrDefault();

                if (request == null)
                {
                    MessageBox.Show("Данный продукт не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                groupPriceText.Text = request.PriceTag.NameProduct;
            
        }
    }
}
