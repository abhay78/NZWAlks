using System.ComponentModel.DataAnnotations;

namespace NZWalksAPI.Models.DTO
{
    public class AddRegionRequestDto
    {
        [Required]
        [MinLength(3,ErrorMessage ="MinimumLength will be 3") ]
        [MaxLength (5,ErrorMessage="Maximum Length will be 5")]
        public string Code { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "MinimumLength will be 100")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
