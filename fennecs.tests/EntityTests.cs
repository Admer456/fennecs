﻿// SPDX-License-Identifier: MIT

using System.Numerics;

namespace fennecs.tests;

public class EntityTests
{
    #region Input Data
    private struct CompoundComponent
    {
        // ReSharper disable once NotAccessedField.Local
        public required bool B1;

        // ReSharper disable once NotAccessedField.Local
        public required int I1;
    }
    
    private class ComponentDataSource : List<object[]>
    {
        public ComponentDataSource()
        {
            Add([123]);
            Add([1.23f]);
            Add([float.NegativeInfinity]);
            Add([float.NaN]);
            Add([new Vector2(1, 2)]);
            Add([new Vector3(1, 2, 3)]);
            Add([new Vector4(1, 2, 3, 4)]);
            Add([new Matrix4x4()]);
            Add([new CompoundComponent {B1 = true, I1 = 5}]);
            Add([new CompoundComponent {B1 = default, I1 = default}]);
        }
    }
    #endregion

    [Fact]
    public Entity Entity_is_Alive_after_Spawn()
    {
        using var world = new World();
        var entity = world.Spawn().Id();
        Assert.True(world.IsAlive(entity));
        return entity;
    }

    [Fact]
    private void Entity_is_Not_Alive_after_Despawn()
    {
        using var world = new World();
        var entity = world.Spawn().Id();
        world.Despawn(entity);
        Assert.False(world.IsAlive(entity));
    }

    [Fact]
    private void Entity_has_no_Components_after_Spawn()
    {
        using var world = new World();
        var entity = world.Spawn().Id();
        var components = world.GetComponents(entity);
        Assert.False(world.HasComponent<int>(entity));
        Assert.True(components.Count() == 1);
    }

    [Theory]
    [ClassData(typeof(ComponentDataSource))]
    private void Entity_can_Add_Component<T>(T t1) where T : struct
    {
        using var world = new World();
        var entity = world.Spawn().Id();
        world.On(entity).Add(t1);
        Assert.True(world.HasComponent<T>(entity));
        var components = world.GetComponents(entity);
        Assert.True(components.Count() == 2);
    }

    [Theory]
    [ClassData(typeof(ComponentDataSource))]
    private void Entity_can_Get_Component<T>(T t1) where T : struct
    {
        using var world = new World();
        var entity = world.Spawn().Add(t1).Id();
        var x = world.GetComponent<T>(entity);
        Assert.Equal(t1, x);
    }

    [Theory]
    [ClassData(typeof(ComponentDataSource))]
    private void Entity_can_Remove_Component<T>(T t1) where T : struct
    {
        using var world = new World();
        var entity = world.Spawn().Id();
        world.On(entity).Add(t1);
        world.On(entity).Remove<T>();
        Assert.False(world.HasComponent<T>(entity));
    }

    [Theory]
    [ClassData(typeof(ComponentDataSource))]
    private void Entity_can_ReAdd_Component<T>(T t1) where T : struct
    {
        using var world = new World();
        var entity = world.Spawn().Id();
        world.On(entity).Add(t1);
        world.On(entity).Remove<T>();
        world.On(entity).Add(t1);
        Assert.True(world.HasComponent<T>(entity));
    }

    [Theory]
    [ClassData(typeof(ComponentDataSource))]
    private void Entity_cannot_Add_Component_twice<T>(T t1) where T : struct 
    {
        using var world = new World();
        var entity = world.Spawn().Id();
        world.On(entity).Add(t1);
        Assert.Throws<ArgumentException>(() => world.On(entity).Add(t1));
    }

    [Theory]
    [ClassData(typeof(ComponentDataSource))]
    private void Entity_cannot_Remove_Component_twice<T>(T t1) where T : struct
    {
        using var world = new World();
        var entity = world.Spawn().Id();
        world.On(entity).Add(t1);
        world.On(entity).Remove<T>();
        Assert.Throws<ArgumentException>(() => world.On(entity).Remove<T>());
    }

    [Theory]
    [ClassData(typeof(ComponentDataSource))]
#pragma warning disable xUnit1026
    private void Entity_cannot_Remove_Component_without_Adding<T>(T _) where T : struct
    {
        using var world = new World();
        var entity = world.Spawn().Id();
        Assert.Throws<ArgumentException>(() => world.On(entity).Remove<T>());
    }
#pragma warning restore xUnit1026
}