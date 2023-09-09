namespace EdsSpotify
{
    public class Songs
    {
        public int Id { get; set; }
        public int TrackNumber { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }


        public int AlbumId { get; set; }
        public Albums Album { get; set; }
    }
}