﻿namespace irods_Csharp.Objects;

/// <summary>
/// Interface to force objects which can have metadata to be able to supply necessary information about the object
/// </summary>
public interface ITaggable
{
    string MetaType { get; }

    string Path { get; }
}