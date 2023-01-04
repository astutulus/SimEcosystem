# Simulated Ecosystem

A fun personal project intended to practise some C# techniques, rather than being a particularly accurate model of animal behviour! Solution comprises two projects:

## Project: EcosystemClassLibrary

Class inheritance heirarchy:

    abstract                 concrete

    Entity
     |_ LivingThing
         |_ Plant
         |   |______________ Grass         
         |
         |_ Animal
             |
             |_ Herbivore
             |   |__________ Rabbit
             |
             |_ Carnivore
                 |__________ Fox

- Every Entity has a Position (in pixels on the screen)
- Every LivingThing has a Birthday (time of day when spawned by player) and a Lifespan (in minutes of player time)
- Every Animal looks for it's required Food.
- etc.

## Project: SimEco

Windows Form GUI currently allows placement of Grass, Rabbit and Fox on a blank field.

They are represented by colour coded circles. The goal is not currently to master computer graphics.

The afforementioned Entities act independently, and I have only just begun to implement their behaviour.
