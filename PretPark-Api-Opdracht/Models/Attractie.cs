namespace PretPark_Api_Opdracht.Models;
// Attractie.cs
public class Attractie
{
    public int Id { get; set; }
    public string Naam { get; set; } = null!;
    public DateTime bouwJaar {get;set;}
    public int engheid {get;set;}

    public List<GebruikerMetWachwoord> UserLikes = new List<GebruikerMetWachwoord>();
}