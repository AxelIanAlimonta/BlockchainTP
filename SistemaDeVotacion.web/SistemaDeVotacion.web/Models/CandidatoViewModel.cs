namespace SistemaDeVotacion.web.Models;


public class CandidateViewModel
{
    public uint Id { get; set; }
    public required string NombreCandidato { get; set; }
    public uint CantidadDeVotos { get; set; }
}


