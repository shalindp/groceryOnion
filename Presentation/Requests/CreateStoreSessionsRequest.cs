using System.ComponentModel.DataAnnotations;

namespace Presentation.Requests;

public class CreateStoreSessionsRequest
{
    [Required] public string[] WoolworthsStoresAddressIds { get; set; }
    
    [Required] bool ShouldCreateForPaknSave { get; set; }
}