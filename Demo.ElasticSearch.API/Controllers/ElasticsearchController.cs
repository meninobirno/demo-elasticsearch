using Demo.Elasticsearch.API.Models;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace Demo.ElasticSearch.API.Controllers
{
    [Route("elastic-search")]
    public class ElasticsearchController : Controller
    {
        private readonly IElasticClient _client;

        public ElasticsearchController(IElasticClient client)
        {
            _client = client;
        }

        [HttpPost("create-indices")]
        public async Task<IActionResult> CreateIndices()
        {
            await _client.Indices.CreateAsync("movies", opt => opt
                .Map<Title>(m => m.AutoMap())
                .Settings(settings => settings
                    .NumberOfReplicas(0))
            );

            await _client.Indices.PutAliasAsync("movies", "alias-movies");

            return Ok();
        }

        [HttpPost("populate-indices")]
        public async Task<IActionResult> PopulateIndices()
        {
            var dataSet = System.IO.File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "DataSet", "data.tsv"));

            foreach (var entry in dataSet.Skip(1))
            {
                var entities = entry.Split("\t");

                try
                {
                    var title = new Title
                    {
                        TtConst = entities[0],
                        TitleType = entities[1],
                        PrimaryTitle = entities[2] == @"\N" ? string.Empty : entities[2],
                        OriginalTitle = entities[3] == @"\N" ? string.Empty : entities[3],
                        IsAdult = entities[4] == "1",
                        StartYear = int.Parse(entities[5]),
                        EndYear = entities[6] == @"\N" ? 0 : int.Parse(entities[6]),
                        RuntimeMinutes = entities[7] == @"\N" ? 0 : int.Parse(entities[7]),
                        Genres = entities[8].Split(',').ToList()
                    };

                    await _client.IndexAsync(title, idx => idx.Index("movies").Id(title.TtConst));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Skipping entry -> {entities[0]} - {ex}");
                }
            }

            return Ok();
        }

        
    }

}
