namespace App.Domain.Entity
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