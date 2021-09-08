using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;
using SalesWebMvc.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService; // Dependência
        private readonly DepartmentService _departmentService; // Dependência para o DepartmentService

        // Construtor para injetar a dependência no objeto
        public SellersController(SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }

        public IActionResult Index() // Controlador: Gera um IActionResult contendo uma lista;
        {
            var list = _sellerService.FindAll(); // Model: o controlador acessa o model, retorna uma lista de Seller e encaminha para a view
            return View(list); // View: Recebe os dados do model
        }

        // Método que abre o formulário para cadastrar um vendedor
        public IActionResult Create()
        {
            var departments = _departmentService.FindAll(); // 1° Carregar os departamentos (buscando no DB)
            var viewModel = new SellerFormViewModel { Departments = departments }; // 2° Instanciar um objeto do ViewModel
            return View(viewModel); // 3° Passamos o objeto viewModel para a View
        }

        [HttpPost] // Notação: Indica que é uma ação de Post e não de Get
        [ValidateAntiForgeryToken] // Notação para previnir que a app sofra ataque CSRF (tirar proveito de autenticação)
        public IActionResult Create(Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departments = _departmentService.FindAll();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            }
            _sellerService.Insert(seller);
            return RedirectToAction(nameof(Index)); // Redireciona para o Index
        }

        // Para abrir uma tela de confirmação (não é para deletar de fato)
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided"});
            }

            var obj = _sellerService.FindById(id.Value); // O value é por ele ser um objeto opicional, pegar o valor caso exista (int?)
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }
            return View(obj);
        }

        [HttpPost] // Notação: Indica que é uma ação de Post e não de Get
        [ValidateAntiForgeryToken] // Notação para previnir que a app sofra ataque CSRF (tirar proveito de autenticação)
        public IActionResult Delete(int id)
        {
            _sellerService.Remove(id);
            return RedirectToAction(nameof(Index)); // Redireciona para o Index
        }

        // Details GET Action
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = _sellerService.FindById(id.Value); // O value é por ele ser um objeto opicional, pegar o valor caso exista (int?)
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }
            return View(obj);
        }

        // Essa ação de Edit abre a tela para editar o vendedor
        public IActionResult Edit(int? id)
        {
            if (id == null) // Se o id não for informado, a requisição foi feita de forma errada
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }
            // Testar se o Id existe no Db
            var obj = _sellerService.FindById(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            // Abrir tela de edição
            List<Department> departments = _departmentService.FindAll();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };
            return View(viewModel);
        }

        // Ação Edit com POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit (int id, Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departments = _departmentService.FindAll();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            }

            if (id != seller.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id missmath" });
            }
            try
            {
                _sellerService.Update(seller);
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel { Message = message, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            return View(viewModel);
        }        
    }
}
