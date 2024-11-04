using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDeVotacion.BlockchainServicio
{
    public class VoterInfo
    {
        public string AddressVoter { get; set; }
        public bool HasVoted { get; set; }
        public BigInteger CandidateVoted { get; set; }
    }
}
