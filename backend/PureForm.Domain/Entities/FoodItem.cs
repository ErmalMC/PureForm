using PureForm.Domain.Common;
using PureForm.Domain.Enums;

namespace PureForm.Domain.Entities;

public class FoodItem : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public FoodCategory Category { get; set; }
    public decimal CaloriesPer100g { get; set; }
    public decimal ProteinPer100g { get; set; }
    public decimal CarbsPer100g { get; set; }
    public decimal FatsPer100g { get; set; }
    public string DefaultUnit { get; set; } = "g";
}