using PhotoGallery.core.Models;
using System.Collections.Generic;

namespace PhotoGallery.core.Interfaces.Managers
{
    public interface IAlbumManager<T> where T : AlbumModel
    {
        //create new album
        T CreateAlbum(T obj, int userID);

        //Get all albums of a particular user
        IList<T> GetAlbums(int userID);

        //Get album by ID
        T GetAlbumByID(int id);

        //Get album by title and userID
        T GetAlbumByTitle(string title, int userID);

        //edit album
        bool EditAlbum(T obj, int userID);

        //delete album
        bool DeleteAlbum(int albumID);

        //Get number of pictures
        //int CountPictures(int albumID);
    }
}
