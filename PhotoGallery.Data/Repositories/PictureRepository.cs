using PhotoGallery.core.Interfaces.Repositories;
using PhotoGallery.core.Models;
using PhotoGallery.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace PhotoGallery.Data.Repositories
{
    public class PictureRepository : IPictureRepository<PictureModel>
    {
        private DataContext _context;

        public PictureRepository(DataContext context)
        {
            _context = context;
        }

        public int AddPicture(PictureModel obj, int albumID)
        {
            var pictureEntity = new Picture
            {
                PictureTitle = obj.PictureTitle,
                FileName = obj.FileName,
                FilePath = obj.FilePath,
                FileSize = obj.FileSize,
                FileMime = obj.FileMime,
                AlbumID = albumID
            };
            _context.Pictures.Add(pictureEntity);
            return _context.SaveChanges();
        }

        public int DeletePicture(PictureModel obj)
        {
            var pictureEntity = new Picture
            {
                PictureID = obj.PictureID,
                PictureTitle = obj.PictureTitle,
                FileName = obj.FileName,
                FilePath = obj.FilePath,
                FileSize = obj.FileSize,
                FileMime = obj.FileMime,
                AlbumID = obj.AlbumID
            };
            _context.Pictures.Remove(pictureEntity);
            return _context.SaveChanges();
        }

        public int EditPicture(PictureModel obj)
        {
            var pictureEntity = new Picture
            {
                PictureTitle = obj.PictureTitle,
                FilePath = obj.FilePath,
            };
            _context.Entry(pictureEntity).State = System.Data.Entity.EntityState.Modified;
            return _context.SaveChanges();
        }

        public List<PictureModel> GetAlbumPictures(int albumID)
        {
            //Get all pictures in that album
            var pictures = _context.Pictures.Where(p => p.AlbumID == albumID);

            //return pictures as a list of picture models
            var pictureModel = (from picture in pictures
                    select new PictureModel
                    {
                        AlbumID = picture.AlbumID,
                        PictureID = picture.PictureID,
                        PictureTitle = picture.PictureTitle,
                        FileName = picture.FileName,
                        FilePath = picture.FilePath,
                        FileSize = picture.FileSize,
                        FileMime = picture.FileMime
                    }).ToList();
            return pictureModel;
        }

        public PictureModel GetPictureByID(int pictureID)
        {
            var pictureEntity = _context.Pictures.FirstOrDefault(p => p.AlbumID == pictureID);
            return new PictureModel
            {
                PictureID = pictureEntity.PictureID,
                PictureTitle = pictureEntity.PictureTitle,
                FileName = pictureEntity.FileName,
                FilePath = pictureEntity.FilePath,
                FileSize = pictureEntity.FileSize,
                FileMime = pictureEntity.FileMime
            };
        }
    }
}
