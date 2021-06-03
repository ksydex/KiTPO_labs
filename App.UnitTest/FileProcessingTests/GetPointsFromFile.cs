using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using KiTPO.Enums;
using KiTPO.Helpers;
using Xunit;

namespace App.UnitTest.FileProcessingTests
{
    public class GetPointsFromFile
    {
        public GetPointsFromFile()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        }

        [Theory]
        [MemberData(nameof(ReturnsAwaitedResultData))]
        public async Task ReturnsAwaitedResult(string path, GetPointsFromFileAwaitedResult awaitedResult)
        {
            var (error, list) = await FileProcessing.GetPointsFromFile(path);
            Assert.Equal(error, awaitedResult.FileReadError);
            if (error == null) Assert.Equal(list.Count, awaitedResult.ListCount);
        }

        [Fact]
        public async Task ParsesNumbersProperly()
        {
            var (error, list) =
                await FileProcessing.GetPointsFromFile("C:/dotnet/KiTPO/App.UnitTest/dataNumberParsing.txt");
            Assert.Null(error);
            Assert.Equal(list, new List<(double, double)> { (-0.4, 1.1), (2, 3) });
        }

        public static readonly List<object[]> ReturnsAwaitedResultData = new()
        {
            new object[]
            {
                "C:/dotnet/KiTPO/App.UnitTest/dataEmpty.txt",
                new GetPointsFromFileAwaitedResult { FileReadError = FileReadError.FileIsEmpty },
            },
            new object[]
            {
                "C:/dotnet/KiTPO/App.UnitTest/data.txt",
                new GetPointsFromFileAwaitedResult { FileReadError = null, ListCount = 8 },
            },
            new object[]
            {
                "C:/dotnet/KiTPO/App.UnitTest/dataOutOfRange.txt",
                new GetPointsFromFileAwaitedResult { FileReadError = FileReadError.OutOfRange },
            },
            new object[]
            {
                "C:/dotnet/KiTPO/App.UnitTest/dataWrongFormat.txt",
                new GetPointsFromFileAwaitedResult { FileReadError = FileReadError.WrongFormat },
            },
        };
    }

    public class GetPointsFromFileAwaitedResult
    {
        public FileReadError? FileReadError { get; set; }
        public int ListCount { get; set; }
    }
}