﻿using Recipes.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Domain.Contracts
{
    public interface IRecipesService : IServiceBase<RecipeModel>
    {
        Task<List<RecipeModel>> FindAllRecipesAsync();
    }
}
