using SalesWebMvc.Data;
using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Services
{
    public class SellerService
    {
        private readonly SalesWebMvcContext _context;

        public SellerService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public List<Seller> FindAll() // Retorna uma lista com todos os vendedores
        {
            return _context.Seller.ToList(); // Acesso ao banco de dados
        }

        // Para inserir um novo vendedor no DB
        public void Insert(Seller obj)
        {
            obj.Department = _context.Department.First(); // Para pergar o primeiro dep do DB e associar ao seller(obj)
            _context.Add(obj);
            _context.SaveChanges();
        }
    }
}
