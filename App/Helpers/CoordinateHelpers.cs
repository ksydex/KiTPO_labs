using System;

namespace KiTPO.Helpers
{
    public static class CoordinateHelpers
    {
        public const double Accuracy = 0.02;
        public static (double, double) FlattenCoordinates(double x, double y, double offset, double scale) =>
            ((x - offset) / scale, ((y - offset) / scale) * -1);

        
        public static PointPosition GetPointPosition(double x, double y, double offset, double scale)
        {
            (x, y) = FlattenCoordinates(x, y, offset, scale);
            if (!(x >= -1 - Accuracy && x <= -0.4 + Accuracy)) return PointPosition.Outside;

            return Math.Sqrt(x * x + y * y) switch
            {
                < 1 + Accuracy and > 1 - Accuracy => PointPosition.OnTheBorder,
                < 1 => x >= -0.4 - Accuracy && x <= -0.4 + Accuracy ? PointPosition.OnTheBorder : PointPosition.Inside,
                > 1 => PointPosition.Outside,
                _ => PointPosition.Outside
            };
        }
    }
}