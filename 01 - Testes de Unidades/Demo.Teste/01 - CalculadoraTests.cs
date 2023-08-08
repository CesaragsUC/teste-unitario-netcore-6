namespace Demo.Teste
{
    public class CalculadoraTests
    {
        [Fact(DisplayName = "Retornar valor soma")]
        [Trait("Demo", "Validacao basicas")]
        public void Calculadora_Somar_Retonar_Valor_Soma()
        {
            //Arange
            var calculadora = new Calculadora();

            //Act
            var resultado = calculadora.Somar(2, 2);

            //Assert
            Assert.Equal(4, resultado);
        }

        [Fact(DisplayName = "Retornar valor divisao")]
        [Trait("Demo", "Validacao basicas")]
        public void Calculadora_Somar_Retonar_Valor_Divisao()
        {
            //Arange
            var calculadora = new Calculadora();

            //Act
            var resultado = calculadora.Dividir(2, 2);

            //Assert
            Assert.Equal(1, resultado);
        }

        [Theory(DisplayName = "Retornar valor soma correto")]
        [Trait("Demo", "Validacao basicas")]
        [InlineData(1,1,2)]
        [InlineData(2, 2, 4)]
        [InlineData(36, 36, 72)]
        [InlineData(3, 3, 6)]
        [InlineData(15, 15, 30)]
        [InlineData(8, 8, 16)]
       
        public void Calculadora_Somar_RetornarValoresSomaCorretos(double v1,double v2,double total)
        {
            //Arange
            var calculadora = new Calculadora();

            //Act
            var resultado = calculadora.Somar(v1, v2);

            //Assert
            Assert.Equal(total, resultado);
        }
    }
}