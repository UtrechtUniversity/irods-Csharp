using System;
using System.Text;
using Enums.Options;
using irods_Csharp;
using irods_Csharp.Enums;
using irods_Csharp.Objects;
using Objects.Objects;
using Testing;
using FileMode = Enums.Options.FileMode;
using Microsoft.Extensions.Configuration;
Utility.Log = Console.Out;
Utility.Text = Console.Out;

IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true)
    .AddJsonFile("appsettings.Development.json", true, true);
AccountOptions options = builder.Build().Get<AccountOptions>()!;

ClientServerNegotiation clientServerNegotiation = new (
    ClientServerPolicyRequest.RequireSSL,
    options.irods_encryption_algorithm,
    options.irods_encryption_key_size,
    options.irods_encryption_salt_size,
    options.irods_encryption_num_hash_rounds
);
IrodsSession testSession = new (
    options.irods_host,
    options.irods_port,
    options.irods_home,
    options.irods_user_name,
    options.irods_zone_name,
    options.irods_authentication_scheme switch
    {
        "native" => AuthenticationScheme.Native,
        "pam" or _ => AuthenticationScheme.Pam
    },
    24,
    clientServerNegotiation
);
            
bool connected = false;
if (options.irods_password is { } envPassword)
{
    try
    {
        string hashedPassword = testSession.Setup(envPassword);
        testSession.Authenticate(hashedPassword);
        connected = true;
    }
    catch (Exception e) when (e.Message is "CAT_INVALID_AUTHENTICATION" or "CAT_INVALID_USER")
    {

    }
}
while (!connected)
{
    try
    {
        string password = testSession.Setup(Utility.GetPassword());
        string hashedPassword = testSession.Setup(password);
        testSession.Authenticate(hashedPassword);
        connected = true;
    }
    catch (Exception e2)
    {
        if (e2.Message == "PAM_AUTH_PASSWORD_FAILED") Utility.WriteText(ConsoleColor.Red, "Authentication error");
    }
}

Collection testCollection = testSession.HomeCollection();
DataObject? testDataObj = null;

void SetDataObject(DataObject? dataObject)
{
    if (testDataObj is { } old) old.Dispose();
    testDataObj = dataObject;
}

bool busy = true;
while (busy)
{
    try
    {
        if (testDataObj is not { } obj)
        {
            Utility.WriteText(
                ConsoleColor.Yellow,
                "Input Command (cd, pwd, mkdir, rmdir, rename, create, open, query, addMeta, rmMeta, stop)"
            );
            {
                string[] input = Console.ReadLine()!.Split(' ');
                switch (input[0])
                {
                    case "cd":
                        testCollection = testCollection.OpenCollection(input[1]);
                        break;
                    case "pwd":
                        Utility.WriteText(ConsoleColor.Cyan, testCollection.Path);
                        break;
                    case "mkdir":
                        testCollection.CreateCollection(input[1]);
                        break;
                    case "rmdir":
                        testCollection.RemoveCollection(input[1]);
                        break;
                    case "rename":
                        switch (input[1])
                        {
                            case "object":
                                testCollection.RenameDataObj(input[2], input[3]);
                                break;
                            case "collection":
                                testCollection.RenameCollection(input[2], input[3]);
                                break;
                            case "this":
                                testCollection.Rename(input[2]);
                                break;
                        }

                        break;
                    case "create":
                        SetDataObject(testCollection.OpenDataObj(input[1], FileMode.Read, false, true));
                        break;
                    case "open":
                        SetDataObject(
                            testCollection.OpenDataObj(input[1], (FileMode)Enum.Parse(typeof(FileMode), input[2]))
                        );
                        break;
                  
                    case "query":
                        switch (input[1])
                        {
                            case "obj":
                                DataObjectReference[] dataObjs = input[2] switch
                                {
                                    "std" => testCollection.QueryDataObject(input[3]),
                                    "meta" => testCollection.MQueryDataObject(input[3], input[4], int.Parse(input[5])),
                                    _ => Array.Empty<DataObjectReference>()
                                };
                                foreach (DataObjectReference obj2 in dataObjs)
                                    Utility.WriteLine(ConsoleColor.Blue, obj2.Path, Utility.Log);
                                break;
                            case "col":
                                Collection[] collections = input[2] switch
                                {
                                    "std" => testCollection.QueryCollection(input[3]),
                                    "meta" => testCollection.MQueryCollection(input[3], input[4], int.Parse(input[5])),
                                    _ => Array.Empty<Collection>()
                                };
                                foreach (Collection collection in collections)
                                    Utility.WriteLine(ConsoleColor.Blue, collection.Path, Utility.Log);
                                break;
                            case "meta":
                                Metadata[] metas = input[2] switch
                                {
                                    "obj" => testDataObj?.QueryMetadata(),
                                    "col" => testCollection.QueryMetadata(),
                                    _ => null
                                } ?? Array.Empty<Metadata>();
                                foreach (Metadata meta in metas)
                                {
                                    if (meta.Units == null)
                                        Utility.WriteText(ConsoleColor.Blue, "<" + meta.Name + "," + meta.Value + ">");
                                    else
                                        Utility.WriteText(
                                            ConsoleColor.Blue,
                                            "<" + meta.Name + "," + meta.Value + "," + meta.Units + ">"
                                        );
                                }

                                break;
                        }

                        break;
                    case "addMeta":
                        switch (input[1])
                        {
                            case "col":
                                testCollection.AddMetadata(input[2], input[3], int.Parse(input[4]));
                                break;
                            case "obj":
                                testDataObj?.AddMetadata(input[2], input[3], int.Parse(input[4]));
                                break;
                        }

                        break;
                    case "rmMeta":
                        switch (input[1])
                        {
                            case "col":
                                testCollection.RemoveMetadata(
                                    input[2],
                                    input[3],
                                    input.Length > 4 ? int.Parse(input[4]) : -1
                                );
                                break;
                            case "obj":
                                testDataObj?.RemoveMetadata(
                                    input[2],
                                    input[3],
                                    input.Length > 4 ? int.Parse(input[4]) : -1
                                );
                                break;
                        }

                        break;
                    case "stop":
                        testSession.Dispose();
                        busy = false;
                        break;
                    default:
                        Utility.WriteText(ConsoleColor.Red, "Command Unknown");
                        break;
                }
            }
        }
        else
        {
            Utility.WriteText(
               ConsoleColor.Yellow,
               "Input Command (write, insert, read, seek, remove, close)"
           );
            {
                string[] input = Console.ReadLine()!.Split(' ');
                switch (input[0])
                {

                    case "write":
                        if (input.Length < 3) obj.Write(Encoding.UTF8.GetBytes(input[1]));
                        else testCollection.WriteDataObj(input[1], Encoding.UTF8.GetBytes(input[2]));
                        break;
                    case "insert":
                        obj.Insert(Encoding.UTF8.GetBytes(input[1]));
                        break;
                    case "read":
                        Utility.WriteText(
                            ConsoleColor.Blue,
                            Encoding.UTF8.GetString(
                                input.Length < 2 ? obj.Read(int.MaxValue) : testCollection.ReadDataObj(input[1])
                            )
                        );
                        break;
                    case "seek":
                        Utility.WriteText(
                            ConsoleColor.Blue,
                            obj.Seek(int.Parse(input[1]), (SeekMode)Enum.Parse(typeof(SeekMode), input[2]))
                        );
                        break;
                    case "remove":
                        if (input.Length < 2) obj.Remove();
                        else testCollection.RemoveDataObj(input[1]);
                        break;
                    case "close":
                        SetDataObject(null);
                        break;
                }
            }
        }
    }
    catch (Exception e) when (e.Message == "CAT_NO_ROWS_FOUND")
    {
        Utility.WriteText(ConsoleColor.Red, "Nothing found");
    }
    catch (Exception e)
    {
        Utility.WriteText(ConsoleColor.Red, e.Message);
        Utility.WriteText(ConsoleColor.Red, e.Data["error"]);
        Utility.WriteText(ConsoleColor.White, e.Data["body"]);
    }
}