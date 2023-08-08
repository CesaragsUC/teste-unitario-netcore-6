using Bogus;
using NerdStore.BDD.Tests.Usuario;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace NerdStore.BDD.Tests.Config
{

    [CollectionDefinition(nameof(AutomacaoWebTestsFixtureCollection))]

    public class AutomacaoWebTestsFixtureCollection : ICollectionFixture<AutomacaoWebTestsFixture> { }
    public class AutomacaoWebTestsFixture
    {
        public SeleniumHelper BrowserHelper;
        public readonly ConfigurationHelper Configuration;


        public User Usuario;

        public AutomacaoWebTestsFixture()
        {
            Usuario = new User();
            Configuration = new ConfigurationHelper();
            BrowserHelper = new SeleniumHelper(Browser.Chrome, Configuration);
        }

        public void GerarDadosUsuario()
        {
            var faker = new Faker("pt_BR");
            Usuario.Email = faker.Internet.Email().ToLower();
            Usuario.Senha = faker.Internet.Password(8, false, "", "@1Ab_");
        }
    }
}
