using System.Collections.Generic;
using KiTPO.Enums;
using KiTPO.Helpers;
using Xunit;

namespace App.UnitTest.CoordinatesProcessingTests
{
    public class GetPointPosition
    {
        private const double Accuracy = CoordinatesProcessing.Accuracy;
        private const double Offset = 200;
        private const double ScaleValue = 114;

        [Theory]
        [MemberData(nameof(ReturnsAwaitedResultData))]
        public void  ReturnsAwaitedResult(double x, double y, PointPosition awaitedResult)
        {
            (x, y) = CoordinatesProcessing.UnFlattenCoordinates(x, y, Offset, ScaleValue);
            
            var (p, _, _) = CoordinatesProcessing.GetPointPosition(x, y, Offset, ScaleValue);
            
            Assert.Equal(awaitedResult, p);
        }
        
        public static readonly List<object[]> ReturnsAwaitedResultData = new()
        {
            new object[] { -0.5, -0.5, PointPosition.Inside },
            new object[] { 1, 1, PointPosition.Outside },
            new object[] { -0.4, 0.6, PointPosition.OnTheBorder },
        };
    }
}