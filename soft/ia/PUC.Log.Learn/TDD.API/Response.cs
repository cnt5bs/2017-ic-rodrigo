using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Machine;
using System.IO;
using Newtonsoft.Json.Linq;

namespace TDD.API
{
    /// <summary>
    /// Descrição resumida para Response
    /// </summary>
    [TestClass]
    public class ResponseShaker
    {
        #region Atributos de teste adicionais
        //
        // É possível usar os seguintes atributos adicionais enquanto escreve os testes:
        //
        // Use ClassInitialize para executar código antes de executar o primeiro teste na classe
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup para executar código após a execução de todos os testes em uma classe
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize para executar código antes de executar cada teste 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup para executar código após execução de cada teste
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void GetItemResponses()
        {
            Response response = new Response();
            Configuration.InsertAppPath(Directory.GetCurrentDirectory());
            string environment = "questionario";
            JObject responses = response.GetItemResponses(new Usuario(), environment);
            Assert.IsNotNull(responses);
        }

        [TestMethod]
        public void GetConclusion()
        {
            Response response = new Response();
            Configuration.InsertAppPath(Directory.GetCurrentDirectory());
            string environment = "questionario";
            JObject responses = response.GetConclusion(new Usuario(), environment);
            Assert.IsNotNull(responses);
        }
    }
    class Usuario
    {
        public string nome { get; set; } = "Marcelo";
        public string idade { get; set; } = "21";
        public string cidade { get; set; } = "SP";
    }
    class Questionario
    {
        public string pergunta1 { get; set; } = "resposta4";
        public string pergunta2 { get; set; } = "resposta6";
        public string pergunta3 { get; set; } = "resposta2";
    }
}
