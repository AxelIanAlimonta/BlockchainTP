using System.Threading.Tasks;
using Nethereum.Web3;
using Nethereum.Contracts;
using Nethereum.Web3.Accounts;
using Nethereum.Hex.HexTypes; // Asegúrate de tener este using
using Nethereum.RPC.Eth.DTOs;

public class VotingService
{
    private readonly Web3 _web3;
    private readonly string _contractAddress;

    public VotingService(string rpcUrl, string contractAddress)
    {
        // Crear conexión con Ganache
        _web3 = new Web3(rpcUrl);
        _contractAddress = contractAddress;
    }

    // ABI y dirección del contrato
    private const string abi = @"[
        {
            ""inputs"": [],
            ""stateMutability"": ""nonpayable"",
            ""type"": ""constructor""
        },
        {
            ""inputs"": [
                {
                    ""internalType"": ""uint256"",
                    ""name"": """",
                    ""type"": ""uint256""
                }
            ],
            ""name"": ""candidates"",
            ""outputs"": [
                {
                    ""internalType"": ""string"",
                    ""name"": ""name"",
                    ""type"": ""string""
                },
                {
                    ""internalType"": ""uint256"",
                    ""name"": ""voteCount"",
                    ""type"": ""uint256""
                }
            ],
            ""stateMutability"": ""view"",
            ""type"": ""function"",
            ""constant"": true
        },
        {
            ""inputs"": [
                {
                    ""internalType"": ""address"",
                    ""name"": """",
                    ""type"": ""address""
                }
            ],
            ""name"": ""voters"",
            ""outputs"": [
                {
                    ""internalType"": ""bool"",
                    ""name"": ""hasVoted"",
                    ""type"": ""bool""
                },
                {
                    ""internalType"": ""uint256"",
                    ""name"": ""candidateVoted"",
                    ""type"": ""uint256""
                }
            ],
            ""stateMutability"": ""view"",
            ""type"": ""function"",
            ""constant"": true
        },
        {
            ""inputs"": [
                {
                    ""internalType"": ""uint256"",
                    ""name"": ""candidateId"",
                    ""type"": ""uint256""
                }
            ],
            ""name"": ""vote"",
            ""outputs"": [],
            ""stateMutability"": ""nonpayable"",
            ""type"": ""function""
        },
        {
            ""inputs"": [
                {
                    ""internalType"": ""uint256"",
                    ""name"": ""candidateId"",
                    ""type"": ""uint256""
                }
            ],
            ""name"": ""getCandidateVoteCount"",
            ""outputs"": [
                {
                    ""internalType"": ""uint256"",
                    ""name"": """",
                    ""type"": ""uint256""
                }
            ],
            ""stateMutability"": ""view"",
            ""type"": ""function"",
            ""constant"": true
        },
        {
            ""inputs"": [
                {
                    ""internalType"": ""uint256"",
                    ""name"": ""candidateId"",
                    ""type"": ""uint256""
                }
            ],
            ""name"": ""getCandidateName"",
            ""outputs"": [
                {
                    ""internalType"": ""string"",
                    ""name"": """",
                    ""type"": ""string""
                }
            ],
            ""stateMutability"": ""view"",
            ""type"": ""function"",
            ""constant"": true
        }
    ]";

    public async Task VoteAsync(uint candidateId, string accountPrivateKey)
    {
        var account = new Account(accountPrivateKey);
        var contract = _web3.Eth.GetContract(abi, _contractAddress);
        var voteFunction = contract.GetFunction("vote");

        // Cambiar Hexuint a HexBigInteger
        var transactionInput = voteFunction.CreateTransactionInput(
            account.Address,
            new HexBigInteger(300000),
            new HexBigInteger(0),
            candidateId
        );

        var transactionHash = await _web3.Eth.Transactions.SendTransaction.SendRequestAsync(transactionInput);

        // Esperar la confirmación de la transacción
        await WaitForTransactionReceiptAsync(transactionHash);
    }

    private async Task<TransactionReceipt> WaitForTransactionReceiptAsync(string transactionHash)
    {
        TransactionReceipt receipt = null;
        while (receipt == null)
        {
            receipt = await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);
            if (receipt == null)
            {
                // Esperar 1 segundo antes de volver a consultar
                await Task.Delay(1000);
            }
        }
        return receipt;
    }

    public async Task<string> GetCandidateNameAsync(uint candidateId)
    {
        var contract = _web3.Eth.GetContract(abi, _contractAddress);
        var getCandidateNameFunction = contract.GetFunction("getCandidateName");

        return await getCandidateNameFunction.CallAsync<string>(candidateId);
    }

    public async Task<uint> GetCandidateVoteCountAsync(uint candidateId)
    {
        var contract = _web3.Eth.GetContract(abi, _contractAddress);
        var getCandidateVoteCountFunction = contract.GetFunction("getCandidateVoteCount");

        return await getCandidateVoteCountFunction.CallAsync<uint>(candidateId);
    }
}



//public async Task VoteAsync(string accountAddress, string privateKey, uint candidateId)
//{
//    // Crear cuenta del votante
//    var account = new Account(privateKey);

//    // Crear conexión con Ganache usando la cuenta
//    var web3 = new Web3(account, _web3.Client);

//    // Referencia al contrato
//    var contract = web3.Eth.GetContract(abi, _contractAddress);

//    // Llamar al método de votación
//    var voteFunction = contract.GetFunction("vote");

//    // Establecer el gas y gasPrice (ajusta estos valores según sea necesario)
//    var gas = new Hexuint(6000000); // Ajusta la cantidad de gas que estás dispuesto a usar
//    var gasPrice = new Hexuint(20000000000); // Gas price en Wei (20 Gwei)

//    try
//    {
//        // Llamar a la función vote y esperar la transacción
//        var transactionHash = await voteFunction.SendTransactionAsync(accountAddress, gas, gasPrice, candidateId);

//        // Esperar la confirmación de la transacción
//        var receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);

//        Console.WriteLine($"Transacción completada. Hash: {transactionHash}");
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"Ocurrió un error al registrar el voto: {ex.Message}");
//    }
//}


//// Obtener el conteo de votos de un candidato
//public async Task<uint> GetCandidateVoteCountAsync(uint candidateId)
//{
//    // Referencia al contrato
//    var contract = _web3.Eth.GetContract(abi, _contractAddress);

//    // Obtener la función para obtener el conteo de votos
//    var voteCountFunction = contract.GetFunction("getCandidateVoteCount");

//    // Llamar a la función del contrato y devolver el conteo de votos
//    return await voteCountFunction.CallAsync<uint>(candidateId);
//}

//// Obtener el nombre de un candidato por su ID
//public async Task<string> GetCandidateNameAsync(uint candidateId)
//{
//    // Referencia al contrato
//    var contract = _web3.Eth.GetContract(abi, _contractAddress);

//    // Obtener la función para obtener el nombre del candidato
//    var getNameFunction = contract.GetFunction("getCandidateName");

//    // Llamar a la función del contrato y devolver el nombre del candidato
//    return await getNameFunction.CallAsync<string>(candidateId);
//}


