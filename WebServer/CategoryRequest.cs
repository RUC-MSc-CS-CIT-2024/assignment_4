using System.ComponentModel.DataAnnotations;

public class CategoryRequest
{
    public required string Name { get; set; }
    public required string Description { get; set; }
}