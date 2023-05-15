
using Dapper;

namespace STGeneticsTest.Models
{
    public static class AnimalData
    {
        // ↓ Some options to randomize that with Bogus
        public static string[] AnimalBreed = new[] { "Angus", "Hereford", "Charolais", "Simmental", "Red Angus", "Limousin", "Gelbvieh", "Brangus", "Beefmaster", "Salers" };
        public static string[] AnimalSex = new[] { "F", "M" };
    }

    public class Animal
    {
        public int AnimalId { get; set; }
        public string Name { get; set; }
        public string Breed { get; set; }
        public DateTime BirthDate { get; set; }
        public string Sex { get; set; }
        public double Price { get; set; }
        public bool Status { get; set; }
    }

    public class AnimalDto
    {
        public string Name { get; set; }
        public string Breed { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Sex { get; set; }
        public double? Price { get; set; }
        public bool? Status { get; set; }

        public Animal ToAnimal(int id)
        {
            return new Animal
            {
                AnimalId = id,
                Name = this.Name,
                Breed = this.Breed,
                BirthDate = (DateTime)this.BirthDate,
                Sex = this.Sex,
                Price = (double)this.Price,
                Status = (bool)this.Status,
            };
        }
    }

    public class AnimalFilterDto
    {
        public int? AnimalId { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public bool? Status { get; set; }
    }

    public class AnimalPurchaseDto
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public bool Status { get; set; }
    }

}
