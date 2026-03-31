namespace PureForm.Application.DTOs;

public class FoodItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    /// <summary>String representation of FoodCategory enum for API serialization</summary>
    public string Category { get; set; } = string.Empty;
    public decimal CaloriesPer100g { get; set; }
    public decimal ProteinPer100g { get; set; }
    public decimal CarbsPer100g { get; set; }
    public decimal FatsPer100g { get; set; }
    public string DefaultUnit { get; set; } = "g";
}

public class CreateFoodItemDto
{
    public string Name { get; set; } = string.Empty;
    /// <summary>String representation of FoodCategory enum</summary>
    public string Category { get; set; } = string.Empty;
    public decimal CaloriesPer100g { get; set; }
    public decimal ProteinPer100g { get; set; }
    public decimal CarbsPer100g { get; set; }
    public decimal FatsPer100g { get; set; }
    public string? DefaultUnit { get; set; } = "g";
}