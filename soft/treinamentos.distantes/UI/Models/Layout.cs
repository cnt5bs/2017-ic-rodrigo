using Newtonsoft.Json.Linq;

namespace App.Models
{
    public class Layout : Base
    {
        public string nomeLayout { get; set; }
        public string caminhoImagemDeFundo { get; set; }
        public bool temImagemDeFundo { get; set; }
        public JObject configuracoesLayout { get; set; }
    }
}