using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlogDemo.Core.Entities;
using BlogDemo.Core.Interface;
using BlogDemo.Infrastructure.Database;
using BlogDemo.Infrastructure.Resources;
using BlogDemo.Infrastructure.Extentions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using BlogDemo.Infrastructure.Services;
using System.Dynamic;
using BlogDemo.Api.Helpers;
using Microsoft.AspNetCore.JsonPatch;

namespace BlogDemo.Api.Controllers
{
    [Route("api/posts")]
    public class PostController : Controller
    {
        // Repository和UnitOfWork都用到了DbContext而且生命周期都是Scope, 
        // 所以ASP.NET Core 会保证这两个里的DbContext是同一个
        private readonly IPostRepository _postRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PostController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IUrlHelper _urlHelper;
        private readonly ITypeHelperService _typeHelperService;
        private readonly IPropertyMappingContainer _propertyMappingContainer;

        public PostController(
            IPostRepository postRepository,
            IUnitOfWork unitOfWork,
            ILogger<PostController> logger,
            IConfiguration configuration,
            IMapper mapper,
            IUrlHelper urlHelper,
            ITypeHelperService typeHelperService,
            IPropertyMappingContainer propertyMappingContainer)
        {
            // 已经注册到容器了，可以直接通过构造函数注入
            _postRepository = postRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _configuration = configuration;
            _mapper = mapper;
            _urlHelper = urlHelper;
            _typeHelperService = typeHelperService;
            _propertyMappingContainer = propertyMappingContainer;
        }

        [HttpGet(Name = "GetPosts")]
        [RequestHeaderMatchingMediaType("Accept", new[] { "application/vnd.cgzl.hateoas+json" })]
        public async Task<IActionResult> GetHateoas(PostParameters postParameters)
        {
            #region Test and obsolete methods
            // Controller等不应该直接使用DbContext, DbContext已经实现了Unit of Work 和 Repository 模式
            // 下面这种是直接和数据访问层做链接高耦合，直接绑定了, 太依赖, 不好改, 补好切换
            //var posts = await _myContext.Posts.ToListAsync();

            // Test global ErrorHandler
            //throw new Exception("An error occurred while searching for the posts !!!");
            // Test ASP.NET core configuration: appsetting.json
            //var v = _configuration["Key1"];
            // Test Serilog
            //_logger.LogError(string.Format("Get all posts...{0}", typeof(Post)));
            #endregion

            if (!_propertyMappingContainer.ValidateMappingExistsFor<PostResource, Post>(postParameters.OrderBy))
            {
                return BadRequest("Sorting items not exists/allowed");
            }

            if (!_typeHelperService.TypeHasProperties<PostResource>(postParameters.Fields))
            {
                return BadRequest("Fields not exists");
            }
            var posts = await _postRepository.GetAllPostsAsync(postParameters);

            var postResource = _mapper.Map<IEnumerable<Post>, IEnumerable<PostResource>>(posts);

            var shapedPostResource = postResource.ToDynamicIEnumerable(postParameters.Fields);

            var shapedWithLinks = shapedPostResource.Select<ExpandoObject, IDictionary<string, object>>(x =>
            {
                var dict = x as IDictionary<string, object>;
                var postLinks = CreateLinkForPost((int)dict["Id"], postParameters.Fields);
                dict.Add("links", postLinks);
                return dict;
            });

            var links = CreateLinksForPosts(postParameters, posts.HasPrevious, posts.HasNext);

            var result = new
            {
                value = shapedWithLinks,
                links
            };

            var meta = new
            {
                posts.PageSize,
                posts.PageIndex,
                posts.TotalItemsCount,
                pageCount = posts.Count,
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(meta, new JsonSerializerSettings
            {
                // Use camelcase notation
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }));

            return Ok(result);
        }

        [HttpGet(Name = "GetPosts")]
        [RequestHeaderMatchingMediaType("Accept", new[] { "application/json" })]
        public async Task<IActionResult> Get(PostParameters postParameters)
        {
            if (!_propertyMappingContainer.ValidateMappingExistsFor<PostResource, Post>(postParameters.OrderBy))
            {
                return BadRequest("Sorting items not exists/allowed");
            }

            if (!_typeHelperService.TypeHasProperties<PostResource>(postParameters.Fields))
            {
                return BadRequest("Fields not exists");
            }
            var posts = await _postRepository.GetAllPostsAsync(postParameters);

            var postResource = _mapper.Map<IEnumerable<Post>, IEnumerable<PostResource>>(posts);

            var previousPageLink = posts.HasPrevious ?
            CreatePostUri(postParameters,
                PaginationResourceUriType.PreviousPage) : null;

            var nextPageLink = posts.HasNext ?
                CreatePostUri(postParameters,
                    PaginationResourceUriType.NextPage) : null;

            var meta = new
            {
                posts.TotalItemsCount,
                posts.PageSize,
                posts.PageIndex,
                posts.PageCount,
                previousPageLink,
                nextPageLink
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(meta, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }));

            return Ok(postResource.ToDynamicIEnumerable(postParameters.Fields));
        }

        [HttpGet("{id}", Name = "GetPost")]
        public async Task<IActionResult> Get(int id, string fields = null)
        {
            if (!_typeHelperService.TypeHasProperties<PostResource>(fields))
            {
                return BadRequest("Fields not exists");
            }
            var post = await _postRepository.GetPostByIdAsync(id);

            if (post == null)
            {
                return NotFound();
            }
            var postResource = _mapper.Map<Post, PostResource>(post);

            var shapedPostResource = postResource.ToDynamic(fields);

            var links = CreateLinkForPost(id, fields);

            var result = shapedPostResource as IDictionary<string, object>;

            result.Add("links", links);

            return Ok(result);
        }

        [HttpPost(Name = "CreatePost")]
        [RequestHeaderMatchingMediaType("Content-Type", new[] { "application/vnd.cgzl.post.create+json" })]
        [RequestHeaderMatchingMediaType("Accept", new[] { "application/vnd.cgzl.hateoas+json" })]
        public async Task<IActionResult> Post([FromBody] PostAddResource postAddResource)
        {
            if (postAddResource == null)
            {
                return BadRequest("Resource is empty");
            }

            if (!ModelState.IsValid)
            {
                return new MyUnprocessableEntityObjectResult(ModelState);
            }

            var newPost = _mapper.Map<PostAddResource, Post>(postAddResource);

            newPost.Author = "Post Test";
            newPost.LastModified = DateTime.Now;

            _postRepository.AddPost(newPost);

            if (!await _unitOfWork.SaveAsync())
            {
                throw new Exception("Save failed!");
            }

            var resultResource = _mapper.Map<Post, PostResource>(newPost);

            var links = CreateLinkForPost(newPost.Id);
            var linkedPostResource = resultResource.ToDynamic() as IDictionary<string, object>;
            linkedPostResource.Add("links", links);

            return CreatedAtRoute("GetPost", new { id = linkedPostResource["Id"] }, linkedPostResource);
        }

        [HttpDelete("{id}", Name = "DeletePost")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _postRepository.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            _postRepository.Delete(post);

            if (!await _unitOfWork.SaveAsync())
            {
                throw new Exception($"Deleting post {id} failed when saving.");
            }

            return NoContent();
        }

        [HttpPut("{id}", Name = "UpdatePost")]
        [RequestHeaderMatchingMediaType("Content-Type", new[] { "application/vnd.cgzl.post.update+json" })]
        public async Task<IActionResult> UpdatePost(int id, [FromBody] PostUpdateResource postUpdate)
        {
            if (postUpdate == null)
            {
                return BadRequest("Resource is empty");
            }

            if (!ModelState.IsValid)
            {
                return new MyUnprocessableEntityObjectResult(ModelState);
            }

            var post = await _postRepository.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            post.LastModified = DateTime.Now;
            // Update the data from postUpdate to post
            _mapper.Map(postUpdate, post);


            if (!await _unitOfWork.SaveAsync())
            {
                throw new Exception($"An error occurred while saving the data with the Post id: {id}");
            }

            return NoContent();
        }

        [HttpPatch("{id}", Name = "PatiallyUpdatePost")]
        [RequestHeaderMatchingMediaType("Content-Type", new[] { "application/json-patch+json" })]
        public async Task<IActionResult> PartiallyUpdatePost(int id, [FromBody]JsonPatchDocument<PostUpdateResource> postDoc)
        {
            if (postDoc == null)
            {
                return BadRequest("Resource is empty");
            }


            var post = await _postRepository.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }
 
            var postToPatch = _mapper.Map<Post, PostUpdateResource>(post);  // Map from Entity model to Request model

            // Apply the patch to that post object. 
            postDoc.ApplyTo(postToPatch, ModelState);

            TryValidateModel(postToPatch);

            if (!ModelState.IsValid)
            {
                return new MyUnprocessableEntityObjectResult(ModelState);
            }

            // Use automapper to map the postToPatch back ontop of the database object.
            // Update the database object 'post' with 'postToPatch'
            _mapper.Map(postToPatch, post);
            post.LastModified = DateTime.Now;


            if (!await _unitOfWork.SaveAsync())
            {
                throw new Exception($"An error occurred while updating the data with the Post id: {id}");
            }

            return NoContent();
        }


        private string CreatePostUri(PostParameters parameters, PaginationResourceUriType uriType)
        {
            switch (uriType)
            {
                case PaginationResourceUriType.PreviousPage:
                    var previousParameters = new
                    {
                        pageIndex = parameters.PageIndex - 1,
                        pageSize = parameters.PageSize,
                        orderBy = parameters.OrderBy,
                        fields = parameters.Fields
                    };
                    return _urlHelper.Link("GetPosts", previousParameters);
                case PaginationResourceUriType.NextPage:
                    var nextParameters = new
                    {
                        pageIndex = parameters.PageIndex + 1,
                        pageSize = parameters.PageSize,
                        orderBy = parameters.OrderBy,
                        fields = parameters.Fields
                    };
                    return _urlHelper.Link("GetPosts", nextParameters);
                default:
                    var currentParameters = new
                    {
                        pageIndex = parameters.PageIndex,
                        pageSize = parameters.PageSize,
                        orderBy = parameters.OrderBy,
                        fields = parameters.Fields
                    };
                    return _urlHelper.Link("GetPosts", currentParameters);
            }
        }

        private IEnumerable<LinkResource> CreateLinkForPost(int id, string fields = null)
        {
            var links = new List<LinkResource>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                    new LinkResource(
                        _urlHelper.Link("GetPost", new { id }), "self", "GET"));
            }
            else
            {
                links.Add(
                    new LinkResource(
                        _urlHelper.Link("GetPost", new { id, fields }), "self", "GET"));
            }

            links.Add(
                new LinkResource(
                    _urlHelper.Link("DeletePost", new { id }), "delete_post", "DELETE"));

            return links;
        }

        private IEnumerable<LinkResource> CreateLinksForPosts(PostParameters postParameters, bool hasPrevious, bool hasNext)
        {
            var links = new List<LinkResource>
            {
                new LinkResource(
                    CreatePostUri(postParameters, PaginationResourceUriType.CurrentPage),
                    "self", "GET")
            };

            if (hasPrevious)
            {
                links.Add(
                    new LinkResource(
                        CreatePostUri(postParameters, PaginationResourceUriType.PreviousPage),
                        "previous_page", "GET"));
            }

            if (hasNext)
            {
                links.Add(
                    new LinkResource(
                        CreatePostUri(postParameters, PaginationResourceUriType.NextPage),
                        "next_page", "GET"));
            }
            return links;
        }
    }
}