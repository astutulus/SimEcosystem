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

The GUI is fit for purpose for the moment. Most of the interesting (math) ideas involve expansion of the Class Library

### GUI

- Entities to have size proportional to mass.

### Class Library

- Entity to have separation according to mass (size).

- Plant to have mass reduction when eaten.

- Prey to die when caught by Predator.
- Animal to have mass transfer when eating.
- LivingThings to disappear when mass zero.

- World to have static getInstance() method, so that...
- Entity to have no more, it's own home-World object reference.

- Herbivore to sense fear (controlled by proximity of predator vs hunger level).
