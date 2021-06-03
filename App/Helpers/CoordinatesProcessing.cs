using System;
using KiTPO.Enums;

namespace KiTPO.Helpers
{
    public static class CoordinatesProcessing
    {
        public const double Accuracy = 0.02;

        public static (double, double) FlattenCoordinates(double x, double y, double offset, double scale) =>
            ((x - offset) / scale, ((y - offset) / scale) * -1);

        public static (double, double) UnFlattenCoordinates(double x, double y, double offset, double scale) => (
            (x * scale) + offset, ((-y * scale) + offset));
        
        public static (PointPosition, double, double) GetPointPosition(double x, double y, double offset, double scale)
        {
            (x, y) = FlattenCoordinates(x, y, offset, scale);
            if (!(x >= -1 - Accuracy && x <= -0.4 + Accuracy)) return (PointPosition.Outside, x, y);

            return (Math.Sqrt(x * x + y * y) switch
            {
                < 1 + Accuracy and > 1 - Accuracy => PointPosition.OnTheBorder,
                < 1 => x >= -0.4 - Accuracy && x <= -0.4 + Accuracy ? PointPosition.OnTheBorder : PointPosition.Inside,
                > 1 => PointPosition.Outside,
                _ => PointPosition.Outside
            }, x, y);
        }

        public static string GenerateMessage(PointPosition p, double x, double y)
            => "|  Точка с координатами x: " + x.ToString("0.##") + ", y: " + y.ToString("0.##") + " находится " + p switch
            {
                PointPosition.Inside => "внутри зоны",
                PointPosition.OnTheBorder => " на границе зоны",
                PointPosition.Outside => "за зоной",
                _ => ""
            };
    }
}