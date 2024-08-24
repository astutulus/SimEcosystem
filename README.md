# Simulated Ecosystem

### Keywords
GUI, animation, events, algorithms, architecture

Visual Studio 2022 Solution comprising two projects:

## Project 1: EcosystemClassLibrary

Inheritance heirarchy. 
All superclasses are abstract; only 'leaf' classes are concrete.

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
- Every Animal looks for it's required Food, and moves toward it.
- Not a very accurate model of animal behviour!

## Project 2: SimEco

References the project above.

Entry point. A Windows Form that allows the user to place Grass, Rabbits and Foxes on a blank field.

These entities:
- are sufficiently represented by coloured circles.
- act independently, and I have only just begun to implement their behaviour.

# TODO List

The GUI is fit for purpose for the moment; the following ideas involve development of the Class Library.

- Qty of each Entity ("count") next to each tool, in GUI

- Every entity to track every other Entity.

- No Entity to overlap.
- Entity to push others away when growing.

- Plant to increase mass when growing.
- Plant to reduce mass when eaten.
- LivingThings to disappear when mass zero.

- Herbivores to increase mass when eating.
- Herbivores to reduce mass when not eating.

- Prey to die when caught by Predator.
- Animal to have mass transfer when eating.

- Herbivore to sense fear (controlled by proximity of predator vs hunger level).

- Replace the World singleton