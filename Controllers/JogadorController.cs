using MeuPrimeiroMVC.Contexts;
using MeuPrimeiroMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeuPrimeiroMVC.Controllers
{
    [Route("[controller]")]
    public class JogadorController : Controller
    {
        // Criair uma referência (instância) sobre a comunicação do meu banco de dados

            ProjetoTesteContext context = new ProjetoTesteContext();
        public IActionResult Index()
        {
            // .Include () - trago os dados das tabelas relacionadas
            var listaJogador = context.Jogadors.Include("IdEquipeNavigation").ToList();


            ViewBag.listaJogador = listaJogador;

            //Passando alista de Equipes para fazer o Select
            var listaEquipes = context.Equipes.ToList();

            ViewBag.listaEquipes = listaEquipes;

            return View();
        }
        [Route("Cadastrar")]
        public IActionResult CadastrarJogador(Jogador jogador)
        {
            // Armazenar a equipe no banco de dados
            context.Add(jogador);

            // Registrar as alterações no banco de dados
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [Route("ExcluirJogador/{idJogador}")]
        public IActionResult ExcluirJogador(int idJogador)
        {
            // Pegar um Id de referencia, e vou procurar a equipe no banco de dados
            Jogador jogador = context.Jogadors.FirstOrDefault(x => x.Id == idJogador); // Select * from EQUIPE where id == (valor da equipe da tabela)

            context.Remove(jogador);

            context.SaveChanges();

            return RedirectToAction("Index");
        }
        

        [Route("Atualizar/{idJogador}")]
        public IActionResult Atualizar(int idJogador)
        {
            Jogador jogador = context.Jogadors.FirstOrDefault(x => x.Id == idJogador);

            ViewBag.Jogador = jogador;

            return View();
        }
            [Route("AtualizarJogador")]
        public IActionResult AtualizarJogador(Jogador jogador)
        {
            context.Jogadors.Update(jogador);

            context.SaveChanges();

            return RedirectToAction ("Index");
        }
    }
}