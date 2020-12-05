using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2020
{
    public static class DayFour
    {
        public static void Run()
        {
            var File = new StreamReader(Path.GetFullPath("Resource/DayFour.txt"));

            var CurrentLine = string.Empty;
            var PartOneCount = 0;
            var PartTwoCount = 0;

            var RequiredFields = new List<string>() {
                "byr","iyr","eyr","hgt","hcl","ecl","pid"
            };

            while ((CurrentLine = File.ReadLine()) != null)
            {
                var PassportContents = new Dictionary<string, string>();

                while (CurrentLine != null && !string.IsNullOrEmpty(CurrentLine))
                {
                    CurrentLine.Split(" ").ToList().ForEach(x => PassportContents.TryAdd(x.Split(":")[0], x.Split(":")[1]));
                    CurrentLine = File.ReadLine();
                }

                if (!RequiredFields.Except(PassportContents.Keys).Any())
                {
                    PartOneCount++;
                    if (IsPassportValid(PassportContents))
                    {
                        PartTwoCount++;
                    }
                }
            }
            File.Close();

            Console.WriteLine($"Part One Count: {PartOneCount}");
            Console.WriteLine($"Part Two Count: {PartTwoCount}");
        }

        private static bool IsPassportValid(Dictionary<string, string> passportDetails)
        {
            Match heightRegex = Regex.Match(passportDetails["hgt"], "(\\d+)(cm|in)", RegexOptions.IgnoreCase);
            // byr (Birth Year) - four digits; at least 1920 and at most 2002.
            var Valid = (int.TryParse(passportDetails["byr"], out int BirthYear) && BirthYear >= 1920 && BirthYear <= 2002)
            // iyr (Issue Year) - four digits; at least 2010 and at most 2020.
                        && (int.TryParse(passportDetails["iyr"], out int IssueYear) && IssueYear >= 2010 && IssueYear <= 2020)
            // eyr (Expiration Year) - four digits; at least 2020 and at most 2030.
                        && (int.TryParse(passportDetails["eyr"], out int ExpirationYear) && ExpirationYear >= 2020 && ExpirationYear <= 2030)
            // hgt (Height) - a number followed by either cm or in:
                        && (heightRegex.Success)
            // If cm, the number must be at least 150 and at most 193.
                        && ((heightRegex.Groups[2].Value.ToLower() == "cm"
                            && int.TryParse(heightRegex.Groups[1].Value, out int HeightCm)
                            && HeightCm >= 150 && HeightCm <= 193)
            // If in, the number must be at least 59 and at most 76.
                        || (heightRegex.Groups[2].Value.ToLower() == "in"
                            && int.TryParse(heightRegex.Groups[1].Value, out int HeightIn)
                            && HeightIn >= 59 && HeightIn <= 76))
            // hcl (Hair Color) - a # followed by exactly six characters 0-9 or a-f.
                        && (Regex.IsMatch(passportDetails["hcl"], "^#[0-9a-f]{6}$", RegexOptions.IgnoreCase))
            // ecl (Eye Color) - exactly one of: amb blu brn gry grn hzl oth.
                        && (Regex.IsMatch(passportDetails["ecl"], "^(amb|blu|brn|gry|grn|hzl|oth)$", RegexOptions.IgnoreCase))
            // pid (Passport ID) - a nine-digit number, including leading zeroes.
                        && (Regex.IsMatch(passportDetails["pid"], "^[\\d]{9}$", RegexOptions.IgnoreCase));


            return Valid;
        }
    }
}