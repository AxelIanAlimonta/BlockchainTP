using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SistemaDeVotacion.web.Models;

namespace SistemaDeVotacion.web.Controllers;
[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly VotingService _votingService;
    private readonly SignInManager<IdentityUser> _signInManager;

    public HomeController(VotingService votingService, ILogger<HomeController> logger, SignInManager<IdentityUser> signInManager)
    {
        _votingService = votingService;
        _logger = logger;
        _signInManager = signInManager;
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

    public async Task<IActionResult> Votar(uint candidateId)
    {
        string privateKey = "0x894e43aaa80b3b0a9e6f701feb836b0a6b31da8adc8c63d83e97d6c333aefa95";

        try
        {

            // Llamar al servicio de votaci�n para registrar el voto
            await _votingService.VoteAsync(candidateId, privateKey);
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
}
