using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personal_Blog.Model.Exceptions;
using Personal_Blog.Model.Requests;
using Personal_Blog.Model.Responses;
using Personal_Blog.Services;

namespace Personal_Blog.Controllers;
[ApiController]
[Route("api/articles")]
public class ArticleController(ILogger<ArticleController> logger, ArticleService articleService) : ControllerBase
{
    private readonly ILogger<ArticleController> _logger = logger;
    private readonly ArticleService _articleService = articleService;

    [HttpGet("getAll")]
    public async Task<ActionResult<IEnumerable<ArticleResponse>>> GetAll()
    {
        var result = await _articleService.GetAll();
        return Ok(result);
    }

    [HttpGet("get/{id}")]
    public async Task<ActionResult<ArticleResponse>> GetById(string id)
    {
        if (string.IsNullOrEmpty(id) || id.Length != _articleService.GetRequiredIdLength())
        {
            return BadRequest();
        }
        try
        {
            var result = await _articleService.GetById(id);

            return Ok(result);
        }
        catch (ArticleNotfoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost("add")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ArticleResponse>> AddArticle([FromBody] CreateArticleRequest request)
    {
        var result = await _articleService.InsertArticle(request);
        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ArticleResponse>> DeleteArticle(string id)
    {
        if (string.IsNullOrEmpty(id) || id.Length != _articleService.GetRequiredIdLength())
        {
            return BadRequest();
        }
        try
        {
            var result = await _articleService.DeleteById(id);

            return Ok(result);
        }
        catch (ArticleNotfoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
    [HttpPatch("update/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ArticleResponse>> UpdateArticle(string id, [FromBody] UpdateArticleRequest request)
    {
        if (string.IsNullOrEmpty(id) || id.Length != _articleService.GetRequiredIdLength())
        {
            return BadRequest();
        }
        try
        {
            var result = await _articleService.UpdateOne(id, request);

            return Ok(result);
        }
        catch (ArticleNotfoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}