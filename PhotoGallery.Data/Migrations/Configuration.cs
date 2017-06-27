namespace PhotoGallery.Data.Migrations
{
    using Entities;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(DataContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            var user = new User { Firstname = "Gabriel", Lastname = "Okolie", Email = "user@email.com", Password = "password" };
            var album = new Album { AlbumTitle = "Test Album", AlbumDescription = "This is a test Album", };
            var picture = new Picture { PictureTitle = "TestPic", FileSize = 1000, FileMime = "jpg", FileName = "geometry_dash_v2-wallpaper-1600x900.jpg", FilePath = "~/Pictures/geometry_dash_v2-wallpaper-1600x900.jpg" };

            user.Albums = new List<Album> { album };

            album.User = user;
            album.Pictures = new List<Picture> { picture };
            album.PictureCount = album.Pictures.Count();

            picture.Album = album;

            context.Users.AddOrUpdate(u => u.Email, user);
            context.Albums.AddOrUpdate(a => a.AlbumTitle, album);
            context.Pictures.AddOrUpdate(p => p.FileName, picture);
        }
    }
}
