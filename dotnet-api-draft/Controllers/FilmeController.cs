using Draft.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Draft.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]

    public class FilmeController : ControllerBase
    {

        private static List<Filme> filmes = new List<Filme>();
        [HttpPost]
        public void AddFilme([FromBody] Filme  filme)
        {
            filme.Id = Guid.NewGuid(); // Gera um novo ID para o filme
            filmes.Add(filme);  // Adiciona o filme à lista
            System.Console.WriteLine("Filme adicionado: " + filme.Titulo);
            System.Console.WriteLine("Duração Filme: " + filme.Duracao);            
        }

        [HttpGet]
        public ActionResult getAllFilmes()
        {
            return Ok(filmes); // Retorna a lista de filmes
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult getFilmeById(Guid id)
        {
            var filmeEncontrado = filmes.Find(x => x.Id == id);
            if (filmeEncontrado == null)
            {
                return NotFound("Filme não encontrado.");
            }
            return Ok(filmeEncontrado);
        }

        [HttpPut]
        public ActionResult getAllFilmes([FromBody] Filme filme)
        {
            var filmeEncontrado = filmes.Find(x => x.Id == filme.Id);

            if (filmeEncontrado == null)
            {
                return NotFound("Filme não encontrado.");
            }

            filmeEncontrado.Titulo = filme.Titulo;
            filmeEncontrado.Genero = filme.Genero;
            filmeEncontrado.Duracao = filme.Duracao;
            
            return Ok(filme);
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult DeleteFilme(Guid id)
        {
            var filmeEncontrado = filmes.Find(x => x.Id == id);
            if (filmeEncontrado == null)
            {
                return NotFound("Filme não encontrado.");
            }
            filmes.Remove(filmeEncontrado);
            return Ok("Filme removido com sucesso.");
        }        
    }
}
