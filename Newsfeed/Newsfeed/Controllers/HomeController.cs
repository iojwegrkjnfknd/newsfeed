using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newsfeed.Data.FakeDbModels;
using Newsfeed.Extensions;
using Newsfeed.Models;
using Newsfeed.Models.Home;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Newsfeed.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public const string SessionKeyArticles = "_Articles";

        [AllowAnonymous]
        public IActionResult Index()
        {
            if (HttpContext.Session.Get<List<Article>>(SessionKeyArticles) == default(List<Article>))
            {
                HttpContext.Session.Set<List<Article>>(SessionKeyArticles, new List<Article>());
            }
            var dbArticles = HttpContext.Session.Get<List<Article>>(SessionKeyArticles);

            var articles = dbArticles.Select(a => new ArticleSummary { CreatedByUserName = a.CreatedByUserName, BodyPreview = a.Contents.OrderByDescending(c => c.CreatedDate).FirstOrDefault(c => !c.RetiredDate.HasValue)?.Body.Length > 97 ? a.Contents.OrderByDescending(c => c.CreatedDate).FirstOrDefault(c => !c.RetiredDate.HasValue).Body.Substring(0, 97) + "..." : a.Contents.OrderByDescending(c => c.CreatedDate).FirstOrDefault(c => !c.RetiredDate.HasValue)?.Body, CreatedDate = a.CreatedDate, Headline = a.Contents.OrderByDescending(c => c.CreatedDate).FirstOrDefault(c => !c.RetiredDate.HasValue)?.Headline, Id = a.Id, LastEditedDate = a.Contents.OrderByDescending(c => c.CreatedDate).FirstOrDefault(c => !c.RetiredDate.HasValue)?.CreatedDate, NumComments = a.Comments.Count(), NumLikes = a.Likes.Count() }).ToList();

            return View(new IndexVM { ArticleSummaries = articles });
        }

        public IActionResult Create()
        {
            var model = new ArticleVM();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ArticleVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dbArticles = _getArticles();

            if (dbArticles.Select(d => d.Id).Contains(model.Id))
                return BadRequest();

            dbArticles.Add(new Article { Id = model.Id, Contents = new List<ArticleContent> { new ArticleContent { ArticleId = model.Id, CreatedDate = DateTime.Now, Body = model.Body, Headline = model.Headline } }, CreatedByUserName = User.Identity.Name });

            HttpContext.Session.Set<List<Article>>(SessionKeyArticles, dbArticles);

            return RedirectToAction("Details", new { id = model.Id });
        }

        public IActionResult Edit(Guid? id)
        {
            if (id == null)
                return BadRequest();

            var dbArticles = _getArticles();

            var article = dbArticles.FirstOrDefault(a => a.Id == id);

            if (article == null)
                return NotFound();
            if (article.CreatedByUserName != User.Identity.Name)
                return Unauthorized();

            var model = _convertArticleToArticleVM(article);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ArticleVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dbArticles = _getArticles();

            var article = dbArticles.FirstOrDefault(a => a.Id == model.Id);

            if (article == null)
                return NotFound();
            if (article.CreatedByUserName != User.Identity.Name)
                return Unauthorized();

            var index = dbArticles.IndexOf(article);
            if (index == -1)
                return StatusCode(StatusCodes.Status500InternalServerError);

            article.Contents.FirstOrDefault(c => c.RetiredDate == null).RetiredDate = DateTime.Now;
            var contents = article.Contents.ToList();
            if (contents == null)
                contents = new List<ArticleContent>();
            contents.Add(new ArticleContent { ArticleId = article.Id, Body = model.Body, Headline = model.Headline, CreatedDate = DateTime.Now });
            article.Contents = contents;

            dbArticles[index] = article;

            HttpContext.Session.Set<List<Article>>(SessionKeyArticles, dbArticles);

            return RedirectToAction("Details", new { id = model.Id });
        }

        [AllowAnonymous]
        public IActionResult Details(Guid? id)
        {
            if (id == null)
                return BadRequest();

            var dbArticles = _getArticles();

            var article = dbArticles.FirstOrDefault(a => a.Id == id);

            if (article == null)
                return NotFound();

            var model = _convertArticleToArticleVM(article);

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ArticleLike(ArticleLikeVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dbArticles = _getArticles();

            var article = dbArticles.FirstOrDefault(a => a.Id == model.ArticleId);

            if (article == null)
                return NotFound();

            var index = dbArticles.IndexOf(article);
            if (index == -1)
                return StatusCode(StatusCodes.Status500InternalServerError);

            var likes = article.Likes.ToList();
            if (likes == null)
                likes = new List<ArticleLike>();

            likes.Add(new ArticleLike { ArticleId = model.ArticleId, CreatedByUserName = User.Identity.Name ?? "anonymous" });
            article.Likes = likes;

            dbArticles[index] = article;

            HttpContext.Session.Set<List<Article>>(SessionKeyArticles, dbArticles);

            return RedirectToAction("Details", new { id = model.ArticleId });
        }
        
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Comment(CommentVM model)
        {
            var dbArticles = _getArticles();

            var article = dbArticles.FirstOrDefault(a => a.Id == model.ArticleId);

            if (article == null)
                return NotFound();

            var index = dbArticles.IndexOf(article);
            if (index == -1)
                return StatusCode(StatusCodes.Status500InternalServerError);

            var comments = article.Comments.ToList();
            if (comments == null)
                comments = new List<Data.FakeDbModels.ArticleComment>();
            comments.Add(new Data.FakeDbModels.ArticleComment { ArticleId = model.ArticleId, CreatedByUserName = User.Identity.Name ?? "anonymous", Text = model.Text });
            article.Comments = comments;

            dbArticles[index] = article;

            HttpContext.Session.Set<List<Article>>(SessionKeyArticles, dbArticles);

            return RedirectToAction("Details", new { id = model.ArticleId });
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult CommentLike(CommentLikeVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dbArticles = _getArticles();

            var article = dbArticles.FirstOrDefault(a => a.Id == model.ArticleId);

            if (article == null)
                return NotFound();

            var index = dbArticles.IndexOf(article);
            if (index == -1)
                return StatusCode(StatusCodes.Status500InternalServerError);

            var comments = article.Comments.ToList();
            if (comments != null)
            {
                var comment = comments.FirstOrDefault(c => c.Id == model.ArticleCommentId);
                if (comment != null)
                {
                    var cIndex = comments.IndexOf(comment);
                    if (index == -1)
                        return StatusCode(StatusCodes.Status500InternalServerError);

                    var likes = comment.Likes.ToList();
                    if (likes == null)
                        likes = new List<CommentLike>();
                    likes.Add(new CommentLike { ArticleCommentId = model.ArticleCommentId, CreatedByUserName = User.Identity.Name ?? "anonymous" });
                    comment.Likes = likes;

                    comments[cIndex] = comment;

                    article.Comments = comments;

                    dbArticles[index] = article;

                    HttpContext.Session.Set<List<Article>>(SessionKeyArticles, dbArticles);
                }
            }

            return RedirectToAction("Details", new { id = model.ArticleId });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private List<Article> _getArticles()
        {
            if (HttpContext.Session.Get<List<Article>>(SessionKeyArticles) == default(List<Article>))
            {
                HttpContext.Session.Set<List<Article>>(SessionKeyArticles, new List<Article>());
            }
            return HttpContext.Session.Get<List<Article>>(SessionKeyArticles);
        }

        private ArticleVM _convertArticleToArticleVM(Article article)
        {
            return new ArticleVM { Id = article.Id, CreatedByUserName = article.CreatedByUserName, Body = article.Contents.OrderByDescending(c => c.CreatedDate).FirstOrDefault(c => !c.RetiredDate.HasValue)?.Body, Comments = article.Comments.Select(c => new Models.Home.ArticleComment { CreatedDate = c.CreatedDate, CreatedByUserName = c.CreatedByUserName, Id = c.Id, NumLikes = c.Likes.Count(), Text = c.Text }), CreatedDate = article.CreatedDate, Headline = article.Contents.OrderByDescending(c => c.CreatedDate).FirstOrDefault(c => !c.RetiredDate.HasValue)?.Headline, NumLikes = article.Likes.Count(), LastEditedDate = article.Contents.OrderByDescending(c => c.CreatedDate).Count() > 1 ? article.Contents.OrderByDescending(c => c.CreatedDate).FirstOrDefault(c => !c.RetiredDate.HasValue)?.CreatedDate : null };
        }
    }
}
