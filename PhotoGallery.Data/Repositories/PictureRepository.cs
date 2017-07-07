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

        public int DeletePicture(int pictureID)
        {
            var picture = _context.Pictures.SingleOrDefault(a => a.PictureID == pictureID);
            _context.Pictures.Remove(picture);
            return _context.SaveChanges();
        }

        public int RenamePicture(PictureModel obj)
        {
            var pictureEntity = _context.Pictures.SingleOrDefault(p => p.PictureID == obj.PictureID);
            pictureEntity.PictureTitle = obj.PictureTitle;
            //var pictureEntity = new Picture
            //{
            //    PictureID = pictureModel.PictureID,
            //    AlbumID = pictureModel.AlbumID,
            //    PictureTitle = pictureModel.PictureTitle,
            //    FileName = pictureModel.FileName,
            //    FilePath = pictureModel.FilePath,
            //    FileSize = pictureModel.FileSize,
            //    FileMime = pictureModel.FileMime
            //};
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
            var pictureEntity = _context.Pictures.SingleOrDefault(p => p.PictureID == pictureID);
            return new PictureModel
            {
                PictureID = pictureEntity.PictureID,
                AlbumID = pictureEntity.AlbumID,
                PictureTitle = pictureEntity.PictureTitle,
                FileName = pictureEntity.FileName,
                FilePath = pictureEntity.FilePath,
                FileSize = pictureEntity.FileSize,
                FileMime = pictureEntity.FileMime
            };
        }
    }
}
