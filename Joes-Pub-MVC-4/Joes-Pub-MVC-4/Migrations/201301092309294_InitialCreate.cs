namespace Joes_Pub_MVC_4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Artists",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        JoomlaID = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                        UpdatedBy = c.Int(nullable: false),
                        Name = c.String(),
                        ShortName = c.String(),
                        Subtitle = c.String(),
                        Description = c.String(),
                        Webpage = c.String(),
                        Published = c.Boolean(nullable: false),
                        TileFilename = c.String(),
                        CheckedOut = c.Int(nullable: false),
                        CheckedOutOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ArtistComments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ArtistID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                        Name = c.String(),
                        Title = c.String(),
                        Message = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ArtistGenres",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ArtistID = c.Int(nullable: false),
                        GenreID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ArtistMp3s",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ArtistID = c.Int(nullable: false),
                        Name = c.String(),
                        FileName = c.String(),
                        Priority = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ArtistPhotos",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ArtistID = c.Int(nullable: false),
                        Caption = c.String(),
                        FileName = c.String(),
                        Priority = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ArtistVideos",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ArtistID = c.Int(nullable: false),
                        Caption = c.String(),
                        Priority = c.Int(nullable: false),
                        YouTubeLink = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ArtistVideos");
            DropTable("dbo.ArtistPhotos");
            DropTable("dbo.ArtistMp3s");
            DropTable("dbo.ArtistGenres");
            DropTable("dbo.ArtistComments");
            DropTable("dbo.Artists");
        }
    }
}
