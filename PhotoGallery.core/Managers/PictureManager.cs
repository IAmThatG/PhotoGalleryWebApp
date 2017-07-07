using PhotoGallery.core.Interfaces.Managers;
using PhotoGallery.core.Interfaces.Repositories;
using PhotoGallery.core.Models;
using System.Collections.Generic;

namespace PhotoGallery.core.Managers
{
    public class PictureManager : IPictureManager<PictureModel>
    {
        private IPictureRepository<PictureModel> _pictureRepository;

        public PictureManager(IPictureRepository<PictureModel> pictureRepository)
        {
            _pictureRepository = pictureRepository;
        }

        public bool AddPicture(PictureModel obj, int albumID)
        {
            return _pictureRepository.AddPicture(obj, albumID) > 0 ? true : false;
        }

        public bool DeletePicture(int pictureID)
        {
            return _pictureRepository.DeletePicture(pictureID) > 0 ? true : false;
        }

        public bool RenamePicture(PictureModel obj)
        {
            return _pictureRepository.RenamePicture(obj) > 0 ? true : false;
        }

        public List<PictureModel> GetAlbumPictures(int albumID)
        {
            return _pictureRepository.GetAlbumPictures(albumID);
        }

        public PictureModel GetPictureByID(int pictureID)
        {
            return _pictureRepository.GetPictureByID(pictureID);
        }
    }
}
