# Unity-Easy-Tasks
Let's say I would like to make the screen fade to red color when my character is hit, wait a second and then fade back to normal colors.
Or maybe I would like to change smoothly scale of an object to 1.5 and then go back to 1.0 and it all should take 4 seconds.

To achieve that I can use **coroutines** (which has drawbacks such as not being visible in editor mode etc.) or **async tasks** - for example using https://github.com/Cysharp/UniTask library.
**However, I end up writing new methods and keep repeating writing the same code** for every thing that happens in my game (correct me if I'm wrong).

**A solution could be a task library, that would allow me to run asynchronous tasks in one line**, in order to:

*Interpolate between values every frame for some time
*Run actions in loop
*Run actions in loop while some condition is true
*Wait some time and then run some action
*Chain all these kinds of tasks, so they gonna be executed one by one

In the .cs file I put a draft of a library without implementation details, just to demonstrate, how the interface gonna work. It could be implemented using coroutines or Unitask under the hood - but Unitask is probably better because of problems I mentioned.
At the bottom of the .cs file there are some examples of potential use.

A chain of task could be terminated by calling terminate() method. TerminateAndGoToNext() method would terminate current task and run the next task in chain.

Things to consider, but not included in the draft are:
tasks hierarchy, handling multiple interpolations that are performed on the same variable.
If my interface lacked some functionality, it would be good if my interface allowed to write custom Unitask tasks and chain them with tasks implementing my interface.

I believe that such an interface could make programming games much easier and make the code much cleaner and readable!



