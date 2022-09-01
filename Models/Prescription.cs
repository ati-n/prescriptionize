namespace PrescribeAPI.Models
{
    public record Prescription(int Id)
    {
        public string Name { get; set; } = default!;
        public bool IsPrescribed { get; set; } = default!;
        public string TAJNumber { get; set; } = default!;
    }
}

