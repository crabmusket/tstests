# Tasman

A TorqueScript testing framework for [T3D][] inspired by [Jasmine][].

 [T3D]: https://github.com/GarageGames/Torque3D
 [Jasmine]: http://pivotal.github.io/jasmine/

# Example

```cs
// Declare a new test suite for MyObject.
test(MyObject);

// Set up before each test.
function MyObjectTests::before() {
   new ScriptObject(MyObject) {
      property = "value";
      vector = "1 2 3";
   };
}

// And pull down afterwards!
function MyObjectTests::after() {
   MyObject.delete();
}

// A basic test - expect that the setup works correctly!
function MyObjectShould::exist() {
   expect(MyObject).toBeAnObject();
}

// More advanced matchers.
function MyObjectShould::have_a_property() {
   expect(MyObject.property).toHave(1).word();
   expect(MyObject.property).toEqual("value");
}

// toBeDefined checks for "". Any test can be inverted with not().
function MyObjectShould::have_a_vector() {
   expect(MyObject.vector).toBeDefined();
   expect(MyObject.vector).not().toHave(3).words();
}
```

The output should look something like:

 ![Tasman test output](http://i.imgur.com/rpePs6u.png)
