using AgentsSecond.Models;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Numerics;

namespace AgentsSecond;



public partial class AddNewAgent : Window
{
    public bool isnew;
    public Agent partner1;
    public int idNewAgent = -1;
    public bool checkUpdate { get; private set; }

    public bool IsButtonVisible { get; set; } = true;

    public AddNewAgent()
    {
        isnew = true;
        InitializeComponent();
        checkUpdate = false;
    }

    public AddNewAgent(Agent partner)
    {
        IsButtonVisible = false;
        using var context = new AkapylkaContext();

        isnew = false;
        InitializeComponent();
        gridEdit.DataContext = partner;
        partner1 = partner;

        type.SelectedItem = context.AgentTypes.FirstOrDefault(e => e.Title == partner.Title);
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
        
    }

    private void OkButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        bool check = true;
        using var context = new AkapylkaContext();


        if (title.Text.Length == 0)
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

        if (inn.Text.Length == 0)
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
        

        if (phone.Text.Length == 0)
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

        if (mail.Text.Length == 0)
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

        if (addres.Text.Length == 0)
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


        if (priority.Text.Length == 0)
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

        if (kpp.Text.Length == 0)
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

        if (name.Text.Length == 0)
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

            context.Update(existingAgent); 
        }

        context.SaveChanges();
        checkUpdate = true;
        Close(this);
        MainWindow mainWindow = new MainWindow();
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

    private void Exit_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show(this);
        Close(this);
    }
    private void Hisory_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        CheckHistory checkHistory = new CheckHistory(idNewAgent);
        checkHistory.Show();
    }

}
