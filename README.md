![fennecs logo](./www/logos/fennecs-logo-darkmode.svg#gh-dark-mode-only) ![fennecs logo](./www/logos/fennecs-logo-lightmode.svg#gh-light-mode-only)

# _... the tiny, tiny, high-energy Entity-Component System!_

<table style="width: 100%">
   <tr>
      <th colspan="2">
         <a href="https://discord.gg/3SF4gWhANS"><img alt="Discord" src="https://img.shields.io/badge/Discord-_%E2%A4%9Coutfox%E2%A4%8F-blue?logo=discord&logoColor=f5f5f5"/></a>
         <a href="https://www.nuget.org/packages/fennecs/"><img alt="Nuget" src="https://img.shields.io/nuget/v/fennecs?color=blue"/></a>
         <a href="https://github.com/outfox/fennecs/actions"><img alt="GitHub Actions Workflow Status" src="https://img.shields.io/github/actions/workflow/status/outfox/fennecs/xUnit.yml"/></a>
         <a href="https://github.com/outfox/fennecs/issues"><img alt="Open issues" src="https://img.shields.io/github/issues-raw/outfox/fennecs?color=green"/></a>
         <img alt="GitHub top language" src="https://img.shields.io/badge/C%23-100%25_-blue"/>
         <a href="https://github.com/outfox/fennecs?tab=MIT-1-ov-file#readme"><img alt="License: MIT" src="https://img.shields.io/github/license/outfox/fennecs?color=blue"/></a>
      </th>
   </tr>
   <tr>
      <td>
         <img src="www/logos/fennecs.png" alt="a box of fennecs, 8-color pixel art" style="min-width: 320px; max-width: 320px"/>
      </td>
      <td style="width: fit-content">
         <h1>Okay, what the fox!<br/><em>Another ECS?!</em></h1>
         <p>We know... oh, <em>we know.</em> 😩</p>
         <p>But in a nutshell, <a href="https://fennecs.tech"><b>fenn</b>ecs</a> is...</p>
         <p>
            🐾 zero codegen<br/>
            🐾 minimal boilerplate<br/>
            🐾 archetype-based<br/>
            🐾 intuitively relational<br/>
            🐾 lithe and fast<br/>
         </p>
      </td>
   </tr>
   <tr>
      <td colspan="2">
         <p><span style="font-size: larger"><em><b>fenn</b>ecs</em></span> is a re-imagining of <a href="https://github.com/Byteron/HypEcs">RelEcs/HypEcs</a> 
            which <em>feels just right<a href="#quickstart-lets-go">*</a></em> for high performance game development in any modern C# engine. Including, of course, the fantastic <a href="https://godotengine.org">Godot</a>.
         </p>
      </td>
   </tr>
</table>

# 📕 DOCUMENTATION: [fennecs.tech](https://fennecs.tech) (official website) 
Grab a cup of coffee to [get started](https://fennecs.tech), try [the Cookbook](https://fennecs.tech/cookbook/), view [the Demos](https://fennecs.tech/examples/) , and more!  
![coffee cup](https://fennecs.tech/emoji/neofox_cofe.png) 

## Quickstart: Let's go!
📦`>` `dotnet add package fennecs`

At the basic level, all you need is a 🧩**component type**, a number of ~~small foxes~~ 🦊**entities**, and a query to ⚙️**iterate and modify** components, occasionally passing in some uniform 💾**data**.

```cs
// Declare a component record. (we can also use most existing value & reference types)
record struct Velocity(Vector3 Value);

// Create a world. (fyi, World implements IDisposable)
var world = new fennecs.World();

// Spawn an entity into the world with a choice of components. (or add/remove them later)
var entity = world.Spawn().Add<Velocity>();

// Queries are cached & we use ultra-lightweight Stream Views to feed data to our code!
var stream = world.Query<Velocity>().Stream();

// Run code on all entities in the query. (exchange 'For' with 'Job' for parallel processing)
stream.For(
    uniform: DeltaTime * 9.81f * Vector3.UnitZ,
    action: (Vector3 uniform, ref Velocity velocity) =>
    {
        velocity.Value -= uniform;
    }
);
```

#### 💢... when we said minimal boilerplate, <em>we meant it.</em>
Even using the strictest judgment, that's no more than 2 lines of boilerplate! Merely instantiating the world and building the query aren't directly moving parts of the actor/gravity feature we just built, and should be seen as "enablers" or "infrastructure".  

The 💫*real magic*💫 is that none of this brevity compromises on performance.

------------------------

## 🥊 Comparisons: Punching above our weight?
So how does _**fenn**ecs_ compare to other ECSs? 

This library is a tiny, tiny ECS with a focus on good performance and great simplicity. But it *cares enough* to provide a few things you might not expect.

> [!IMPORTANT]
> The idea of _**fenn**ecs_ was to fill the gaps that the author felt working with various established Entity-Component Systems. This is why this matrix is clearly imbalanced, it's a shopping list of things that _**fenn**ecs_ does well and was made to do
well; and things it may aspire to do but compromised on in order to be able to achieve the others.
>
> <em>(TL;DR - Foxes are soft, choices are hard - Unity dumb, .NET 8 really sharp.)</em>


<details>

<summary>🥇🥈🥉 (click to expand) ECS Comparison Matrix<br/><b></b></summary>

> Here are some of the key properties where _**fenn**ecs_ might be a better or worse choice than its peers. Our resident fennecs have worked with all of these ECSs, and we're happy to answer any questions you might have.

|                                                                           |           _**fenn**ecs_            |                HypEcs                | Entitas |            Unity DOTS            |            DefaultECS            |
|:--------------------------------------------------------------------------|:----------------------------------:|:------------------------------------:|:-------:|:--------------------------------:|:--------------------------------:|
| Boilerplate-to-Feature Ratio                                              |               3-to-1               |                5-to-1                | 12-to-1 |            27-to-1 😱            |              7-to-1              |
| Entity-Component Queries                                                  |                 ✅                  |                  ✅                   |    ✅    |                ✅                 |                ✅                 |
| Entity-Entity Relations                                                   |                 ✅                  |                  ✅                   |    ❌    |                ❌                 | ✅<br/><sup>(Map/MultiMap)</sup> |
| Entity-Object-Relations                                                   |                 ✅                  | 🟨</br><sup>(System.Type only)</sup> |    ❌    |                ❌                 |                ❌                 |
| Target Querying<br/>*<sup>(find all targets of specific relations)</sup>* |                 ✅                  |                  ❌                   |    ❌    |                ❌                 |                ✅                 |
| Wildcard Semantics<br/>*<sup>(match multiple relations in 1 query)</sup>* |                 ✅                  |                  ❌                   |    ❌    |                ❌                 |                ❌                 |
| Journaling                                                                |                 ❌                  |                  ❌                   |   🟨    |                ✅                 |                ❌                 |
| Shared Components                                                         | ✅<br/><sup>(ref types only)</sup>  |                  ❌                   |    ❌    |                🟨<br/><sup>(restrictive)</sup>                |                ✅                 | 
| Mutable Shared Components                                                 |                 ✅                  |                  ❌                   |    ❌    |                ❌                 |                ✅                 | 
| Reference Component Types                                                 |                 ✅                  |                  ❌                   |    ❌    |                ❌                 |                ❌                 |
| Arbitrary Component Types                                                 |                 ✅                  | ✅<br/><sup>(value types only)</sup>  |    ❌    |                ❌                 |                ✅                 |
| Structural Change Events                                                  |    🟨<br/><sup>(planned)</sup>     |                  ❌                   |    ✅    |  ☠️<br/><sup>(unreliable)</sup>  |                ❌                 |
| Workload Scheduling                                                       |                 ❌                  |                  ❌                   |      ❌  | ✅<br/><sup>(highly static)</sup> |                ✅                 |
| No Code Generation Required                                               |                 ✅                  |                  ✅                   |    ❌    |                ❌                 | 🟨<br/><sup>(roslyn addon)</sup> |
| Enqueue Structural Changes at Any Time                                    |                 ✅                  |                  ✅                   |    ✅    | 🟨<br/><sup>(restrictive)</sup>  |                🟨                |
| Apply Structural Changes at Any Time                                      |                 ❌                  |                  ❌                   |    ✅    |                ❌                 |                ❌                 |
| Parallel Processing                                                       |                 ⭐⭐                 |                  ⭐                   |    ❌    |               ⭐⭐⭐                |                ⭐⭐                |
| Singleton / Unique Components                                             | 🟨<br/><sup>(ref types only)</sup> |                  ❌                   |    ✅    |  🟨<br/><sup>(per system)</sup>  |                ✅                 |

</details>

------------------------

## 💡Highlights / Design Goals

- [x] Modern C# 12 codebase, targeting .NET 8.
- [x] Full Unit Test coverage.
- [ ] Benchmarking suite. (Work in Progress)

- [x] Workloads can be easily parallelized across *and within* Archetypes

- [x] Expressive, queryable relations between Entities and Objects
- [x] Powerfully intuitive ways to access data... _fast!_

- [x] No code generation and no reflection required.



------------------------

## ⏩ Nimble: _**fenn**ecs_ benchmarks

Preliminary (WIP) benchmarks suggest you can expect to process over 2 million components per millisecond on a 2020 CPU.

We worked hard to minimize allocations, and if using static anonymous methods or delegates, even with uniform parameters, the ECS iterates Entities allocation-free.

_**fenn**ecs_ provides a variety of ways to iterate over and modify components, to offer a good balance of control and elegance without compromising too much. 

Here are some raw results from our WIP benchmark suite, from the Vector3 operations parts, better ones soon.
(don't @ us)

> Example: Allocation-free enumeration of a million entities with a System.Numerics.Vector3 component, calculating a cross product against a uniform value, and writing the result back to memory. Processing methods included parallel jobs with different batch/chunk sizes and single threaded runs.


| Method     | entities  | chunk | Mean       | StdDev    | Jobs | Contention | Alloc |
|----------- |-----------|------:|-----------:|----------:|-----:|-----------:|------:|
| Cross_JobU | 1_000_000 | 32768 |   349.9 us |   1.53 us |    32|     0.0029 |     - |
| Cross_JobU | 1_000_000 | 16384 |   350.5 us |   5.82 us |    64|     0.0005 |     - |
| Cross_JobU | 1_000_000 | 4096  |   356.1 us |   1.78 us |   248|     0.0083 |     - |
| Cross_Job  | 1_000_000 | 4096  |   371.7 us |  15.36 us |   248|     0.0103 |     - |
| Cross_Job  | 1_000_000 | 32768 |   381.6 us |   4.22 us |    32|          - |     - |
| Cross_Job  | 1_000_000 | 16384 |   405.2 us |   4.56 us |    64|     0.0039 |     - |
| Cross_RunU | 1_000_000 |     - | 1,268.4 us |  44.76 us |    - |          - |   1 B |
| Cross_Run  | 1_000_000 |     - | 1,827.0 us |  16.76 us |    - |          - |   1 B |


------------------------

# 🧡 Acknowledgements
Many thanks to [Byteron (Aaron Winter)](https://github.com/Byteron) for creating [HypEcs](https://github.com/Byteron/HypEcs) and [RelEcs](https://github.com/Byteron/RelEcs), the inspiring libraries that _**fenn**ecs_ evolved from.

Neofox was created by [Volpeon](https://volpeon.ink/emojis/) and is in the Creative Commons [CC BY-NC-SA 4.0](https://creativecommons.org/licenses/by-nc-sa/4.0), the same license applies to all Neofox-derived works made for this documentation.
