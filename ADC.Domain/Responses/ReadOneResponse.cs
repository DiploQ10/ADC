using System.Text.Json.Serialization;

namespace ADC.Domain.Responses;

public class ReadOneResponse<M> : ResponseBase
{

    /// <summary>
    /// Modelo
    /// </summary>
    public M Model { get; set; }

    /// <summary>
    /// Constructor
    /// </summary>
    public ReadOneResponse()
    {
        Response = Responses.Undefined;
        Model = default!;
    }

    public ReadOneResponse(M model)
    {
        Response = Responses.Undefined;
        Model = model;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    public ReadOneResponse(Responses response = Responses.Undefined)
    {
        Response = response;
        Model = default!;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    [JsonConstructor]
    public ReadOneResponse(Responses response, M model)
    {
        Response = response;
        Model = model;
    }

}