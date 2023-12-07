# Day 6 - Parabola

## Bruteforce :)
So first solution was just to bruteforce with the knowledge that this is a parabola.
With that knowledge we can bruteforce to the first win and calculate what the total would be which is
$$time + 1 - winAt * 2$$
(+1 because time = 0 also counts):

``` C#
var winAt = 0;
while (winAt * (time - winAt) <= distanceToBeat)
    winAt++;

var wins = time + 1 - winAt * 2;
```

This gives answers for part 1 and 2 in milliseconds which is fast enough, but not as much of fun as math.

## Parabola formula
Now onto the real solution:
This is a parabola that is hitting points `(0,0)` and `(time,0)`. The top is always at `(time / 2, (time / 2)^2)`.
So if time is 8 then the top is at `(8 / 2, (8 / 2)^2)` => `(4, 16)`.

With this information we can assemble the parabola formula where `p = xTop` and `$q = yTop`:
$$y = a * (x - p)^2 + q$$
Since we always have the parabola going through `(0,0)` and $yTop = xTop^2$ we now can determine $a$:
$$0 = a * (0 - xTop)^2 + xTop^2$$
$$0 - xTop = -xTop$$
$$-xTop^2 = xTop^2$$

This means that $a * xTop^2$ should be $-(xTop^2)$.  
So $a$ is always $-1$ for all of our parabolas in this days puzzle.

And so the formula is:
$$y = -1 * (x - xTop)^2 + xTop^2$$

So we need to know our top which is already stated as: `(time / 2, (time / 2)^2)`
We know the distance we need to beat which is $y$. We want to know at which $x$ that is.

Let's break down the formula:
$$0 = -1 * (x - xTop)^2 + xTop^2 - y$$
$$(x - xTop)^2 = (xTop^2 - y) * -1$$
$$x - xTop = \sqrt{xTop^2 - y} * -1$$
$$x = \sqrt{xTop^2 - y} * -1 + xTop$$

Putting the distance in the formula, we know at which time we start winning:  
$x + 1$

Now let's calculate the losses (+1 since 0 is also a loss):
$$losses = (xDistance + 1) * 2$$

Now that we know the losses, we know the wins (+1 since 0 is also counted):
$$wins = (time + 1) - losses$$

Which can be simplified to:
$$wins = time - xDistance * 2 - 1$$

### Example 1
```
Time: 6
Distance to beat: 5

                  9
              8       8
Distance    5           5
           0             0
-----------------------------
Time       0  2   3   4  6
            1           5
-----------------------------
Wins          X   X   X
```

Top = `(3, 9)`  
$x$ for distance to beat = $\sqrt{9 - 5} * -1 + 3 = 1$  
So first win is at 2

$$wins = 6 - 1 * 2 - 1 = 3$$

### Example 2
Ok that was fun, but what if time is uneven? That would mean that xTop is no longer a whole number. Will that still work. Let's find out.


```
Time: 7
Distance to beat: 10

                 12 12
              10       10
Distance    6             6
           0               0
-----------------------------
Time       0   2  3  4  5  7
            1             6
-----------------------------
Wins              X  X
```

Top = `(3.5, 12.25)`
$$xDistance = \sqrt{12.25 - 10} * -1 + 3.5 = 1.5 * -1 + 3.5 = 2$$
`xDistance` is 2 which is correct by looking at the example.

Now for the rest of the equation:
$$wins = 7 - 2 * 2 - 1 = 2$$
That's also correct by looking at the example.

### Example 3
Last test, from the example input:
```
Time: 7
Distance to beat: 9
```

Same `time` as _Example 2_ but a `distance` that is not a whole number. Will it work?

Top = `(3.5, 12.25)`
$$xDistance = Sqrt(12.25 - 9) * -1 + 3.5 = 1.697$$

Let's round this down to the first whole number, which is 1, before calculating the win. That makes it easier to work with.  
So last loss is at 1 looking at whole numbers.  
`(int)xDistance = 1`

$wins = 7 - 1 * 2 - 1 = 4$

That also seems to work :)

### Conclusion
TL;DR

This is the code needed to calculate the number of wins:
``` C#
public static int GetWins(int time, long distance)
{
    var xTop = time / 2D;
    var yTop = xTop * xTop;
    var xDistance = (int)(Math.Sqrt(yTop - distance) * -1 + xTop);

    return time - xDistance * 2 - 1;
}
```

And yes, this can be a one-lines, but it should remain readable.