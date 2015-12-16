Because Minecraft treats chained commands and conditionals differently, the Command Block Language Interpreter has issues with importing conditional commands.

See /Examples/Conditionals Issue.png for visual

Any commands in the red locations cannot be conditional command blocks, and here is the reason:

Each command block has an arrow on the texture, showing which way that the chain will continue. The arrows are shown exactly as they are when commands are imported using the interpreter.

The way Minecraft handles chained commands is different than the way it handles conditionals.
For power to continue through a chain, each command block must have its arrow point directly into another command block. (The (o) symbol in the image means it is pointing up)
This lets the chain signal continue through the commands.

However, conditionals are treated differently.
When a command is conditional, it checks the command block BEHIND the arrow to check if it ran successfully. The command block behind the arrow is not necessarily the command before it in the chain.
For example, since the command block at index 4 is conditional, it would check for a successful command behind it, which is NOT the location of the command block at index 3.

This means that, until this is hopefully fixed, when creating a Minecraft Command Block Language script, make sure you only have conditionals in the green locations.