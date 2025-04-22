using AgentsSecond.Models;
using Avalonia.Controls;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace AgentsSecond
{
    class ViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }
        public int Count { get; set; }
    }

    public partial class CheckHistory : Window
    {

        public int agentIdgl;
        private ObservableCollection<ViewModel> _sales = new ObservableCollection<ViewModel>();

        public CheckHistory()
        {
            InitializeComponent();
            HistoryListBox.ItemsSource = _sales; 
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
               .Select(e => new ViewModel
               {
                   Id = e.Id,
                   Title = e.Product.Title,
                   Date = e.SaleDate.ToString("dd.MM.yyyy"),
                   Count = e.ProductCount
               }).ToList();

            _sales.Clear();
            foreach (var item in items)
            {
                _sales.Add(item);
            }
        }



        private async void AddButton_CLick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            addingNewProduct addingNewProduct1 = new addingNewProduct();
            await addingNewProduct1.ShowDialog(this);
            LoadData(agentIdgl);
            this.Close();
        }
        
        private async void BackButton(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            AddNewAgent addNewAgent = new AddNewAgent();
            Close(addNewAgent);
            Close(this);
        }


        private async void DeleteProductClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var button = (Button)sender;
            var sale = (ViewModel)button.DataContext;

            using (var context = new AkapylkaContext())
            {
                var dbSale = context.ProductSales.FirstOrDefault(ps => ps.Id == sale.Id);
                if (dbSale != null)
                {
                    context.ProductSales.Remove(dbSale);
                    context.SaveChanges();
                }
            }

            _sales.Remove(sale);
        }
    }
}