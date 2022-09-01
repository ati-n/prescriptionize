using System;
namespace PrescribeAPI.Models
{
    public class PrescriptionItemDTO
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public bool IsPrescribed { get; set; }
    }
}

