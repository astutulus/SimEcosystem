# Simulated Ecosystem

A fun personal project intended to practise some C# techniques, rather than being a particularly accurate model of animal behviour! Solution comprises two projects:

## Project 1: EcosystemClassLibrary

Inheritance heirarchy. Leaf classes are concrete; ancestors are all abstract.

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

## Project 2: SimEco (references the project above)

Entry point.

Windows Form GUI that currently allows placement of Grass, Rabbit and Fox on a blank field.

Entities are represented by coloured circles. The goal is not currently to master computer graphics.

The afforementioned Entities act independently, and I have only just begun to implement their behaviour.
