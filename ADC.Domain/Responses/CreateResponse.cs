namespace ADC.Domain.Responses;

/// <summary>
/// Respuesta de 'Crear'
/// </summary>
public class CreateResponse : ResponseBase
{

    /// <summary>
    /// ID del elemento
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nueva respuesta 
    /// </summary>
    public CreateResponse()
    {
        Response = Responses.Undefined;
    }

    /// <summary>
    /// Nueva respuesta 
    /// </summary>
    /// <param name="response">Respuesta</param>
    public CreateResponse(Responses response, Guid lastId, string message = "")
    {
        Response = response;
        Message = message;
        Id = lastId;
    }

}