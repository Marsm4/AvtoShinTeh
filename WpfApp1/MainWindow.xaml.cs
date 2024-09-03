using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly List<Seller> sellers = new List<Seller>
        {
            new Seller { Id = 10153, Name = "Чупашева А.И.", HireDate = new DateTime(2015, 10, 1) },
            new Seller { Id = 20174, Name = "Иванов А.В.", HireDate = new DateTime(2017, 1, 10) },
            new Seller { Id = 30191, Name = "Кривцов О.П.", HireDate = new DateTime(2019, 2, 5) },
            new Seller { Id = 40140, Name = "Янаева Э.С.", HireDate = new DateTime(2014, 12, 15) }
        };
        public MainWindow()
        {
            InitializeComponent();
        }
        private void CalculateCommission_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = SellerNameTextBox.Text;
                DateTime hireDate = DateTime.ParseExact(HireDateTextBox.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                decimal dailyRevenue = decimal.Parse(DailyRevenueTextBox.Text, CultureInfo.InvariantCulture);

                var seller = sellers.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                if (seller == null)
                {
                    MessageBox.Show("Продавец не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int tenure = (DateTime.Now.Year - seller.HireDate.Year) - (DateTime.Now.DayOfYear < seller.HireDate.DayOfYear ? 1 : 0);
                decimal commissionRate = dailyRevenue < 50000 ? 0.03m : 0.06m;
                decimal commissionAmount = dailyRevenue * commissionRate;
                if (tenure > 3)
                {
                    commissionAmount *= 1.05m;
                }

                // Отображаем фамилию продавца и комиссионные
                CommissionSellerName.Text = $"{seller.Name}: {commissionAmount:F2} руб.";

            
                ResultDataGrid.Items.Add(new CommissionResult
                {
                    SellerName = seller.Name,
                    CommissionAmount = commissionAmount.ToString("F2"),
                    DailyRevenue = dailyRevenue.ToString("F2"),
                    Tenure = tenure.ToString()
                });
            }
            catch (FormatException)
            {
                MessageBox.Show("Некорректный ввод данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

        public class Seller
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime HireDate { get; set; }
    }

    public class CommissionResult
    {
        public string SellerName { get; set; }
        public string CommissionAmount { get; set; }
        public string DailyRevenue { get; set; }
        public string Tenure { get; set; }
    }
}