//It's a pseudocode/draft
//At the bottom of the file there are examples of use
public class Task
{
    protected Action action;
    protected Task previous = null;
    protected Task next = null;

    public bool HasFinished; //{...}

    public Task()
    {
        //empty task; can be useful in chaining - see example below
    }

    public Task(Action action)
    {
        this.action = action;
    }

    public void Start()
    {
        //implementation dependent (coroutines vs Unitask)
        //run action asynchronously
        //...
        //when finished:
        if (this.next != null)
        {
            next.Start();
        }
    }

    public void Terminate()
    {
        ///terminate task and its successors
        //...
        HasFinished = true;
    }

    Task void TerminateAndGoToNext()
    {
        ///terminate task and start its successor immediately
        //...
        HasFinished = true;
        next.Start():
		}

    //chaining methods
    public Task getPrevious() { return previous; }
    public Task getNext() { return next; }
    public Task getFirst() { /*...*/ }
    public Task getLast() { /*...*/ }


    public Task Wait(float seconds)
    {
        return this.nextEvent = new Timeout(seconds);
    }

    //to run some action after timeout please use a chain: i.e. Wait(2f).Then(() => {...})
    //or... overloaded version below

    public Task Wait(float seconds, Action action)
    {
        return this.nextEvent = new Timeout(seconds, action);
    }


    public Task Then(Task t)
    {
        return this.nextTask = t;
    }

    public Task Then(Action action)
    {
        return this.next = new Task(action);
    }

    //Then() can be used in chain after event of any type, not only wait

    public Task Repeat(float interval, Action action)
    {
        return this.next = new Interval(interval, action);
    }

    public Task Repeat(Action action)
    {
        return this.next = new EveryFrameAction(action);
    }

    //repeat is an endless loop... unless you call Terminate()
    //if you use Repeat chained with Then: Repeat(() => {...}).Then(() => {})
    //call TerminateAndGoToNext()

    public Task RepeatWhile(Action action, Func<bool> repeatCondition)
    {
        return this.next = new ConditionalEveryFrameAction(action, repeatCondition);
    }

    public Task Interpolate<T>(ref T variable, T target, float seconds, Func<float> curve, Action additionalAction)
    {
        return this.next = new Interpolation<T>(variable, targetm, seconds, curve, additionalAction);
    }

    public Task Interpolate<T>(ref T variable, T target, float seconds, Func<float> curve)
    {
        return this.next = new Interpolation<T>(variable, targetm, seconds, curve);
    }

    //STATIC METHODS for conveinent creation of first task in chain
    //they're started automatically when a method is called

    public static Task Wait(....)
    {
        Task t = new Timeout(....);
        t.Start();

        return t;
    }
    //etc.
    public static Task Then... 

    public static Task Repeat...

    public static Task RepeatWhile...

    public static Task Interpolate...

    }


//names: Then, Timeout, Interval inspired by js async programming - SetTimeout, SetInterval, fetch....then(() => {})
public class Timeout : Task
{
    float seconds;

    public Timeout(float seconds, Action action) : base(action)
    {
        this.seconds = seconds;
    }

    public Timeout(float seconds)
    {
        this.seconds = seconds;
    }



    public void Start()
    {
        //waitForSeconds
        //then
        if (action != null)
        {
            action();
        }

        next.Start();
    }
}

public class Interval : Task
{
    float interval;

    public Interval(float interval, Action action) : base(action)
    {
        this.interval = interval;
    }

    public void Start()
    {
        while (!HasFinished)
        {
            //WaitForSeconds(interval)
            action();
        }

        //to go to next Task in chain, TerminateAndGoToNext() has to be called
    }
}

public class EveryFrameAction : Task
{
    public EveryFrameAction(Action action) : base(action)
    {
    }

    public void Start()
    {
        while (!HasFinished)
        {
            //WaitForEndOfFrame(interval)
            //and then:
            action();
        }

        //to go to next Task in chain, TerminateAndGoToNext() has to be called
    }
}

public class ConditionalEveryFrameAction : EveryFrameAction
{
    Func<Bool> repeatCondition;

    public ConditionalEveryFrameAction(Action action, Func<Bool> repeatCondition) : base(action)
    {
        this.repeatCondition = repeatCondition;
    }

    public void Start()
    {
        while (!HasFinished && repeatCondition())
        {
            //WaitForEndOfFrame(interval)
            //and then:
            action();
        }

        //to go to next Task in chain, TerminateAndGoToNext() has to be called
    }
}

public class Interpolation<T> : Task
{
    float duration;
    Func<float> curve; //or maybe make possible choose also AnimationCurve

    T variable;
    T target;

    float passedTime = 0f;

    public Interpolation(ref T variable, T target, float duration)
    {
        this.variable = ref variable;
        this.target = target;
        this.duration = duration;

    }

    public Interpolation(ref T variable, T target, float duration, Func<T> curve)
    {
        this.variable = ref variable;
        this.target = target;
        this.duration = duration;
        this.curve = curve;

    }

    public Interpolation(ref T variable, T target, float duration, Func<T> curve, Action additionalAction) : basse(additionalAction)
    {
        //additionalAction is run after every step of interpolation
        this.variable = ref variable;
        this.target = target;
        this.duration = duration;
        this.curve = curve;

    }

    public Interpolation(ref T variable, T target, float duration, Action additionalAction) : basse(additionalAction)
    {
        //additionalAction is run after every step of interpolation
        this.variable = ref variable;
        this.target = target;
        this.duration = duration;
    }

    void Start()
    {
        while (passedTime < duration)
        {
            //WaitForEndOfFrame

            passedTime += Time.deltaTime;
            //make on step of interpolation concerning Time.deltaTime and curve (if defined)
            //variable += ...

            action();
        }

        variable = new T(target); //what's best way to ensure copying by value?
    }
}

//#1 count 100 frames
public class SampleGameObject1 : MonoBehaviour
{
    int count = 0;
    Task t;

    void Start()
    {
        t = Task.Repeat(() => { Debug.Log(count++); });
    }

    void Update()
    {
        if (count == 100)
        {
            t.terminate();
        }
    }
}

//#2 count 100 frames but using repeatWhile
public class SampleGameObject2 : MonoBehaviour
{
    int count = 0;
    Task t;

    void Start()
    {
        t = Task.RepeatWhile(() => { Debug.Log(count++); }, () => return count < 100);
    }

    void Update()
    {
    }
}

//#3 wait 2 seconds and then do something
public class SampleGameObject3 : MonoBehaviour
{
    void Start()
    {
        Task.Wait(2f).Then(() => { doSomething(); });
    }

    void Update()
    {
    }
}

//#4 do something repeatedly with 2s interval
public class SampleGameObject4 : MonoBehaviour
{
    void Start()
    {
        Task.Repeat(2f, () => { doSomething(); });
    }

    void Update()
    {
    }
}

//#5 interpolate between positions with acceleration; movement should last 2s
public class SampleGameObject5 : MonoBehaviour
{
    void Start()
    {
        Task.Interpolate(transform.position, somePosition, x => x * x, 2f);
    }

    void Update()
    {
    }
}

//#6 interpolate between values when we cant pass a value by ref, but have  to call a setter method to update it
public class SampleGameObject6 : MonoBehaviour
{

    float opacity = 0.1f;

    void Start()
    {
        Task.Interpolate(opacity, 1f, 2f, () => { .....setOpacity(opacity)});
    }

    void Update()
    {
    }
}

//#7 you can chain all types of tasks if you want them to be executed one by one
//It can be used for i. e. creating simple animations
public class SampleGameObject7 : MonoBehaviour
{
    void Start()
    {
        Task.Interpolate(transform.position, somePosition, x => x, 2f)
            .Then(() => { doSomething(); })
            .Wait(1f)
            .Then(() => { doOtherThing(); });
    }

    void Update()
    {
    }
}

//#8 you can create a looping chain
public class SampleGameObject8 : MonoBehaviour
{
    void Start()
    {

        T task = new Task(); //creating a task manually and not by static method, creates a Task that's has to be explicitly started - there's no autostart in this case

        task.Interpolate(transform.position, somePosition, x => x, 2f)
            .Then(() => { doSomething(); })
            .Wait(1f)
            .Then(() => { doOtherThing(); })
            .Then(task); //loop

        task.Start(); //explicitly starting
    }

    void Update()
    {
    }
}
