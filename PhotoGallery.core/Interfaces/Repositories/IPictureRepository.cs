using PhotoGallery.core.Models;
using System.Collections.Generic;

namespace PhotoGallery.core.Interfaces.Repositories
{
    public interface IPictureRepository<T> where T : PictureModel
    {
        //Add picture to album
        int AddPicture(T obj, int albumID);

        //Return all pictures in an album
        List<T> GetAlbumPictures(int albumID);

        //Get Picture By ID
        T GetPictureByID(int pictureID);

        //Edit Picture
        int EditPicture(T obj);

        //Delete Picture
        int DeletePicture(T obj);
    }
}
