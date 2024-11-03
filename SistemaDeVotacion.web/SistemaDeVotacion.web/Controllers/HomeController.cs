using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SistemaDeVotacion.web.Models;

namespace SistemaDeVotacion.web.Controllers
{
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
            string privateKey = "0xa317b32d3ca21d8a6f2c036605def8f5509e075b378bab255a4aaa0becca8748";

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

        public async Task<IActionResult> Test()
        {
            var blockData = await _votingService.Test();

            if (blockData == null)
            {
                return NotFound(); // O manejar el error de otra forma
            }

            // Acceder al primer bloque y sus transacciones
            var firstBlock = blockData.Skip(5).First();
            var transactions = firstBlock.GetType().GetProperty("Transactions").GetValue(firstBlock, null) as IEnumerable<object>;


            ViewBag.HashCode = firstBlock.GetType().GetProperty("Hash").GetValue(firstBlock, null);
            ViewBag.Transactions = transactions;
            return View();
        }

    }
}
