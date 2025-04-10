using System.Threading.Tasks;
using AutoMapper;
using dotnet_api_draft;
using dotnet_api_draft.Data;
using dotnet_api_draft.ViewModel;
using Draft.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult AddFilme([FromBody] CreateFilmeVM filmeVM)
        {
            var filme = _mapper.Map<Filme>(filmeVM); // Converte o ViewModel para o modelo Filme usando AutoMapper
            filme.Id = Guid.NewGuid(); // Gera um novo ID para o filme
            _context.Filmes.Add(filme); // Adiciona o filme ao contexto do banco de dados
            _context.SaveChanges(); // Salva as alterações no banco de dados

            return CreatedAtAction(nameof(GetFilmeById), new { id = filme.Id }, filme); // Retorna o filme criado com o ID gerado

        }

        [HttpGet]
        public async Task<ActionResult> GetAllFilmes(int PageNumber = 0, int SizePage = 50)
        {
            try
            {
                var searchFilmeVm = new SearchFilmeVM();
                searchFilmeVm.PageNumber = PageNumber;
                searchFilmeVm.SizePage = SizePage;
                searchFilmeVm.TotalItens = await _context.Filmes.CountAsync(); // Conta o total de filmes no banco de dados
                

                //_context.Filmes.Skip(PageNumber * SizePage).Take(SizePage).ForEachAsync(x =>
                await _context.Filmes.Skip(PageNumber * SizePage).Take(SizePage).ForEachAsync(async x =>
                {
                     searchFilmeVm.dicCreatedFilmesVM.Add(x.Id, _mapper.Map<CreateFilmeVM>(x));
                });                

                return Ok(searchFilmeVm);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult GetFilmeById(Guid id)
        {
            var filmeEncontrado = _context.Filmes.FirstOrDefault(x => x.Id == id);
            if (filmeEncontrado == null)
            {
                return NotFound("Filme não encontrado.");
            }
            
            return Ok(_mapper.Map<CreateFilmeVM>(filmeEncontrado));
        }

        [HttpPut]
        [Route("{id}")]
        public ActionResult UpdateFilme(Guid id, [FromBody] CreateFilmeVM filmeVM)
        {
            var filmeEncontrado = _context.Filmes.FirstOrDefault(x => x.Id == id);

            if (filmeEncontrado == null)
            {
                return NotFound("Filme não encontrado.");
            }

            _mapper.Map(filmeVM, filmeEncontrado); // Atualiza o filme encontrado com os dados do ViewModel           

            //_context.Filmes.Update(filmeEncontrado);
            _context.SaveChanges();

            return NoContent();
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
