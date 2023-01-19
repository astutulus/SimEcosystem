# Simulated Ecosystem

A fun project, created in my own time. Intended to practise any C# techniques I learn about. No intent to be a particularly accurate model of animal behviour! Solution comprises two projects:

## Project 1: EcosystemClassLibrary

Inheritance heirarchy. All superclasses are abstract.

    Entity
      ├─── Terrain
      │      ├─── Rock
      │      └─── Burrow
      └─── LivingThing
             ├─── Plant
             │      └─── Grass
             └─── Animal
                    ├─── Herbivore
                    │      └─── Rabbit
                    └─── Carnivore
                           └─── Fox
                           
- Every Entity has a Position (in pixels on the screen)
- Every LivingThing has a DateTime "Birthday" (e.g. a moment ago when spawned), and a Lifespan (e.g. 2 minutes)
- Every Animal looks for it's required Food.
- etc.

## Project 2: SimEco

Entry point. References the project above.

Windows Form that allows the user to place Grass, Rabbits and Foxes on a blank field.

Entities are represented by coloured circles. The goal is *not* currently to master computer graphics!

The afforementioned Entities act independently, and I have only just begun to implement their behaviour.

## To Do List

The GUI is fit for purpose for the moment; the following ideas involve development of the Class Library.

- No Entity to overlap.
- Entity to push others away when growing.

- Plant to increase mass when growing.
- Plant to reduce mass when eaten.
- LivingThings to disappear when mass zero.

- Herbivores to increase mass when eating.
- Herbivores to reduce mass when not eating.

- Prey to die when caught by Predator.
- Animal to have mass transfer when eating.

- World to have static getInstance() method, so that...
- Entity to have no more, it's own home-World object reference.

- Herbivore to sense fear (controlled by proximity of predator vs hunger level).
