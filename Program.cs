using System;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Globalization;
using System.Xml.Schema;
using System.Threading;


namespace EdsSpotify
{
  class Program
  {
    // Function to prompt the user for a string input
    static string PromptString(string prompt)
    {
      Console.WriteLine(prompt);
      var userInput = Console.ReadLine();
      return userInput;
    }

    // Function to prompt the user for a date input in the format "yyyy-MM-dd"
    static DateTime PromptDate(string prompt)
    {
      DateTime userInput;
      while (true)
      {
        Console.Write(prompt);
        string input = Console.ReadLine();
        if (DateTime.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out userInput))
        {
          return userInput;
        }
        else
        {
          Console.WriteLine("Invalid input. Please enter a valid date in the format yyyy-MM-dd.");
        }
      }
    }

    // Function to prompt the user for an integer input
    static int PromptInt(string prompt)
    {
      int userInput;
      while (true)
      {
        Console.Write(prompt);
        string input = Console.ReadLine();
        if (int.TryParse(input, out userInput))
        {
          return userInput;
        }
        else
        {
          Console.WriteLine("Invalid input. Please enter a valid integer.");
        }
      }
    }

    // Function to display a welcome message
    static void Greetings()
    {

      Console.WriteLine("");
      Console.WriteLine("");
      Console.WriteLine("Welcome to Ed'sSpotify");
      Console.WriteLine("Your Personal Music Playlist - \nCreate, Read, Update, Delete");
      Console.WriteLine("");


    }



    static void Main(string[] args)
    {

      // Create an instance of the database context.
      var context = new EdsSpotifyContext();

      bool menuKeepGoing = true;

      Greetings();

      while (menuKeepGoing)
      {
        int userMenuInput = Menu();

        switch (userMenuInput)
        {
          case 10:
            menuKeepGoing = quit();
            break;
          case 1:
            {
              var nameToSearch = PromptString("what is the Bands Name?: ");


              var searchNewBandName = context.Bands.FirstOrDefault(value => value.Name.ToUpper() == nameToSearch.ToUpper());

              if (searchNewBandName != null)
              {
                newBandIsNotNull(searchNewBandName);
              }
              else
              {
                string newBandName, countryOfOrigin, website, genre;
                int numberOfMembers;
                askingNewBandInfo(nameToSearch, out newBandName, out countryOfOrigin, out numberOfMembers, out website, out genre);

                int userInput;
                bool isSigned;
                newBandSigned(out userInput, out isSigned);

                if (userInput == 1)
                {
                  isSigned = true;
                  Console.WriteLine($"{newBandName} is Signed");

                }
                else if (userInput == 2)
                {
                  isSigned = false;
                  Console.WriteLine($"{newBandName} is NOT Signed");


                }
                else
                {
                  Console.WriteLine("Invalid input. Please enter 1 or 2.");
                  return;
                  // You might want to handle this case further, depending on your application's requirements.
                }

                var contactName = PromptString("What is the name of their contact?: ");

                newBandInfo(context, newBandName, countryOfOrigin, numberOfMembers, website, genre, isSigned, contactName);

              }

              break;
            }

          case 2:
            viewBands(context);

            break;
          case 3:
            {
              Bands bandNameSearch = bandNameForAlbum(context);

              if (bandNameSearch == null)
              {
                bandNameNull();
                return;
              }
              else
              {
                string albumTitle;
                bool IsExplicit;
                DateTime releaseDate;
                albumInfo(out albumTitle, out IsExplicit, out releaseDate);

                addingAlbumForBand(context, bandNameSearch, albumTitle, IsExplicit, releaseDate);

              }

              break;
            }

          case 4:
            {
              Albums albumNameSearch = albumName(context);

              if (albumNameSearch == null)
              {
                albumNull();
                return;

              }
              else
              {
                addingSong(context, albumNameSearch);

              }

              break;
            }

          case 5:
            unSignBand(context);
            break;
          case 6:
            signBand(context);

            break;
          case 7:
            PromptBandName(context);

            break;
          case 8:
            albumsByReleaseDate(context);

            break;
          case 9:
            isItSigned(context);

            break;
        }







      }
    }
    // Helper method to handle signing a new band
    private static void newBandSigned(out int userInput, out bool isSigned)
    {
      Console.WriteLine("Is the Band signed?: (1) Yes, Band is Signed (2) Band is NOT Signed");
      userInput = Convert.ToInt32(Console.ReadLine());
      isSigned = true;
    }
    // Helper method to display a message when a band already exists
    private static void newBandIsNotNull(Bands searchNewBandName)
    {
      Console.WriteLine($"Name for the Band {searchNewBandName} Already exist");
    }
    // Helper method to collect information for a new band
    private static void askingNewBandInfo(string nameToSearch, out string newBandName, out string countryOfOrigin, out int numberOfMembers, out string website, out string genre)
    {
      Console.WriteLine($"New Album Name:{nameToSearch}");
      newBandName = nameToSearch;
      countryOfOrigin = PromptString("\nWhat is the country of origin: ");
      numberOfMembers = PromptInt("How many Members are in the Band?: ");
      website = PromptString($"What is {newBandName} Website?:  ");
      genre = PromptString("what is the Genre?: ");
    }
    // Helper method to add a new band to the database
    private static void newBandInfo(EdsSpotifyContext context, string newBandName, string countryOfOrigin, int numberOfMembers, string website, string genre, bool isSigned, string contactName)
    {
      var newBand = new Bands
      {
        Name = newBandName,
        CountryOfOrigin = countryOfOrigin,
        NumberOfMembers = numberOfMembers,
        Website = website,
        Genre = genre,
        IsSigned = isSigned,
        ContactName = contactName
      };

      context.Bands.Add(newBand);
      context.SaveChanges();
      Console.WriteLine($"Success! Congratulations on adding {newBandName} to our record label!");
      Console.WriteLine("\n");
      Console.WriteLine("Going back to the Main menu now.");
    }
    // Helper method to collect information for a new album
    private static void albumInfo(out string albumTitle, out bool IsExplicit, out DateTime releaseDate)
    {
      // Now, you can add an album for the existing band.
      albumTitle = PromptString("Enter the title of the album: ");
      Console.WriteLine("Is the album explicit? \n(1)Yes, It's Explicit \n(2)No, It's Not Explicit: ");
      var userExplicit = Convert.ToInt32(Console.ReadLine());

      IsExplicit = true;
      if (userExplicit == 1)
      {
        IsExplicit = true;
        Console.WriteLine($"{albumTitle} is Signed");

      }
      else if (userExplicit == 2)
      {
        IsExplicit = false;
        Console.WriteLine($"{albumTitle} is NOT Signed");


      }

      releaseDate = PromptDate("Enter the release date (yyyy-MM-dd): ");
      releaseDate = DateTime.SpecifyKind(releaseDate, DateTimeKind.Utc);
    }
    // Helper method to add a new album for a band
    private static void addingAlbumForBand(EdsSpotifyContext context, Bands bandNameSearch, string albumTitle, bool IsExplicit, DateTime releaseDate)
    {
      var newAlbum = new Albums
      {
        Title = albumTitle,
        IsExplicit = IsExplicit,
        ReleaseDate = releaseDate,
        BandId = bandNameSearch.Id // Assign the band's ID to the album's BandId property.
      };



      context.Albums.Add(newAlbum);
      context.SaveChanges();
      Console.WriteLine($"Success! Congratulations on adding {albumTitle} to our record label!");
      Console.WriteLine("\n");
      Console.WriteLine("Going back to the Main menu now.");
    }
    // Helper method to handle the case when the band does not exist
    private static void bandNameNull()
    {
      Console.WriteLine("Please, go back to the menu and Insert that New Band First");
      return;
    }
    // Helper method to search for a band by name
    private static Bands bandNameForAlbum(EdsSpotifyContext context)
    {
      var bandNameGiven = PromptString("What is the Bands name you're trying to an Album in?: ");

      var bandNameSearch = context.Bands.FirstOrDefault(value => value.Name.ToUpper() == bandNameGiven.ToUpper());
      return bandNameSearch;
    }
    // Helper method to handle the case when the album does not exist
    private static void albumNull()
    {
      Console.WriteLine("Album does NOT Exist.\nPlease Add the Album First in Menu");
      return;
    }
    //Helper method to get Album name
    private static Albums albumName(EdsSpotifyContext context)
    {
      var nameAlbumGiven = PromptString("What is the name of the Album you want to Add a Song To?: ");

      var albumNameSearch = context.Albums.FirstOrDefault(value => value.Title.ToUpper() == nameAlbumGiven.ToUpper());
      return albumNameSearch;
    }
    // Helper method to add a new song to an album
    private static void addingSong(EdsSpotifyContext context, Albums albumNameSearch)
    {
      var songTitle = PromptString("What is the Name of the SONG?: ");

      var trackNumber = PromptInt($"What number track is the SONG in the Album {albumNameSearch}: ");

      var trackDuration = PromptInt("What is the Duration of the SONG in SECONDS: ");


      var newSong = new Songs
      {
        Title = songTitle,
        TrackNumber = trackNumber,
        Duration = trackDuration,
        AlbumId = albumNameSearch.Id // Associate the song with the found album
      };

      // Add the song to the database context and save changes
      context.Songs.Add(newSong);
      context.SaveChanges();

      Console.WriteLine($"Song '{songTitle}' added to ALBUM '{albumNameSearch.Title}'.");
    }
    // Helper method to display the main menu and get user input
    private static int Menu()
    {
      Console.WriteLine("What would you Like to do?:\n(1)Add a new band \n(2)View all the bands \n(3)Add an album for a band \n(4)Add a song to an album \n(5)Let a band go (update isSigned to false) \n(6)Resign a band (update isSigned to true) \n(7)Prompt for a band name and view all their albums \n(8)View all albums ordered by ReleaseDate \n(9)View bands that are signed or Un-Signed \n(10)Quit");
      var userMenuInput = Convert.ToInt32(Console.ReadLine().ToUpper());
      return userMenuInput;
    }
    // Helper method to view all bands
    private static void viewBands(EdsSpotifyContext context)
    {
      Console.WriteLine("Viewing All bands");

      var allBands = context.Bands.ToList();
      var bandCount = context.Bands.Count();

      foreach (var bands in allBands)
      {
        Console.WriteLine($"Band Name: {bands.Name} \nCountry of Origin: {bands.CountryOfOrigin} \nNumber of Members: {bands.NumberOfMembers} \nWebsite: {bands.Website} \nGenre: {bands.Genre} \nIs Signed: {bands.IsSigned} \nContact Name: {bands.ContactName}\n");
      }
      Console.WriteLine($"Total Bands in Ed's Spotify : {bandCount}\n");
    }
    // Helper method to un-sign a band
    private static void unSignBand(EdsSpotifyContext context)
    {
      var bandNameGiven = PromptString("What Band do you want to UN-Sign?: ");

      var searchNameGiven = context.Bands.FirstOrDefault(value => value.Name.ToUpper() == bandNameGiven.ToUpper());

      if (searchNameGiven == null)
      {
        Console.WriteLine("That Band is Not Found in Eds Spotify.\nPlease, add the album or try the name Band again");
      }
      else
      {
        Console.WriteLine($"Is {bandNameGiven} the right Band to Un-Sign?: \n(Y)Yes \n(N)No  ");
        var userYesOrNo = Console.ReadLine().ToUpper();

        if (userYesOrNo == "Y")
        {
          // Update the IsSigned property
          searchNameGiven.IsSigned = false;


          // Save the changes to the database
          context.SaveChanges();

          Console.WriteLine($"{bandNameGiven} has been successfully UN-Signed.");


        }
        else if (userYesOrNo == "N")
        {


          Console.WriteLine("Sorry to hear that. We all make mistakes. Going back to the Menu Now.");
          Console.WriteLine("");



        }









      }
    }
    // Helper method to sign a band
    private static void signBand(EdsSpotifyContext context)
    {
      var bandNameGiven = PromptString("What Band do you want to SIGN?: ");

      var searchNameGiven = context.Bands.FirstOrDefault(value => value.Name.ToUpper() == bandNameGiven.ToUpper());

      if (searchNameGiven == null)
      {
        Console.WriteLine("That Band is Not Found in Eds Spotify.\nPlease, add the album or try the name Band again");
      }
      else
      {
        Console.WriteLine($"Is {bandNameGiven} the right Band to SIGN?: \n(Y)Yes \n(N)No  ");
        var userYesOrNo = Console.ReadLine().ToUpper();

        if (userYesOrNo == "Y")
        {
          // Update the IsSigned property
          searchNameGiven.IsSigned = true;


          // Save the changes to the database
          context.SaveChanges();

          Console.WriteLine($"{bandNameGiven} has been successfully Signed.");
          Console.WriteLine("");


        }
        else if (userYesOrNo == "N")
        {


          Console.WriteLine("Sorry to hear that. We all make mistakes. Going back to the Menu Now.");
          Console.WriteLine("");



        }
      }
    }
    // Helper method to view albums ordered by release date
    private static void albumsByReleaseDate(EdsSpotifyContext context)
    {
      Console.WriteLine("Going to View All Albums By Release Date in Order ");
      Console.WriteLine("");

      var albumCount = context.Albums.Count();
      Console.WriteLine($"There are currently: {albumCount} Albums in Ed's Spotify");
      Console.WriteLine("");

      var orderAlbums = context.Albums.OrderBy(value => value.ReleaseDate).ToList();
      Console.WriteLine("Order from earliest release date to the latest release date");
      Console.WriteLine("");

      foreach (var album in orderAlbums)
      {

        Console.WriteLine($"");
        Console.WriteLine($"Album: {album.Title} - Release Date: {album.ReleaseDate}");
        Console.WriteLine($"");




      }
    }
    // Helper method to view signed or un-signed bands
    private static void isItSigned(EdsSpotifyContext context)
    {
      Console.WriteLine("How would you like to view Bands? By (S)Signed or (U)Un-Signed");
      var isItSigned = Console.ReadLine().ToUpper();
      if (isItSigned == "S")
      {

        var signedBands = context.Bands.Where(value => value.IsSigned).ToList();
        Console.WriteLine("Bands that are Signed:");
        foreach (var signedBand in signedBands)
        {

          Console.WriteLine("___________________________________");
          Console.WriteLine($"Band:{signedBand.Name} is SIGNED");
          Console.WriteLine("___________________________________");
        }




      }
      else if (isItSigned == "U")
      {

        var unSignedBands = context.Bands.Where(value => !value.IsSigned).ToList();
        Console.WriteLine("Bands that are Un-Signed:");
        foreach (var unSignedBand in unSignedBands)
        {

          Console.WriteLine("___________________________________");
          Console.WriteLine($"Band:{unSignedBand.Name} is UN-SIGNED");
          Console.WriteLine("___________________________________");

        }

      }
    }
    // Helper method to prompt for a band name and view their album discography
    private static void PromptBandName(EdsSpotifyContext context)
    {
      var nameGiven = PromptString("What is the Bands name that you would like to see their Album Discography?: ").ToUpper();

      var searchNameGiven = context.Bands.FirstOrDefault(value => value.Name.ToUpper() == nameGiven.ToUpper());

      if (searchNameGiven == null)
      {
        Console.WriteLine("No such Band is in Ed's Spotify.\nTry to add it or Re-write it");
      }
      else
      {

        Console.WriteLine($"Album Discography for {searchNameGiven.Name}:");

        // Query the albums for the selected band
        var bandAlbums = context.Albums.Where(album => album.BandId == searchNameGiven.Id).ToList();

        if (bandAlbums.Any())
        {
          foreach (var album in bandAlbums)
          {
            Console.WriteLine($"Title: {album.Title}");
            Console.WriteLine($"Release Date: {album.ReleaseDate.ToString("yyyy-MM-dd")}");
            Console.WriteLine($"Explicit: {(album.IsExplicit ? "Yes" : "No")}");
            Console.WriteLine();
          }
        }
        else
        {
          Console.WriteLine($"No albums found for {searchNameGiven.Name}.");
        }



      }
    }
    // Helper method to quit the program
    private static bool quit()
    {
      bool menuKeepGoing;
      Console.WriteLine("Thanks for indulging in EdsSpotify.\nGood-Bye");
      menuKeepGoing = false;
      return menuKeepGoing;
    }
  }
}



