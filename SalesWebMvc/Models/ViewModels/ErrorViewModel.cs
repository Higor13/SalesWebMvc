using System;

namespace SalesWebMvc.Models.ViewModels
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        public string Message { get; set; } // Para adc uma menssagem customizada ao objeto

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}