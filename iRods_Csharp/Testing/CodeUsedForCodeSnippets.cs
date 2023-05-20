using System;
using System.IO;
using System.Text;
using irods_Csharp;
using irods_Csharp.Enums;
using irods_Csharp.Objects;
using Objects.Objects;

namespace Testing
{
    // Class used to display base functionality of the library.
    // These snippets are used in the documentation.
    class SnippetProgram
    {
        private static void SnippetMain()
        {
            // Setting up a session using login parameters
            IrodsSession session = new (
                "host.rods.nl",
                9999,
                "/nlex1/home",
                "exampleuser@rods.com",
                "nlex1",
                AuthenticationScheme.Pam,
                24,
                null
            );

            // Getting hashed password
            string hashedPassword = session.Setup("secretPassword");

            // Starting session
            session.Start(hashedPassword);

            
            // Creating a new collection inside a directory which already exists
            session.CreateCollection("exampleDir/newCollection");

            // Opening this collection
            Collection newCollection = session.OpenCollection("exampleDir/newCollection");

            // Creating collection within this collection
            newCollection.CreateCollection("deeperCollection");

            // This collection can be renamed
            newCollection.RenameCollection("deeperCollection", "deeperCollectionV2");

            // Removing this new collection, this time from session
            session.RemoveCollection("exampleDir/newCollection/deeperCollectionV2");


            // Creating a new Data object inside a pre-existing directory
            session.CreateDataObject("exampleDir/newObject.txt");

            // Opening this new data object
            DataObject newObject = session.OpenDataObject("exampleDir/newObject.txt", Options.FileMode.ReadWrite);

            // Writing data to this new file
            newObject.Write(File.ReadAllBytes("myPc/exampleFile.txt"));

            // Writing file contents to console directly from session this time
            Console.Write(Encoding.UTF32.GetString(session.ReadDataObject("exampleDir/newObject.txt")));


            // Adding metadata to newCollection from before, without units
            session.AddMetadata(newCollection, "metaName", "metaValue");

            // Adding metadata directly, this time also with some units
            newCollection.AddMetadata("metaName", "metaValue", 8);

            // The following method will return an array with two meta objects: <metaName, metaValue> and <metaName, metaValue, 8>
            Metadata[] metadata = newCollection.QueryMetadata();

            // Metadata tags can also be removed, but the values need to be an exact match, even the units, as these do not aggregate
            newCollection.RemoveMetadata("metaName", "metaValue", 8);


            // Query all collections within /example/dir with a name that contains the word "apple"
            Collection[] collections = session.QueryCollectionPath("/example/dir", "apple");

            // A new query could be performed on the first of this collections, this time querying a data object
            // This particular query will find all .txt files within the collection
            DataObject[] objects = collections[0].QueryDataObject(".txt");

            // It is also possible to query based on meta data
            DataObject[] objects2 = session.MQueryDataObject("/example/dir", "color", "red", 5);

            // Not all meta data triple values need to be specified however,
            // this wil query all collections within newCollection
            Collection[] collections2 = newCollection.MQueryCollection(metaValue: "red");

            // Then it is also possible to get all the meta triples attached to a collection
            Metadata[] collectionMeta = collections2[0].QueryMetadata();
        }
    }
}