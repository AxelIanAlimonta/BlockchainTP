using System.Numerics;

namespace SistemaDeVotacion.web.Models;

public class BlockDataModelView
{
    public int BlockNumber { get; set; }
    public string Hash { get; set; }
    public string Miner { get; set; }
    public BigInteger Timestamp { get; set; }

    //public TransactionModelView Transaction { get; set; }
}
