using MeuPrimeiroMVC.Contexts;
using MeuPrimeiroMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace MeuPrimeiroMVC.Controllers
{
    [Route("[controller]")]
    public class EquipeController : Controller
    {
        // Criair uma referência (instância) sobre a comunicação do meu banco de dados

        ProjetoTesteContext context = new ProjetoTesteContext();
        public IActionResult Index()
        {
            // Forma de listar todos os itens da tabela de (Equipe)
            var listaEquipes = context.Equipes.ToList();

            // Passar a tela (em forma de memoria ) os dados das equipes cadastradas
            ViewBag.listaEquipes = listaEquipes;

            return View();
        }
        [Route("Cadastrar")]
        public IActionResult CadastrarEquipe(IFormCollection formEquipe) /* Recebendo dados no padrão FormData - para trabalhar com arquivos*/
        {

            if (formEquipe.Files.Count > 0)
            {
                // Recebendo o arquivo anexado
                var arquivoAnexado = formEquipe.Files[0]; /* Dentro da possibilidade de receber vários arquivos, estamos recebendo apenas o primeiro (único)*/

                /* Directory.GetCurrentDirectory - Função para pegar a localização da pasta do projeto*/
                /* Criar a Pasta 'wwwroot' - é o local configurado para acessar o arquivos no navegador*/
                var pastaArmazenamento = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/equipe");


                /* Verifique se a pasta de armazenamento não existe*/
                if (!Directory.Exists(pastaArmazenamento))
                {
                    /* Caso não - o projeto fica responsável por criar essa pasta*/
                    Directory.CreateDirectory(pastaArmazenamento);
                }
                /* Passando a localização da pasta de armazenamento + o nome do arquivo a ser salvo*/
                var arquivoArmazenado = Path.Combine(pastaArmazenamento, arquivoAnexado.FileName);

                /*Chamamos uma função do C# para criação de arquivo - dentro da pasta de armazenamento*/
                using (var stream = new FileStream(arquivoArmazenado, FileMode.Create))
                {
                    // Para esse novo arquivo, copiamos o conteudo do arquivo anexado
                    arquivoAnexado.CopyTo(stream);
                }

                //
                Equipe equipe = new Equipe()
                {
                    Nome = formEquipe["Nome"],
                    Imagem = arquivoAnexado.FileName
                };

                // Armazenar a equipe no banco de dados
                context.Add(equipe);

            }
            else
            {
                Equipe equipe = new Equipe()
                {
                    Nome = formEquipe["Nome"],
                    Imagem = "padrão.jpg"
                };
                
                // Armazenar a equipe no banco de dados
                context.Add(equipe);

            }






            
            
            // Registrar as alterações no banco de dados
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        // Na rota Excluir, vamos capturar a id que vem da url
        [Route("ExcluirEquipe/{idEquipe}")]
        public IActionResult ExcluirEquipe(int idEquipe)
        {
            // Verificar se existe Jogadores que contenham o vinculo com a equipe
            List<Jogador> listaJogadores = context.Jogadors.Where(x => x.IdEquipe == idEquipe).ToList();

            if (listaJogadores.Count > 0)
            {
                // Remover todos os Jogadores vinculados
                foreach (Jogador jgd in listaJogadores)
                {
                    context.Remove(jgd);
                }

                /// Salvando a remoção dos jogadores
                context.SaveChanges();
            }

            // Pegar um Id de referencia, e vou procurar a equipe no banco de dados
            Equipe equipe = context.Equipes.FirstOrDefault(x => x.Id == idEquipe); // Select * from EQUIPE where id == (valor da equipe da tabela)

            context.Remove(equipe);

            context.SaveChanges();

            return RedirectToAction("Index");
        }


        [Route("Atualizar/{idEquipe}")]
        public IActionResult Atualizar(int idEquipe)
        {
            Equipe equipe = context.Equipes.FirstOrDefault(x => x.Id == idEquipe);

            ViewBag.Equipe = equipe;

            return View();
        }
        [Route("AtualizarEquipe")]
        public IActionResult AtualizarEquipe(Equipe equipe)
        {
            context.Equipes.Update(equipe);

            context.SaveChanges();

            return RedirectToAction("Index");
        }
    }

}