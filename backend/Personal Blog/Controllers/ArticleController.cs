using Microsoft.AspNetCore.Mvc;
using Personal_Blog.Model.Domain;
using Personal_Blog.Model.Requests;
using Personal_Blog.Services;

namespace Personal_Blog.Controllers;
[ApiController]
[Route("api/articles")]
public class ArticleController(ILogger<ArticleController> logger, ArticleService articleService) : ControllerBase
{
    private readonly ILogger<ArticleController> _logger = logger;
    private readonly ArticleService _articleService = articleService;

    [HttpGet("getAll")]
    public async Task<ActionResult<IEnumerable<Article>>> GetAll()
    {
        var result = await _articleService.GetAll();
        return Ok(result);
    }

    [HttpGet("get/{id}")]
    public async Task<ActionResult<Article>> GetById(string id)
    {
        var result = await _articleService.GetById(id);
        if (result == null)
        {
            return NotFound(new{ Id = id });
        }
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<ActionResult> AddArticle(CreateArticleRequest request)
    {
        await _articleService.InsertArticle(request);
        return Ok();
    }

    [HttpDelete("delete/{id}")]
    public async Task<ActionResult<Article>> DeleteArticle(string id)
    {
        var result = await _articleService.DeleteById(id);
        if (result) return Ok(new { Id = id });
        else return NotFound();
    }
    [HttpPatch("delete/{id}")]
    public async Task<ActionResult<Article>> UpdateArticle(string id)
    {
        var result = await _articleService.DeleteById(id);
        if (result) return Ok(new { Id = id });
        else return NotFound();
    }
}