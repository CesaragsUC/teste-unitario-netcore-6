using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Teste
{
    public class AssertStringsTests
    {
        [Fact(DisplayName = "Retorna Nome completo")]
        [Trait("Demo", "Validacao basicas")]

        public void StringsTools_UnirNomes_RetornarNomeCompleto()
        {
            // Arrange
            var sut = new StringsTools();

            // Act
            var nomeCompleto = sut.Unir("Eduardo", "Pires");

            // Assert
            Assert.Equal("Eduardo Pires", nomeCompleto);
        }



        [Fact(DisplayName = "Nome deve ignorar case sensitive")]
        [Trait("Demo", "Validacao basicas")]
        public void StringsTools_UnirNomes_DeveIgnorarCase()
        {
            // Arrange
            var sut = new StringsTools();

            // Act
            var nomeCompleto = sut.Unir("Eduardo", "Pires");

            // Assert
            Assert.Equal("EDUARDO PIRES", nomeCompleto, true);
        }



        [Fact(DisplayName = "Nome deve conter palavra")]
        [Trait("Demo", "Validacao basicas")]
        public void StringsTools_UnirNomes_DeveConterTrecho()
        {
            // Arrange
            var sut = new StringsTools();

            // Act
            var nomeCompleto = sut.Unir("Eduardo", "Pires");

            // Assert
            Assert.Contains("ardo", nomeCompleto);
        }


        [Fact(DisplayName = "Nome deve comecar com")]
        [Trait("Demo", "Validacao basicas")]
        public void StringsTools_UnirNomes_DeveComecarCom()
        {
            // Arrange
            var sut = new StringsTools();

            // Act
            var nomeCompleto = sut.Unir("Eduardo", "Pires");

            // Assert
            Assert.StartsWith("Edu", nomeCompleto);
        }


        [Fact(DisplayName = "Nome deve terminar com")]
        [Trait("Demo", "Validacao basicas")]
        public void StringsTools_UnirNomes_DeveAcabarCom()
        {
            // Arrange
            var sut = new StringsTools();

            // Act
            var nomeCompleto = sut.Unir("Eduardo", "Pires");

            // Assert
            Assert.EndsWith("res", nomeCompleto);
        }


        [Fact(DisplayName = "Some somente texto")]
        [Trait("Demo", "Validacao basicas")]
        public void StringsTools_UnirNomes_ValidarExpressaoRegular()
        {
            // Arrange
            var sut = new StringsTools();

            // Act
            var nomeCompleto = sut.Unir("Eduardo", "Pires");

            // Assert
            Assert.Matches("[A-Z]{1}[a-z]+ [A-Z]{1}[a-z]+", nomeCompleto);
        }
    }
}
