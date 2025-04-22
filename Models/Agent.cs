using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;

namespace AgentsSecond.Models;

public partial class Agent
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int AgentTypeId { get; set; }

    public string? Address { get; set; }

    public string Inn { get; set; } = null!;

    public string? Kpp { get; set; }

    public string? DirectorName { get; set; }

    public string Phone { get; set; } = null!;

    public string? Email { get; set; }

    public string? Logo { get; set; }

    public int Priority { get; set; }


    public Bitmap? LogoImage
    {
        get
        {
            try
            {
                return new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "/" + Logo);
            }
            catch
            {
                return null;
            }
        }
    }
    public virtual ICollection<AgentPriorityHistory> AgentPriorityHistories { get; set; } = new List<AgentPriorityHistory>();

    public virtual AgentType AgentType { get; set; }

    public virtual ICollection<ProductSale> ProductSales { get; set; } = new List<ProductSale>();

    public virtual ICollection<Shop>? Shops { get; set; } = new List<Shop>();
}
