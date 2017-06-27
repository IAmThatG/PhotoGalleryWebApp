using PhotoGallery.core.Models;
using System.Collections.Generic;

namespace PhotoGallery.core.Interfaces.Managers
{
    public interface IPictureManager<T> where T : PictureModel
    {
        //Add picture to album
        bool AddPicture(T obj, int albumID);

        //Return all pictures in an album
        List<T> GetAlbumPictures(int albumID);

        //Get Picture By ID
        T GetPictureByID(int pictureID);

        //Edit Picture
        bool EditPicture(T obj);

        //Delete Picture
        bool DeletePicture(T obj);
    }
}
