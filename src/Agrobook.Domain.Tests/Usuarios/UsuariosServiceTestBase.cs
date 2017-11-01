using Agrobook.Domain.Tests.Utils;
using Agrobook.Domain.Usuarios;
using Agrobook.Common.Cryptography;
using Agrobook.Common.Serialization;
using Eventing.Core.Serialization;
using Eventing.TestHelpers;

namespace Agrobook.Domain.Tests.Usuarios
{
    public abstract class UsuariosServiceTestBase
    {
        protected TestableEventSourcedService<UsuariosService> sut;
        protected CryptoSerializer crypto;

        public UsuariosServiceTestBase()
        {
            this.crypto = new CryptoSerializer(new FauxCrypto(), new NewtonsoftJsonSerializer());
            this.sut = new TestableEventSourcedService<UsuariosService>(
                r => new UsuariosService(r, new SimpleDateTimeProvider(), this.crypto));
        }
    }
}
