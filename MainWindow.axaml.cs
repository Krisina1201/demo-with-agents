using AgentsSecond.Models;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Metsys.Bson;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Npgsql.EntityFrameworkCore.PostgreSQL.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AgentsSecond;

public class AgentListBox
{
    public string Title { get; set; }
    public int NumberSales { get; set; }
    public string Discount { get; set; }
    public string Phone { get; set; }

    public string Logo { get; set; }
    public string AgentType { get; set; }
    public int Priority { get; set; }
    public string Email { get; set; }


    public Bitmap GetImage { get; set; }
}

public class checkDisc
{
    public int id { get; set; }
    public decimal sum { get; set; }
}

public partial class MainWindow : Window
{
    ObservableCollection<AgentListBox> services;
    ObservableCollection<string> typeList;
    List<AgentListBox> dataAgent;
    public MainWindow()
    {
        InitializeComponent();
        using var context = new AkapylkaContext();

        var agentsData = context.Agents
            .Include(a => a.ProductSales)
            .Include(a => a.AgentType)
            .ToList();

        var discountData = context.ProductSales
            .Include(ps => ps.Product)
            .GroupBy(e => e.AgentId)
            .Select(g => new checkDisc
            {
                id = g.Key,
                sum = g.Sum(e => e.ProductCount * e.Product.MinCostForAgent)
            })
            .ToList();

        dataAgent = agentsData.Select(it => new AgentListBox
        {
            Title = it.Title,
            NumberSales = it.ProductSales.Sum(ps => ps.ProductCount),
            Discount = checkDiskount(
                discountData.FirstOrDefault(t => t.id == it.Id)?.sum ?? 0
            ).ToString() + "%",
            Phone = it.Phone,
            AgentType = it.AgentType.Title,
            Priority = it.Priority,
            Email = it.Email,
            GetImage = it.LogoImage
        }).ToList();

        services = new ObservableCollection<AgentListBox>(dataAgent);
        AgentListBox.ItemsSource = services;

        var typeDirection = context.AgentTypes.Select(e => e.Title).ToList();
        var updatedTypes = typeDirection.Append("Все типы");
        typeList = new ObservableCollection<string>(updatedTypes);
        typeCombox.ItemsSource = typeList;

        discountCombox.SelectionChanged += sortComboxByDiscount;
        priorityCombox.SelectionChanged += sortComboxByPriority;
        titleCombox.SelectionChanged += sortComboxByTitle;
        typeCombox.SelectionChanged += sortComboboxByType;

        SearchBox.TextChanged += SearchBoxChanging;
    }

    public void sortComboboxByType(object sender, SelectionChangedEventArgs e)
    {
        if (typeCombox.SelectedItem == null) return;

        string selectedItem = typeCombox.SelectedItem.ToString();

        var allServices = LoadServicesFromDatabase();

        if (selectedItem == "Все типы")
        {
            services.Clear();
            foreach (var item in allServices)
            {
                services.Add(item);
            }
        } else{
            var sortedAscending = allServices
                .Where(e => e.AgentType == selectedItem)
                .ToList();
            services.Clear();
            foreach (var item in sortedAscending)
            {
                services.Add(item);
            }
        }
    }

    public void sortComboxByDiscount(object sender, SelectionChangedEventArgs e)
    {
        var selectedItem = discountCombox.SelectedItem as ComboBoxItem;
        string selectedText = selectedItem.Content?.ToString();

        if (selectedText == "По возрастанию")
        {
            var sortedAscending = services
                    .OrderBy(s => int.Parse(s.Discount.Replace("%", "")))
                    .ToList();
            services.Clear();
            foreach (var item in sortedAscending)
            {
                services.Add(item);
            }
        } else
        {
            var sortedDescending = services
                        .OrderByDescending(s => int.Parse(s.Discount.Replace("%", "")))
                        .ToList();
            services.Clear();
            foreach (var item in sortedDescending)
            {
                services.Add(item);
            }
        }
    }

    public void sortComboxByTitle(object sender, SelectionChangedEventArgs e)
    {
        var selectedItem = titleCombox.SelectedItem as ComboBoxItem;
        string selectedText = selectedItem.Content?.ToString();

        if (selectedText == "По возрастанию")
        {
            var sortedAscending = services
            .OrderBy(s => s.Title)
            .ToList();
            services.Clear();
            foreach (var item in sortedAscending)
            {
                services.Add(item);
            }
        }
        else
        {
            var sortedDescending = services
            .OrderByDescending(s => s.Title)
            .ToList();
            services.Clear();
            foreach (var item in sortedDescending)
            {
                services.Add(item);
            }
        }
    }

    public void sortComboxByPriority(object sender, SelectionChangedEventArgs e)
    {
        var selectedItem = priorityCombox.SelectedItem as ComboBoxItem;
        string selectedText = selectedItem.Content?.ToString();

        if (selectedText == "По возрастанию")
        {
            var sortedAscending = services
                    .OrderBy(s => s.Priority)
                    .ToList();
            services.Clear();
            foreach (var item in sortedAscending)
            {
                services.Add(item);
            }
        } 
        else
        {
            var sortedDescending = services
                        .OrderByDescending(s => s.Priority)
                        .ToList();
            services.Clear();
            foreach (var item in sortedDescending)
            {
                services.Add(item);
            }
        }
    }


    public void SearchBoxChanging(object sender, TextChangedEventArgs e)
    {
        var searchText = SearchBox.Text.ToLower();

        if (string.IsNullOrWhiteSpace(searchText))   
        {
            services.Clear();
            foreach (var item in dataAgent)
            {
                services.Add(item);
            }
        }
        else
        {
            var filteredService = dataAgent.Where(s => s.Email.ToLower().Contains(searchText) ||
                                                    (s.Phone.ToLower().Contains(searchText))).ToList();
            services.Clear();
            foreach(var item in filteredService)
            {
                services.Add(item);
            }
        }
    }



    public ObservableCollection<AgentListBox> LoadServicesFromDatabase()
    {
        using var context = new AkapylkaContext();
        var agentsData = context.Agents
            .Include(a => a.ProductSales)
            .Include(a => a.AgentType)
            .ToList();

        var discountData = context.ProductSales
            .Include(ps => ps.Product)
            .GroupBy(e => e.AgentId)
            .Select(g => new checkDisc
            {
                id = g.Key,
                sum = g.Sum(e => e.ProductCount * e.Product.MinCostForAgent)
            })
            .ToList();

        dataAgent = agentsData.Select(it => new AgentListBox
        {
            Title = it.Title,
            NumberSales = it.ProductSales.Sum(ps => ps.ProductCount),
            Discount = checkDiskount(
                discountData.FirstOrDefault(t => t.id == it.Id)?.sum ?? 0
            ).ToString() + "%",
            Phone = it.Phone,
            AgentType = it.AgentType.Title,
            Priority = it.Priority,
            Email = it.Email
        }).ToList();

        return new ObservableCollection<AgentListBox>(dataAgent);
    }

    public int checkDiskount(decimal sum)
    {
        if (sum < 10000) return 0;
        if (sum < 50000) return 5;
        if (sum < 150000) return 10;
        if (sum < 500000) return 20;
        else
        {

            //SaleTextBlock.Foreground = Brushes.LightGreen;
            return 25;
        }
    }

    private async void OnListBoxDoubleTapped(object sender, TappedEventArgs e)
    {
        if (AgentListBox.SelectedItem is AgentListBox selectedItem)
        {
            AkapylkaContext akapylkaContext = new AkapylkaContext();

            var agentSelect = akapylkaContext.Agents.FirstOrDefault(e => e.Email == selectedItem.Email);

            AddNewAgent detailWindow = new AddNewAgent(agentSelect!);
            await detailWindow.ShowDialog(this);
            loadData();
        }
    }

    public void loadData()
    {
        AgentListBox.ItemsSource = null;
        using var context = new AkapylkaContext();

        var agentsData = context.Agents
            .Include(a => a.ProductSales)
            .Include(a => a.AgentType)
            .ToList();

        var discountData = context.ProductSales
            .Include(ps => ps.Product)
            .GroupBy(e => e.AgentId)
            .Select(g => new checkDisc
            {
                id = g.Key,
                sum = g.Sum(e => e.ProductCount * e.Product.MinCostForAgent)
            })
            .ToList();

        dataAgent = agentsData.Select(it => new AgentListBox
        {
            Title = it.Title,
            NumberSales = it.ProductSales.Sum(ps => ps.ProductCount),
            Discount = checkDiskount(
                discountData.FirstOrDefault(t => t.id == it.Id)?.sum ?? 0
            ).ToString() + "%",
            Phone = it.Phone,
            AgentType = it.AgentType.Title,
            Priority = it.Priority,
            Email = it.Email
        }).ToList();

        services = new ObservableCollection<AgentListBox>(dataAgent);
        AgentListBox.ItemsSource = services;
    }

    public async void OpenAddAgentWindow(object? sender, RoutedEventArgs e)
    {
        AddNewAgent addNewAgent = new AddNewAgent();
        await addNewAgent.ShowDialog(this);
        this.Hide();
        loadData();
    }
}
