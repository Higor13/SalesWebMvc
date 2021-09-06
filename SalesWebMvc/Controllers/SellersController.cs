using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService; // Dependência

        // Construtor para injetar a dependência
        public SellersController(SellerService sellerService)
        {
            _sellerService = sellerService;
        }

        public IActionResult Index() // Controlador: Gera um IActionResult contendo uma lista;
        {
            var list = _sellerService.FindAll(); // Model: o controlador acessa o model, retorna uma lista de Seller e encaminha para a view
            return View(list); // View: Recebe os dados do model
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost] // Notação: Indica que é uma ação de Post e não de Get
        [ValidateAntiForgeryToken] // Notação para previnir que a app sofra ataque CSRF (tirar proveito de autenticação)
        public IActionResult Create(Seller seller)
        {
            _sellerService.Insert(seller);
            return RedirectToAction(nameof(Index)); // Redireciona para o Index
        }
    }
}
