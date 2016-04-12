namespace EniacSpi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "PostalCode", c => c.String(maxLength: 128));
            AddColumn("dbo.User", "AccountConfirmed", c => c.Boolean(nullable: false));
            AlterColumn("dbo.User", "LastName", c => c.String(maxLength: 50));
            AlterColumn("dbo.User", "Address", c => c.String(maxLength: 128));
            AlterColumn("dbo.User", "Country", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.User", "Country", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.User", "Address", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.User", "LastName", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.User", "AccountConfirmed");
            DropColumn("dbo.User", "PostalCode");
        }
    }
}
