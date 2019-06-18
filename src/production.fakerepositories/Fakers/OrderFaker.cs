using Bogus;
using production.models;

// dotnet add package Bogus
namespace production.fakerepositories
{

    public class OrderFaker : Faker<Order>
    {
        public OrderFaker()
        {
            StrictMode(true);
            RuleFor(p => p.Id, f=>f.IndexFaker);
            RuleFor(p => p.Status, f => f.PickRandom<Order.OrderStatus>());
            RuleFor(p => p.CreatedOn, f => f.Date.Past());
            RuleFor(p => p.DueDate, f => f.Date.Future());
            RuleFor(p => p.IsDeleted, f => f.Random.Bool(0.2f));
            RuleFor(p => p.ItemName, f => f.Commerce.ProductName());
            RuleFor(p => p.Description, f => f.Lorem.Paragraph());
            RuleFor(p => p.ItemCode, f => f.Commerce.Ean8());
            RuleFor(p => p.Number, (f, o) => $"Order #{o.Id}/{o.CreatedOn.Year}");
            RuleFor(p => p.Quantity, f => f.Random.Int(1, 100));
            RuleFor(p => p.CreatedBy, f => f.Person.UserName);



        }
    }
}