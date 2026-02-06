using System.ComponentModel.DataAnnotations;

namespace Presentation.Requests;

public class SelectStoresRequest
{
    [Required] private int[] WoolworthsStoresAddressIds { get; set; }
    
    [Required] bool ShouldCreateForPaknSave { get; set; }
}