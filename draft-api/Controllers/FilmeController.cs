using System.Threading.Tasks;
using AutoMapper;
using DraftApi.ViewModel;
using DraftDomain.IRepository;
using DraftDomain.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Draft.Controllers
{
    /// <summary>
    /// Controlador responsável por gerenciar as operações relacionadas a filmes.
    /// Permite adicionar, atualizar, remover e buscar filmes no banco de dados.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class FilmeController : ControllerBase
    {
        private IMapper _mapper;
        private IFilmeRepository _filmeRepository;

        /// <summary>
        /// Construtor da classe FilmeController.
        /// /// Recebe o contexto do banco de dados e o mapeador AutoMapper como dependências.
        /// O contexto é usado para interagir com o banco de dados e o mapeador é usado para converter entre modelos e ViewModels.
        /// </summary>
        /// <param name="filmeRepository">Repositório de filmes usado para acessar os dados do banco de dados.</param>
        public FilmeController(IFilmeRepository filmeRepository)
        {
            _filmeRepository = filmeRepository;
            //_context = context;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Filme, CreateFilmeVM>().ReverseMap(); // Mapeia entre o modelo Filme e o ViewModel CreateFilmeVM
            });
            _mapper = config.CreateMapper(); // Cria uma instância do mapeador AutoMapper
        }

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
            _filmeRepository.AddAsync(filme); // Adiciona o filme ao contexto do banco de dados            

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
                searchFilmeVm.TotalItens = await _filmeRepository.GetCountAsync();
                
                var filmes = await _filmeRepository.GetAllAsync(); // Obtém todos os filmes do repositório  

                foreach (var filme in filmes.Skip(PageNumber * SizePage).Take(SizePage))
                {
                    searchFilmeVm.dicCreatedFilmesVM.Add(filme.Id, _mapper.Map<CreateFilmeVM>(filme)); 
                }

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
            var filmeEncontrado = _filmeRepository.GetByIdAsync(id);
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
        public async Task<ActionResult> UpdateFilme(Guid id, [FromBody] CreateFilmeVM filmeVM)
        {
            var filmeEncontrado = await _filmeRepository.GetByIdAsync(id);
            if (filmeEncontrado == null)
            {
                return NotFound("Filme não encontrado.");
            }
            _mapper.Map(filmeVM, filmeEncontrado); // Atualiza o filme encontrado com os dados do ViewModel           
            await _filmeRepository.UpdateAsync(filmeEncontrado);            

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
        public async Task<ActionResult> DeleteFilme(Guid id)
        {
            var filmeEncontrado = _filmeRepository.GetByIdAsync(id);
            if (filmeEncontrado == null)
            {
                return NotFound("Filme não encontrado.");
            }

            await _filmeRepository.DeleteAsync(id);
            return NoContent(); // Retorna 204 No Content para indicar que a operação foi bem-sucedida
        }
    }
}
