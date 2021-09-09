using System.Collections.Generic;

namespace SalesWebMvc.Models.ViewModels
{
    // Classe que contém os dados necessários para o formulário de cadastro de vendedor. 
    public class SellerFormViewModel
    {
        public Seller Seller { get; set; } // O vendedor
        public ICollection<Department> Departments { get; set; } // A lista com os departamentos
    }
}
