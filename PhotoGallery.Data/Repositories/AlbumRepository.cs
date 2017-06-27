using PhotoGallery.core.Interfaces.Repositories;
using PhotoGallery.core.Models;
using PhotoGallery.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System;

namespace PhotoGallery.Data.Repositories
{
    public class AlbumRepository : IAlbumRepository<AlbumModel>
    {
        private DataContext _context;

        public AlbumRepository(DataContext context)
        {
            _context = context;
        }

        public int CreateAlbum(AlbumModel obj, int userID)
        {
            //map model properties to entity
            var albumEntity = new Album
            {
                AlbumTitle = obj.AlbumTitle,
                AlbumDescription = obj.AlbumDescription,
                UserID = userID
            };
            _context.Albums.Add(albumEntity);
            return _context.SaveChanges();
        }

        public int DeleteAlbum(int albumID)
        {
            var album = _context.Albums.SingleOrDefault(a => a.AlbumID == albumID);
            _context.Albums.Remove(album);
            return _context.SaveChanges();
        }

        public int EditAlbum(AlbumModel obj, int userID)
        {
            var albumEntity = new Album
            {
                AlbumID = obj.AlbumID,
                AlbumTitle = obj.AlbumTitle,
                AlbumDescription = obj.AlbumDescription,
                PictureCount = 0,
                UserID = userID
            };
            _context.Entry(albumEntity).State = System.Data.Entity.EntityState.Modified;
            return _context.SaveChanges();
        }

        public AlbumModel GetAlbumByID(int id)
        {
            var albumEntity = _context.Albums.Where(a => a.AlbumID == id).SingleOrDefault();
            var albumModel = new AlbumModel
            {
                AlbumID = albumEntity.AlbumID,
                AlbumTitle = albumEntity.AlbumTitle,
                AlbumDescription = albumEntity.AlbumDescription,
                PictureCount = albumEntity.PictureCount,
                UserID = albumEntity.UserID
            };
            return albumModel;
        }

        public AlbumModel GetAlbumByTitle(string title, int userID)
        {
            AlbumModel albumModel = null;
            var album = _context.Albums.Where(a => a.AlbumTitle.Equals(title) && a.UserID == userID).SingleOrDefault();

            if (album != null)
            {
                albumModel = new AlbumModel
                {
                    AlbumID = album.AlbumID,
                    AlbumTitle = album.AlbumTitle,
                    AlbumDescription = album.AlbumDescription,
                };
            }
            return albumModel;
        }

        public IList<AlbumModel> GetAlbums(int userID)
        {
            var albums = _context.Albums.Where(a => a.UserID == userID).OrderBy(a => a.AlbumTitle);
            var albumModel = (from album in albums
                             select new AlbumModel
                             {
                                 AlbumID = album.AlbumID,
                                 AlbumTitle = album.AlbumTitle,
                                 AlbumDescription = album.AlbumDescription,
                                 UserID = album.UserID,
                                 PictureCount = album.PictureCount
                             }).ToList();
            return albumModel;
        }
    }
}
