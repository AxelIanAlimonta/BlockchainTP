using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SistemaDeVotacion.BlockchainServicio;
using SistemaDeVotacion.web.Models;

namespace SistemaDeVotacion.web.Controllers;
[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly VotingService _votingService;
    private readonly UserService _userService;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public HomeController(VotingService votingService, ILogger<HomeController> logger, SignInManager<IdentityUser> signInManager, UserService userService, UserManager<IdentityUser> userManager)
    {
        _votingService = votingService;
        _logger = logger;
        _signInManager = signInManager;
        _userService = userService;
        _userManager = userManager;
    }


    public async Task<IActionResult> Index()
    {
        var userWithWallet = await _userService.GetUserWithWalletAsync(User);

        // Pasar la información al ViewBag
        ViewBag.userWithWallet = userWithWallet;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddWallet(string walletAddress)
    {
        var user = await _signInManager.UserManager.GetUserAsync(User);
        var result = await _userService.AddWalletAsync(user, walletAddress);

        if (result)
        {
            // Wallet añadida correctamente
            return RedirectToAction("Index");
        }
        else
        {
            // Manejo de errores si no se pudo añadir la wallet
            ModelState.AddModelError("", "No se pudo añadir la dirección de la wallet.");
            return View("Index");
        }
    }




    public async Task<IActionResult> ListaDeCandidatos()
    {
        var candidates = new List<CandidateViewModel>();

        const uint cantCandidatos = 3;

        for (uint i = 0; i < cantCandidatos; i++)
        {
            var name = await _votingService.GetCandidateNameAsync(i);
            var voteCount = await _votingService.GetCandidateVoteCountAsync(i);

            candidates.Add(new CandidateViewModel
            {
                Id = i,
                NombreCandidato = name,
                CantidadDeVotos = voteCount
            });
        }

        return View(candidates);
    }

    public async Task<IActionResult> ElegirCandidato()
    {
        var candidates = new List<CandidateViewModel>();
        const uint cantCandidatos = 3;

        for (uint i = 0; i < cantCandidatos; i++)
        {
            var name = await _votingService.GetCandidateNameAsync(i);
            var voteCount = await _votingService.GetCandidateVoteCountAsync(i);

            candidates.Add(new CandidateViewModel
            {
                Id = i,
                NombreCandidato = name,
                CantidadDeVotos = voteCount
            });
        }

        return View(candidates);
    }

    public async Task<IActionResult> Votar(uint candidateId)
    {
        try
        {
            var userWithWallet = await _userService.GetUserWithWalletAsync(User);

            // Llamar al servicio de votaci�n para registrar el voto
            await _votingService.VoteAsync(candidateId, userWithWallet.WalletAddress);
            return RedirectToAction("MensajeDelSistema", new { msj = "Voto registrado exitosamente." });
        }
        catch (Exception ex)
        {
            return RedirectToAction("MensajeDelSistema", new { msj = $"Ocurri� un error al registrar el voto: {ex.Message}" });
        }
    }


    public IActionResult MensajeDelSistema(string msj)
    {
        ViewBag.Mensaje = msj;
        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    public async Task<IActionResult> Transaccion(int? id)
    {
        var blockData = await _votingService.Test();

        if (blockData == null || blockData.Count < 2)
        {
            return NotFound(); // O manejar el error de otra forma
        }

        var contractAddress = Environment.GetEnvironmentVariable("CONTRACT_ADDRESS");

        var bloques = blockData.ToList();
        var ListT = new List<TransactionModelView>();

        for (int i = 0; i < bloques.Count(); i++)
        {
            var blockIndividual = blockData.Skip(i).First();
            var transactionsIn = blockIndividual.GetType().GetProperty("Transactions").GetValue(blockIndividual, null) as IEnumerable<object>;

            foreach (var tx in (IEnumerable<object>)transactionsIn)
            {
                TransactionModelView transactionModelView = new TransactionModelView
                {
                    TxHash = (string)tx.GetType().GetProperty("TxHash").GetValue(tx, null),
                    From = (string)tx.GetType().GetProperty("From").GetValue(tx, null),
                    To = (string)tx.GetType().GetProperty("To").GetValue(tx, null),
                };

                if (transactionModelView.To != null && transactionModelView.To.Equals(contractAddress.ToLower()))
                {
                    ListT.Add(transactionModelView);
                }
            }

        }
        return View(ListT);
    }

    public async Task<IActionResult> ListaDeVotantes()
    {
        var votantes = await _votingService.GetListVotantes();
        
        var votantesViewModel = new List<VoterViewModel>();

        foreach (var votante in votantes)
        {
            
            var candidateName = await _votingService.GetCandidateNameAsync((uint)votante.CandidateVoted);

            
            votantesViewModel.Add(new VoterViewModel
            {
                AddressVoter = votante.AddressVoter,
                CandidateVoted = votante.CandidateVoted,
                CandidateName = candidateName 
            });
        }

        return View(votantesViewModel);
    }

}
