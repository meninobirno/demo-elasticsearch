using Demo.Elasticsearch.API.Models;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace Demo.ElasticSearch.API.Controllers
{
    [Route("movies")]
    public class MoviesController : Controller
    {
        private readonly IElasticClient _elasticClient;

        public MoviesController(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Prâmetros inválidos.");

            var result = await _elasticClient.SearchAsync<Title>(s => s
            .Index("movies")
                .Query(q => q
                    .Term(t => t.TtConst, id)));

            if (result.IsValid)
                return Ok(result.Documents.First());

            return BadRequest("Não foi possível realizar a consulta.");
        }
    }
}
