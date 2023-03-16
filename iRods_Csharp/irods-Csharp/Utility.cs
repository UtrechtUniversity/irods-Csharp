using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace irods_Csharp;

public class Utility
{
    // Log is used by library to write logging data, text can be used to write custom debugging data
    public static TextWriter? Log = null;
    public static TextWriter? Text = null;

    /// <summary>
    /// Gets a password from the user, without writing it to the console.
    /// </summary>
    /// <returns>The inserted password.</returns>
    public static string GetPassword()
    {
        WriteText(ConsoleColor.Yellow, "Please enter your password below:");
        StringBuilder password = new ();
        bool busy = true;
        while (busy)
        {
            ConsoleKeyInfo input = Console.ReadKey(true);

            switch (input.Key)
            {
                case ConsoleKey.Backspace:
                    if (input.Modifiers == ConsoleModifiers.Control) password.Clear();
                    else if (password.Length > 0) password.Remove(password.Length - 1, 1);
                    break;
                case ConsoleKey.Enter:
                    busy = false;
                    break;
                default:
                    password.Append(input.KeyChar);
                    break;
            }
        }

        return password.ToString();
    }

    /// <summary>
    /// Does a writeline with the supplied ConsoleColor.
    /// </summary>
    /// <param name="color">The color to do the writeline in.</param>
    /// <param name="data">The data to write to the console.</param>
    public static void WriteText(ConsoleColor color, object? data)
    {
        if (data == null || Text == null) return;
        WriteLine(color, data, Text);
        Text.Flush();
    }

    /// <summary>
    /// Does a writeline with the supplied ConsoleColor.
    /// </summary>
    /// <param name="color">The color to do the writeline in.</param>
    /// <param name="data">The data to write to the console.</param>
    internal static void WriteLog(ConsoleColor color, object? data)
    {
        if (data == null || Log == null) return;
        WriteLine(color, data, Log);
        Log.Flush();
    }

    /// <summary>
    /// Does a writeline with the supplied ConsoleColor and textwriter.
    /// </summary>
    /// <param name="color">The color to do the writeline in.</param>
    /// <param name="data">The data to write to the console.</param>
    /// <param name="writer">The textwriter to write to.</param>
    public static void WriteLine(ConsoleColor color, object data, TextWriter writer)
    {
        TextWriter currentWriter = Console.Out;
        ConsoleColor currentColor = Console.ForegroundColor;
        Console.SetOut(writer);
        Console.ForegroundColor = color;
        Console.WriteLine(data.ToString());
        Console.SetOut(currentWriter);
        Console.ForegroundColor = currentColor;
    }

    /// <summary>
    /// Loads a json from the supplied filepath into a dictionary.
    /// </summary>
    /// <param name="filepath">The filepath of the json.</param>
    /// <returns>The dictionary with the information from the json.</returns>
    public static Dictionary<string, string> LoadJson(string filepath)
    {
        string[] lines = File.ReadAllLines(filepath);

        Dictionary<string, string> values = new ();
        for (int i = 1; i < lines.Length - 1; i++)
        {
            string[] tempString = lines[i].Replace(" ", "").Replace("\"", "").Replace("\\", "").Replace(",", "").Split(':').ToArray();
            values.Add(tempString[0], tempString[1]);
        }

        return values;
    }
}