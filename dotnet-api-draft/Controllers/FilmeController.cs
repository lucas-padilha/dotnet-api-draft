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

/// <summary>
/// Adiciona um novo filme ao banco de dados.
/// Recebe um objeto CreateFilmeVM com os dados do filme a ser adicionado.
/// </summary>
/// <param name="filmeVM"></param>
/// <returns>Retorna 201 para o cadastro feito com sucesso</returns>
        [HttpPost]
        public IActionResult AddFilme([FromBody] CreateFilmeVM filmeVM)
        {
            var filme = _mapper.Map<Filme>(filmeVM); // Converte o ViewModel para o modelo Filme usando AutoMapper
            filme.Id = Guid.NewGuid(); // Gera um novo ID para o filme
            _context.Filmes.Add(filme); // Adiciona o filme ao contexto do banco de dados
            _context.SaveChanges(); // Salva as alterações no banco de dados

            return CreatedAtAction(nameof(GetFilmeById), new { id = filme.Id }, filme); // Retorna o filme criado com o ID gerado

        }

/// <summary>
/// Retorna todos os filmes cadastrados no banco de dados.
/// Recebe dois parâmetros: PageNumber e SizePage para paginação dos resultados.
/// </summary>
/// <param name="PageNumber"></param>
/// <param name="SizePage"></param>
/// <returns>Status Code 200</returns>
        [HttpGet]
        public async Task<ActionResult> GetAllFilmes(int PageNumber = 0, int SizePage = 50)
        {
            try
            {
                var searchFilmeVm = new SearchFilmeVM();
                searchFilmeVm.PageNumber = PageNumber;
                searchFilmeVm.SizePage = SizePage;
                searchFilmeVm.TotalItens = await _context.Filmes.CountAsync(); // Conta o total de filmes no banco de dados

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

/// <summary>
/// Retorna um filme específico com base no ID fornecido.
/// Recebe um parâmetro id do tipo Guid que representa o ID do filme a ser buscado.
/// </summary>
/// <param name="id"></param>
/// <returns>Retorna Status Code 200</returns>
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

/// <summary>
/// Atualiza um filme existente no banco de dados.
/// </summary>
/// <param name="id">Id do filme</param>
/// <param name="filmeVM">Dados para atualizar</param>
/// <returns></returns>
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

/// <summary>
/// Remove um filme do banco de dados com base no ID fornecido.
/// Recebe um parâmetro id do tipo Guid que representa o ID do filme a ser removido.
/// </summary>
/// <param name="id">Id do filme a ser deletado</param>
/// <returns>Retorna 204</returns>
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
            return NoContent();
        }
    }
}
