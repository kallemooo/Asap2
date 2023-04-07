using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApprovalTests;
using ApprovalTests.Reporters;
using NUnit.Framework;
using PublicApiGenerator;

namespace Asap2.Tests
{
    [TestFixture]
    [UseReporter(typeof(DiffReporter))]
    public class ApiSpec
    {
        [Test]
        public void ApproveApiChanges()
        {
            var publicApi = ApiGenerator.GeneratePublicApi(typeof(Asap2File).Assembly,
                new ApiGeneratorOptions { IncludeAssemblyAttributes = false, });

            Approvals.Verify(publicApi);
        }
    }
}
