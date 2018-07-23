using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Machine;
using Newtonsoft.Json.Linq;
using System.IO;

namespace TDD
{
    /// <summary>
    /// Descrição resumida para Memory
    /// </summary>
    [TestClass]
    public class MemoryShaker
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
        public void InsertRelatedMemories()
        {
            Usuario responsible = new Usuario();

            Usuario responsible2 = new Usuario()
            {
                nome = "Bruno",
                idade = "21",
                cidade = "SP"
            };

            Questionario fact = new Questionario();
           
            Configuration.InsertAppPath(Directory.GetCurrentDirectory());
            string environment = "questionario";
            Memory.InsertRelatedMemories(responsible2, fact, environment);
            Assert.IsTrue(true);
        }
    }
    class Usuario
    {
        public string nome { get; set; } = "Bruno";
        public string idade { get; set; } = "32";
        public string cidade { get; set; } = "ES";
    }
    class Questionario
    {
        public string pergunta1 { get; set; } = "resposta4";
        public string pergunta2 { get; set; } = "resposta6";
        public string pergunta3 { get; set; } = "resposta2";
    }
    //class Usuario{
    //    public string nome { get; set; } = "Rodrigo";
    //    public string idade { get; set; } = "21";
    //    public string cidade { get; set; } = "SP";
    //}
    //class Questionario
    //{
    //    public string pergunta1 { get; set; } = "resposta5";
    //    public string pergunta2 { get; set; } = "resposta3";
    //    public string pergunta3 { get; set; } = "resposta4";
    //}
}
