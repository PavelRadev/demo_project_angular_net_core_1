using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class SingleValueModel<T> where T: class 
    {
        [Required]
        public T Value { get; set; }
    }
}