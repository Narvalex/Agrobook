using Agrobook.Domain.Common;
using System;

namespace Agrobook.Domain.Tests
{
    public static class TestFirma
    {
        public static Firma New => new Firma("Test", DateTime.Now);
    }
}
