using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace KiTPO.Helpers
{
    public static class FileProcessing
    {
        public async static Task<List<(double, double)>> GetPointsFromFile(string path)
        {
            var content = (await File.ReadAllLinesAsync(path)).ToList();
            var points = new List<Point>();
            foreach (var line in content)
            {
                var vals = line.Replace(" ", "").Split(",");
                
            }
            return new List<(double, double)>();
        }
    }
}