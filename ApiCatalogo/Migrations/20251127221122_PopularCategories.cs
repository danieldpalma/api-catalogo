using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiCatalogo.Migrations
{
	/// <inheritdoc />
	public partial class PopularCategories : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder mb)
		{
			mb.Sql("INSERT INTO Categories(Name, ImageUrl) VALUES ('Drinks', 'drinks.jpg')");
			mb.Sql("INSERT INTO Categories(Name, ImageUrl) VALUES ('Foods', 'foods.jpg')");
			mb.Sql("INSERT INTO Categories(Name, ImageUrl) VALUES ('Desserts', 'desserts.jpg')");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder mb)
		{
			mb.Sql("TRUNCATE TABLE Categories");
		}
	}
}
