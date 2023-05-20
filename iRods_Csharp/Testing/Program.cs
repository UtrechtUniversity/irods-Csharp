using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using irods_Csharp;
using irods_Csharp.Enums;
using irods_Csharp.Objects;
using Objects;
using Objects.Objects;
using Path = System.IO.Path;

namespace Testing
{
    /// <summary>
    /// Console program which can be used to communicate with the irods backend in a simple way.
    /// Was also used primarily for testing purposes.
    /// </summary>
    class Program
    {
        private static void Main()
        {
            Utility.Log = Console.Out;
            Utility.Text = Console.Out;

            string accountLocation = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\irods_environment.json"));
            Dictionary<string, string> accountOptions =
                JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(accountLocation));
            string passwordLocation = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\password.txt"));


            const ClientServerPolicyRequest clientServerPolicy = ClientServerPolicyRequest.RequireSSL;
            ClientServerNegotiation clientServerNegotiation = new (
                clientServerPolicy,
                accountOptions["irods_encryption_algorithm"],
                Convert.ToInt32(accountOptions["irods_encryption_encryption"]),
                Convert.ToInt32(accountOptions["irods_encryption_salt_size"]),
                Convert.ToInt32(accountOptions["irods_encryption_num_hash_rounds"])
            );
            IrodsSession testSession = new (
                accountOptions["irods_host"],
                int.Parse(accountOptions["irods_port"]),
                accountOptions["irods_home"],
                accountOptions["irods_user_name"],
                accountOptions["irods_zone_name"],
                accountOptions["irods_authentication_scheme"] switch
                {
                    "native" => AuthenticationScheme.Native,
                    "pam" or _ => AuthenticationScheme.Pam
                },
                24,
                clientServerNegotiation
            );
            
            bool connected = false;
            while (!connected)
            {
                try
                {
                    string hashedPassword = testSession.Setup(File.ReadAllText(passwordLocation));
                    testSession.Start(hashedPassword);
                    connected = true;
                }
                catch (Exception e)
                {
                    if (e.Message == "CAT_INVALID_AUTHENTICATION" || e.Message == "CAT_INVALID_USER") { }
                    else if (e.GetType() != typeof(FileNotFoundException)) throw;

                    try
                    {
                        string password = testSession.Setup(Utility.GetPassword());
                        File.WriteAllText(passwordLocation, password);
                    }
                    catch (Exception e2)
                    {
                        if (e2.Message == "PAM_AUTH_PASSWORD_FAILED") Utility.WriteText(ConsoleColor.Red, "Authentication error");
                    }

                }
            }

            Collection testCollection = testSession.HomeCollection();
            DataObject testDataObj = null;

            bool busy = true;
            while (busy)
            {
                try
                {
                    Utility.WriteText(ConsoleColor.Yellow, "Input Command (cd, pwd, mkdir, rmdir, rename, create, open, write, read, remove, query, addMeta, rmMeta, stop)");
                    {
                        string[] input = Console.ReadLine().Split(' ');
                        switch (input[0])
                        {
                            case "cd":
                                testCollection.ChangeDirectory(input[1]);
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
                                testCollection.CreateDataObj(input[1]);
                                break;
                            case "open":
                                testDataObj = testCollection.OpenDataObj(input[1], (Options.FileMode)Enum.Parse(typeof(Options.FileMode), input[2]));
                                break;
                            case "write":
                                if (input.Length < 3) testDataObj.Write(Encoding.UTF8.GetBytes(input[1]));
                                else testCollection.WriteDataObj(input[1], Encoding.UTF8.GetBytes(input[2]));
                                break;
                            case "insert":
                                testDataObj.Insert(Encoding.UTF8.GetBytes(input[1]));
                                break;
                            case "read":
                                Utility.WriteText(ConsoleColor.Blue, Encoding.UTF8.GetString(input.Length < 2 ? testDataObj.Read(int.MaxValue) : testCollection.ReadDataObj(input[1])));
                                break;
                            case "seek":
                                Utility.WriteText(ConsoleColor.Blue,testDataObj.Seek( int.Parse(input[1]), (Options.SeekMode)Enum.Parse(typeof(Options.SeekMode), input[2])));
                                break;
                            case "remove":
                                if (input.Length < 2) testDataObj.Remove();
                                else testCollection.RemoveDataObj(input[1]);
                                break;
                            case "query":
                                switch (input[1])
                                {
                                    case "obj":
                                        DataObject[] dataObjs = input[2] switch
                                        {
                                            "std" => testCollection.QueryDataObject(input[3]),
                                            "meta" => testCollection.MQueryDataObject(input[3], input[4], int.Parse(input[5])),
                                            _ => Array.Empty<DataObject>()
                                        };
                                        foreach (DataObject obj in dataObjs)
                                            Utility.WriteLine(ConsoleColor.Blue, obj.Path, Utility.Log);
                                        break;
                                    case "col":
                                        Collection[] collections = input[2] switch
                                        {
                                            "std" => testCollection.QueryCollection(input[3]),
                                            "meta" => testCollection.MQueryCollection(
                                                input[3],
                                                input[4],
                                                int.Parse(input[5])
                                            ),
                                            _ => Array.Empty<Collection>()
                                        };
                                        foreach (Collection collection in collections)
                                            Utility.WriteLine(ConsoleColor.Blue, collection.Path, Utility.Log);
                                        break;
                                    case "meta":
                                        Metadata[] metas = input[2] switch
                                        {
                                            "obj" => testDataObj.QueryMetadata(),
                                            "col" => testCollection.QueryMetadata(),
                                            _ => Array.Empty<Metadata>()
                                        };
                                        foreach (Metadata meta in metas)
                                        {
                                            if (meta.Units == null)  Utility.WriteText(ConsoleColor.Blue, "<" + meta.Name + "," + meta.Value + ">");
                                            else Utility.WriteText(ConsoleColor.Blue, "<" + meta.Name + "," + meta.Value + "," + meta.Units + ">");
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
                                        testDataObj.AddMetadata(input[2], input[3], int.Parse(input[4]));
                                        break;
                                }
                                break;
                            case "rmMeta":
                                switch (input[1])
                                {
                                    case "col":
                                        testCollection.RemoveMetadata(input[2], input[3], input.Length > 4 ? int.Parse(input[4]) : -1);
                                        break;
                                    case "obj":
                                        testDataObj.RemoveMetadata(input[2], input[3], input.Length > 4 ? int.Parse(input[4]) : -1);
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
                catch (Exception e)
                {
                    Utility.WriteText(ConsoleColor.Red, e.Message);
                    Utility.WriteText(ConsoleColor.Red, e.Data["error"]);
                    Utility.WriteText(ConsoleColor.White, e.Data["body"]);
                }
            }
        }
    }
}