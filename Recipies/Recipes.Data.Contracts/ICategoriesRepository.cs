﻿using Recipies.Data.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Data.Contracts
{
    public interface ICategoriesRepository : IRepositoryBase<Category>
    {
        Task<List<string>> FindAllCategoryNames();
    }
}
