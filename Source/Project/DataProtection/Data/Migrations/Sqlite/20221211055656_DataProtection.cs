﻿#nullable disable
using Microsoft.EntityFrameworkCore.Migrations;

namespace RegionOrebroLan.CertificateIdentity.DataProtection.Data.Migrations.Sqlite
{
	/// <inheritdoc />
	public partial class DataProtection : Migration
	{
		#region Methods

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "DataProtectionKeys");
		}

		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "DataProtectionKeys",
				columns: table => new
				{
					Id = table.Column<int>(type: "INTEGER", nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					FriendlyName = table.Column<string>(type: "TEXT", nullable: true),
					Xml = table.Column<string>(type: "TEXT", nullable: true)
				},
				constraints: table => { table.PrimaryKey("PK_DataProtectionKeys", x => x.Id); });
		}

		#endregion
	}
}