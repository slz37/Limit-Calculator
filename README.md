## Limit Calculator
This became bigger than I expected it to be, so I moved it out of my C# Practice repository into it's own.

Takes in a function and limit and calculates the given limit. I first start <img src="https://latex.codecogs.com/svg.latex?\pm&space;" title="\pm x" />1 away from the limit and converge to it, checking that the absolute difference between the function values reaches a satisfactory value. The functions are converted from a string to mathematical values through the use of postfix evaluation. If the left and right x values get too close, and we still haven't reached a converging value, then the program exits with a limit of <img src="https://latex.codecogs.com/svg.latex?\infty" title="\infty" />.

List of known things that do not work:

1) Any limit whose function's domain does not extend to both sides of the limit. Ex: <img src="https://latex.codecogs.com/svg.latex?\lim_{x&space;\to&space;0}&space;\sqrt{x}&space;=&space;0" title="\lim_{x \to 0} \sqrt{x} = 0" />, but 
code fails since <img src="https://latex.codecogs.com/svg.latex?\sqrt{x}" title="\sqrt{x}" /> is undefined for x<0. I might leave this since you could define the definition of a limit as existing if only both left and right sided limits exist at the value for the function.
2) Limits that lead to an undefined form such as ![equation](https://latex.codecogs.com/svg.latex?%5Cfrac%7B%5Cinfty%7D%7B%5Cinfty%7D). Currently, I am attempting to implement L'Hopital's Rule to deal with these scenarios. Currently, the main function runs test cases for taking the derivatives of a variety of functions and evaluates those derivatives at x=2. I check the results with wolframalpha to ensure that the derivatives are being taken correctly.


## Current Status:
L'Hopital's rule runs into the issue of non-simplified expressions after taking the derivatives of the numerator and denominator. This results in a function that evaluates to NaN when unsimplified but evaluates to the correct limit if simplified. I'm not sure if I can fix this issue, as that would mean writing an expression tree of some sort to translate the function to its simplest form. At the momemt, I am pausing progress here.

## Sources Used for Research:

[Taking derivatives of postfix expressions](http://elib.mi.sanu.ac.rs/files/journals/yjor/21/yujorn21p61-75.pdf)

[Lanczos Approximation for factorials](https://en.wikipedia.org/wiki/Lanczos_approximation)

[Calculating the Lanczos Coefficients](https://mrob.com/pub/ries/lanczos-gamma.html)

[More info calculating Lanczos Coefficients](https://www.boost.org/doc/libs/1_43_0/libs/math/doc/sf_and_dist/html/math_toolkit/backgrounders/lanczos.html)

[Paul Godfrey's original notes on Lanczos Coefficients](http://my.fit.edu/~gabdo/gamma.txt)

## Example Execution Traces:

```
Input f(x):
cos(x)
Input limit:
pi/2
Limit as x->1.570795 of cos(x) = 0
Input f(x):
sin(x)
Input limit:
pi/2
Limit as x->1.570795 of sin(x) = 1
Input f(x):
sin(x)/x
Input limit:
0
Limit as x->0 of sin(x)/x = 1
Input f(x):
cos(x)/x
Input limit:
1
Limit as x->1 of cos(x)/x = NaN
Input f(x):
arctan(x)
Input limit:
pi
Limit as x->3.14159 of arctan(x) = 1.26263
```
