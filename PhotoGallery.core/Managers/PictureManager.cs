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

        public bool DeletePicture(PictureModel obj)
        {
            return _pictureRepository.DeletePicture(obj) > 0 ? true : false;
        }

        public bool EditPicture(PictureModel obj)
        {
            return _pictureRepository.EditPicture(obj) > 0 ? true : false;
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
