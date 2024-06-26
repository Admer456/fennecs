﻿using System.Numerics;

namespace fennecs.tests.Integration;

public class DocumentationExampleTests
{
    record struct Position(Vector3 Value);
    
    [Fact]
    public void QuickStart_Example_Works()
    {
        using var world = new World();
        var entity1 = world.Spawn().Add<Position>();
        var entity2 = world.Spawn().Add(new Position(new(1, 2, 3))).Add<int>();

        var query = world.Query<Position>(Match.Plain).Stream();

        const float multiplier = 10f;
        
        foreach ((Entity, Position p) item in query)
        {
        }
        
        query.Job(multiplier,(float uniform, ref Position pos) => { pos.Value *= uniform; });

        var pos1 = world.GetComponent<Position>(entity1, Match.Plain);
        var expected = new Position(pos1.Value * multiplier);
        Assert.Equal(expected, pos1);

        var pos2 = world.GetComponent<Position>(entity2, Match.Plain);
        expected = new Position( new Vector3(1, 2, 3) * multiplier);
        Assert.Equal(expected, pos2);
    }
}