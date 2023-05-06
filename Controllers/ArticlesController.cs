using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DSS_API.Models;
using TodoApi.Models;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace DSS_API.Controllers
{
    public class CreateArticleRequest
    {
        public int UserId { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Body { get; set; }
    }
    public class CreateCommentRequest
    {
        public string Text { get; set; }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly DataContext _context;

        public ArticlesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Articles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticleItems()
        {
            return await _context.Article
                .Include(a => a.User)
                .Include(a => a.Comments)
                .ThenInclude(c => c.User)
                .ToListAsync();
        }

        // GET: api/Articles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Article>> GetArticle(int id)
        {
            var article = await _context.Article
                .Include(a => a.User)
                .Include(a => a.Comments)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (article == null)
            {
                return NotFound();
            }

            return article;
        }
        [HttpGet("{id}/Comments")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetCommentsForArticle(int id)
        {
            var article = await _context.Article.FindAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            return await _context.Comment
                .Include(c => c.User)
                .Where(c => c.ArticleId == id)
                .ToListAsync();
        }

        // PUT: api/Articles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticle(int id, Article article)
        {
            if (id != article.Id)
            {
                return BadRequest();
            }

            _context.Entry(article).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Articles
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Article>> PostArticle(CreateArticleRequest request)
        {
            if (_context.Article == null)
            {
                return Problem("Entity set 'TodoContext.ArticleItems' is null.");
            }

            var accessToken = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(accessToken);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "sub");

            if (userIdClaim == null)
            {
                return BadRequest("UserId claim is not present in the token.");
            }

            int userId;
            if (!int.TryParse(userIdClaim.Value, out userId))
            {
                return BadRequest("Invalid UserId claim value.");
            }

            var article = new Article
            {
                UserId = userId,
                Title = request.Title,
                ImageUrl = request.ImageUrl,
                Body = request.Body,
            };

            _context.Article.Add(article);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArticle", new { id = article.Id }, article);
        }


        // DELETE: api/Articles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            if (_context.Article == null)
            {
                return NotFound();
            }
            var article = await _context.Article.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            _context.Article.Remove(article);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("{articleId}/Comments")]
[Authorize]
public async Task<ActionResult<Comment>> PostCommentForArticle(int articleId, CreateCommentRequest request)
{
    if (_context.Comment == null)
    {
        return Problem("Entity set 'TodoContext.CommentItems' is null.");
    }

    var article = await _context.Article.FindAsync(articleId);
    if (article == null)
    {
        return NotFound("Article not found.");
    }

    var accessToken = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
    var handler = new JwtSecurityTokenHandler();
    var jwtToken = handler.ReadJwtToken(accessToken);
    var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "sub");

    if (userIdClaim == null)
    {
        return BadRequest("UserId claim is not present in the token.");
    }

    int userId;
    if (!int.TryParse(userIdClaim.Value, out userId))
    {
        return BadRequest("Invalid UserId claim value.");
    }

    var comment = new Comment
    {
        UserId = userId,
        ArticleId = articleId,
        Text = request.Text
    };

    _context.Comment.Add(comment);
    await _context.SaveChangesAsync();

    return CreatedAtRoute("GetComment", new { controller = "Comments", id = comment.Id }, comment);
        }
        private bool ArticleExists(int id)
        {
            return (_context.Article?.Any(e => e.Id == id)).GetValueOrDefault();
        }   
    }
}
