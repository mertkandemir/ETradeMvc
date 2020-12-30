namespace ETradeDataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        UnitPrice = c.Double(nullable: false),
                        StockAmount = c.Int(),
                        CategoryId = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        ImagePath = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 25),
                        EMail = c.String(maxLength: 300),
                        BirthDay = c.DateTime(),
                        Active = c.Boolean(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .Index(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Products", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropIndex("dbo.Products", new[] { "CategoryId" });
            DropTable("dbo.Users");
            DropTable("dbo.Roles");
            DropTable("dbo.Products");
            DropTable("dbo.Categories");
        }
    }
}
