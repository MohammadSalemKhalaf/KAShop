namespace KAShop.DTO.Responses

{
    public class CategoryResponseDTO
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Status { get; set; }   // or use Status enum if you prefer
        public List<CategoryTranslationResponse> Translations { get; set; }
    }
}
