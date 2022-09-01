using System;
namespace PrescribeAPI.Models
{
    public class PrescriptionItem
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public bool IsPrescribed { get; set; }
        public string? TAJNumber { get; set; }

        public PrescriptionItem()
        {      
        }
    }
}

