using System.Diagnostics;
using System.Security.Policy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaDeVotacion.web.Models;

namespace SistemaDeVotacion.web.Controllers;
[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly VotingService _votingService;

    public HomeController(VotingService votingService, ILogger<HomeController> logger)
    {
        _votingService = votingService;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
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

    //public ActionResult Votar(uint candidateId)
    //{
    //    return RedirectToAction("MensajeDelSistema", new { msj = $"{candidateId}" });
    //}

    public async Task<IActionResult> Votar(uint candidateId)
    {
        string privateKey = "0xfe567b46961bed93a1eea718d9fec33caab94961cb33e69007440b12afbf815c";

        try
        {

            // Llamar al servicio de votación para registrar el voto
            await _votingService.VoteAsync(candidateId, privateKey);
            return RedirectToAction("MensajeDelSistema", new { msj = "Voto registrado exitosamente." });
        }
        catch (Exception ex)
        {
            return RedirectToAction("MensajeDelSistema", new { msj = $"Ocurrió un error al registrar el voto: {ex.Message}" });
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
}