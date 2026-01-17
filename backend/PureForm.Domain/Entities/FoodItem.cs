// ============================================================
// PureForm.Domain/Entities/FoodItem.cs - NEW
// ============================================================
namespace PureForm.Domain.Entities;

public class FoodItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; // Protein, Carbs, Vegetables, Fruits, etc.
    public decimal CaloriesPer100g { get; set; }
    public decimal ProteinPer100g { get; set; }
    public decimal CarbsPer100g { get; set; }
    public decimal FatsPer100g { get; set; }
    public string DefaultUnit { get; set; } = "g"; // g, oz, cup, piece
    public bool IsPopular { get; set; } // For quick access
    public DateTime CreatedAt { get; set; }
}