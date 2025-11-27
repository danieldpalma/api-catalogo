using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiCatalogo.Migrations
{
	/// <inheritdoc />
	public partial class PopularProducts : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder mb)
		{
			mb.Sql("INSERT INTO Products(Name, Description, Price, ImageUrl, Supply, CreatedAt, CategoryId) Values('Coke Diet', 'Diet Coke 350ml', '5.45', 'diet-coke.jpg', 50, now(), 1)");
			mb.Sql("INSERT INTO Products(Name, Description, Price, ImageUrl, Supply, CreatedAt, CategoryId) Values('Hot-Dog', 'Hot Dog', '7', 'hot-dog.jpg', 20, now(), 2)");
			mb.Sql("INSERT INTO Products(Name, Description, Price, ImageUrl, Supply, CreatedAt, CategoryId) Values('Chocolate Donut', 'Medium Chocolate Donut', '1.50', 'chocolate-donut.jpg', 10, now(), 2)");
			mb.Sql("INSERT INTO Products(Name, Description, Price, ImageUrl, Supply, CreatedAt, CategoryId) Values('Cheesecake', 'Piece Cheesecake', '9.50', 'cheesecake.jpg', 5, now(), 3)");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder mb)
		{
			mb.Sql("TRUNCATE TABLE Products");
		}
	}
}
