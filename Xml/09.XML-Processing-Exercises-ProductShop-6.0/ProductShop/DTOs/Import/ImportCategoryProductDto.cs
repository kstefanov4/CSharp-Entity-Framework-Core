using System.Xml.Serialization;

namespace ProductShop.DTOs.Import
{
    [XmlType("CategoryProduct")]
    public class ImportCategoryProductDto
    {
        [XmlElement(nameof(CategoryId))]
        public int CategoryId { get; set; }
        [XmlElement(nameof(ProductId))]
        public int ProductId { get; set; }
    }
}
