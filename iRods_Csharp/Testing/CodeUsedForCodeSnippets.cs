using System;
using System.IO;
using System.Text;
using irods_Csharp;

namespace Testing
{
    // Class used to display base functionality of the library.
    // These snippets are used in the documentation.
    class SnippetProgram
    {
        private static void SnippetMain()
        {
            // Setting up a session using login parameters
            IrodsSession session = new IrodsSession("host.rods.nl", 9999, "/nlex1/home", "exampleuser@rods.com", "nlex1", "Pam", 24, null);

            // Getting hashed password
            string hashedPassword = session.Setup("secretPassword");

            // Starting session
            session.Start(hashedPassword);

            
            // Creating a new collection inside a directory which already exists
            session.Collections.Create("exampleDir/newCollection");

            // Opening this collection
            Collection newCollection = session.Collections.Open("exampleDir/newCollection");

            // Creating collection within this collection
            newCollection.CreateCollection("deeperCollection");

            // This collection can be renamed
            newCollection.RenameCollection("deeperCollection", "deeperCollectionV2");

            // Removing this new collection, this time from session
            session.Collections.Remove("exampleDir/newCollection/deeperCollectionV2");


            // Creating a new Data object inside a pre-existing directory
            session.DataObjects.Create("exampleDir/newObject.txt");

            // Opening this new data object
            DataObj newObject = session.DataObjects.Open("exampleDir/newObject.txt", Options.FileMode.ReadWrite);

            // Writing data to this new file
            newObject.Write(File.ReadAllBytes("myPc/exampleFile.txt"));

            // Writing file contents to console directly from session this time
            Console.Write(Encoding.UTF32.GetString(session.DataObjects.Read("exampleDir/newObject.txt")));


            // Adding metadata to newCollection from before, without units
            session.Meta.AddMeta(newCollection, "metaName", "metaValue");

            // Adding metadata directly, this time also with some units
            newCollection.AddMeta("metaName", "metaValue", 8);

            // The following method will return an array with two meta objects: <metaName, metaValue> and <metaName, metaValue, 8>
            Meta[] metadata = newCollection.Meta();

            // Metadata tags can also be removed, but the values need to be an exact match, even the units, as these do not aggregate
            newCollection.RemoveMeta("metaName", "metaValue", 8);


            // Query all collections within /example/dir with a name that contains the word "apple"
            Collection[] collections = session.Queries.QueryCollection("/example/dir", "apple");

            // A new query could be performed on the first of this collections, this time querying a data object
            // This particular query will find all .txt files within the collection
            DataObj[] objects = collections[0].QueryObj(".txt");

            // It is also possible to query based on meta data
            DataObj[] objects2 = session.Queries.MQueryObj("/example/dir", "color", "red", 5);

            // Not all meta data triple values need to be specified however,
            // this wil query all collections within newCollection
            Collection[] collections2 = newCollection.MQueryCollection(metaValue: "red");

            // Then it is also possible to get all the meta triples attached to a collection
            Meta[] collectionMeta = collections2[0].Meta();
        }
    }
}