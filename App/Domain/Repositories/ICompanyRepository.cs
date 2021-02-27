using App.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Domain.Repository
{
    public interface ICompanyRepository
    {
        Company GetById(int id);
    }
}
