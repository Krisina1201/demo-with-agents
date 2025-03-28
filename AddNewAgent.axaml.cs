using AgentsSecond.Models;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Linq;
using System.Numerics;

namespace AgentsSecond;


public partial class AddNewAgent : Window
{
    public bool isnew;
    public Agent partner1;
    public AddNewAgent()
    {
        isnew = true;
        InitializeComponent();
    }

    public AddNewAgent(Agent partner)
    {
        using var context = new AkapylkaContext();

        isnew = false;
        InitializeComponent();
        gridEdit.DataContext = partner;
        partner1 = partner;

        type.SelectedItem = context.AgentTypes.Select(e => e.Title == partner.Title);
        title.Text = partner.Title;
        inn.Text = partner.Inn;
        phone.Text = partner.Phone;
        mail.Text = partner.Email;
        addres.Text = partner.Address;
        priority.Text = partner.Priority.ToString();
    }

    private void OkButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        bool check = true;
        using var context = new AkapylkaContext();

        if (isnew)
        {
            if (context.Agents.Count()>0)
            {
                partner1.Id = context.Agents.OrderBy(x => x.Id).Last().Id+1;
            } else
            {
                partner1.Id = 1;
            }

            if (title.Text.Length > 0)
            {
                partner1.Title = title.Text;
            } else
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

            if (inn.Text.Length > 0)
            {
                partner1.Inn = inn.Text;
            }
            else
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

            if (phone.Text.Length > 0)
            {
                partner1.Phone = phone.Text;
            }
            else
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

            if (mail.Text.Length > 0)
            {
                partner1.Email = mail.Text;
            }
            else
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

            if (addres.Text.Length > 0)
            {
                partner1.Address = addres.Text;
            }
            else
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

            if (priority.Text.Length > 0)
            {
                partner1.Priority = int.Parse(priority.Text);
            }
            else
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

            if (kpp.Text.Length > 0)
            {
                partner1.Kpp = kpp.Text;
            }
            else
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

            if (Name.Text.Length > 0)
            {
                partner1.DirectorName = Name.Text;
                context.Agents.Add(partner1);
            }
            else
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


        }
    }
}