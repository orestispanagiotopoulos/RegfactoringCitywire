﻿using App.Model.Entities;

namespace App.Repositories
{
    public interface ICompanyRepository
    {
        Company GetById(int id);
    }
}
