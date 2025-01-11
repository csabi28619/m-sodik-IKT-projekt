using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Text;

namespace IKT_projekt_2
{
    public partial class MainWindow : Window
    {
        private const string FilePath = "alkatreszek.txt";

        public MainWindow()
        {
            InitializeComponent();
            LoadParts();
        }

        private void LoadParts()
        {
            PartsListBox.ItemsSource = LoadPartsFromFile();
        }

        public static class FileHandler
        {
            public static List<ComputerPart> LoadPartsFromFile(string FilePath)
            {
                if (!File.Exists(FilePath))
                    return new List<ComputerPart>();

                var lines = File.ReadAllLines(FilePath);
                return lines.Select(line =>
                {
                    var parts = line.Split(';');
                    return new ComputerPart
                    {
                        Type = parts[0],
                        Name = parts[1],
                        Parameters = parts[2],
                        Price = decimal.Parse(parts[3])
                    };
                }).ToList();
            }
            public static void ClearFile(string FilePath)
            {
                File.WriteAllText(FilePath, string.Empty);
            }

        }

        private void AddPartButton_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(PriceTextBox.Text, out var price))
            {
                var part = new ComputerPart
                {
                    Type = TypeTextBox.Text,
                    Name = NameTextBox.Text,
                    Parameters = ParametersTextBox.Text,
                    Price = price
                };

                if (!IsPartExist(part))
                {
                    SavePartToFile(part);
                    LoadParts();
                    MessageBox.Show("Alkatrész hozzáadva.");
                    ClearInputFields();
                }
                else
                {
                    MessageBox.Show("Az alkatrész már hozzá lett adva.");
                }
            }
            else
            {
                MessageBox.Show("Hamis érték.");
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var searchQuery = SearchNameTextBox.Text.ToLower();
            var parts = FileHandler.LoadPartsFromFile(FilePath);

            var results = parts.Where(p => p.Type.ToLower().Contains(searchQuery) || p.Name.ToLower().Contains(searchQuery)).ToList();

            PartsListBox.ItemsSource = results;
            ClearInputFields();
        }


        private void ApplyDiscountButton_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(DiscountTextBox.Text, out var discount))
            {
                var parts = LoadPartsFromFile();
                foreach (var part in parts)
                {
                    part.Price -= part.Price * (discount / 100);
                }
                SavePartsToFile(parts);
                LoadParts();
                ClearInputFields();
            }
            else
            {
                MessageBox.Show("Hamis érték.");
            }
        }

        private void StatisticsButton_Click(object sender, RoutedEventArgs e)
        {
            var parts = LoadPartsFromFile();
            var intelCount = parts.Count(p => p.Type.Equals("CPU", StringComparison.OrdinalIgnoreCase) && p.Name.Contains("Intel", StringComparison.OrdinalIgnoreCase));
            var amdCount = parts.Count(p => p.Type.Equals("CPU", StringComparison.OrdinalIgnoreCase) && p.Name.Contains("AMD", StringComparison.OrdinalIgnoreCase));
            var nvidiaCount = parts.Count(p => p.Type.Equals("GPU", StringComparison.OrdinalIgnoreCase) && p.Name.Contains("Nvidia", StringComparison.OrdinalIgnoreCase));
            var amdGpuCount = parts.Count(p => p.Type.Equals("GPU", StringComparison.OrdinalIgnoreCase) && p.Name.Contains("AMD", StringComparison.OrdinalIgnoreCase));
            var hyperxCount = parts.Count(p => p.Type.Equals("RAM", StringComparison.OrdinalIgnoreCase) && p.Name.Contains("HyperX", StringComparison.OrdinalIgnoreCase));

            MessageBox.Show($"Intel CPU: {intelCount}\nAMD CPU: {amdCount}\nNvidia GPU: {nvidiaCount}\nAMD GPU: {amdGpuCount}\nHyperX RAM: {hyperxCount}");
        }

        private void SavePartToFile(ComputerPart part)
        {
            File.AppendAllLines(FilePath, new[] { part.ToString() });
        }

        private void SavePartsToFile(IEnumerable<ComputerPart> parts)
        {
            File.WriteAllLines(FilePath, parts.Select(p => p.ToString()));
        }

        private List<ComputerPart> LoadPartsFromFile()
        {
            if (!File.Exists(FilePath))
                return new List<ComputerPart>();

            return File.ReadAllLines(FilePath).Select(line =>
            {
                var parts = line.Split(';');
                return new ComputerPart
                {
                    Type = parts[0],
                    Name = parts[1],
                    Parameters = parts[2],
                    Price = decimal.Parse(parts[3])
                };
            }).ToList();
        }

        private bool IsPartExist(ComputerPart part)
        {
            var existingParts = LoadPartsFromFile();
            return existingParts.Any(p => p.Type == part.Type && p.Name == part.Name && p.Parameters == part.Parameters);
        }

        private void ClearInputFields()
        {
            TypeTextBox.Clear();
            NameTextBox.Clear();
            ParametersTextBox.Clear();
            PriceTextBox.Clear();
            SearchNameTextBox.Clear();
            SearchTypeTextBox.Clear();
            DiscountTextBox.Clear();
        }

        private void GenerateHtmlButton_Click(object sender, RoutedEventArgs e)
        {
            var parts = LoadPartsFromFile();
            var htmlContent = GenerateHtml(parts);
            File.WriteAllText("alkatreszek.html", htmlContent);
            MessageBox.Show("HTML fájl generálva ide: alkatreszek.html.");
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Biztos törlöd a listát?", "Lista törlése", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                PartsListBox.ItemsSource = null;
                FileHandler.ClearFile(FilePath);
            }
            
        }


        private string GenerateHtml(IEnumerable<ComputerPart> parts)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang=\"hu\">");
            sb.AppendLine("<head>");
            sb.AppendLine("    <meta charset=\"UTF-8\">");
            sb.AppendLine("    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
            sb.AppendLine("    <style>");
            sb.AppendLine("        .parts-container { display: flex; flex-wrap: wrap; gap: 10px; }");
            sb.AppendLine("        .part { border: 1px solid #ccc; padding: 10px; width: 200px; box-shadow: 2px 2px 6px #aaa; }");
            sb.AppendLine("        .part h2 { margin: 0 0 10px 0; font-size: 1.2em; }");
            sb.AppendLine("        .part p { margin: 0; }");
            sb.AppendLine("    </style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("    <div class=\"parts-container\">");

            foreach (var part in parts)
            {
                sb.AppendLine($"        <div class=\"component\" data-name=\"{part.Type}\" data-price=\"{part.Price}\">");
                sb.AppendLine($"            <h3>{part.Name}</h3>");
                sb.AppendLine($"            <h3>{part.Parameters}</h3>");
                sb.AppendLine($"            <img src=\"images/....\" alt=\"{part.Type}\"");
                sb.AppendLine("        </div>");
            }

            sb.AppendLine("    </div>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }

    }


    public class ComputerPart
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Parameters { get; set; }
        public decimal Price { get; set; }

        public override string ToString()
        {
            return $"{Type};{Name};{Parameters};{Price}";
        }
    }
}
