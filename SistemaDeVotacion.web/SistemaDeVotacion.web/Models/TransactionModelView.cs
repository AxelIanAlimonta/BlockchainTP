using System.Numerics;

namespace SistemaDeVotacion.web.Models;

public class TransactionModelView
{
    public string TxHash { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public string Value { get; set; } // Ajusta el tipo de datos según tus necesidades
}