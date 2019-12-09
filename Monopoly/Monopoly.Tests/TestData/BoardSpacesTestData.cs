using Monopoly.Providers.Spaces;
using System.Collections.Generic;

namespace Monopoly.Tests.TestData
{
    static class BoardSpacesTestData
    {
        public static IEnumerable<Space> Data = new ExcelSpacesProvider(
            "./TestData/BoardSpaces.xlsx").GetSpacesAsync().Result;
    }
}
