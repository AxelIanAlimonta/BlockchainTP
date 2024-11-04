using System.Numerics;

namespace SistemaDeVotacion.web.Models
{
    public class VoterViewModel
    {
        public string AddressVoter { get; set; }
        public bool HasVoted { get; set; }
        public BigInteger CandidateVoted { get; set; }
        public string CandidateName { get; set; }
    }
}
