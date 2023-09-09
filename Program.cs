using System;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.EntityFrameworkCore;

namespace EdsSpotify
{
  class Program
  {

    static string PromtString(string prompt)
    {
      Console.WriteLine(prompt);
      var userInput = Console.ReadLine();

      return userInput;

    }
    static void Main(string[] args)
    {

      var context = new EdsSpotifyContext();

      var songAlbumBand = context.Albums.Include(value => value.Band).Include(value => value.Songs);


      foreach (var songBandAlbum in songAlbumBand)
      {
        Console.WriteLine($"Album {songBandAlbum.Title} Band {songBandAlbum.Band.Name}");

        foreach (var songs in songBandAlbum.Songs)
        {
          Console.WriteLine($"With Songs: {songs.Title} and DUration of {songs.Duration}");
        }
      }



    }
  }
}
