using System.ComponentModel.DataAnnotations;

namespace NewsAggregation.Models.DTO
{
    public class AddModeratedKeywordDto
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Keyword { get; set; }
    }
}