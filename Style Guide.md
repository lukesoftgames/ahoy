
<h1>Ahoy Style Guide</h1>

<h2>SOLID Principles</h2>
Solid programming principles are a set of object oriented programming principles that aim to make software designs more understandable, flexible and maintainable. They are particularly useful in <i>agile development</i> which will be ideal for us. Please familiarise yourself with the ideas behind the principles - I promise that they're not difficult, and will save us a lot of time and anguish as the project grows and changes. The wiki for SOLID can be found at [SOLID Principles of Programming](https://en.wikipedia.org/wiki/SOLID), and [this series of videos](https://www.youtube.com/watch?v=Eyr7_l5NMds&list=PLB5_EOMkLx_WjcjrsGUXq9wpTib3NCuqg&index=1) gives an excellent and concise explanation, with clear examples of why the principles are useful. A brief summary of the principles is as follows:
1. <b>S</b>ingle responsibility principle - each class should have only a single responsibility. A change to only one part of the specification should affect the class' implementation.
1. <b>O</b>pen/Close principle - software entities should be open for extension but closed for modification (this is the least intuitive principle, I highly recommend the second video in the series for an explanation).
1. <b>L</b>iskov substitution principle - objects in an implementation should be replaceable with instances of their subtypes without needing to alter the program, or change its control flow, etc.
1. <b>I</b>nterface segregation principle - it's better to have many specific interfaces than one general interface. At it's core, an object should not implement an interface that contracts methods it doesn't need, and there shouldn't be overlap between interfaces (for example, the same health system implemented on two different interfaces) as this defeats the point of an interface.
1. <b>D</b>ependency inversion principle - this is an interesting one, but I think not as useful for <b>Unity</b>. If it becomes necessary we can discuss it later in the project, but for now just follow the other principles and we should be fine.

<h2>Coding Style</h2>
<h4>Code Structure</h4>
To make it easier to share code between contributors, follow the below style for your scripts. It should be easy enough to set the automatic formatting in your code editor to follow these bullet points. I've included a little code snippet at the end to give an example of how it should lay out.
* Tabs should be set to insert 4 spaces in your code editor.
* Curly brace should start on the same line as the code block, and end below the function. The exception to this is one line code blocks for getters or setters, which should all be on the same line.
* No spaces between parentheses and the enclosed variables, etc.
* Space after a comma between parameters.
* Empty line between the end of a code block and the next code block.
* 'else' statements start on the same line as the closing brace for the previous control flow option.
```csharp
    private int foo(int bar, bool foofoo) {
        if(foofoo) {
            bar++;
        } else {
            bar--;
        }

        return bar;
    }
```

Another thing worth considering is that if a method is starting to get very long, not only is it likely to become unreadable to both yourself down the line and everyone else immediately, but there is also a high probability that there is not enough attention being paid to the SOLID principles. Classes and functions rarely need to be that large when the logic underlying them is sound and concise.

<h4>Naming Conventions</h4>
<b>Functions</b> - Nothing too complicated for this. Names should be indicative of the purpose of the function. Camel case should be used; I am aware that the C# standard is to begin the function name with a capital as well, and that this is evident in the built in <b>Unity</b> functions, but that's no reason for us to lower ourselves to their level. It also means that, at a glance, we can tell whether a function is built in or has been implemented by one of us.

<b>Variables</b> - Standard practice for naming variables. The names should be indicative of the purpose of the variable. Camel case should be used - lowercase to begin the name, and then capital letters to start all the other words within the name. Wherever possible, keep it to one word, for ease of use and readability. The only reasons to use more than one word are because the variable's function can't be conveyed in just one, or to distinguish more easily between two variables.</br>&nbsp;&nbsp;&nbsp;&nbsp;The exception to this are <i>boolean</i> variables, which should start with '<i>is</i>', and should almost always be one word after that. Camel case still applies.

<b>Classes</b> - Straightforward enough. Start with a capital, and then every other word within the class name should also be capitalised. Make sure that the name gives a clue to what the class does.

<b>Interfaces</b> - Exactly the same rules as for classes, but convention also dictates that the name should begin with a capital <i>I</i> before everything else.

<h2>File System Structure</h2>
Everything should be stored in files. For a start, nothing should be in the top level directory except other files. Separate scripts in a manner that makes sense, etc. It's quite common sense dependent, you just need to be paying attention. An example file system within <b>Unity</b> might be:
* Assets
    * Prefabs
        * <i>Player.prefab</i>
    * Scenes
        * <i>Test.unity</i>
    * Scripts
        * Interactable
            * Interactions
                * <i>Interaction.cs</i>
                * <i>InteractionMenu.cs</i>
                * <i>InteractionMove.cs</i>
                * <i>InteractionPickup.cs</i>
            * Item
                * <i>Item.cs</i>
                * <i>ItemData.cs</i>
                * <i>ObjectHighlight.cs</i>
            * <i>IInteractable.cs</i>
        * Inventory
            * <i>Inventory.cs</i>
        * Player
            * <i>PlayerInputController.cs</i>
            * <i>PlayerInteract.cs</i>
        * <i>CharacterMovement.cs</i>

<h2>Contributing to GitHub</h2>
Use GitHub properly.
