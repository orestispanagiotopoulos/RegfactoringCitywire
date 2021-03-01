using App.Enum;

namespace App.Model.Entities
{
    public class Company
    {
        public const string VeryImportantClient = "VeryImportantClient";
        public const string ImportantClient = "ImportantClient";

        public int Id { get; set; }

        public string Name { get; set; }

        public Classification Classification { get; set; }
    }
}