﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Recipes.Domain.Contracts;
using Recipes.Domain.Models;
using Recipies.Models.AdminModels;
using Recipies.Models.CommentModels;
using Recipies.Models.RecipesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipies.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly IRecipesService _recipesService;
        private readonly IMapper _mapper;
        private readonly IAdminService _adminService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ICommentService _commentService;
        private readonly IImageService _imageService;
        public CommentController(
            IRecipesService recipesService,
            IMapper mapper,
            IAdminService adminService,
            UserManager<IdentityUser> userManager,
            ICommentService commentService, 
            IImageService imageService)
        {
            this._recipesService = recipesService;
            this._mapper = mapper;
            this._adminService = adminService;
            this._userManager = userManager;
            this._commentService = commentService;
            this._imageService = imageService;
        }

        [HttpGet]
        public async Task<IActionResult> All(string id)
        {
            var recipe = await this._recipesService.ReadAsync(Guid.Parse(id));           
            var allComments = await this._commentService.FindAllAsync();
            var commentsForRecipe = allComments.Where(x => x.RecipeId == id).ToList();
            var recipeCommentsViewModel = _mapper.Map<List<CommentViewModel>>(commentsForRecipe);
            this.ViewData["RecipeId"] = id;
            foreach (var commentModel in recipeCommentsViewModel)
            {
                commentModel.RecipeName = recipe.Name;
                commentModel.RecipeId = recipe.Id;
                commentModel.ImageUrl = recipe.ImageUrl;
                
            }
            for (int i = 0; i < recipeCommentsViewModel.Count; i++)
            {
                for (int j = 0; j < commentsForRecipe.Count; j++)
                {
                    var user = await _userManager.FindByIdAsync(commentsForRecipe[j].ApplicationUserId);
                    var userEmail = user.Email;
                    recipeCommentsViewModel[i].SenderEmail = userEmail;
                    i++;
                }
                break;
            }                                                        
            if(recipeCommentsViewModel.Count == 0)
            {
                recipeCommentsViewModel.Add(new CommentViewModel
                {
                    RecipeId = id,
                    RecipeName = recipe.Name,
                    ImageUrl = recipe.ImageUrl
                });
            }
            var recipeViewModel = _mapper.Map<RecipeViewModel>(recipe);

            var imagesPaths = await _imageService.PopulateRecipeViewModelImages(Guid.Parse(recipe.Id));
            recipeCommentsViewModel[0].ImagesFilePaths = imagesPaths;
            return View(recipeCommentsViewModel);
        }

        [HttpPost]

        public async Task<IActionResult> Add(CommentSendModel model)
        {
            var allUsers = await this._adminService.GetAllUsersAsync();
            var usersViewModels = this._mapper.Map<IList<UserDetailsViewModel>>(allUsers);
            var isUserRegistered = allUsers.FirstOrDefault(x => x.Email == model.SenderEmail);           
            var user = await this._userManager.GetUserAsync(HttpContext.User);
            var currentUserEmail = user.Email;                      
            var userID = user.Id;
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Please insert valid form data!" });
            }
            if (currentUserEmail != model.SenderEmail)
            {
                return Json(new { success = false, message = "You cannot add comment from other users emails" });
            }
            if (isUserRegistered == null)
            {
                return Json(new { success = false, message = "There isn't registered user w*-ith this Email address!" });
            }
            var comment = new CommentModel
            {
                Description = model.CommentMessage,
                RecipeId = model.RecipeId,
                ApplicationUserId = userID,
                CreatedOn = DateTime.Now,
                
            };
            var recipe = await this._recipesService.ReadAsync(Guid.Parse(model.RecipeId));
            recipe.NumberOfComments++;
            await this._recipesService.UpdateAsync(recipe);
            await this._commentService.CreateAsync(comment);
            model.CommentCreation = DateTime.Now.ToShortDateString();            
            return Json(new { success = true, message = "You have added successfully a comment",commentModel = model });
        }
       
    }
}
