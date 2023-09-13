# EdsSpotify

Explorer Mode

    Create a database that stores Albums, Bands, and Songs. They should have the following properties, use your best judgment for types. Also include the foreign keys when making your CREATE TABLE statements. HINT: You might have to create your tables in a specific order

        Albums
            Id
            Title
            IsExplicit
            ReleaseDate

        Bands
            Id
            Name
            CountryOfOrigin
            NumberOfMembers
            Website
            Genre
            IsSigned
            ContactName

        Songs
            Id
            Track Number
            Title
            Duration

        A Band has many Albums and Album belongs to one Band. An Album has many Songs and a Song belongs to one Album.

    Create a menu system that shows the following options to the user until they choose to quit your program
        Add a new band
        View all the bands
        Add an album for a band
        Add a song to an album
        Let a band go (update isSigned to false)
        Resign a band (update isSigned to true)
        Prompt for a band name and view all their albums
        View all albums ordered by ReleaseDate
        View all bands that are signed
        View all bands that are not signed
        Quit the program
