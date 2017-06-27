using PhotoGallery.core.Interfaces.Managers;
using PhotoGallery.core.Interfaces.Repositories;
using PhotoGallery.core.Models;
using System.Collections.Generic;
using System;

namespace PhotoGallery.core.Managers
{
    public class AlbumManager : IAlbumManager<AlbumModel>
    {
        private IAlbumRepository<AlbumModel> _albumRepository;

        public AlbumManager(IAlbumRepository<AlbumModel> albumRepository)
        {
            _albumRepository = albumRepository;
        }

        public AlbumModel CreateAlbum(AlbumModel obj, int userID)
        {
            int createdAlbum = 0;

            var existingAlbum = GetAlbumByTitle(obj.AlbumTitle, userID);

            if (existingAlbum == null)
                createdAlbum = _albumRepository.CreateAlbum(obj, userID);    
            return createdAlbum > 0 ? GetAlbumByTitle(obj.AlbumTitle, userID) : null;
        }

        public bool DeleteAlbum(int albumID)
        {
            var result = _albumRepository.DeleteAlbum(albumID) > 0 ? true : false;
            return result;
        }

        public bool EditAlbum(AlbumModel obj, int userID)
        {
            return _albumRepository.EditAlbum(obj, userID) > 0 ? true : false;
        }

        public AlbumModel GetAlbumByID(int id)
        {
            return _albumRepository.GetAlbumByID(id);
        }

        public AlbumModel GetAlbumByTitle(string title, int userID)
        {
            return _albumRepository.GetAlbumByTitle(title, userID);
        }

        public IList<AlbumModel> GetAlbums(int userID)
        {
            return _albumRepository.GetAlbums(userID);
        }
    }
}
