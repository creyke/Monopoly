using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Monopoly.Providers.Spaces
{
    public class ExcelSpacesProvider : ISpacesProvider
    {
        private readonly string fileName;

        public ExcelSpacesProvider(string fileName)
        {
            this.fileName = fileName;
        }

        public Task<IEnumerable<Space>> GetSpacesAsync()
        {
            using var fileStream = new FileStream(fileName, FileMode.Open);
            using var package = new ExcelPackage(fileStream);
            var worksheet = package.Workbook.Worksheets.First();

            var rowCount = worksheet.Dimension.Rows;
            var ColCount = worksheet.Dimension.Columns;

            var properties = new string[]
            {
                    nameof(Space.Name),
                    nameof(Space.Type),
                    nameof(Space.PropertyGroup),
                    nameof(Space.Fine),
                    nameof(Space.Cost),
                    nameof(Space.Rent1Apt),
                    nameof(Space.Rent2Apt),
                    nameof(Space.Rent3Apt),
                    nameof(Space.Rent4Apt),
                    nameof(Space.RentHotel),
                    nameof(Space.CostPerApt)
            };

            var columnMap = new Dictionary<int, string>();

            for (int col = 1; col <= ColCount; col++)
            {
                var colName = worksheet.Cells[1, col].Value.ToString();
                if (properties.Contains(colName))
                {
                    columnMap[col] = colName;
                }
            }

            int ParseOrZero(string raw)
            {
                return raw is null ? 0 : int.Parse(raw);
            }

            var spaces = new List<Space>();

            for (int row = 2; row <= rowCount; row++)
            {
                var space = new Space();
                for (int col = 1; col <= ColCount; col++)
                {
                    if (columnMap.ContainsKey(col))
                    {
                        var raw = worksheet.Cells[row, col].Value?.ToString();
                        switch (columnMap[col])
                        {
                            case nameof(Space.Name):
                                space.Name = raw;
                                break;
                            case nameof(Space.Type):
                                space.Type = raw;
                                break;
                            case nameof(Space.PropertyGroup):
                                space.PropertyGroup = raw;
                                break;
                            case nameof(Space.Fine):
                                space.Fine = ParseOrZero(raw);
                                break;
                            case nameof(Space.Cost):
                                space.Cost = ParseOrZero(raw);
                                break;
                            case nameof(Space.Rent1Apt):
                                space.Rent1Apt = ParseOrZero(raw);
                                break;
                            case nameof(Space.Rent2Apt):
                                space.Rent2Apt = ParseOrZero(raw);
                                break;
                            case nameof(Space.Rent3Apt):
                                space.Rent3Apt = ParseOrZero(raw);
                                break;
                            case nameof(Space.Rent4Apt):
                                space.Rent4Apt = ParseOrZero(raw);
                                break;
                            case nameof(Space.RentHotel):
                                space.RentHotel = ParseOrZero(raw);
                                break;
                            case nameof(Space.CostPerApt):
                                space.CostPerApt = ParseOrZero(raw);
                                break;
                            default:
                                break;
                        }
                    }
                }

                spaces.Add(space);
            }

            return Task.FromResult((IEnumerable<Space>)spaces);
        }
    }
}
