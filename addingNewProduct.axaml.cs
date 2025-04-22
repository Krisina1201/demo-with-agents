using AgentsSecond.Models;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Linq;

namespace AgentsSecond;

public partial class addingNewProduct : Window
{
    public AkapylkaContext context = new AkapylkaContext();
    public addingNewProduct()
    {
        InitializeComponent();

        AgentsCombobox.ItemsSource = context.Agents.
            Select(e => e.Title)
            .OrderBy(e => e).ToList();
        ProductCombobox.ItemsSource = context.Products.Select(e => e.Title).OrderBy(e => e).ToList();
    }

    private void AddProductClick (object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        string agent = AgentsCombobox.SelectedItem.ToString();
        string product = ProductCombobox.SelectedItem.ToString();
        DateOnly date = DateOnly.Parse(DataBox.Text);
        int count = int.Parse(CountBox.Text);

        int agentId = context.Agents.FirstOrDefault(e => e.Title == agent).Id;
        int productId = context.Products.FirstOrDefault(e => e.Title == product).Id;

        ProductSale chelick = new ProductSale
        {
            Id = context.ProductSales.Select(e => e.Id).Count() + 2,
            ProductId = productId,
            AgentId = agentId,
            SaleDate = date,
            ProductCount = count
        };

        context.ProductSales.Add(chelick);
        context.SaveChanges();
        Close(this);
    }
    
}