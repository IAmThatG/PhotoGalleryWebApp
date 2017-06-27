using PhotoGallery.core.Models;
using System.Collections.Generic;

namespace PhotoGallery.core.Interfaces.Repositories
{
    public interface IAlbumRepository<T> where T : AlbumModel
    {
        //Create New Album
        int CreateAlbum(T obj, int userID);

        //Get All Albums
        IList<T> GetAlbums(int userID);

        //Get Album By ID
        T GetAlbumByID(int id);

        //Get Album by title and userID
        T GetAlbumByTitle(string title, int userID);

        //Edit Album
        int EditAlbum(T obj, int userID);

        //Delete Album
        int DeleteAlbum(int albumID);
    }
}
