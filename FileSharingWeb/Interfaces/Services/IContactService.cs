using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileSharingWeb.Models;

namespace FileSharingWeb.Interfaces.Services
{
    public interface IContactService
    {
        Task<bool> CreateAsync(Contact contact);

        Task<bool> SaveAllAsync();
    }
}