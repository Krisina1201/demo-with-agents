using AgentsSecond.Models;
using Avalonia.Controls;
using System.Linq;

namespace AgentsSecond
{
    public partial class CheckHistory : Window
    {
        public int agentIdgl;
        public CheckHistory()
        {
            InitializeComponent();
        }
        public CheckHistory(int agentId) : this() 
        {
            agentIdgl = agentId;
            LoadData(agentId);
        }

        private void LoadData(int agentId)
        {
            using var context = new AkapylkaContext();
            var items = context.ProductSales
                .Where(e => e.AgentId == agentId)
                .Select(e => new
                {
                    Title = e.Product.Title,
                    Date = e.SaleDate.ToString("dd.MM.yyyy"),
                    Count = e.ProductCount
                }).ToList();

            HistoryListBox.ItemsSource = items;
        }



        private async void AddButton_CLick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            addingNewProduct addingNewProduct1 = new addingNewProduct();
            await addingNewProduct1.ShowDialog(this);
            LoadData(agentIdgl);
            this.Close();
        }
    }
}