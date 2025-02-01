using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TimeQuestLogDesktopApp.Models.DTOs
{
    internal class UserGameDTO
    {
        private List<Genres> _genres = new List<Genres>();

        public event PropertyChangedEventHandler? PropertyChanged;
        public int GameId {  get; set; }

        public string Title { get; set; }
        public string Cover { get; set; }
        public string Exe { get; set; }
        public string Username { get; set; }
        public List<Genres> Genres
        {
            get => _genres;
            set
            {
                _genres = value;
            }
        }

        public UserGameDTO(int gameId, string title, string cover, string exe, string username)
        {
            GameId = gameId;
            Title = title;
            Cover = cover;
            Exe = exe;
            Username = username;
            Genres = new List<Genres>();
        }

        public string GenresToString => string.Join(", ", Genres.Select(genre => genre.Name));

    }
}
