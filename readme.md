#FailFast

An experimental unit test framework.  It was made for educating myself on unit test framework development.

It emphasizes on fast iterations, which is where the name originated. The framework will run tests until a failure is detected, then it will stop.

Other key points:

* Tests are defined with a sentence string, not a method name.
* Test classes inherit from a base class and do not use attributes.
* The framework is intentially light-weight.