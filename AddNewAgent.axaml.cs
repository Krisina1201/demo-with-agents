using AgentsSecond.Models;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace AgentsSecond;



public partial class AddNewAgent : Window, INotifyPropertyChanged
{
    public bool isnew;
    public Agent partner1;
    public Agent agentSecond;
    public int idNewAgent = -1;
    public bool checkUpdate { get; private set; }

    public int changePriority;
    public int newPriority;

    public ICollection<ProductSale> ProductSales { get; set; }

    private bool _isButtonVisible;
    public bool IsButtonVisible
    {
        get => _isButtonVisible;
        set
        {
            if (_isButtonVisible != value)
            {
                _isButtonVisible = value;
                OnPropertyChanged();
            }
        }
    }
    
    private bool _isButtonVisibleGet;
    public bool IsButtonVisibleGet
    {
        get => _isButtonVisibleGet;
        set
        {
            if (_isButtonVisibleGet != value)
            {
                _isButtonVisibleGet = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public AddNewAgent()
    {
        isnew = true;
        InitializeComponent();
        checkUpdate = false;
        this.DataContext = this;
    }

    public AddNewAgent(Agent partner)
    {
        IsButtonVisible = false;
        IsButtonVisibleGet = false;
        using var context = new AkapylkaContext();
        this.DataContext = this;

        isnew = false;
        InitializeComponent();
        gridEdit.DataContext = partner;
        partner1 = partner;

        type.SelectedItem = partner.AgentType;
        title.Text = partner.Title;
        inn.Text = partner.Inn;
        phone.Text = partner.Phone;
        mail.Text = partner.Email;
        addres.Text = partner.Address;
        priority.Text = partner.Priority.ToString();
        kpp.Text = partner.Kpp;
        name.Text = partner.DirectorName;

        idNewAgent = partner.Id;
        checkUpdate = false;

        changePriority = partner.Priority;
    }

    private void OkButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        bool check = true;
        using var context = new AkapylkaContext();

        var n = title.Text;


        if (title.Text == null)
        {

            var dialog = new Window
            {
                Title = "Ошибка",
                Content = new TextBlock { Text = "Заголовок не может быть пустым" },
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            dialog.ShowDialog(this);
        }

        if (inn.Text == null)
        {
            var dialog = new Window
            {
                Title = "Ошибка",
                Content = new TextBlock { Text = "Поле инн не заполнено" },
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            dialog.ShowDialog(this);
        }
        

        if (phone.Text == null)
        {
            var dialog = new Window
            {
                Title = "Ошибка",
                Content = new TextBlock { Text = "Поле номер телефона не заполнено" },
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            dialog.ShowDialog(this);
        }

        if (mail.Text == null)
        {
            var dialog = new Window
            {
                Title = "Ошибка",
                Content = new TextBlock { Text = "Поле почта не заполнено" },
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            dialog.ShowDialog(this);
        }

        if (addres.Text == null)
        {
            var dialog = new Window
            {
                Title = "Ошибка",
                Content = new TextBlock { Text = "Поле адрес не заполнено" },
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            dialog.ShowDialog(this);
        }


        if (priority.Text == null)
        {
            var dialog = new Window
            {
                Title = "Ошибка",
                Content = new TextBlock { Text = "Поле приоритет не заполнено" },
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            dialog.ShowDialog(this);
        }

        if (kpp.Text == null)
        {
            var dialog = new Window
            {
                Title = "Ошибка",
                Content = new TextBlock { Text = "Поле кпп не заполнено" },
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            dialog.ShowDialog(this);
        }

        if (name.Text == null)
        {
            var dialog = new Window
            {
                Title = "Ошибка",
                Content = new TextBlock { Text = "Поле имя не заполнено" },
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            dialog.ShowDialog(this);
        }

        

        try 
        {
            if (isnew)
            {
                var newAgent = new Agent
                {
                    Title = title.Text,
                    Inn = inn.Text,
                    Phone = phone.Text,
                    Email = mail.Text,
                    Address = addres.Text,
                    Priority = int.Parse(priority.Text),
                    Kpp = kpp.Text,
                    DirectorName = name.Text,
                    AgentTypeId = (type.SelectedItem as AgentType)?.Id ?? 1
                };

                idNewAgent = newAgent.Id;

                agentSecond = newAgent;

                context.Agents.Add(newAgent);
            }
            else
            {
                var existingAgent = context.Agents.FirstOrDefault(e => e.Id == idNewAgent);
                if (existingAgent == null)
                {
                    ShowError("Агент не найден");
                    return;
                }

                existingAgent.Title = title.Text;
                existingAgent.Inn = inn.Text;
                existingAgent.Phone = phone.Text;
                existingAgent.Email = mail.Text;
                existingAgent.Address = addres.Text;
                existingAgent.Priority = int.Parse(priority.Text);
                existingAgent.Kpp = kpp.Text;
                existingAgent.DirectorName = name.Text;
                existingAgent.AgentTypeId = (type.SelectedItem as AgentType)?.Id ?? existingAgent.AgentTypeId;

                idNewAgent = existingAgent.Id;
                newPriority = existingAgent.Priority;
                agentSecond = existingAgent;

                context.Update(existingAgent); 
            }

            if (changePriority != newPriority)
            {
                context.AgentPriorityHistories.Add(new AgentPriorityHistory
                {
                    AgentId = idNewAgent,
                    ChangeDate = DateTime.Now,
                    PriorityValue = newPriority
                });
            }

            context.SaveChanges();
            checkUpdate = true;
            Close(this);
        } catch
        {
            var dialog = new Window
            {
                Title = "Ошибка",
                Content = new TextBlock { Text = "Ошибка при сохранении" },
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };


            dialog.ShowDialog(this);
        }
    }

    private void ShowError(string message)
    {
        var dialog = new Window
        {
            Title = "Ошибка",
            Content = new TextBlock { Text = message },
            SizeToContent = SizeToContent.WidthAndHeight,
            WindowStartupLocation = WindowStartupLocation.CenterScreen
        };
        dialog.ShowDialog(this);
    }

    private void Delete_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        
    }
    private void Hisory_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        CheckHistory checkHistory = new CheckHistory(idNewAgent);
        checkHistory.Show();
    }
        
    private void BackButton(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close(this);
    }

    public void DeleteAgentWithRelatedData(int id)
    {
        //using var context = new AkapylkaContext();
        //using var transaction = context.Database.BeginTransaction();

        //try
        //{
        //    var agent = context.Agents
        //        .Include(a => a.ProductSales)
        //        .Include(e => e.AgentPriorityHistories)
        //        .FirstOrDefault(e => e.Id == agentSecond.Id);

        //    if (agent == null) return;

        //    foreach (var sale in agent.ProductSales)
        //    {
        //        sale.Agent = null;
        //        context.Entry(sale).State = EntityState.Modified;
        //    }

        //    context.OtherEntities.RemoveRange(agent.OtherEntities);
        //}
    }

}
