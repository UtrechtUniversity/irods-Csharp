# irods-Csharp

## Installation
1. Download the repository .
2. Build the solution (using visualstudio).
3. Add a reference to irods-Csharp.dll in your solution.
4. Done.

## Usage

### Session startup

For any action that the user wishes to perform on the IRODS server, a session object is required.\
The session contains the following objects that contain operations corresponding to that category:

* IrodsSession.Collections
* IrodsSession.DataObjects
* IrodsSession.Queries
* IrodsSession.Meta

#### Authentication

When starting a new session, authentication parameters are required to authenticate with the server.\
The parameters required are:
* host : string - Domain name of server host
* port : int - Port on which server can be reached
* home : string - Collection name of home directory
* user : string - User name of user
* zone : string - Zone name of server
* scheme : string - Authentication scheme to be used (Currently only “pam” is supported)

When a session has been created, the session needs to be set up using the users password. The setup method will return a hashed password.
This hashed password can then be used to start the session after which the session is ready to be used.

```csharp
// Setting up a session using login parameters
IrodsSession session = new IrodsSession("host.rods.nl", 9999, "/nlex1/home", "exampleuser@rods.com", "nlex1","pam", 24);
// Getting hashed password
string hashedPassword = session.Setup("secretPassword");
// Starting session
session.Start(hashedPassword);
```

### Using Collections

Methods regarding collections are located within the collection manager of the session : IrodsSession.Collections\
It contains the following methods:
* Rename - Rename a collection
* Open - Returns a collection object for the specified path
* Create - Creates new collection at specified path
* Remove - Removes collection at specified path

From the session, a new collection can be created, after which it can be opened.\
When opening a collection, a Collection object is returned which can be used to perform actions inside the corresponding collection

```csharp
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
```

#### The Collection class

Collection objects house almost every method available within the iRods_Csharp library. The advantage of using a collection object is that the methods that it contains will be executed using the collection’s path as location for executing the method.
For example, when creating a new collection using session.Collections, one would need to pass two parameters, the path of the collection where the new collection should be created and the name of the new collection.
When using a collection object, only the name of the new collection is needed, the collection is created inside of the collection object’s path.

The collection class also houses some methods not available in session.Collections:
* Collection.ChangeDirectory - Changes collection directory, as one would do using cd
* Collection.Rename - Changes the current collection’s name
* Collection.PrintDirectory - Returns string of the currents collections’ path
* Collection.Meta - Returns array of metadatas attached to this collection

From a Collection object, the following methods are available which perform methods from IrodsSession.DataObjects using its own path as starting point:
* Collection.OpenCollection
* Collection.RenameCollection
* Collection.CreateCollection
* Collection.RemoveCollection

### Using Data Objects

Methods regarding data objects are found within the DataObject manager of the session : IrodsSession.DataObjects\
It contains the following methods:
* Open - Returns DataObject object for the specified path and name (requires filemode Read, Write or ReadWrite from Options.FileMode enum)
* Create - Creates new data object in specified path
* Write - Writes byte array to specified file
* Read - Returns byte array with content of specified file
* Remove - Removes specified file

#### The DataObj Class

When opening a data object with IrodsSession.DataObjects.Open, a DataObj Object is returned. This object contains the following methods:
* DataObj.Write - Write byte array to this file
* DataObj.Insert - Write byte array at the beginning of this file
* DataObj.Seek - Offset the file pointer from start, current offset or end (seekmode)
* DataObj.Read - Returns byte array with contents of this file
* DataObj.Remove - Removes this file
* DataObj.Meta - Returns an array of metadatas attached to this data object

#### Access from Collection Object

From a collection object, the following methods are available which perform methods from IrodsSession.DataObjects using its own path as starting point:
* Collection.OpenDataObj
* Collection.RenameDataObj
* Collection.CreateDataObj
* Collection.WriteDataObj
* Collection.ReadDataObj
* Collection.RemoveDataObj

Below is an example of the Data Object Functionality

```csharp
// Creating a new Data object inside a pre-existing directory
session.DataObjects.Create("exampleDir/newObject.txt");

// Opening this new data object
DataObj newObject = session.DataObjects.Open("exampleDir/newObject.txt", Options.FileMode.ReadWrite);

// Writing data to this new file
newObject.Write(File.ReadAllBytes("myPc/exampleFile.txt"));

// Writing file contents to console directly from session this time
Console.Write(Encoding.UTF32.GetString(session.DataObjects.Read("exampleDir/newObject.txt")));
```

### Adding Metadata

Metadata can be added to Collections or Data Objects using the MetaManager in IrodsSession: IrodsSession.Meta\
It contains the following methods:
* AddMeta - Adds meta Name, Value and (optionally) Units triple
* RemoveMeta - Removes meta Name, Value and (optionally) Units triple

Units are optional. However, they won’t stack.\
Adding triple <name,value,1> to an object which already has a tag <name,value,2> will result in the object having both tags attached to it as unique tags.
To use these methods, the user needs to supply either a DataObj or Collection Object and the meta values, or call the methods directly from these objects.

Below is a small example of adding metadata to collections and data objects

```csharp
// Adding metadata to newCollection from before, without units
session.Meta.AddMeta(newCollection, "metaName", "metaValue");

// Adding metadata directly, this time also with some units
newCollection.AddMeta("metaName", "metaValue", 8);

// The following method will return an array with two meta objects: <metaName, metaValue> and <metaName, metaValue, 8>
Meta[] metadata = newCollection.Meta();

// Metadata tags can also be removed, but the values need to be an exact match, even the units, as these do not aggregate
newCollection.RemoveMeta("metaName", "metaValue", 8);
```

### Performing queries

Methods to query data are found in the query manager of the IrodsSession : IrodsSession.Queries\
It contains the following methods:
* QueryCollection - query collections based on name
* MQueryCollection - query collections based on metadata
* QueryObj - query objects based on name
* MQueryObj - query objects based on metadata
* QueryMeta - query meta tags for collection or data objects

When using the QueryCollection and QueryObj methods, the a name and a path need to be provided. The query will search for all collections or data objects within that path or deeper that contain the provided name in their path.
For example, consider the path “home/collection” and name “example”

Executing the method
```csharp
session.Queries.QueryCollection(“home/collection”, “example”);
```
Might return an array of collections with the following paths:
* “home/collection/example”
* “home/collection/abcexample12”
* “home/collection/example/nestedCollection”

#### Queries based on metadata

MQueryCollection and MQueryObj will perform a query to find collections or data objects which are tagged with metadata with the specified values.
It is not required to specify all three parts of the metadata triple.

For example, one could query all objects with a specific metadata triple
```csharp
session.Queries.MQueryObj(“path”, “name”,”value”,1);
```
But if the user would like to query all objects which have metadata that matches a certain metadata value, then the user would need to specify it wants to query based on metadata value:
```csharp
session.Queries.MQueryObj(“path”, metaValue : “value”);
```

#### Querying metadata

Querying metadata attached to a collection or data object can be done either by calling IrodsSession.Queries.QueryMeta using the path, name and type of the object (e.g. -c or -d), or by calling the .Meta() function of a Collection or Data Object object:
Collection.Meta() and DataObj.Meta()

#### Access from Collection object

As with most methods that have a path as paramenter, the query methods can also be called directly from a collection object. The following methods are supported:
* Collection.QueryCollection
* Collection.MQueryCollection
* Collection.QueryObj
* Collection.MQueryObj

Below is a code snippet showing the query functionality

```csharp
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
```

### Logging

There are two TextWriters in the Utility class which are set to null by default:
Utility.Log and Utility.Text
Utility.Log is used by the library to write debugging data such as messages that are sent to and received from the server, Utility.Text can be used to write a line with a console color by using the Utility.WriteText method.\
For example, to make sure the library writes debug data to the console, simply set the logger:
```csharp
Utility.Log = Console.Out;
```

## To-do
* Multiple parallel connections
* Other authentication schemes next to PAM
* Query parser to allow more customized queries

## Known bugs
* No bugs detrimental to normal use are known at the moment.
* If any bugs are found, please report this using the contact information below

## Contact info
Any question or problems regarding this library can be mailed to h.fidder2@students.uu.nl