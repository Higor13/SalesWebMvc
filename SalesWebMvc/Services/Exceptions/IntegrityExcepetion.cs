using System;

namespace SalesWebMvc.Services.Exceptions
{
    // Exceção personalizada de serviço para erros de integridade referencial
    public class IntegrityExcepetion : ApplicationException
    {
        public IntegrityExcepetion(string message) : base(message)
        {

        }
    }
}
