﻿namespace fennecs.tests;

public class StorageTests
{
    private struct ValueType;

    private class ReferenceType;

    [Fact]
    public void Storage_Can_Be_Created()
    {
        Assert.NotNull(new Storage<ValueType>());
        Assert.NotNull(new Storage<ReferenceType>());
    }

    [Fact]
    public void Storage_Stores_Values()
    {
#pragma warning disable CA1859
        IStorage storage = new Storage<int>();
#pragma warning restore CA1859

        storage.Append(1);
        Assert.Equal(1, storage.Count);
        storage.Append(337, 2);
        Assert.Equal(3, storage.Count);

        var refStorage = new Storage<ReferenceType>();
        var rt = new ReferenceType();
        refStorage.Append(rt);
        Assert.Equal(1, refStorage.Count);
        refStorage.Append(rt, 2);
        Assert.Equal(3, refStorage.Count);
        Assert.Equal(rt, refStorage[0]);
        Assert.Equal(rt, refStorage[1]);
        Assert.Equal(rt, refStorage[2]);
    }

    [Fact]
    public void Storage_Interface_Denies_Wrong_Types()
    {
#pragma warning disable CA1859
        IStorage storage = new Storage<int>();
#pragma warning restore CA1859

        Assert.Throws<InvalidCastException>(() => storage.Append(8.5f));
        Assert.Throws<InvalidCastException>(() => storage.Append("Dieter", 69));
        Assert.Throws<InvalidCastException>(() => storage.Append(new object()));
        storage.Append(420);
    }

    [Fact]
    public void Storage_Can_Blit()
    {
        var storage = new Storage<int>();

#pragma warning disable CA1859
        IStorage generic = storage;
#pragma warning restore CA1859

        generic.Append(7, 3);
        Assert.Equal(7, storage[0]);
        Assert.Equal(7, storage[1]);
        Assert.Equal(7, storage[2]);

        generic.Blit(42);
        Assert.Equal(42, storage[0]);
        Assert.Equal(42, storage[1]);
        Assert.Equal(42, storage[2]);
    }

    [Fact]
    public void Storage_Can_Clear()
    {
        var storage = new Storage<int>();
        storage.Append(7, 3);
        Assert.Equal(3, storage.Count);

        storage.Clear();
        Assert.Equal(0, storage.Count);
        Assert.Equal(default, storage[0]);
    }

    [Fact]
    public void Storage_Contiguous_After_Delete()
    {
        var storage = new Storage<int>();
        storage.Append(420, 3);
        storage.Append(69, 3);
        Assert.Equal(6, storage.Count);

        storage.Delete(1);
        Assert.Equal(5, storage.Count);

        Assert.Equal(420, storage[0]);
        Assert.Equal(420, storage[1]);
        Assert.Equal(69, storage[2]);
        Assert.Equal(69, storage[3]);
        Assert.Equal(69, storage[4]);
        Assert.Equal(default, storage[5]);
    }

    [Fact]
    public void Storage_Identical_After_Compact()
    {
        var storage = new Storage<int>();
        storage.Append(420, 3);
        storage.Append(69, 3);
        Assert.Equal(6, storage.Count);

        storage.Delete(1);
        Assert.Equal(5, storage.Count);

        storage.Compact();
        Assert.Equal(5, storage.Count);

        Assert.Equal(420, storage[0]);
        Assert.Equal(420, storage[1]);
        Assert.Equal(69, storage[2]);
        Assert.Equal(69, storage[3]);
        Assert.Equal(69, storage[4]);
    }

    [Fact]
    public void Can_Append_or_Delete_Zero()
    {
        var storage = new Storage<int>();
        storage.Append(420, 0);
        Assert.Equal(0, storage.Count);

        storage.Append(420, 3);
        storage.Delete(1, 0);
        Assert.Equal(3, storage.Count);
        Assert.Equal(420, storage[0]);
        Assert.Equal(420, storage[1]);
        Assert.Equal(420, storage[2]);
    }

    [Fact]
    public void Can_Migrate()
    {
        var source = new Storage<string>();
        var destination = new Storage<string>();
        
        destination.Append("world", 3);
        
        source.Append("hello", 3);

#pragma warning disable CA1859
        var genericSource = (IStorage)source;
#pragma warning restore CA1859
        genericSource.Migrate(destination);
        
        Assert.Equal(0, source.Count);
        Assert.Equal(6, destination.Count);
        
        Assert.Equal("world", destination[0]);
        Assert.Equal("world", destination[1]);
        Assert.Equal("world", destination[2]);
        Assert.Equal("hello", destination[3]);
        Assert.Equal("hello", destination[4]);
        Assert.Equal("hello", destination[5]);
    }


    [Fact]
    public void Can_Move()
    {
        var source = new Storage<string>();
        var destination = new Storage<string>();

        destination.Append("world", 3);
        source.Append("hello", 3);
        
        source.Move(1, destination);
        
        Assert.Equal(2, source.Count);
        Assert.Equal(4, destination.Count); 
        
        Assert.Equal("hello", source[0]);
        Assert.Equal("hello", source[1]);
        Assert.Equal("world", destination[0]);
        Assert.Equal("world", destination[1]);
        Assert.Equal("world", destination[2]);
        Assert.Equal("hello", destination[3]);

    }
}