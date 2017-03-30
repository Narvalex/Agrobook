using Agrobook.Domain.Usuarios.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Agrobook.Domain.Tests.Usuarios
{
    [TestClass]
    public class UsuariosDenormalizerTests
    {
        private UsuariosDenormalizer sut;

        public UsuariosDenormalizerTests()
        {
            this.sut = new UsuariosDenormalizer(null);
        }

         
    }
}
