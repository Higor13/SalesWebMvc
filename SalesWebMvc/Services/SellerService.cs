using SalesWebMvc.Data;
using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Services.Exceptions;

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
            _context.Add(obj);
            _context.SaveChanges();
        }

        // Encontra o vendedor pelo Id
        public Seller FindById (int id)
        {
            return _context.Seller.Include(obj => obj.Department).FirstOrDefault(obj => obj.Id == id);
        }

        // Remover o vendedor pelo Id
        public void Remove(int id)
        {
            var obj = _context.Seller.Find(id);
            _context.Seller.Remove(obj); // Remove o objeto do DbSet. Precisamos confirmar para o Entity efetivar no DB
            _context.SaveChanges();
        }

        // Atualizar um objeto utilizando o Entity FrameWork
        public void Update(Seller obj)
        {
            // Testar se o Id do obj já existe no DB
            if(!_context.Seller.Any(x => x.Id == obj.Id)) // Se NÃO existir, lançar exceção
            {
                throw new NotFoundException("Id not found");
            }
            try
            {
                _context.Update(obj);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }
    }
}
