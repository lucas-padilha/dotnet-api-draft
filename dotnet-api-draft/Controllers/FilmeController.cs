using AutoMapper;
using dotnet_api_draft;
using dotnet_api_draft.Data;
using dotnet_api_draft.ViewModel;
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
        private FilmeContext _context;
        private IMapper _mapper;
        public FilmeController(FilmeContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }        

        private static List<Filme> filmes = new List<Filme>();
        
        [HttpPost]
        public IActionResult AddFilme([FromBody] CreateFilmeVM  filmeVM)
        {
            var filme = _mapper.Map<Filme>(filmeVM); // Converte o ViewModel para o modelo Filme usando AutoMapper
            filme.Id = Guid.NewGuid(); // Gera um novo ID para o filme
            _context.Filmes.Add(filme); // Adiciona o filme ao contexto do banco de dados
            _context.SaveChanges(); // Salva as alterações no banco de dados
            
            return CreatedAtAction(nameof(getFilmeById), new { id = filme.Id }, filme); // Retorna o filme criado com o ID gerado
                       
        }

        [HttpGet]
        public ActionResult getAllFilmes()
        {
            return Ok(_context.Filmes); // Retorna a lista de filmes
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

            var filmeEncontrado = _context.Filmes.FirstOrDefault(x => x.Id == filme.Id);

            if (filmeEncontrado == null)
            {
                return NotFound("Filme não encontrado.");
            }

            filmeEncontrado.Titulo = filme.Titulo;
            filmeEncontrado.Genero = filme.Genero;
            filmeEncontrado.Duracao = filme.Duracao;

            _context.Filmes.Update(filmeEncontrado);
            _context.SaveChanges();
            
            return Ok(filme);
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult DeleteFilme(Guid id)
        {
            var filmeEncontrado = _context.Filmes.FirstOrDefault(x => x.Id == id);
            if (filmeEncontrado == null)
            {
                return NotFound("Filme não encontrado.");
            }

            _context.Filmes.Remove(filmeEncontrado);
            _context.SaveChanges(); // Salva as alterações no banco de dados
            return Ok("Filme removido com sucesso.");
        }        
    }
}
