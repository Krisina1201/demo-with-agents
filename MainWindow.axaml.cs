using AgentsSecond.Models;
using Avalonia.Controls;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AgentsSecond;

public class AgentListBox
{
    public string Title { get; set; }
    public int NumberSales { get; set; }
    public string Discount { get; set; }
    public string Phone { get; set; }
    public string AgentType { get; set; }
    public int Priority { get; set; }
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
            Priority = it.Priority
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
        var selectedItem = discountCombox.SelectedItem;

        switch (selectedItem)
        {
            case "По возрастанию":
                var sortedAscending = services
                    .OrderBy(s => int.Parse(s.Discount.Replace("%", "")))
                    .ToList();
                services.Clear();
                foreach (var item in sortedAscending)
                {
                    services.Add(item);
                }
                break;
            case "По убыванию":
                var sortedDescending = services
                    .OrderByDescending(s => int.Parse(s.Discount.Replace("%", "")))
                    .ToList();
                services.Clear();
                foreach (var item in sortedDescending)
                {
                    services.Add(item);
                }
                break;
            default:
                var unsortedServices = LoadServicesFromDatabase();
                services.Clear();
                foreach (var item in unsortedServices)
                {
                    services.Add(item);
                }
                break;
        }
    }

    public void sortComboxByTitle(object sender, SelectionChangedEventArgs e)
    {
        var selectedItem = titleCombox.SelectedItem;

        switch (selectedItem)
        {
            case "По возрастанию":
                var sortedAscending = services
                    .OrderBy(s => s.Title)
                    .ToList();
                services.Clear();
                foreach (var item in sortedAscending)
                {
                    services.Add(item);
                }
                break;
            case "По убыванию":
                var sortedDescending = services
                    .OrderByDescending(s => s.Title)
                    .ToList();
                services.Clear();
                foreach (var item in sortedDescending)
                {
                    services.Add(item);
                }
                break;
            default:
                var unsortedServices = LoadServicesFromDatabase();
                services.Clear();
                foreach (var item in unsortedServices)
                {
                    services.Add(item);
                }
                break;
        }
    }

    public void sortComboxByPriority(object sender, SelectionChangedEventArgs e)
    {
        var selectedItem = priorityCombox.SelectedItem;

        switch (selectedItem)
        {
            case "По возрастанию":
                var sortedAscending = services
                    .OrderBy(s => s.Priority)
                    .ToList();
                services.Clear();
                foreach (var item in sortedAscending)
                {
                    services.Add(item);
                }
                break;
            case "По убыванию":
                var sortedDescending = services
                    .OrderByDescending(s => s.Priority)
                    .ToList();
                services.Clear();
                foreach (var item in sortedDescending)
                {
                    services.Add(item);
                }
                break;
            default:
                var unsortedServices = LoadServicesFromDatabase();
                services.Clear();
                foreach (var item in unsortedServices)
                {
                    services.Add(item);
                }
                break;
        }
    }

    private ObservableCollection<AgentListBox> LoadServicesFromDatabase()
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
            Priority = it.Priority
        }).ToList();

        return new ObservableCollection<AgentListBox>(dataAgent);
    }

    public int checkDiskount(decimal sum)
    {
        if (sum < 10000) return 0;
        if (sum < 50000) return 5;
        if (sum < 150000) return 10;
        if (sum < 500000) return 20;
        return 25;
    }
}