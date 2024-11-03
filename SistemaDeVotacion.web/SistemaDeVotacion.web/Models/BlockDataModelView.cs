namespace SistemaDeVotacion.web.Models
{
    public class BlockDataModelView
    {
        public ulong BlockNumber { get; set; }
        public string Hash { get; set; }
        public string Miner { get; set; }
        public ulong Timestamp { get; set; }
        public List<TransactionModelView> Transactions { get; set; }
    }
}
