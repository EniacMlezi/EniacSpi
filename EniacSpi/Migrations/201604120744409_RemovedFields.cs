namespace EniacSpi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedFields : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.User", "LastName");
            DropColumn("dbo.User", "FirstName");
            DropColumn("dbo.User", "Address");
            DropColumn("dbo.User", "PostalCode");
            DropColumn("dbo.User", "Country");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "Country", c => c.String(maxLength: 128));
            AddColumn("dbo.User", "PostalCode", c => c.String(maxLength: 128));
            AddColumn("dbo.User", "Address", c => c.String(maxLength: 128));
            AddColumn("dbo.User", "FirstName", c => c.String(maxLength: 50));
            AddColumn("dbo.User", "LastName", c => c.String(maxLength: 50));
        }
    }
}
